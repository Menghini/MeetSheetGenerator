using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Word = Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.IO;

namespace MeetSheetGenerator
{
    public partial class MainForm : Form
    {
        private List<Event> events; //Keeps track of all of the events in for the meet sheet.
        private List<string> schools; //Keeps track of all the schools competing.
        private int nameFormat = 0;
        private Boolean extraSlot = false;
        public MainForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Opens up a file dialog box and returns the string to the file. If file is not found, returns null.
        /// </summary>
        private string findFile(string filter, string title)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog(); //Make a new file open dialog.
            openFileDialog1.Filter = filter; //Set it filter
            openFileDialog1.Title = title; //Set the title for the dialog box.
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) //If a file was selected.
            {
                return openFileDialog1.FileName; //Return the location to the file.
            }
            return null; //If no file was selected, return null.
        }
        /// <summary>
        /// Checks to see what type of document or input this file is. Returns null if none.
        /// </summary>
        private string checkDocument(PdfReader reader)
        {
            String pageText = PdfTextExtractor.GetTextFromPage(reader, 1);
            if (pageText.Contains("Hy-Tek's MEET MANAGER"))
            {
                //It seems that the flight sheet always has this line in it.
                return "Hy-Tek Flight Sheet";
            }
            else if(pageText.Contains("TRXC Timing, LLC") && pageText.Contains("Rank Team First Name Last Name School Year\nDivision Performance School"))
            {
                //It seems that the personal pdf that is generated for coaches contains this.
                return "TRXC Performance List";
            }
            return null;
        }
        private void readTRXCFlightSheet(PdfReader reader)
        {
            iTextSharp.text.Rectangle mediabox = reader.GetPageSize(2); //Get the page size as a Rectangle
            float pageHeight = mediabox.Height; //Get the page Height
            float pageWidth = mediabox.Width; //Get the page width
                                              //Console.WriteLine(mediabox.Height); //Print the page height
                                              //Console.WriteLine(mediabox.Width); //Print the page width.

            int intPageNum = reader.NumberOfPages; //Get the number of pages this document has.
            Event currentEvent = null; //The current event that we are on.
            String eventType = null;
            for (int currentPage = 1; currentPage <= intPageNum; currentPage++)
            {
                for (int i = 0; i <= 1; i++)
                {
                    //When i is 0, we are looking at the left column.
                    //When i is 1, we are looking at the right column.
                    float columnLocationX = 0;
                    float columnLocationY = 0;
                    if (i == 1)
                    {
                        columnLocationX = pageWidth / 2; //Change this value to be on the right side.
                        //columnLocationY = 0;
                    }

                    System.util.RectangleJ rect = new System.util.RectangleJ(columnLocationX, columnLocationY, pageWidth / 2, pageHeight); //x, y, width, height
                    RenderFilter[] filter = { new RegionTextRenderFilter(rect) };
                    ITextExtractionStrategy strategy = new FilteredTextRenderListener(
                        new LocationTextExtractionStrategy(), filter);
                    String single_column = PdfTextExtractor.GetTextFromPage(reader, currentPage, strategy); //Get the entire text from the column.
                    String[] line = single_column.Split('\n'); //Get the columns one by one
                    

                    for (int j = 5; j < line.Length; j++)
                    {
                        //First off, are we on an event?
                        string currentLine = line[j];
                        int position;
                        if (currentLine.Substring(0, 5).Equals("Event"))
                        {
                            //Check to see if we already had a current event in progress
                            if(currentEvent!= null)
                            {
                                events.Add(currentEvent); //Add the current event (the one we just finished)
                            }
                            //We need to get to the end of event # in the line because it holds the event name.
                            currentLine = currentLine.Substring(currentLine.IndexOf("   ") + 3);
                            currentLine = currentLine.Substring(0, currentLine.LastIndexOf("("));
                            currentLine = currentLine.Trim(' ');
                            //currentLine = currentLine.Substring(currentLine.IndexOf(' '));
                            //Console.WriteLine(currentLine);
                            //bool isNumeric = int.TryParse("123", out postion);
                            //What is left of currentLine is the event title.
                            currentEvent = new Event(currentLine);
                            eventType = null; //We don't know what type of event this is just yet.
                        }
                        //Check to see if we have determined the event type
                        else if (eventType == null)
                        {
                            //Since we haven't we need to check.
                            if (currentLine.Contains("Lane Team")) //Must be a relay event
                            {
                                eventType = "Relay";
                                currentEvent.setType(eventType);
                            }
                            else if (currentLine.Contains("Pos Name Yr  School Seed Mark")) //Must be a field event
                            {
                                eventType = "Field";
                                currentEvent.setType(eventType);
                            }
                            else if (currentLine.Contains("Lane Name Yr  School")) //Must be a track event
                            {
                                eventType = "Track";
                                currentEvent.setType(eventType);
                            }
                        }
                        else if (currentLine.StartsWith("Flight"))
                        {
                            continue; //This line is useless to us... skip it.
                        }
                        //else if(int.TryParse(currentLine.Substring(0,1), out position))
                        else if (eventType.Equals("Track") || eventType.Equals("Field"))
                        {
                            //Console.WriteLine(currentLine);
                            string[] lineAtHand = currentLine.Split(' ');
                            //With a split on a space the incoming format should read...
                            //position lastName firstName (optional nickname) year school seedMark
                            string lastName;
                            string firstName;
                            if (lineAtHand[1].Contains(","))
                            {
                                //The first thing must be the last name.
                                lastName = lineAtHand[1].Trim(',');
                                firstName = lineAtHand[2];
                            }
                            else
                            {
                                //The second thing must be the last name.
                                lastName = lineAtHand[2];
                                firstName = lineAtHand[1];
                            }

                            string school = lineAtHand[4];
                            //For whatever reason a person can have a name with a parenthesis
                            if (lineAtHand[3].StartsWith("("))
                            {
                                firstName = lineAtHand[3].Substring(1, lineAtHand[3].Length - 2); //Cut off the parenthesis
                                school = lineAtHand[5]; //Have to redefine school as well.
                            }
                            //Check to see if the school is in the school list.
                            if (!schools.Contains(school))
                            {
                                schools.Add(school);
                            }
                            Athlete person = new Athlete(firstName, lastName, school); //Create an athlete
                            currentEvent.addAthlete(person); //Add the athlete to the event.
                            //Console.WriteLine(lastName);
                        }
                    }
                }
                progressBarFileRead.Value=currentPage/intPageNum*100;
                //break; //Let's just keep this to one page for now.

            }
            //Do we still have an event to take care of?
            if(currentEvent!= null)
            {
                events.Add(currentEvent); //We need to add the last event to the list before we forget.
            }
            
            /*foreach (Event currentEvents in events)
            {
                Console.WriteLine(currentEvents.getName());
            }*/
        }
        private void readTRXCPerformanceList(PdfReader reader)
        {
            iTextSharp.text.Rectangle mediabox = reader.GetPageSize(2); //Get the page size as a Rectangle
            float pageHeight = mediabox.Height; //Get the page Height
            float pageWidth = mediabox.Width; //Get the page width

            int intPageNum = reader.NumberOfPages; //Get the number of pages this document has.
            Event currentEvent = null; //The current event that we are on.
            String eventType = null;
            for (int currentPage = 1; currentPage <= intPageNum; currentPage++)
            {

                float columnLocationX = 0;
                float columnLocationY = 0;
                columnLocationX = 0; 

                System.util.RectangleJ rect = new System.util.RectangleJ(columnLocationX, columnLocationY, pageWidth, pageHeight); //x, y, width, height
                RenderFilter[] filter = { new RegionTextRenderFilter(rect) };
                //ITextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter);
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                String entirePage = PdfTextExtractor.GetTextFromPage(reader, currentPage, strategy); //Get the entire text from the column.

                //String entirePage = PdfTextExtractor.GetTextFromPage(reader, currentPage);
                String[] line = entirePage.Split('\n'); //Get the columns one by one
                Boolean isRelay = false;

                //The first event is always on line 6 for this type of file.
                for (int j = 6; j < line.Length; j++)
                {
                    //First off, are we on an event?
                    string currentLine = line[j];
                    if (currentLine.Contains("Men's") || currentLine.Contains("Women's"))
                    {
                        //Check to see if we already had a current event in progress
                        if (currentEvent != null)
                        {
                            events.Add(currentEvent); //Add the current event (the one we just finished)
                        }
                        //CurrentLine is the event title.
                        currentEvent = new Event(currentLine);
                        eventType = null; //We don't know what type of event this is just yet.
                        if (currentLine.Contains("Relay"))
                        {
                            //This event MUST be a relay event...
                            currentEvent.setType("Relay");
                        }
                        else if (currentLine.Any(c => char.IsDigit(c)))
                        {
                            //Does the line contain a number at all? Then it must be a track event.
                            currentEvent.setType("Track");
                        }
                        else
                        {
                            //If the line is not a relay event or a track event, it must be a field event.
                            currentEvent.setType("Field");
                        }
                    }
                    else if (currentLine.Contains(","))
                    {
                        //If the line contains a comma, it might be a name... so let's just break out of this. We already checked to see if it was an event so it can't be 3,200 or 1,600
                        return;
                    }
                    else
                    {
                        if (currentLine.Contains("First Name") || currentLine.Contains("Relay Letter"))
                        {
                            //We must be the header, and not the actual names of the people in this event.
                            //So... move on to the next line.
                            continue;
                        }

                        string[] lineAtHand = currentLine.Split(' ');
                        string lastName = "";
                        string firstName = "";
                        if (currentEvent.getType()!=null && !currentEvent.getType().Equals("Relay"))
                        {
                            //Since this is not a relay event... there is a first and/or last name.
                            lastName = lineAtHand[1];
                            firstName = lineAtHand[0];
                        }
                        //With a split on a space the incoming format should read...
                        //FirstName, LastName, Class, Division, Performance, Empty String, Rank, Team, School
                        /*
                        TODO: Not sure if this is needed....
                        if(lineAtHand.Length<2)
                        {
                            continue;
                        }
                        */
                        string school = ""; 
                        if (lineAtHand[lineAtHand.Length-2].Length==4)
                        {
                            //If the school does NOT have a space....
                            school = lineAtHand[lineAtHand.Length-1];
                        }
                        else
                        {
                            //If the school does have a space....
                            school = lineAtHand[lineAtHand.Length - 2];
                            school += " " + lineAtHand[lineAtHand.Length - 1];
                        }
                        if(school.Any(c => char.IsDigit(c)))
                        {
                            //For some reason, relays can have a number in them... this will detect that number.
                            school = Regex.Replace(school, @"[\d-].", string.Empty);
                            school = school.Substring(1);
                        }
                        //Check to see if the school is in the school list.
                        if (!schools.Contains(school))
                        {
                            schools.Add(school);
                        }
                        Athlete person = new Athlete(firstName, lastName, school); //Create an athlete
                        currentEvent.addAthlete(person); //Add the athlete to the event.
                        //Console.WriteLine(lastName);
                    }
                }
            progressBarFileRead.Value = currentPage / intPageNum * 100;
            //break; //Let's just keep this to one page for now.

            }
            //Do we still have an event to take care of?
            if (currentEvent != null)
            {
                events.Add(currentEvent); //We need to add the last event to the list before we forget.
            }

            /*foreach (Event currentEvents in events)
            {
                Console.WriteLine(currentEvents.getName());
            }*/
        }
        private void readCSVFile(String fileLocation)
        {
            Boolean firstLine = true;
            using (var reader = new StreamReader(fileLocation))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    //---------The First Line---------
                    //The first line is just event names.
                    //The first two columns will be "last name" and "first name" so we can skip about them and start on the 3rd(2) column.
                    if(firstLine)
                    {
                        for (int i = 2; i < values.Length; i++) //Add each event that exist.
                        {
                            String eventName = values[i]; //Get the entire event name.
                            String eventType = null;
                            if (eventName.Substring(0, 3).ToLower().Equals("(F)".ToLower()))
                                eventType = "Field";
                            else if (eventName.Substring(0, 3).ToLower().Equals("(R)".ToLower()))
                                eventType = "Relay";
                            else
                                eventType = "Track"; //Assume it's a track event if it's not entered.
                            eventName = eventName.Substring(3); //The name of the event is everything after the third character.
                            Event newEvent = new Event(eventName);
                            newEvent.setType(eventType);
                            events.Add(newEvent); //Add the event to the list.

                            //---------All Other Lines---------
                            //All the other lines are athletes.
                            comboBoxSchools.Items.Add("Your School Here");

                        }
                        firstLine = false;
                        continue;
                    }
                    

                    Athlete person = new Athlete(values[1], values[0], "Your School Here");
                    //Users's School is listed as it doesn't really matter since everyone will be from the same school.
                    for (int i=2; i<values.Length; i++) //Now we loop through each event and add each student.
                    {
                        if(!values[i].Equals(""))
                        {
                            if (events[i - 2].getType().Equals("Relay")) //If the kid is in a relay we should add their priority.
                            {
                                try //Try to catch a possible error.
                                {
                                    events[i - 2].addAthlete(person, Convert.ToDouble(values[i])); //It's i-2 because we events start on the 3rd column.
                                }
                                catch (System.FormatException e)
                                {
                                    MessageBox.Show(person.getName(0)+" is causing an error with the "+events[i-2].getName(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                
                            }
                            else
                            {
                                events[i - 2].addAthlete(person); //It's i-2 because we events start on the 3rd column.
                            }
                        }
                    }
                }
                progressBarFileRead.Value = 100;
            }
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string fileLocation = findFile("CSV File|*.csv|PDF File|*.pdf", "Select a PDF File"); //First ask where the file is.
            //The filter is separated by the character, "|".
            //Check to see if a file was selected.
            if (fileLocation == null)
                return;
            if(fileLocation.EndsWith("pdf")) //If the file is a PDF file...
            {
                PdfReader reader = new PdfReader(fileLocation);

                string fileType = checkDocument(reader);
                //Check to see if the file type is one of the valid ones.
                if (fileType == null)
                {
                    MessageBox.Show("File type is not recognized.", "File Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (fileType.Equals("Hy-Tek Flight Sheet"))
                {
                    readTRXCFlightSheet(reader);
                }
                else if (fileType.Equals("TRXC Performance List"))
                {
                    readTRXCPerformanceList(reader);
                }
                //comboBoxSchools.Items.Add("nothing");
                foreach (string currentSchool in schools)
                {
                    comboBoxSchools.Items.Add(currentSchool);
                }
            }
            else //If the file is a CSV file.
            {
                //TODO: For now I'm assuming it is a correct CSV file.
                readCSVFile(fileLocation);
            }
            comboBoxSchools.SelectedIndex = 0; //Set to the first element
            groupBox2.Enabled = true;

            foreach (Event currentEvent in events)
            {
                listBoxEvents.Items.Add(currentEvent);
            }

            return;
            //comboBoxSchools.Sorted

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            events = new List<Event>();
            schools = new List<string>();
        }
        /// <summary>
        /// Sets the format for tabs into a relay format
        /// </summary>
        private void setFormatRelay(Word.Paragraph oPara1)
        {
            oPara1.TabStops.ClearAll();
            float tabIndex = (float).5 * 72; //Add takes a "point". 1 inch = 72 points
            for (int i = 0; i < 5; i++)
            {
                oPara1.TabStops.Add(tabIndex, 0, 0);
                tabIndex += (float)1.5 * 72;
            }
        }
        /// <summary>
        /// Sets the format for tabs into field events
        /// </summary>
        private void setFormatField(Word.Paragraph oPara1)
        {
            oPara1.TabStops.ClearAll();
            float tabIndex = (float)0 * 72; //Add takes a "point". 1 inch = 72 points
            oPara1.TabStops.Add(tabIndex, 0, 0);
            tabIndex += (float)1.5 * 72;
            oPara1.TabStops.Add(tabIndex, 0, 0);
        }
        /// <summary>
        /// Sets the format for tabs into a individual athelete format
        /// </summary>
        private void setFormatIndividual(Word.Paragraph oPara1)
        {
            oPara1.TabStops.ClearAll();
            float tabIndex = (float)1 * 72; //Add takes a "point". 1 inch = 72 points
            for (int i = 0; i < 4; i++)
            {
                oPara1.TabStops.Add(tabIndex, 0, 0);
                tabIndex += (float)1.5 * 72;
            }
        }
 
        private void openMeetSheet()
        {
            //TODO: Schools that have a name with a space do not work (they do not display correctly)
            object oMissing = System.Reflection.Missing.Value;
            object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

            //Start Word and create a new document.
            Word._Application oWord;
            Word._Document oDoc;
            oWord = new Word.Application();
            oWord.Visible = true;
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                ref oMissing, ref oMissing);

            //Insert a paragraph at the beginning of the document.
            Word.Paragraph oPara1;
            oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
            oPara1.Range.Text = (String)comboBoxSchools.SelectedItem + " Track & Field Meet Sheet";
            oWord.ActiveDocument.PageSetup.LeftMargin = (float)36;
            oWord.ActiveDocument.PageSetup.RightMargin = (float)36;
            oWord.ActiveDocument.PageSetup.TopMargin = (float)36;
            oWord.ActiveDocument.PageSetup.BottomMargin = (float)36;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.Font.Size = 10;
            oPara1.Range.Font.Name = "Times New Roman";
            oPara1.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oPara1.Format.SpaceAfter = 0;    //24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = "Monday, January" + " " + "1st" + ", " + "2018" + ", @ " + "TBD";
            oPara1.Range.InsertParagraphAfter();

            //oPara1.Range.Text = "Running Events:";
            oPara1.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            oPara1.Range.InsertParagraphAfter();

            foreach (Event currentEvent in listBoxEvents.SelectedItems)
            {
                if(currentEvent.getType().Equals("Field"))
                {
                    continue; //We don't need to do anything this time around. Just skip. Field events will happen later in the code.
                }
                Console.WriteLine(currentEvent.getName());
                //String tempPrint = currentEvent.getName();
                oPara1.Range.Font.Bold = 1;
                oPara1.Range.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;
                oPara1.Range.Text = currentEvent.getName();
                oPara1.Range.InsertParagraphAfter();
                oPara1.Range.Font.Bold = 0;
                oPara1.Range.Font.Underline = 0;



                String tempPrint = "";
                int index = 0;
                //Is this a track event (as opposed to a relay or field event?
                if(currentEvent.getType().Equals("Track"))
                {
                    setFormatIndividual(oPara1); //Set the format for tabs to be a track event
                    foreach (Athlete currentAthlete in currentEvent.getAthletes((String)comboBoxSchools.SelectedItem))
                    {
                        /*oPara1.Range.Text += currentAthlete.getName(0);
                        Console.WriteLine(currentAthlete.getName(0));*/
                        if (index % 4 == 0 && index != 0)
                        {
                            tempPrint += "\n\t___ " + (String)currentAthlete.getName(nameFormat);
                        }
                        else
                        {
                            tempPrint += "\t___ " + (String)currentAthlete.getName(nameFormat);
                        }
                        index++;
                    }
                    //Where they any people in this event in the first place?
                    if(index==0)
                    {
                        //If not, add a NO ENTRY
                        tempPrint += "\t___ NO ENTRY";
                    }
                    //Should we include an extra slot in the event?
                    if (extraSlot)
                    {
                        if (index % 4 != 0 || index==0)
                        {
                            tempPrint += "\t___ ";
                        }
                        else
                        {
                            tempPrint += "\n\t___ ";
                        }
                    }

                    oPara1.Range.Text = tempPrint;
                    oPara1.Range.InsertParagraphAfter();
                    oPara1.Range.Text = "";
                    oPara1.Range.InsertParagraphAfter(); //Just an extra line :)
                }
                else if (currentEvent.getType().Equals("Relay"))
                {
                    //TODO: This only works with CSV files. This might not work for PDFs....
                    /*setFormatRelay(oPara1);
                    tempPrint += "\t___\t___\t___\t___\t:_______\n\t{\t___\t___\t}";
                    oPara1.Range.Text = tempPrint;
                    oPara1.Range.InsertParagraphAfter();
                    oPara1.Range.Text = "";
                    oPara1.Range.InsertParagraphAfter(); //Just an extra line :)*/
                    setFormatRelay(oPara1); //Set the format for tabs to be a relay event
                    foreach (Athlete currentAthlete in currentEvent.getAthletes((String)comboBoxSchools.SelectedItem))
                    {
                        int athletePostion = Int32.Parse(currentAthlete.getPriority().ToString().Substring(currentAthlete.getPriority().ToString().Length-1));
                        if (athletePostion < 4) //I do this to see the position of the person in the relay (the decimal part).
                        {
                            tempPrint += "\t___ " + (String)currentAthlete.getName(nameFormat);
                        }
                        else if(athletePostion == 4)
                        {
                            tempPrint += "\t___ " + (String)currentAthlete.getName(nameFormat)+ "\t:_______\n\t{\t___\t___\t}\n";
                        }
                        index++;
                    }
                    //Where they any people in this event in the first place?
                    if (index == 0)
                    {
                        //If not, add a NO ENTRY
                        setFormatIndividual(oPara1); //But first make sure the tabs will line up nicely. 
                        tempPrint += "\t___ NO ENTRY";
                    }

                    oPara1.Range.Text = tempPrint;
                    oPara1.Range.InsertParagraphAfter();
                    oPara1.Range.Text = "";
                    oPara1.Range.InsertParagraphAfter(); //Just an extra line :)
                }
            }
            oDoc.Words.Last.InsertBreak(Word.WdBreakType.wdSectionBreakContinuous);
            //oPara1.Range.Text = "test";
            //Now let's print the field events.
            //Is this a track event (as opposed to a relay or field event?

            foreach (Event currentEvent in listBoxEvents.SelectedItems)
            {
                if (currentEvent.getType().Equals("Field"))
                {
                    Console.WriteLine(currentEvent.getName());
                    //String tempPrint = currentEvent.getName();
                    oPara1.Range.Font.Bold = 1;
                    oPara1.Range.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;
                    oPara1.Range.Text = currentEvent.getName();
                    oPara1.Range.InsertParagraphAfter();
                    oPara1.Range.Font.Bold = 0;
                    oPara1.Range.Font.Underline = 0;
                    String tempPrint = "";
                    setFormatField(oPara1); //Set the tab formats to be for field events.
                    int index = 0;
                    foreach (Athlete currentAthlete in currentEvent.getAthletes((String)comboBoxSchools.SelectedItem))
                    {
                        /*oPara1.Range.Text += currentAthlete.getName(0);
                        Console.WriteLine(currentAthlete.getName(0));*/
                        if (index % 2 == 1)
                        {
                            tempPrint += "\t___ " + (String)currentAthlete.getName(nameFormat);
                        }
                        else if(index == 0)
                        {
                            tempPrint += "___ " + (String)currentAthlete.getName(nameFormat);
                        }
                        else if(index %2 == 0)
                        {
                            tempPrint += "\n___ " + (String)currentAthlete.getName(nameFormat);
                        }
                        index++;
                    }
                    //Where they any people in this event in the first place?
                    if (index == 0)
                    {
                        //If not, add a NO ENTRY
                        tempPrint += "___ NO ENTRY";
                    }
                    //Should we include an extra slot in the event?
                    if (extraSlot)
                    {
                        if (index % 2 == 1)
                        {
                            tempPrint += "\t___ ";
                        }
                        else if (index == 0)
                        {
                            tempPrint += "___ ";
                        }
                        else if (index % 2 == 0)
                        {
                            tempPrint += "\n___ ";
                        }
                    }

                    oPara1.Range.Text = tempPrint;
                    oPara1.Range.InsertParagraphAfter();
                    oPara1.Range.Text = "";
                    oPara1.Range.InsertParagraphAfter(); //Just an extra line :)

                }
            }
            oDoc.Sections[2].PageSetup.TextColumns.SetCount(2); //Only have the second section be two columns.
            //oDoc.PageSetup.TextColumns.SetCount(2);
            

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Event currentItem = (Event)listBoxEvents.SelectedItem;

            //List<Athlete> athletes = currentItem.getAthletes((String)comboBoxSchools.SelectedItem);
            /*foreach (Event currentEvent in listBoxEvents.SelectedItems)
            {
                Console.WriteLine("---------"+currentEvent.getName()+"----------");
                foreach (Athlete currentAthlete in currentEvent.getAthletes((String)comboBoxSchools.SelectedItem))
                {
                    Console.WriteLine(currentAthlete.getName(0));
                }
            }*/

            openMeetSheet();




        }

        private void radioButtonFirstLast_CheckedChanged(object sender, EventArgs e)
        {
            nameFormat = 0;
        }

        private void radioButtonLastFirst_CheckedChanged(object sender, EventArgs e)
        {
            nameFormat = 1;
        }

        private void radioButtonFirstInitalLast_CheckedChanged(object sender, EventArgs e)
        {
            nameFormat = 2;
        }

        private void radioButtonLastInitalFirst_CheckedChanged(object sender, EventArgs e)
        {
            nameFormat = 3;
        }

        private void checkBoxExtraSlot_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxExtraSlot.Checked)
            {
                extraSlot = true;
            }
            else
            {
                extraSlot = false;
            }
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBoxEvents.Items.Count; i++)
            {
                listBoxEvents.SetSelected(i, true);
            }
        }

        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBoxEvents.Items.Count; i++)
            {
                listBoxEvents.SetSelected(i, false);
            }
        }
    }
}

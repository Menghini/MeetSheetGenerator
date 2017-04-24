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
            if (pageText.Contains("TRXC Timing, LLC. - Contractor License"))
            {
                //It seems that the flight sheet always has this line in it.
                return "TRXC Flight Sheet";
            }
            else if(pageText.Contains("TRXC Timing, LLC"))
            {
                //It seems that the personal pdf that is generated for coaches contains this.
                return "TRXC Line Up";
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
                            string lastName = lineAtHand[1].Trim(',');
                            string firstName = lineAtHand[2];
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
        
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string fileLocation = findFile("PDF File|*.pdf", "Select a PDF File"); //First ask where the file is.
            //Check to see if a file was selected.
            if (fileLocation == null)
                return;
            PdfReader reader = new PdfReader(fileLocation);

            string fileType=checkDocument(reader);
            //Check to see if the file type is one of the valid ones.
            if (fileType == null)
            {
                MessageBox.Show("File type is not recognized.", "File Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(fileType.Equals("TRXC Flight Sheet"))
            {
                readTRXCFlightSheet(reader);
            }
            else if (fileType.Equals("TRXC Line Up"))
            {
                //TODO
                return;
            }
            //comboBoxSchools.Items.Add("nothing");
            foreach (string currentSchool in schools)
            {
                comboBoxSchools.Items.Add(currentSchool);
                comboBoxSchools.SelectedIndex = 0; //Set to the first element
                groupBox2.Enabled = true;
            }
            foreach(Event currentEvent in events)
            {
                listBoxEvents.Items.Add(currentEvent);
            }
            
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
            oPara1.Range.Text = "Rockwood Summit High School / Falcon Track & Field Meet Sheet";
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
            oPara1.Range.Text = "January" + " " + "1st" + ", " + "2017" + ", @ " + "Rockwood Summit" + " VS " + "Marquette";
            oPara1.Range.InsertParagraphAfter();

            //oPara1.Range.Text = "Running Events:";
            oPara1.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            oPara1.Range.InsertParagraphAfter();

            foreach (Event currentEvent in listBoxEvents.SelectedItems)
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
                        tempPrint += "\t___ NO ENTRY\t___";
                    }
                    //Should we include an extra slot in the event?
                    if (extraSlot)
                    {
                        if (index % 4 != 0)
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
                    setFormatRelay(oPara1);
                    tempPrint += "\t___\t___\t___\t___\t:_______\n\t{\t___\t___\t}";
                    oPara1.Range.Text = tempPrint;
                    oPara1.Range.InsertParagraphAfter();
                    oPara1.Range.Text = "";
                    oPara1.Range.InsertParagraphAfter(); //Just an extra line :)
                }
            }
            

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
    }
}

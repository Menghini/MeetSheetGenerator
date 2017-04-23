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

namespace MeetSheetGenerator
{
    public partial class MainForm : Form
    {
        private List<Event> events; //Keeps track of all of the events in for the meet sheet.
        private List<string> schools; //Keeps track of all the schools competing.
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
                        columnLocationY = 0;
                    }

                    System.util.RectangleJ rect = new System.util.RectangleJ(columnLocationX, columnLocationY, pageWidth / 2, pageHeight); //x, y, width, height
                    RenderFilter[] filter = { new RegionTextRenderFilter(rect) };
                    ITextExtractionStrategy strategy = new FilteredTextRenderListener(
                        new LocationTextExtractionStrategy(), filter);
                    String single_column = PdfTextExtractor.GetTextFromPage(reader, currentPage, strategy); //Get the entire text from the column.
                    String[] line = single_column.Split('\n'); //Get the columns one by one


                    for (int j = 0; j < line.Length; j++)
                    {
                        //First off, are we on an event?
                        string currentLine = line[j];
                        int position;
                        if (currentLine.Substring(0,5).Equals("Event"))
                        {
                            
                            
                            //We need to get to the end of event # in the line because it holds the event name.
                            currentLine = currentLine.Substring(currentLine.IndexOf("   ") + 3);
                            currentLine = currentLine.Substring(0,currentLine.LastIndexOf("("));
                            currentLine = currentLine.Trim(' ');
                            //currentLine = currentLine.Substring(currentLine.IndexOf(' '));
                            //Console.WriteLine(currentLine);
                            //bool isNumeric = int.TryParse("123", out postion);
                            //What is left of currentLine is the event title.
                            currentEvent = new Event(currentLine);
                        }
                        else if(int.TryParse(currentLine.Substring(0,1), out position))
                        {
                            //Console.WriteLine(currentLine);
                            string[] lineAtHand = currentLine.Split(' ');
                            string lastName = lineAtHand[1].Trim(',');
                            string firstName = lineAtHand[2];
                            string school = lineAtHand[4];
                            Athlete person = new Athlete(firstName, lastName, school); //Create an athlete
                            currentEvent.addAthlete(person); //Add the athlete to the event.
                            //Console.WriteLine(lastName);
                        }
                            //Event  3   (G) T.J. (28)
                        //Console.WriteLine(line[j]); //Write it out into a text file.
                        /*if (line[j].Contains("Marquette")) //I only care about Marquette teams
                        {
                            Console.WriteLine(line[j]); //Write it out into a text file.
                            //TRXC Timing, LLC. - Contractor License
                        }*/
                    }
                }
                return; //Let's just keep this to one page for now.

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
                return;
            }
            else if (fileType.Equals("TRXC Line Up"))
            {
                //TODO
                return;
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            events = new List<Event>();
            schools = new List<string>();
        }
    }
}

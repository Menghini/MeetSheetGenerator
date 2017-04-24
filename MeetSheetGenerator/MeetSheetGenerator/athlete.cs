using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetSheetGenerator
{
    class Athlete
    {
        private string firstName;
        private string lastName;
        private string school;
        public Athlete(string firstName, string lastName, string school)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.school = school;
        }
        /// <summary>
        /// 0 setting is "first last"
        /// </summary>
        public string getName(int setting)
        {
            if (setting==0)
            {
                return firstName + " " + lastName;
            }
            else if (setting == 1)
            {
                return lastName + " " + firstName;
            }
            else if (setting == 2)
            {
                return firstName.Substring(0,1) + " " + lastName;
            }
            else if (setting == 3)
            {
                return lastName.Substring(0, 1) + " " + firstName;
            }
            return null;
        }
        public string getSchool()
        {
            return school;
        }
        public override string ToString()
        {
            //Just print the name of the event.
            return firstName + " " + lastName;
        }
    }
}

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
            return null;
        }
    }
}

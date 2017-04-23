using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetSheetGenerator
{
    class Event
    {
        private string name; //The name of the event
        private List<Athlete> athletes; //The athletes in the event

        public Event(string name)
        {
            this.name = name;
            athletes = new List<Athlete>();
        }
        public string getName()
        {
            return name;
        }
        public void addAthlete(Athlete person)
        {
            athletes.Add(person);
        }
    }
}

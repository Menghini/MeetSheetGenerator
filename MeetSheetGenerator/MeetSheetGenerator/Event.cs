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
        private List<string> athletes; //The athletes in the event

        public Event(string name)
        {
            this.name = name;
            athletes = new List<string>();
        }
    }
}

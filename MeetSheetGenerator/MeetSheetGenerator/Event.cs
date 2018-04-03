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
        private List<String> schools; //The schools in the event
        private string eventType;

        public Event(string name)
        {
            this.name = name;
            schools = new List<String>();
            athletes = new List<Athlete>();
            
        }
        public void setType(String eventType)
        {
            this.eventType = eventType;
        }
        public String getType()
        {
            return eventType;
        }
        public string getName()
        {
            return name;
        }
        public void addAthlete(Athlete person)
        {
            if(athletes!= null)
            {
                athletes.Add(person);
            }
            
        }
        //Use this for relays. Priority is x.y. x=relay number and y=position in relay.
        public void addAthlete(Athlete person, double priority)
        {
            if (athletes != null) 
            {
                person.addPriority(priority);
                athletes.Add(person);
            }

        }
        public void addSchools(String school)
        {
            if(schools!= null)
            {
                schools.Add(school);
            }
        }
        public List<Athlete> getAthletes(String school)
        {
            List<Athlete> athletes = new List<Athlete>();
            foreach (Athlete currentAthlete in this.athletes)
            {
                if(currentAthlete.getSchool().Equals(school))
                {
                    athletes.Add(currentAthlete);
                }
                if (eventType.Equals("Relay")) //If this is a relay we should sort the names of how they will appear on the list.
                {
                    //athletes.OrderBy(athletes=>athletes.getName()).toList();
                    athletes = athletes.OrderBy(o => o.priority).ToList(); //TODO: Priority is currently public. Change this!!!
                }
                //listBoxEvents.Items.Add(currentEvent);
            }
            return athletes;
        }
        public override string ToString()
        {
            //Just print the name of the event.
            return name;
        }
    }
}

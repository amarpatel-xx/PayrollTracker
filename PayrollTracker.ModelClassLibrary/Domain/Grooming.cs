using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class Grooming
    {
        public Grooming() { }

   		public Grooming(GroomingType groomingType, Double cost, DateTime date, Dog dog, User user)
		{
            this.GroomingType = groomingType;
            this.Cost = cost;
            this.Date = date;
            this.Dog = dog;
            this.User = user;
		}

        public virtual string GroomingId { get; set; }
        public virtual Double Cost { get; set; }
        public virtual Double Tip { get; set; }  
        public virtual DateTime Date { get; set; }
        public virtual User User { get; set; }
        public virtual Dog Dog { get; set; }
        public virtual GroomingType GroomingType { get; set; }
    }
}

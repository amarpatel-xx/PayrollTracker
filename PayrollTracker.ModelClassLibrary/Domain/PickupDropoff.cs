using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class PickupDropoff
    {
        public PickupDropoff() { }

   		public PickupDropoff(DateTime date, Dog dog, User user)
		{
            this.Date = date;
            this.Dog = dog;
            this.User = user;
		}

        public virtual string PickupDropoffId { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Dog Dog { get; set; }
        public virtual Cost PickupCost { get; set; }
        public virtual Cost DropoffCost { get; set; }
        public virtual User User { get; set; }
    }
}

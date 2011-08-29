using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class Daycare
    {
        public Daycare() { }

   		public Daycare(DateTime date, Dog dog, Cost daycareCost, User user)
		{
            this.Date = date;
            this.Dog = dog;
            this.DaycareCost = daycareCost;
            this.User = user;
		}

        public virtual string DaycareId { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Dog Dog { get; set; }
        public virtual Cost DaycareCost { get; set; }
        public virtual User User { get; set; }

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is Daycare)
            {
                Daycare daycare = obj as Daycare;
                if (daycare != null)
                {
                    if (DaycareId != null && DaycareId == daycare.DaycareId)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "Daycare(Date:" + Date + ", Dog: " + Dog.FullName + ", DaycareCost: " + DaycareCost.CostValue + ", User: " + User.Username + ")\n";
        }
    }
}

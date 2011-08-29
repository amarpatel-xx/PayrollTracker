using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class Boarding
    {
        public Boarding() { }

   		public Boarding(DateTime date, bool isDaycare, Dog dog, Cost boardingCost, User user)
		{
            this.Date = date;
            this.IsDaycare = isDaycare;
            this.Dog = dog;
            this.BoardingCost = boardingCost;
            this.User = user;
		}

        public virtual string BoardingId { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Double Tip { get; set; }
        public virtual bool IsDaycare { get; set; }
        public virtual Dog Dog { get; set; }
        public virtual Cost BoardingCost { get; set; }
        public virtual Cost SundayDaycareCost { get; set; }
        public virtual User User { get; set; }

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is Boarding)
            {
                Boarding boarding = obj as Boarding;
                if (boarding != null)
                {
                    if (BoardingId != null && BoardingId == boarding.BoardingId)
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
            return "Boarding(Date:" + Date + ", IsDaycare: " + IsDaycare + ", Dog: " + Dog.FullName + ", BoardingCost: " + BoardingCost.CostValue + ", Tip: " + Tip + ", User: " + User.Username + ")\n";
        }
    }
}

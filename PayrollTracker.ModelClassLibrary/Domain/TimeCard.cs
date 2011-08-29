using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class TimeCard
    {
   		public TimeCard()
		{
            TimeIn = DateTime.MinValue;
            TimeOut = DateTime.MinValue;
		}

        public virtual string TimeCardId { get; set; }
        public virtual DateTime TimeIn { get; set; }        
        public virtual DateTime TimeOut { get; set; }
        public virtual User User { get; set; }

        public override string ToString()
        {
            return "Time Card for User " + User.Username + ".\nTime In: " + TimeIn + ".\nTime Out: " + TimeOut + ".";
        }
    }
}

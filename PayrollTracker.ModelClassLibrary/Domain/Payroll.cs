using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class Payroll
    {
   		public Payroll()
		{
		}

        public virtual string PayrollId { get; set; }
        public virtual DateTime PayrollStartDate { get; set; }        
        public virtual int PayrollNumberOfWeeks { get; set; }
        public virtual Company Company { get; set; }
    }
}

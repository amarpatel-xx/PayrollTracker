using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class Training
    {
        public Training() { }

   		public Training(DateTime date, CostType classType, Cost classCost, Dog dog, User user)
		{
            this.Date = date;
            this.ClassType = classType;
            this.ClassCost = classCost;
            this.Dog = dog;
            this.User = user;
		}

        public virtual string TrainingId { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual CostType ClassType { get; set; }
        public virtual Cost ClassCost { get; set; }
        public virtual Cost PreK9DaycareCost { get; set; }
        public virtual Dog Dog { get; set; }
        public virtual User User { get; set; }
    }
}

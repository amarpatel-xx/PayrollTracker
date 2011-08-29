using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class Cost : IComparable 
    {
        public Cost() { }

        public Cost(Double costValue)
        {
            this.CostValue = costValue;
        }

        public virtual string CostId { get; set; }
        public virtual Double CostValue { get; set; }

        public virtual int CompareTo(object obj)
        {
            if (obj is Cost)
            {
                Cost cost = (Cost)obj;
                return this.CostValue.CompareTo(cost.CostValue);
            }
            throw new ArgumentException(string.Format("Cannot compare a Cost to an {0}", obj.GetType().ToString()));
        } 

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is Cost)
            {
                Cost cost = obj as Cost;
                if (cost != null)
                {
                    if (CostId == cost.CostId)
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
            return "(CostValue: " + CostValue + ")";
        }

    }
}
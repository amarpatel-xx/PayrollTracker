using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class CostType
    {
        public CostType() 
        {
            PossibleCosts = new List<Cost>();
        }

   		public CostType(String theCostName)
		{
            this.CostName = theCostName;
            PossibleCosts = new List<Cost>();
		}

        public virtual string CostTypeId { get; set; }
        public virtual string CostName { get; set; }
        public virtual IList<Cost> PossibleCosts { get; set; }

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is CostType)
            {
                CostType costType = obj as CostType;
                if (costType != null)
                {
                    if (CostTypeId == costType.CostTypeId)
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
            return CostName;
        }
    }
}
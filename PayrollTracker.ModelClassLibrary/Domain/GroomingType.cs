using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class GroomingType
    {
        public GroomingType() { }

   		public GroomingType(String typeName)
		{
            this.TypeName = typeName;
		}

        public virtual string GroomingTypeId { get; set; }
        public virtual string TypeName { get; set; }

        public virtual int CompareTo(object obj)
        {
            if (obj is GroomingType)
            {
                GroomingType groomingType = (GroomingType)obj;
                return this.TypeName.CompareTo(groomingType.TypeName);
            }
            throw new ArgumentException(string.Format("Cannot compare a GroomingType to an {0}", obj.GetType().ToString()));
        } 

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is GroomingType)
            {
                GroomingType groomingType = obj as GroomingType;
                if (groomingType != null)
                {
                    if (GroomingTypeId == groomingType.GroomingTypeId)
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
    }
}
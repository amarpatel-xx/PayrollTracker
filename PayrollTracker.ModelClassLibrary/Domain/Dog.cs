using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class Dog
    {
   		public Dog()
		{
		}

        public virtual string DogId { get; set; }
        public virtual string FirstName { get; set; }        
        public virtual string LastName { get; set; }
        public virtual string FullName {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is Dog)
            {
                Dog dog = obj as Dog;
                if (dog != null)
                {
                    if (DogId != null && DogId == dog.DogId)
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
            return "Dog: " + FirstName + " " + LastName + ".\n";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class Company
    {
        public Company() 
        {
            this.Users = new List<User>();
        }

        public Company(String name)
        {
            this.Name = name;
            this.Users = new List<User>();
        }

        public virtual string CompanyId { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<User> Users { get; set; }

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is Company)
            {
                Company company = obj as Company;
                if (company != null)
                {
                    if (CompanyId == company.CompanyId && Name == company.Name)
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
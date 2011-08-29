using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class Role
    {
        public Role() { }

   		public Role(String roleName)
		{
            this.RoleName = roleName;
		}

        public virtual string RoleId { get; set; }
        public virtual string RoleName { get; set; }

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is Role)
            {
                Role role = obj as Role;
                if (role != null)
                {
                    if (RoleName == role.RoleName)
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

        public virtual int CompareTo(object obj)
        {
            if (obj is Role)
            {
                Role role = (Role)obj;
                return this.RoleName.CompareTo(role.RoleName);
            }
            throw new ArgumentException(string.Format("Cannot compare a Role to an {0}", obj.GetType().ToString()));
        } 
    }
}
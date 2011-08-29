using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public class User
    {
        public User() 
        {
            AssignedRoles = new List<Role>();
            TimeEntries = new List<TimeCard>();
            WorksForCompanies = new HashSet<Company>();
            Groomings = new List<Grooming>();
            Boardings = new List<Boarding>();
            Daycares = new List<Daycare>();
            PickupDropoffs = new List<PickupDropoff>();
            Trainings = new List<Training>();
        }

   		public User(String username, String password, String firstName, String lastName, DateTime dateOfHire)
		{
            this.Username = username;
            this.Password = password;
            this.IsActive = true;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfHire = dateOfHire;
            AssignedRoles = new List<Role>();
            TimeEntries = new List<TimeCard>();
            WorksForCompanies = new HashSet<Company>();
            Groomings = new List<Grooming>();
            Boardings = new List<Boarding>();
            Daycares = new List<Daycare>();
            PickupDropoffs = new List<PickupDropoff>();
            Trainings = new List<Training>();
		}

        public virtual string UserId { get; set; }
        public virtual string Username { get; set; }        
        public virtual string Password { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleInitial { get; set; }
        public virtual string LastName { get; set; }
        public virtual string SocialSecurityNumber { get; set; }
        public virtual DateTime DateOfHire { get; set; }
        public virtual double HourlyPay { get; set; }
        public virtual double TrainingPercentage { get; set; }
        public virtual double GroomingPercentage { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string EmergencyContactName { get; set; }
        public virtual string EmergencyContactNumber { get; set; }
        public virtual IList<Role> AssignedRoles { get; set; }
        public virtual ICollection<TimeCard> TimeEntries { get; set; }
        public virtual ICollection<Company> WorksForCompanies { get; set; }
        public virtual ICollection<Grooming> Groomings { get; set; }
        public virtual ICollection<Boarding> Boardings { get; set; }
        public virtual ICollection<Daycare> Daycares { get; set; }
        public virtual ICollection<PickupDropoff> PickupDropoffs { get; set; }
        public virtual ICollection<Training> Trainings { get; set; }

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is User)
            {
                User user = obj as User;
                if (user != null)
                {
                    if (UserId == user.UserId && Username == user.Username)
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

        public virtual bool ContainsRole(string roleName)
        {
            bool returnValue = false;

            foreach(Role role in AssignedRoles)
            {
                if (role.RoleName.Equals(roleName))
                {
                    returnValue = true;
                    break;
                }
            }

            return returnValue;
        }
    }
}

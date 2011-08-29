using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PayrollTracker.ModelClassLibrary.Domain;
using PayrollTracker.ModelClassLibrary.Repositories;

namespace PayrollTracker
{
    public class Session
    {
        public Session(User user, Company company, DateTime payrollStartDate, DateTime payrollEndDate)
        {
            this.currentUser = user;
            this.currentCompany = company;
            this.currentPayrollStartDate = payrollStartDate;
            this.currentPayrollEndDate = payrollEndDate;
        }

        public ICollection<Role> GetRoles()
        {
            return currentUser.AssignedRoles;
        }

        public User GetUser()
        {
            return currentUser;
        }

        public Company GetCompany()
        {
            return currentCompany;
        }

        public DateTime GetPayrollStartDate()
        {
            return currentPayrollStartDate;
        }

        public DateTime GetPayrollEndDate()
        {
            return currentPayrollEndDate;
        }

        public override string ToString()
        {
            return "Session for User " + currentUser.Username + ".\nCompany is " + currentCompany.Name + ".\nPayroll Start Date: " + currentPayrollStartDate.ToString() + ".\nPayroll End Date: " + currentPayrollEndDate + ".";
        }

        private User currentUser;
        private Company currentCompany;
        private DateTime currentPayrollStartDate;
        private DateTime currentPayrollEndDate;
    }
}

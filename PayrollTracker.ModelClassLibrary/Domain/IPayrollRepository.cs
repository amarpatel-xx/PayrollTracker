using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface IPayrollRepository
    {
        void Add(Payroll payroll);
        void Update(Payroll payroll);
        void Remove(Payroll payroll);
        Payroll GetById(string payrollId);
        Payroll GetByCompany(Company company);
    }
}

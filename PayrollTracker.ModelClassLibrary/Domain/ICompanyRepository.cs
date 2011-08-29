using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface ICompanyRepository
    {
        void Add(Company company);
        void Update(Company company);
        void Remove(Company company);
        Company GetById(string companyId);
        Company GetByCompanyName(string companyName);
    }
}

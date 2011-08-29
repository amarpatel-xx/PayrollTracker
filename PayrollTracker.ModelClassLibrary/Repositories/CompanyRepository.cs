using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        public void Add(Company company)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(company);
                transaction.Commit();
            }
        }

        public void Update(Company company)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(company);
                transaction.Commit();
            }
        }

        public void Remove(Company company)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(company);
                transaction.Commit();
            }
        }

        public Company GetById(string companyId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<Company>(companyId);
        }

        public Company GetByCompanyName(string companyName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var companies = session
                   .CreateCriteria(typeof(Company))
                   .Add(Restrictions.Eq("Name", companyName))
                   .List<Company>();
                Company returnValue = null;
                if (companies.Count > 0)
                {
                    returnValue = companies[0];
                }

                return returnValue;
            }
        }
    }
}

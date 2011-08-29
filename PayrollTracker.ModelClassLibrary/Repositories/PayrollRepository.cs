using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        public void Add(Payroll payroll)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(payroll);
                transaction.Commit();
            }
        }

        public void Update(Payroll payroll)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(payroll);
                transaction.Commit();
            }
        }

        public void Remove(Payroll payroll)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(payroll);
                transaction.Commit();
            }
        }

        public Payroll GetById(string payrollId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<Payroll>(payrollId);
        }

        public Payroll GetByCompany(Company company)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var payrolls = session
                .CreateCriteria(typeof(Payroll))
                .Add(Restrictions.Eq("Company", company))
                .List<Payroll>();

                Payroll returnValue = null;
                if (payrolls.Count > 0)
                {
                    returnValue = payrolls[0];
                }

                return returnValue;
            }
        }
    }
}

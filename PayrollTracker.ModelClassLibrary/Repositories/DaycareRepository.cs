using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class DaycareRepository : IDaycareRepository
    {
        public void Add(Daycare daycare)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(daycare);
                transaction.Commit();
            }
        }

        public void Update(Daycare daycare)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(daycare);
                transaction.Commit();
            }
        }

        public void Remove(Daycare daycare)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(daycare);
                transaction.Commit();
            }
        }

        public Daycare GetById(string daycareId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<Daycare>(daycareId);
        }

        public ICollection<Daycare> GetByUser(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var daycares = session
                .CreateCriteria(typeof(Daycare))
                .Add(Restrictions.Eq("User", user))
                .List<Daycare>();
                return daycares;
            }
        }

        public IList<Daycare> GetRecentDaycares(User user, DateTime payrollStartTime, DateTime payrollEndTime)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery query = session.GetNamedQuery("Daycare.by.user.and.within.payroll.period");
                query.SetString(0, user.UserId);
                query.SetTimestamp(1, payrollStartTime);
                query.SetTimestamp(2, payrollEndTime);
                IList<Daycare> daycares = query.List<Daycare>();
         
                return daycares;
            }
        }
    }
}

using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class BoardingRepository : IBoardingRepository
    {
        public void Add(Boarding boarding)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(boarding);
                transaction.Commit();
            }
        }

        public void Update(Boarding boarding)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(boarding);
                transaction.Commit();
            }
        }

        public void Remove(Boarding boarding)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(boarding);
                transaction.Commit();
            }
        }

        public Boarding GetById(string boardingId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<Boarding>(boardingId);
        }

        public ICollection<Boarding> GetByUser(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var boardings = session
                .CreateCriteria(typeof(Boarding))
                .Add(Restrictions.Eq("User", user))
                .List<Boarding>();
                return boardings;
            }
        }
        
        public IList<Boarding> GetRecentBoardings(User user, DateTime payrollStartTime, DateTime payrollEndTime)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery q = session.GetNamedQuery("Boarding.by.user.and.within.payroll.period");
                q.SetString(0, user.UserId);
                q.SetTimestamp(1, payrollStartTime);
                q.SetTimestamp(2, payrollEndTime);
                IList<Boarding> boardings = q.List<Boarding>();
                return boardings;
            }
        }
    }
}

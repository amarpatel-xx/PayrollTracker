using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class GroomingRepository : IGroomingRepository
    {
        public void Add(Grooming grooming)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(grooming);
                transaction.Commit();
            }
        }

        public void Update(Grooming grooming)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(grooming);
                transaction.Commit();
            }
        }

        public void Remove(Grooming grooming)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(grooming);
                transaction.Commit();
            }
        }

        public Grooming GetById(string groomingId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<Grooming>(groomingId);
        }

        public ICollection<Grooming> GetByUser(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var groomings = session
                .CreateCriteria(typeof(Grooming))
                .Add(Restrictions.Eq("User", user))
                .List<Grooming>();
                return groomings;
            }
        }

        public IList<Grooming> GetRecentGroomings(User user, DateTime payrollStartTime, DateTime payrollEndTime)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery query = session.GetNamedQuery("Grooming.by.user.and.within.payroll.period");
                query.SetString(0, user.UserId);
                query.SetTimestamp(1, payrollStartTime);
                query.SetTimestamp(2, payrollEndTime);
                IList<Grooming> groomings = query.List<Grooming>();

                return groomings;
            }
        }
    }
}

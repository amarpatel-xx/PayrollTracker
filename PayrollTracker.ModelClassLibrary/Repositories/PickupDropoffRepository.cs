using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class PickupDropoffRepository : IPickupDropoffRepository
    {
        public void Add(PickupDropoff pickupDropoff)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(pickupDropoff);
                transaction.Commit();
            }
        }

        public void Update(PickupDropoff pickupDropoff)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(pickupDropoff);
                transaction.Commit();
            }
        }

        public void Remove(PickupDropoff pickupDropoff)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(pickupDropoff);
                transaction.Commit();
            }
        }

        public PickupDropoff GetById(string pickupDropoffId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<PickupDropoff>(pickupDropoffId);
        }

        public ICollection<PickupDropoff> GetByUser(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var pickupDropoffs = session
                .CreateCriteria(typeof(PickupDropoff))
                .Add(Restrictions.Eq("User", user))
                .List<PickupDropoff>();
                return pickupDropoffs;
            }
        }

        public IList<PickupDropoff> GetRecentPickupsAndDropoffs(User user, DateTime payrollStartTime, DateTime payrollEndTime)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery query = session.GetNamedQuery("PickupDropoff.by.user.and.within.payroll.period");
                query.SetString(0, user.UserId);
                query.SetTimestamp(1, payrollStartTime);
                query.SetTimestamp(2, payrollEndTime);
                IList<PickupDropoff> pickupsAndDropoffs = query.List<PickupDropoff>();

                return pickupsAndDropoffs;
            }
        }
    }
}

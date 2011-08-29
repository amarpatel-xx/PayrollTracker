using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class TimeCardRepository : ITimeCardRepository
    {
        public void Add(TimeCard timeCard)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(timeCard);
                transaction.Commit();
            }
        }

        public void Update(TimeCard timeCard)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(timeCard);
                transaction.Commit();
            }
        }

        public void Remove(TimeCard timeCard)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(timeCard);
                transaction.Commit();
            }
        }

        public TimeCard GetById(string timeCardId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<TimeCard>(timeCardId);
        }

        public ICollection<TimeCard> GetByUser(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var timeCards = session
                .CreateCriteria(typeof(TimeCard))
                .Add(Restrictions.Eq("User", user))
                .List<TimeCard>();
                return timeCards;
            }
        }

        public TimeCard GetMostRecentTimeIn(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                DetachedCriteria maxQuery = DetachedCriteria.For(typeof(TimeCard));
                maxQuery.SetProjection(Projections.Max("TimeIn"));

                IList<TimeCard> timeCard = session.CreateCriteria(typeof(TimeCard))
                    .Add(Restrictions.Eq("User", user))
                    .Add(Property.ForName("TimeIn").Eq(maxQuery)).List<TimeCard>();
                return timeCard[0];
            }
        }

        public TimeCard GetMostRecentTimeOut(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                DetachedCriteria maxQuery = DetachedCriteria.For(typeof(TimeCard));
                maxQuery.SetProjection(Projections.Max("TimeOut"));

                IList<TimeCard> timeCard = session.CreateCriteria(typeof(TimeCard))
                    .Add(Restrictions.Eq("User", user))
                    .Add(Property.ForName("TimeOut").Eq(maxQuery)).List<TimeCard>();
                return timeCard[0];
            }
        }

        public TimeCard GetMostRecentTimeIn(User user, DateTime payrollStartTime, DateTime payrollEndTime)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery q = session.GetNamedQuery("TimeCard.latest.by.user.and.within.payroll.period");
                q.SetString(0, user.UserId);
                q.SetTimestamp(1, payrollStartTime);
                q.SetTimestamp(2, payrollEndTime);
                System.Collections.IList timeCards = q.List();
                TimeCard timeCard = null;
                if (timeCards.Count > 0)
                {
                    timeCard = (TimeCard)timeCards[0];
                }
                return timeCard;
            }
        }

        public IList<TimeCard> GetCurrentTimeCards(User user, DateTime payrollStartTime, DateTime payrollEndTime)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery q = session.GetNamedQuery("TimeCards.by.user.and.within.payroll.period");
                q.SetString(0, user.UserId);
                q.SetTimestamp(1, payrollStartTime);
                q.SetTimestamp(2, payrollEndTime);
                IList<TimeCard> timeCards = q.List<TimeCard>();
                return timeCards;
            }
        }
    }
}

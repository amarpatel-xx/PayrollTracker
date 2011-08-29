using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        public void Add(Training training)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(training);
                transaction.Commit();
            }
        }

        public void Update(Training training)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(training);
                transaction.Commit();
            }
        }

        public void Remove(Training training)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(training);
                transaction.Commit();
            }
        }

        public Training GetById(string trainingId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<Training>(trainingId);
        }

        public ICollection<Training> GetByUser(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var trainings = session
                .CreateCriteria(typeof(Training))
                .Add(Restrictions.Eq("User", user))
                .List<Training>();
                return trainings;
            }
        }

        public IList<Training> GetRecentTrainings(User user, DateTime payrollStartTime, DateTime payrollEndTime)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery query = session.GetNamedQuery("Training.by.user.and.within.payroll.period");
                query.SetString(0, user.UserId);
                query.SetTimestamp(1, payrollStartTime);
                query.SetTimestamp(2, payrollEndTime);
                IList<Training> trainings = query.List<Training>();

                return trainings;
            }
        }
    }
}

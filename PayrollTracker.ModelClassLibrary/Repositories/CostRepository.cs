using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class CostRepository : ICostRepository
    {
        public void Add(Cost cost)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(cost);
                transaction.Commit();
            }
        }

        public void Update(Cost cost)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(cost);
                transaction.Commit();
            }
        }

        public void Remove(Cost cost)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(cost);
                transaction.Commit();
            }
        }

        public Cost GetById(string costId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<Cost>(costId);
        }
    }
}

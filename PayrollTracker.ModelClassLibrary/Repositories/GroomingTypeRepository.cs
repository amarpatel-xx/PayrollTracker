using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class GroomingTypeRepository : IGroomingTypeRepository
    {
        public void Add(GroomingType groomingType)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(groomingType);
                transaction.Commit();
            }
        }

        public void Update(GroomingType groomingType)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(groomingType);
                transaction.Commit();
            }
        }

        public void Remove(GroomingType groomingType)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(groomingType);
                transaction.Commit();
            }
        }

        public GroomingType GetById(string groomingTypeId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<GroomingType>(groomingTypeId);
        }

        public IList<GroomingType> GetAllGroomingTypes()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery query = session.GetNamedQuery("GroomingTypes.All");
                IList<GroomingType> groomingTypes = query.List<GroomingType>();
                return groomingTypes;
            }
        }
    }
}

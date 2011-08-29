using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class CostTypeRepository : ICostTypeRepository
    {
        public void Add(CostType costType)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(costType);
                transaction.Commit();
            }
        }

        public void Update(CostType costType)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(costType);
                transaction.Commit();
            }
        }

        public void Remove(CostType costType)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(costType);
                transaction.Commit();
            }
        }

        public CostType GetById(string costTypeId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<CostType>(costTypeId);
        }

        public CostType GetByCostName(string costName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var costTypes = session
                   .CreateCriteria(typeof(CostType))
                   .Add(Restrictions.Eq("CostName", costName))
                   .List<CostType>();
                CostType returnValue = null;
                if (costTypes.Count > 0)
                {
                    returnValue = costTypes[0];
                }

                return returnValue;
            }
        }

        public IList<CostType> GetAllSimilarCostTypes(string partialCostName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery query = session.GetNamedQuery("CostTypes.Like.Name");
                partialCostName = "%" + partialCostName + "%";
                query.SetString(0, partialCostName);
                IList<CostType> costTypes = query.List<CostType>();

                foreach (CostType costType in costTypes)
                {
                    string costName = costType.CostName;
                    int lastIndexOfDash = costName.LastIndexOf('-');
                    costType.CostName = costName.Substring(lastIndexOfDash + 2);
                }

                return costTypes;
            }
        }
    }
}

using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public void Add(Role role)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(role);
                transaction.Commit();
            }
        }

        public void Update(Role role)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(role);
                transaction.Commit();
            }
        }

        public void Remove(Role role)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(role);
                transaction.Commit();
            }
        }

        public Role GetById(string roleId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<Role>(roleId);
        }

        public IList<Role> GetAllRoles()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery query = session.GetNamedQuery("Roles.within.system");
                IList<Role> roles = query.List<Role>();

                return roles;
            }
        }
    }
}

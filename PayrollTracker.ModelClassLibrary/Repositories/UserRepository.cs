using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class UserRepository : IUserRepository
    {
        public void Add(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }
        }

        public void Update(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(user);
                transaction.Commit();
            }
        }

        public void Remove(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(user);
                transaction.Commit();
            }
        }

        public User GetById(string userId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<User>(userId);
        }

        public User GetByUsername(string username)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var users = session
                   .CreateCriteria(typeof(User))
                   .Add(Restrictions.Eq("Username", username))
                   .List<User>();
                User returnValue = null;
                if (users.Count > 0)
                {
                    returnValue = users[0];
                }

                return returnValue;
            }
        }

        public ICollection<User> GetByFirstName(string firstName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var users = session
                .CreateCriteria(typeof(User))
                .Add(Restrictions.Eq("FirstName", firstName))
                .List<User>();
                return users;
            }
        }

        public ICollection<User> GetByLastName(string lastName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var users = session
                .CreateCriteria(typeof(User))
                .Add(Restrictions.Eq("LastName", lastName))
                .List<User>();
                return users;
            }
        }
    }
}

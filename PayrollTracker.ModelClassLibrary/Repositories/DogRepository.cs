using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.ModelClassLibrary.Repositories
{
    public class DogRepository : IDogRepository
    {
        public void Add(Dog dog)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(dog);
                transaction.Commit();
            }
        }

        public void Update(Dog dog)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(dog);
                transaction.Commit();
            }
        }

        public void Remove(Dog dog)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(dog);
                transaction.Commit();
            }
        }

        public Dog GetById(string dogId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<Dog>(dogId);
        }

        public ICollection<Dog> GetByFirstName(string firstName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var dogs = session
                .CreateCriteria(typeof(Dog))
                .Add(Restrictions.Eq("FirstName", firstName))
                .List<Dog>();
                return dogs;
            }
        }

        public ICollection<Dog> GetByLastName(string lastName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var dogs = session
                .CreateCriteria(typeof(Dog))
                .Add(Restrictions.Eq("LastName", lastName))
                .List<Dog>();
                return dogs;
            }
        }

        public IList<Dog> GetAllDogs()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery query = session.GetNamedQuery("Dogs.within.system");
                IList<Dog> dogs = query.List<Dog>(); 
                return dogs;
            }
        }
    }
}

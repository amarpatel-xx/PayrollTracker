using System;
using System.Collections.Generic;

using NUnit.Framework;

using NHibernate;
using NHibernate.Cfg;

using PayrollTracker.ModelClassLibrary.Domain;
using PayrollTracker.ModelClassLibrary.Repositories;

namespace PayrollTracker.Tests
{
    [TestFixture]
    public class DogTest
    {
        private ISessionFactory _sessionFactory;
        private Configuration _configuration;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _sessionFactory = _configuration.BuildSessionFactory();

            CreateInitialData();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            DeleteInitialData();
        }

        private readonly Dog[] _dogs = new[]
                 {
                     new Dog {FirstName = "Wyatt", LastName = "Patel"},
                     new Dog {FirstName = "Dax", LastName = "Patel"},
                     new Dog {FirstName = "Tag", LastName = "Patel"},
                     new Dog {FirstName = "Trevor", LastName = "Patel"},
                     new Dog {FirstName = "Rafiki", LastName = "Patel"}
                 };

        private void CreateInitialData()
        {

            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var dog in _dogs)
                {
                    session.Save(dog);
                }
                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            IDogRepository repository = new DogRepository();
            foreach (var dog in _dogs)
            {
                Dog fromDb = repository.GetById(dog.DogId);
                if (fromDb != null)
                {
                    repository.Remove(dog);
                }
            }
        }

        [Test]
        public void Can_add_multiple_new_dogs()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getDogTableSize(session);

            ITransaction tx = session.BeginTransaction();
            Dog dog1 = new Dog();
            dog1.FirstName = "Luis";
            dog1.LastName = "Patel";
            session.Save(dog1);

            Dog dog2 = new Dog();
            dog2.FirstName = "Clark";
            dog2.LastName = "Patel";
            session.Save(dog2);

            Dog dog3 = new Dog();
            dog3.FirstName = "Jason";
            dog3.LastName = "Patel";
            session.Save(dog3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getDogTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(dog1);
            session.Delete(dog2);
            session.Delete(dog3);

            tx.Commit();

            session.Close();
        }

        private int getDogTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from Dog").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_dog()
        {
            var dog = new Dog { FirstName = "Adam", LastName = "Patel" };
            IDogRepository repository = new DogRepository();
            repository.Add(dog);

            // use session to try to load the dog
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Dog>(dog.DogId);
                // Test that the dog was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(dog, fromDb);
                Assert.AreEqual(dog.FirstName, fromDb.FirstName);
                Assert.AreEqual(dog.LastName, fromDb.LastName);
            }

            repository.Remove(dog);
        }

        [Test]
        public void Can_update_existing_dog()
        {
            var dog = _dogs[1];
            dog.FirstName = "Mango";
            IDogRepository repository = new DogRepository();
            repository.Update(dog);

            // use session to try to load the dog
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Dog>(dog.DogId);
                Assert.AreEqual(dog.FirstName, fromDb.FirstName);
            }
        }

        [Test]
        public void Can_remove_existing_dog()
        {
            var dog = _dogs[0];
            IDogRepository repository = new DogRepository();
            repository.Remove(dog);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Dog>(dog.DogId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_dog_by_id()
        {
            IDogRepository repository = new DogRepository();
            var fromDb = repository.GetById(_dogs[1].DogId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_dogs[1], fromDb);
            Assert.AreEqual(_dogs[1].FirstName, fromDb.FirstName);
            Assert.AreEqual(_dogs[1].LastName, fromDb.LastName);
        }

        [Test]
        public void Can_get_existing_dogs_by_first_name()
        {
            IDogRepository repository = new DogRepository();
            var fromDb = repository.GetByFirstName(_dogs[1].FirstName);

            Assert.AreEqual(1, fromDb.Count);
            Assert.IsTrue(IsInCollection(_dogs[1], fromDb));
        }

        [Test]
        public void Can_get_existing_dogs_by_last_name()
        {
            IDogRepository repository = new DogRepository();
            var fromDb = repository.GetByLastName(_dogs[1].LastName);

            Assert.AreEqual(5, fromDb.Count);
            Assert.IsTrue(IsInCollection(_dogs[1], fromDb));
        }

        private bool IsInCollection(Dog dog, ICollection<Dog> fromDb)
        {
            bool result = false;

            foreach (var item in fromDb)
            {
                if (dog.DogId == item.DogId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}

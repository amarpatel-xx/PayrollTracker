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
    public class GroomingTest
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
        
        private User _user1 = new User("teena", "teena123", "Teena", "Patel", DateTime.Now);
        private User _user2 = new User("rhyna", "rhyna123", "Rhyna", "Crews", DateTime.Now);
        private Dog _dog1 = new Dog {FirstName = "Wyatt", LastName = "Patel"};
        private Dog _dog2 = new Dog { FirstName = "Dax", LastName = "Patel" };
        private GroomingType _groomingType1 = new GroomingType ("Bath");
        private GroomingType _groomingType2 = new GroomingType("Nose and Ears");

        private Grooming[] _groomings;

        private void CreateInitialData()
        {
            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                // Save the user to populate its UserId.
                session.Save(_user1);
                session.Save(_user2);

                session.Save(_dog1);
                session.Save(_dog2);

                session.Save(_groomingType1);
                session.Save(_groomingType2);

                _groomings = new Grooming [4];
                _groomings[0] = new Grooming (_groomingType1, 45.50, DateTime.Now, _dog1, _user1);
                _groomings[1] = new Grooming (_groomingType1, 35.50, DateTime.Now, _dog1, _user2);
                _groomings[2] = new Grooming (_groomingType2, 25.50, DateTime.Now, _dog2, _user1);
                _groomings[3] = new Grooming (_groomingType2, 15.50, DateTime.Now, _dog2, _user2);

                foreach (var grooming in _groomings)
                {
                    // Get rid of milliseconds to make comparisons work, since MySQL
                    // DateTime does not support milliseconds.
                    grooming.Date = DateTime.ParseExact(grooming.Date.ToString(), "M/d/yyyy h:mm:ss tt", null);
                    session.Save(grooming);
                }

                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            IGroomingRepository groomingRepository = new GroomingRepository();
            foreach (var grooming in _groomings)
            {
                Grooming fromDb = groomingRepository.GetById(grooming.GroomingId);
                if (fromDb != null)
                {
                    groomingRepository.Remove(grooming);
                }
            }

            IUserRepository userRepository = new UserRepository();
            userRepository.Remove(_user1);
            userRepository.Remove(_user2);

            IDogRepository dogRepository = new DogRepository();
            dogRepository.Remove(_dog1);
            dogRepository.Remove(_dog2);

            IGroomingTypeRepository groomingTypeRepository = new GroomingTypeRepository();
            groomingTypeRepository.Remove(_groomingType1);
            groomingTypeRepository.Remove(_groomingType2);
        }

        [Test]
        public void Can_add_multiple_new_groomings()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getGroomingTableSize(session);

            ITransaction tx = session.BeginTransaction();
            Grooming grooming1 = new Grooming();
            grooming1.GroomingType = _groomingType1;
            grooming1.Cost = 99.99;
            grooming1.Date = DateTime.Now;
            grooming1.Dog = _dog1;
            grooming1.User = _user1;
            session.Save(grooming1);

            Grooming grooming2 = new Grooming();
            grooming2.GroomingType = _groomingType2;
            grooming2.Cost = 199.99;
            grooming2.Date = DateTime.Now;
            grooming2.Dog = _dog2;
            grooming2.User = _user2;
            session.Save(grooming2);

            Grooming grooming3 = new Grooming();
            grooming3.GroomingType = _groomingType1;
            grooming3.Cost = 22.16;
            grooming3.Date = DateTime.Now;
            grooming3.Dog = _dog1;
            grooming3.User = _user1;
            session.Save(grooming3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getGroomingTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(grooming1);
            session.Delete(grooming2);
            session.Delete(grooming3);

            tx.Commit();

            session.Close();
        }

        private int getGroomingTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from Grooming").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_grooming()
        {
            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            var grooming = new Grooming(_groomingType1, 245.50, date, _dog1, _user1);
            IGroomingRepository repository = new GroomingRepository();
            repository.Add(grooming);

            // use session to try to load the grooming
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Grooming>(grooming.GroomingId);
                // Test that the grooming was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(grooming, fromDb);
                Assert.AreEqual(grooming.GroomingType, fromDb.GroomingType);
                Assert.AreEqual(grooming.Cost, fromDb.Cost);
                Assert.AreEqual(grooming.Date, fromDb.Date);
                Assert.AreEqual(grooming.Dog, fromDb.Dog);
                Assert.AreEqual(grooming.User, fromDb.User);
            }

            repository.Remove(grooming);
        }

        [Test]
        public void Can_update_existing_grooming()
        {
            var grooming = _groomings[1];
            grooming.Tip = 30.00;
            IGroomingRepository repository = new GroomingRepository();
            repository.Update(grooming);

            // use session to try to load the grooming
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Grooming>(grooming.GroomingId);
                Assert.AreEqual(grooming.Tip, fromDb.Tip);
            }
        }

        [Test]
        public void Can_remove_existing_grooming()
        {
            var grooming = _groomings[0];
            IGroomingRepository repository = new GroomingRepository();
            repository.Remove(grooming);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Grooming>(grooming.GroomingId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_grooming_by_id()
        {
            IGroomingRepository repository = new GroomingRepository();
            var fromDb = repository.GetById(_groomings[1].GroomingId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_groomings[1], fromDb);
            Assert.AreEqual(_groomings[1].GroomingType, fromDb.GroomingType);
            Assert.AreEqual(_groomings[1].Cost, fromDb.Cost);
            Assert.AreEqual(_groomings[1].Date, fromDb.Date);
            Assert.AreEqual(_groomings[1].Dog, fromDb.Dog);
            Assert.AreEqual(_groomings[1].User, fromDb.User);
        }

        [Test]
        public void Can_get_existing_groomings_by_user_id()
        {
            IGroomingRepository repository = new GroomingRepository();
            var fromDb = repository.GetByUser(_groomings[1].User);

            Assert.AreEqual(2, fromDb.Count);
            Assert.IsTrue(IsInCollection(_groomings[1], fromDb));
        }

        private bool IsInCollection(Grooming grooming, ICollection<Grooming> fromDb)
        {
            bool result = false;

            foreach (var item in fromDb)
            {
                if (grooming.GroomingId == item.GroomingId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}

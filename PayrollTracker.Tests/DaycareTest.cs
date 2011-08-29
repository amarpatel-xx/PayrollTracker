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
    public class DaycareTest
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
        private Cost _daycareCost = new Cost(50.99);

        private Daycare[] _daycares;

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

                session.Save(_daycareCost);

                // Get rid of milliseconds to make comparisons work, since MySQL
                // DateTime does not support milliseconds.
                DateTime date = DateTime.ParseExact(DateTime.Now.ToString(), "M/d/yyyy h:mm:ss tt", null);

                _daycares = new Daycare [4];
                _daycares[0] = new Daycare(date, _dog1, _daycareCost, _user1);
                _daycares[1] = new Daycare(date, _dog1, _daycareCost, _user2);
                _daycares[2] = new Daycare(date, _dog2, _daycareCost, _user1);
                _daycares[3] = new Daycare(date, _dog2, _daycareCost, _user2);

                foreach (var daycare in _daycares)
                {
                    session.Save(daycare);
                }

                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            IDaycareRepository daycareRepository = new DaycareRepository();
            foreach (var daycare in _daycares)
            {
                Daycare fromDb = daycareRepository.GetById(daycare.DaycareId);
                if (fromDb != null)
                {
                    daycareRepository.Remove(daycare);
                }
            }

            IUserRepository userRepository = new UserRepository();
            userRepository.Remove(_user1);
            userRepository.Remove(_user2);

            IDogRepository dogRepository = new DogRepository();
            dogRepository.Remove(_dog1);
            dogRepository.Remove(_dog2);

            ICostRepository costRepository = new CostRepository();
            costRepository.Remove(_daycareCost);
        }

        [Test]
        public void Can_add_multiple_new_daycares()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getDaycareTableSize(session);

            ITransaction tx = session.BeginTransaction();
            Daycare daycare1 = new Daycare();
            daycare1.Date = DateTime.Now;
            daycare1.Dog = _dog1;
            daycare1.DaycareCost = _daycareCost;
            daycare1.User = _user1;
            session.Save(daycare1);

            Daycare daycare2 = new Daycare();
            daycare2.Date = DateTime.Now;
            daycare2.Dog = _dog2;
            daycare2.DaycareCost = _daycareCost;
            daycare2.User = _user2;
            session.Save(daycare2);

            Daycare daycare3 = new Daycare();
            daycare3.Date = DateTime.Now;
            daycare3.Dog = _dog1;
            daycare3.DaycareCost = _daycareCost;
            daycare3.User = _user1;
            session.Save(daycare3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getDaycareTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(daycare1);
            session.Delete(daycare2);
            session.Delete(daycare3);

            tx.Commit();

            session.Close();
        }

        private int getDaycareTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from Daycare").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_daycare()
        {
            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            var daycare = new Daycare(date, _dog1, _daycareCost, _user1);
            IDaycareRepository repository = new DaycareRepository();
            repository.Add(daycare);

            // use session to try to load the daycare
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Daycare>(daycare.DaycareId);
                // Test that the daycare was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(daycare, fromDb);
                Assert.AreEqual(daycare.Date, fromDb.Date);
                Assert.AreEqual(daycare.Dog, fromDb.Dog);
                Assert.AreEqual(daycare.DaycareCost, fromDb.DaycareCost);
                Assert.AreEqual(daycare.User, fromDb.User);
            }

            repository.Remove(daycare);
        }

        [Test]
        public void Can_update_existing_daycare()
        {
            var daycare = _daycares[1];
            daycare.User = _user1;
            IDaycareRepository repository = new DaycareRepository();
            repository.Update(daycare);

            // use session to try to load the daycare
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Daycare>(daycare.DaycareId);
                Assert.AreEqual(daycare.User, fromDb.User);
            }
        }

        [Test]
        public void Can_remove_existing_daycare()
        {
            var daycare = _daycares[0];
            IDaycareRepository repository = new DaycareRepository();
            repository.Remove(daycare);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Daycare>(daycare.DaycareId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_daycare_by_id()
        {
            IDaycareRepository repository = new DaycareRepository();
            var fromDb = repository.GetById(_daycares[1].DaycareId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_daycares[1], fromDb);
            Assert.AreEqual(_daycares[1].Date, fromDb.Date);
            Assert.AreEqual(_daycares[1].Dog, fromDb.Dog);
            Assert.AreEqual(_daycares[1].DaycareCost, fromDb.DaycareCost);
            Assert.AreEqual(_daycares[1].User, fromDb.User);
        }

        [Test]
        public void Can_get_existing_daycares_by_user_id()
        {
            IDaycareRepository repository = new DaycareRepository();
            var fromDb = repository.GetByUser(_daycares[1].User);

            Assert.AreEqual(2, fromDb.Count);
            Assert.IsTrue(IsInCollection(_daycares[1], fromDb));
        }

        private bool IsInCollection(Daycare daycare, ICollection<Daycare> fromDb)
        {
            bool result = false;

            foreach (var item in fromDb)
            {
                if (daycare.DaycareId == item.DaycareId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}

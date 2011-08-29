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
    public class BoardingTest
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
        private Cost _boardingCost = new Cost(20);
        private Cost _sundayDaycareCost = new Cost(30);

        private Boarding[] _boardings;

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

                session.Save(_boardingCost);
                session.Save(_sundayDaycareCost);

                // Get rid of milliseconds to make comparisons work, since MySQL
                // DateTime does not support milliseconds.
                DateTime date = DateTime.ParseExact(DateTime.Now.ToString(), "M/d/yyyy h:mm:ss tt", null);

                _boardings = new Boarding [4];
                _boardings[0] = new Boarding(date, true, _dog1, _boardingCost, _user1);
                _boardings[1] = new Boarding(date, true, _dog1, _boardingCost, _user2);
                _boardings[2] = new Boarding(date, false, _dog2, _boardingCost, _user1);
                _boardings[3] = new Boarding(date, false, _dog2, _boardingCost, _user2);

                foreach (var boarding in _boardings)
                {
                    session.Save(boarding);
                }

                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            IBoardingRepository boardingRepository = new BoardingRepository();
            foreach (var boarding in _boardings)
            {
                Boarding fromDb = boardingRepository.GetById(boarding.BoardingId);
                if (fromDb != null)
                {
                    boardingRepository.Remove(boarding);
                }
            }

            IUserRepository userRepository = new UserRepository();
            userRepository.Remove(_user1);
            userRepository.Remove(_user2);

            IDogRepository dogRepository = new DogRepository();
            dogRepository.Remove(_dog1);
            dogRepository.Remove(_dog2);

            ICostRepository costRepository = new CostRepository();
            costRepository.Remove(_boardingCost);
            costRepository.Remove(_sundayDaycareCost);
        }

        [Test]
        public void Can_add_multiple_new_boardings()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getBoardingTableSize(session);

            ITransaction tx = session.BeginTransaction();
            Boarding boarding1 = new Boarding();
            boarding1.Date = DateTime.Now;
            boarding1.IsDaycare = true;
            boarding1.Dog = _dog1;
            boarding1.BoardingCost = _boardingCost;
            boarding1.SundayDaycareCost = _sundayDaycareCost;
            boarding1.User = _user1;
            boarding1.Tip = 10.0;
            session.Save(boarding1);

            Boarding boarding2 = new Boarding();
            boarding2.Date = DateTime.Now;
            boarding2.IsDaycare = true;
            boarding2.Dog = _dog2;
            boarding2.BoardingCost = _boardingCost;
            boarding2.SundayDaycareCost = _sundayDaycareCost;
            boarding2.User = _user2;
            session.Save(boarding2);

            Boarding boarding3 = new Boarding();
            boarding3.Date = DateTime.Now;
            boarding3.IsDaycare = true;
            boarding3.Dog = _dog1;
            boarding3.BoardingCost = _boardingCost;
            boarding3.SundayDaycareCost = _sundayDaycareCost;
            boarding3.User = _user1;
            session.Save(boarding3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getBoardingTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(boarding1);
            session.Delete(boarding2);
            session.Delete(boarding3);

            tx.Commit();

            session.Close();
        }

        private int getBoardingTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from Boarding").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_boarding()
        {
            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            var boarding = new Boarding(date, true, _dog1, _boardingCost, _user1);
            IBoardingRepository repository = new BoardingRepository();
            repository.Add(boarding);

            // use session to try to load the boarding
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Boarding>(boarding.BoardingId);
                // Test that the boarding was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(boarding, fromDb);
                Assert.AreEqual(boarding.Date, fromDb.Date);
                Assert.AreEqual(boarding.IsDaycare, fromDb.IsDaycare);
                Assert.AreEqual(boarding.Dog, fromDb.Dog);
                Assert.AreEqual(boarding.BoardingCost, fromDb.BoardingCost);
                Assert.AreEqual(boarding.SundayDaycareCost, fromDb.SundayDaycareCost);
                Assert.AreEqual(boarding.User, fromDb.User);
            }

            repository.Remove(boarding);
        }

        [Test]
        public void Can_update_existing_boarding()
        {
            var boarding = _boardings[1];
            boarding.Tip = 30.00;
            IBoardingRepository repository = new BoardingRepository();
            repository.Update(boarding);

            // use session to try to load the boarding
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Boarding>(boarding.BoardingId);
                Assert.AreEqual(boarding.Tip, fromDb.Tip);
            }
        }

        [Test]
        public void Can_remove_existing_boarding()
        {
            var boarding = _boardings[0];
            IBoardingRepository repository = new BoardingRepository();
            repository.Remove(boarding);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Boarding>(boarding.BoardingId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_boarding_by_id()
        {
            IBoardingRepository repository = new BoardingRepository();
            var fromDb = repository.GetById(_boardings[1].BoardingId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_boardings[1], fromDb);
            Assert.AreEqual(_boardings[1].Date, fromDb.Date);
            Assert.AreEqual(_boardings[1].IsDaycare, fromDb.IsDaycare);
            Assert.AreEqual(_boardings[1].Dog, fromDb.Dog);
            Assert.AreEqual(_boardings[1].BoardingCost, fromDb.BoardingCost);
            Assert.AreEqual(_boardings[1].SundayDaycareCost, fromDb.SundayDaycareCost);
            Assert.AreEqual(_boardings[1].User, fromDb.User);
        }

        [Test]
        public void Can_get_existing_boardings_by_user_id()
        {
            IBoardingRepository repository = new BoardingRepository();
            var fromDb = repository.GetByUser(_boardings[1].User);

            Assert.AreEqual(2, fromDb.Count);
            Assert.IsTrue(IsInCollection(_boardings[1], fromDb));
        }

        private bool IsInCollection(Boarding boarding, ICollection<Boarding> fromDb)
        {
            bool result = false;

            foreach (var item in fromDb)
            {
                if (boarding.BoardingId == item.BoardingId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}

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
    public class TimeCardTest
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

        private User _user = new User("teena", "teena123", "Teena", "Patel", DateTime.Now);

        private TimeCard[] _timeCards = new[]
                 {
                     new TimeCard {TimeIn = DateTime.Now, TimeOut = DateTime.Now.AddHours(4)},
                     new TimeCard {TimeIn = DateTime.Now.AddHours(5), TimeOut = DateTime.Now.AddHours(9)},
                     new TimeCard {TimeIn = DateTime.Now.AddDays(1), TimeOut = DateTime.Now.AddHours(8)},
                     new TimeCard {TimeIn = DateTime.Now.AddDays(2), TimeOut = DateTime.Now.AddHours(8)},
                     new TimeCard {TimeIn = DateTime.Now.AddDays(3), TimeOut = DateTime.Now.AddHours(8)}
                 };

        private void CreateInitialData()
        {
            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                // Save the user to populate its UserId.
                session.Save(_user);

                foreach (var timeCard in _timeCards)
                {
                    // Assign all time cards to one user named Teena.
                    timeCard.User = _user;
                    // Get rid of milliseconds to make comparisons work, since MySQL
                    // DateTime does not support milliseconds.
                    timeCard.TimeIn = DateTime.ParseExact(timeCard.TimeIn.ToString(), "M/d/yyyy h:mm:ss tt", null);
                    timeCard.TimeOut = DateTime.ParseExact(timeCard.TimeOut.ToString(), "M/d/yyyy h:mm:ss tt", null);

                    session.Save(timeCard);
                }

                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            ITimeCardRepository timeCardRepository = new TimeCardRepository();
            foreach (var timeCard in _timeCards)
            {
                TimeCard fromDb = timeCardRepository.GetById(timeCard.TimeCardId);
                if (fromDb != null)
                {
                    timeCardRepository.Remove(timeCard);
                }
            }

            IUserRepository userRepository = new UserRepository();
            userRepository.Remove(_user);
        }

        [Test]
        public void Can_add_multiple_new_timeCards()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getTimeCardTableSize(session);

            ITransaction tx = session.BeginTransaction();
            TimeCard timeCard1 = new TimeCard();
            timeCard1.TimeIn = DateTime.Now;
            timeCard1.TimeOut = timeCard1.TimeIn.AddHours(8);
            timeCard1.User = _user;
            session.Save(timeCard1);

            TimeCard timeCard2 = new TimeCard();
            timeCard2.TimeIn = DateTime.Now.AddDays(1);
            timeCard2.TimeOut = timeCard2.TimeIn.AddHours(8);
            timeCard2.User = _user; 
            session.Save(timeCard2);

            TimeCard timeCard3 = new TimeCard();
            timeCard3.TimeIn = DateTime.Now.AddDays(2);
            timeCard3.TimeOut = timeCard3.TimeIn.AddHours(8);
            timeCard3.User = _user;
            session.Save(timeCard3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getTimeCardTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(timeCard1);
            session.Delete(timeCard2);
            session.Delete(timeCard3);

            tx.Commit();

            session.Close();
        }

        private int getTimeCardTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from TimeCard").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_timeCard()
        {
            DateTime timeIn = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            timeIn = DateTime.ParseExact(timeIn.ToString(), "M/d/yyyy h:mm:ss tt", null);
            DateTime timeOut = timeIn.AddHours(8);
            var timeCard = new TimeCard { TimeIn = timeIn, TimeOut = timeOut, User = _user };
            ITimeCardRepository repository = new TimeCardRepository();
            repository.Add(timeCard);

            // use session to try to load the timeCard
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<TimeCard>(timeCard.TimeCardId);
                // Test that the timeCard was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(timeCard, fromDb);
                Assert.AreEqual(timeCard.TimeIn, fromDb.TimeIn);
                Assert.AreEqual(timeCard.TimeOut, fromDb.TimeOut);
                Assert.AreEqual(timeCard.User, fromDb.User);
            }

            repository.Remove(timeCard);
        }

        [Test]
        public void Can_update_existing_timeCard()
        {
            var timeCard = _timeCards[1];
            timeCard.TimeOut = timeCard.TimeOut.AddHours(4);
            ITimeCardRepository repository = new TimeCardRepository();
            repository.Update(timeCard);

            // use session to try to load the timeCard
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<TimeCard>(timeCard.TimeCardId);
                Assert.AreEqual(timeCard.TimeOut, fromDb.TimeOut);
            }
        }

        [Test]
        public void Can_remove_existing_timeCard()
        {
            var timeCard = _timeCards[0];
            ITimeCardRepository repository = new TimeCardRepository();
            repository.Remove(timeCard);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<TimeCard>(timeCard.TimeCardId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_timeCard_by_id()
        {
            ITimeCardRepository repository = new TimeCardRepository();
            var fromDb = repository.GetById(_timeCards[1].TimeCardId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_timeCards[1], fromDb);
            Assert.AreEqual(_timeCards[1].TimeIn, fromDb.TimeIn);
            Assert.AreEqual(_timeCards[1].TimeOut, fromDb.TimeOut);
            Assert.AreEqual(_timeCards[1].User, fromDb.User);
        }

        [Test]
        public void Can_get_existing_timeCards_by_user()
        {
            ITimeCardRepository repository = new TimeCardRepository();
            var fromDb = repository.GetByUser(_timeCards[1].User);

            Assert.AreEqual(5, fromDb.Count);
            Assert.IsTrue(IsInCollection(_timeCards[1], fromDb));
        }

        private bool IsInCollection(TimeCard timeCard, ICollection<TimeCard> fromDb)
        {
            bool result = false;

            foreach (var item in fromDb)
            {
                if (timeCard.TimeCardId == item.TimeCardId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}

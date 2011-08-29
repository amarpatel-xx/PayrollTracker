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
    public class PickupDropoffTest
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
        private Cost _pickupCost = new Cost(5);
        private Cost _dropoffCost = new Cost(2);
        private Cost _newPickupCost = new Cost(10);

        private PickupDropoff[] _pickupDropoffs;

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

                session.Save(_pickupCost);
                session.Save(_dropoffCost);
                session.Save(_newPickupCost);

                // Get rid of milliseconds to make comparisons work, since MySQL
                // DateTime does not support milliseconds.
                DateTime date = DateTime.ParseExact(DateTime.Now.ToString(), "M/d/yyyy h:mm:ss tt", null);

                _pickupDropoffs = new PickupDropoff [4];
                _pickupDropoffs[0] = new PickupDropoff(date, _dog1, _user1);
                _pickupDropoffs[1] = new PickupDropoff(date, _dog1, _user2);
                _pickupDropoffs[2] = new PickupDropoff(date, _dog2, _user1);
                _pickupDropoffs[3] = new PickupDropoff(date, _dog2, _user2);

                foreach (var pickupDropoff in _pickupDropoffs)
                {
                    pickupDropoff.PickupCost = _pickupCost;
                    pickupDropoff.DropoffCost = _dropoffCost;

                    session.Save(pickupDropoff);
                }

                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            IPickupDropoffRepository pickupDropoffRepository = new PickupDropoffRepository();
            foreach (var pickupDropoff in _pickupDropoffs)
            {
                PickupDropoff fromDb = pickupDropoffRepository.GetById(pickupDropoff.PickupDropoffId);
                if (fromDb != null)
                {
                    pickupDropoffRepository.Remove(pickupDropoff);
                }
            }

            IUserRepository userRepository = new UserRepository();
            userRepository.Remove(_user1);
            userRepository.Remove(_user2);

            IDogRepository dogRepository = new DogRepository();
            dogRepository.Remove(_dog1);
            dogRepository.Remove(_dog2);

            ICostRepository costRepository = new CostRepository();
            costRepository.Remove(_pickupCost);
            costRepository.Remove(_dropoffCost);
            costRepository.Remove(_newPickupCost);
        }

        [Test]
        public void Can_add_multiple_new_pickupDropoffs()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getPickupDropoffTableSize(session);

            ITransaction tx = session.BeginTransaction();
            PickupDropoff pickupDropoff1 = new PickupDropoff();
            pickupDropoff1.Date = DateTime.Now;
            pickupDropoff1.Dog = _dog1;
            pickupDropoff1.User = _user1;
            session.Save(pickupDropoff1);

            PickupDropoff pickupDropoff2 = new PickupDropoff();
            pickupDropoff2.Date = DateTime.Now;
            pickupDropoff2.Dog = _dog2;
            pickupDropoff2.User = _user2;
            session.Save(pickupDropoff2);

            PickupDropoff pickupDropoff3 = new PickupDropoff();
            pickupDropoff3.Date = DateTime.Now;
            pickupDropoff3.Dog = _dog1;
            pickupDropoff3.User = _user1;
            session.Save(pickupDropoff3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getPickupDropoffTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(pickupDropoff1);
            session.Delete(pickupDropoff2);
            session.Delete(pickupDropoff3);

            tx.Commit();

            session.Close();
        }

        private int getPickupDropoffTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from PickupDropoff").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_pickupDropoff()
        {
            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            var pickupDropoff = new PickupDropoff(date, _dog1, _user1);
            IPickupDropoffRepository repository = new PickupDropoffRepository();
            repository.Add(pickupDropoff);

            // use session to try to load the pickupDropoff
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<PickupDropoff>(pickupDropoff.PickupDropoffId);
                // Test that the pickupDropoff was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(pickupDropoff, fromDb);
                Assert.AreEqual(pickupDropoff.Date, fromDb.Date);
                Assert.AreEqual(pickupDropoff.Dog, fromDb.Dog);
                Assert.AreEqual(pickupDropoff.User, fromDb.User);
            }

            repository.Remove(pickupDropoff);
        }

        [Test]
        public void Can_update_existing_pickupDropoff()
        {
            var pickupDropoff = _pickupDropoffs[1];
            pickupDropoff.PickupCost = _newPickupCost;

            IPickupDropoffRepository repository = new PickupDropoffRepository();
            repository.Update(pickupDropoff);

            // use session to try to load the pickupDropoff
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<PickupDropoff>(pickupDropoff.PickupDropoffId);
                Assert.AreEqual(pickupDropoff.PickupCost, fromDb.PickupCost);
            }
        }

        [Test]
        public void Can_remove_existing_pickupDropoff()
        {
            var pickupDropoff = _pickupDropoffs[0];
            IPickupDropoffRepository repository = new PickupDropoffRepository();
            repository.Remove(pickupDropoff);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<PickupDropoff>(pickupDropoff.PickupDropoffId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_pickupDropoff_by_id()
        {
            IPickupDropoffRepository repository = new PickupDropoffRepository();
            var fromDb = repository.GetById(_pickupDropoffs[1].PickupDropoffId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_pickupDropoffs[1], fromDb);
            Assert.AreEqual(_pickupDropoffs[1].Date, fromDb.Date);
            Assert.AreEqual(_pickupDropoffs[1].Dog, fromDb.Dog);
            Assert.AreEqual(_pickupDropoffs[1].PickupCost, fromDb.PickupCost);
            Assert.AreEqual(_pickupDropoffs[1].DropoffCost, fromDb.DropoffCost);
            Assert.AreEqual(_pickupDropoffs[1].User, fromDb.User);
        }

        [Test]
        public void Can_get_existing_pickupDropoffs_by_user_id()
        {
            IPickupDropoffRepository repository = new PickupDropoffRepository();
            var fromDb = repository.GetByUser(_pickupDropoffs[1].User);

            Assert.AreEqual(2, fromDb.Count);
            Assert.IsTrue(IsInCollection(_pickupDropoffs[1], fromDb));
        }

        private bool IsInCollection(PickupDropoff pickupDropoff, ICollection<PickupDropoff> fromDb)
        {
            bool result = false;

            foreach (var item in fromDb)
            {
                if (pickupDropoff.PickupDropoffId == item.PickupDropoffId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}

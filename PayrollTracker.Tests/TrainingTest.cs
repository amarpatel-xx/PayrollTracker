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
    public class TrainingTest
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
        private Cost _classCost = new Cost(500);
        private Cost _preK9DaycareCost = new Cost(40);
        private Cost _newClassCost = new Cost(600.50);
        private CostType _classType = new CostType("AA");

        private Training[] _trainings;

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

                session.Save(_classCost);
                session.Save(_preK9DaycareCost);
                session.Save(_newClassCost);

                session.Save(_classType);

                // Get rid of milliseconds to make comparisons work, since MySQL
                // DateTime does not support milliseconds.
                DateTime date = DateTime.ParseExact(DateTime.Now.ToString(), "M/d/yyyy h:mm:ss tt", null);
  
                _trainings = new Training [4];
                _trainings[0] = new Training(date, _classType, _classCost, _dog1, _user1);
                _trainings[1] = new Training(date, _classType, _classCost, _dog1, _user2);
                _trainings[2] = new Training(date, _classType, _classCost, _dog2, _user1);
                _trainings[3] = new Training(date, _classType, _classCost, _dog2, _user2);

                foreach (var training in _trainings)
                {
                    session.Save(training);
                }

                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            ITrainingRepository trainingRepository = new TrainingRepository();
            foreach (var training in _trainings)
            {
                Training fromDb = trainingRepository.GetById(training.TrainingId);
                if (fromDb != null)
                {
                    trainingRepository.Remove(training);
                }
            }

            IUserRepository userRepository = new UserRepository();
            userRepository.Remove(_user1);
            userRepository.Remove(_user2);

            IDogRepository dogRepository = new DogRepository();
            dogRepository.Remove(_dog1);
            dogRepository.Remove(_dog2);

            ICostRepository costRepository = new CostRepository();
            costRepository.Remove(_classCost);
            costRepository.Remove(_preK9DaycareCost);
            costRepository.Remove(_newClassCost);

            ICostTypeRepository costTypeRepository = new CostTypeRepository();
            costTypeRepository.Remove(_classType);
        }

        [Test]
        public void Can_add_multiple_new_trainings()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getTrainingTableSize(session);

            ITransaction tx = session.BeginTransaction();
            Training training1 = new Training();
            training1.Date = DateTime.Now;
            training1.ClassType = _classType;
            training1.ClassCost = _classCost;
            training1.Dog = _dog1;
            training1.User = _user1;
            session.Save(training1);

            Training training2 = new Training();
            training2.Date = DateTime.Now;
            training2.ClassType = _classType;
            training2.ClassCost = _classCost;
            training2.Dog = _dog2;
            training2.User = _user2;
            session.Save(training2);

            Training training3 = new Training();
            training3.Date = DateTime.Now;
            training3.ClassType = _classType;
            training3.ClassCost = _classCost;
            training3.Dog = _dog1;
            training3.User = _user1;
            session.Save(training3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getTrainingTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(training1);
            session.Delete(training2);
            session.Delete(training3);

            tx.Commit();

            session.Close();
        }

        private int getTrainingTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from Training").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_training()
        {
            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);
            
            var training = new Training(date, _classType, _classCost, _dog1, _user1);
            ITrainingRepository repository = new TrainingRepository();
            repository.Add(training);

            // use session to try to load the training
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Training>(training.TrainingId);
                // Test that the training was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(training, fromDb);
                Assert.AreEqual(training.Date, fromDb.Date);
                Assert.AreEqual(training.Dog, fromDb.Dog);
                Assert.AreEqual(training.User, fromDb.User);
                Assert.AreEqual(training.ClassType, fromDb.ClassType);
                Assert.AreEqual(training.ClassCost, fromDb.ClassCost);
                Assert.AreEqual(training.PreK9DaycareCost, fromDb.PreK9DaycareCost);
            }

            repository.Remove(training);
        }

        [Test]
        public void Can_update_existing_training()
        {
            var training = _trainings[1];
            training.ClassCost = _newClassCost;

            ITrainingRepository repository = new TrainingRepository();
            repository.Update(training);

            // use session to try to load the training
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Training>(training.TrainingId);
                Assert.AreEqual(training.ClassCost, fromDb.ClassCost);
            }
        }

        [Test]
        public void Can_remove_existing_training()
        {
            var training = _trainings[0];
            ITrainingRepository repository = new TrainingRepository();
            repository.Remove(training);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Training>(training.TrainingId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_training_by_id()
        {
            ITrainingRepository repository = new TrainingRepository();
            var fromDb = repository.GetById(_trainings[1].TrainingId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_trainings[1], fromDb);
            Assert.AreEqual(_trainings[1].Date, fromDb.Date);
            Assert.AreEqual(_trainings[1].Dog, fromDb.Dog);
            Assert.AreEqual(_trainings[1].User, fromDb.User);
            Assert.AreEqual(_trainings[1].ClassType, fromDb.ClassType);
            Assert.AreEqual(_trainings[1].ClassCost, fromDb.ClassCost);
            Assert.AreEqual(_trainings[1].PreK9DaycareCost, fromDb.PreK9DaycareCost);
        }

        [Test]
        public void Can_get_existing_trainings_by_user_id()
        {
            ITrainingRepository repository = new TrainingRepository();
            var fromDb = repository.GetByUser(_trainings[1].User);

            Assert.AreEqual(2, fromDb.Count);
            Assert.IsTrue(IsInCollection(_trainings[1], fromDb));
        }

        private bool IsInCollection(Training training, ICollection<Training> fromDb)
        {
            bool result = false;

            foreach (var item in fromDb)
            {
                if (training.TrainingId == item.TrainingId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}

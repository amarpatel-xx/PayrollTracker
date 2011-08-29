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
    public class UserTest
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

        private readonly User[] _users = new[]
                 {
                     new User ("teena", "teena123", "Teena", "Patel", DateTime.Now),
                     new User ("amar", "amar123", "Amar", "Patel", DateTime.Now),
                     new User ("jilna", "jilna123", "Jilna", "Patel", DateTime.Now)
                 };

        private void CreateInitialData()
        {

            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var user in _users)
                {
                    session.Save(user);
                }
                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            IUserRepository repository = new UserRepository();
            foreach (var user in _users)
            {
                User fromDb = repository.GetById(user.UserId);
                if (fromDb != null)
                {
                    repository.Remove(user);
                }
            }
        }

        [Test]
        public void Can_add_multiple_new_users()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getUserTableSize(session);

            ITransaction tx = session.BeginTransaction();
            User user1 = new User("hinal", "hinal123", "Hinal", "Patel", DateTime.Now);
            session.Save(user1);

            User user2 = new User("komal", "komal123", "Komal", "Patel", DateTime.Now);
            session.Save(user2);

            User user3 = new User("amani", "amani123", "Amani", "Patel", DateTime.Now);
            session.Save(user3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getUserTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(user1);
            session.Delete(user2);
            session.Delete(user3);

            tx.Commit();
            session.Close();
        }

        private int getUserTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from User").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_user()
        {
            var user = new User("Payal", "payal123", "Payal", "Patel", DateTime.Now); ;
            IUserRepository repository = new UserRepository();
            repository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            repository.Remove(user);
        }

        [Test]
        public void Can_update_existing_user()
        {
            var user = _users[1];
            user.FirstName = "Devina";
            IUserRepository repository = new UserRepository();
            repository.Update(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
            }
        }

        [Test]
        public void Can_remove_existing_user()
        {
            var user = _users[0];
            IUserRepository repository = new UserRepository();
            repository.Remove(user);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_user_by_id()
        {
            IUserRepository repository = new UserRepository();
            var fromDb = repository.GetById(_users[1].UserId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_users[1], fromDb);
            Assert.AreEqual(_users[1].FirstName, fromDb.FirstName);
            Assert.AreEqual(_users[1].LastName, fromDb.LastName);
        }

        [Test]
        public void Can_get_existing_user_by_username()
        {
            IUserRepository repository = new UserRepository();
            var fromDb = repository.GetByUsername(_users[2].Username);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_users[2], fromDb);
            Assert.AreEqual(_users[2].FirstName, fromDb.FirstName);
            Assert.AreEqual(_users[2].LastName, fromDb.LastName);
        }

        [Test]
        public void Can_get_existing_users_by_first_name()
        {
            IUserRepository repository = new UserRepository();
            var fromDb = repository.GetByFirstName(_users[1].FirstName);

            Assert.AreEqual(1, fromDb.Count);
            Assert.IsTrue(IsInCollection(_users[1], fromDb));
        }

        [Test]
        public void Can_get_existing_users_by_last_name()
        {
            IUserRepository repository = new UserRepository();
            var fromDb = repository.GetByLastName(_users[1].LastName);

            Assert.AreEqual(3, fromDb.Count);
            Assert.IsTrue(IsInCollection(_users[1], fromDb));
        }

        private bool IsInCollection(User user, ICollection<User> fromDb)
        {
            bool result = false;

            foreach (var item in fromDb)
            {
                if (user.UserId == item.UserId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        [Test]
        public void Can_add_new_user_with_new_role()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now);
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            var role = new Role("Training");
            IRoleRepository roleRepository = new RoleRepository();
            roleRepository.Add(role);

            IList<Role> assignedRoles = new List<Role>();
            assignedRoles.Add(role);
            user.AssignedRoles = assignedRoles;

            userRepository.Update(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
                Assert.AreEqual(assignedRoles.Count, fromDb.AssignedRoles.Count);
            }

            userRepository.Remove(user);
            roleRepository.Remove(role);
        }

        [Test]
        public void Can_add_new_user_with_new_time_cards()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }


            DateTime timeIn = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            timeIn = DateTime.ParseExact(timeIn.ToString(), "M/d/yyyy h:mm:ss tt", null);

            var timeCard1 = new TimeCard { TimeIn = timeIn, TimeOut = timeIn.AddHours(8), User = user};
            var timeCard2 = new TimeCard { TimeIn = timeIn.AddDays(1), TimeOut = timeIn.AddDays(1).AddHours(8), User = user };

            ITimeCardRepository timeCardRepository = new TimeCardRepository();
            timeCardRepository.Add(timeCard1);
            timeCardRepository.Add(timeCard2);

            User userFromDb = userRepository.GetById(user.UserId);
            ICollection<TimeCard> timeEntries = userFromDb.TimeEntries;

            Assert.AreEqual(2, timeEntries.Count);

            // Here we have to explicitly remove the user's time card entries, since
            // they were added using the time card repository and not the user's
            // TimeEntries property.
            timeCardRepository.Remove(timeCard1);
            timeCardRepository.Remove(timeCard2);
            userRepository.Remove(user);
        }

        [Test]
        public void Can_add_new_user_with_new_user_time_entries()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }


            DateTime timeIn = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            timeIn = DateTime.ParseExact(timeIn.ToString(), "M/d/yyyy h:mm:ss tt", null);

            var timeCard1 = new TimeCard { TimeIn = timeIn, TimeOut = timeIn.AddHours(8), User = user};
            var timeCard2 = new TimeCard { TimeIn = timeIn.AddDays(1), TimeOut = timeIn.AddDays(1).AddHours(8), User = user };

            IList<TimeCard> timeEntries = new List<TimeCard>();
            timeEntries.Add(timeCard1);
            timeEntries.Add(timeCard2);

            //Add time entries list via user object.
            user.TimeEntries = timeEntries;

            userRepository.Update(user);

            ITimeCardRepository timeCardRepository = new TimeCardRepository();
            ICollection<TimeCard> timeEntriesFromDb = timeCardRepository.GetByUser(user);

            Assert.AreEqual(2, timeEntriesFromDb.Count);

            //Delete user, will also get rid of user's time_card entries.
            userRepository.Remove(user);
        }

        [Test]
        public void Can_add_new_user_with_new_company()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            var company = new Company("University of Doglando");
            ICompanyRepository companyRepository = new CompanyRepository();
            companyRepository.Add(company);

            ICollection<Company> worksForCompanies = new HashSet<Company>();
            worksForCompanies.Add(company);
            user.WorksForCompanies = worksForCompanies;

            userRepository.Update(user);

            userRepository.Remove(user);
            companyRepository.Remove(company);
        }

        [Test]
        public void Can_add_new_user_with_new_grooming()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            Dog dog = new Dog {FirstName = "Wyatt", LastName = "Patel"};
            IDogRepository dogRepository = new DogRepository();
            dogRepository.Add(dog);

            GroomingType groomingType = new GroomingType ("Bath");
            IGroomingTypeRepository groomingTypeRepository = new GroomingTypeRepository();
            groomingTypeRepository.Add(groomingType);

            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            Grooming grooming1 = new Grooming(groomingType, 45.50, date, dog, user);
            Grooming grooming2 = new Grooming(groomingType, 1000.40, date, dog, user);

            IGroomingRepository groomingRepository = new GroomingRepository();
            groomingRepository.Add(grooming1);
            groomingRepository.Add(grooming2);

            User userFromDb = userRepository.GetById(user.UserId);
            ICollection<Grooming> groomings = userFromDb.Groomings;

            Assert.AreEqual(2, groomings.Count);

            // Here we have to explicitly remove the user's groomings, since
            // they were added using the grooming repository and not the user's
            // Groomings property.
            groomingRepository.Remove(grooming1);
            groomingRepository.Remove(grooming2);

            groomingTypeRepository.Remove(groomingType);
            dogRepository.Remove(dog);
            userRepository.Remove(user);
        }

        [Test]
        public void Can_add_new_user_with_new_grooming_entries()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            Dog dog = new Dog { FirstName = "Wyatt", LastName = "Patel" };
            IDogRepository dogRepository = new DogRepository();
            dogRepository.Add(dog);

            GroomingType groomingType = new GroomingType("Bath");
            IGroomingTypeRepository groomingTypeRepository = new GroomingTypeRepository();
            groomingTypeRepository.Add(groomingType);

            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            Grooming grooming1 = new Grooming(groomingType, 45.50, date, dog, user);
            Grooming grooming2 = new Grooming(groomingType, 1000.40, date, dog, user);

            IList<Grooming> groomings = new List<Grooming>();
            groomings.Add(grooming1);
            groomings.Add(grooming2);

            //Set groomings list via user object.
            user.Groomings = groomings;

            userRepository.Update(user);

            IGroomingRepository groomingRepository = new GroomingRepository();
            ICollection<Grooming> groomingsFromDb = groomingRepository.GetByUser(user);

            Assert.AreEqual(2, groomingsFromDb.Count);

            //Delete user, will also get rid of user's grooming entries.
            userRepository.Remove(user);
            groomingTypeRepository.Remove(groomingType);
            dogRepository.Remove(dog);
        }

        [Test]
        public void Can_add_new_user_with_new_boarding()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            Dog dog = new Dog { FirstName = "Wyatt", LastName = "Patel" };
            IDogRepository dogRepository = new DogRepository();
            dogRepository.Add(dog);

            Cost boardingCost = new Cost(20);
            Cost sundayDaycareCost = new Cost(30);
            ICostRepository costRepository = new CostRepository();
            costRepository.Add(boardingCost);
            costRepository.Add(sundayDaycareCost);

            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            Boarding boarding1 = new Boarding(date, true, dog, boardingCost, user);
            Boarding boarding2 = new Boarding(date, true, dog, boardingCost, user);

            IBoardingRepository boardingRepository = new BoardingRepository();
            boardingRepository.Add(boarding1);
            boardingRepository.Add(boarding2);

            User userFromDb = userRepository.GetById(user.UserId);
            ICollection<Boarding> boardings = userFromDb.Boardings;

            Assert.AreEqual(2, boardings.Count);

            // Here we have to explicitly remove the user's groomings, since
            // they were added using the grooming repository and not the user's
            // Groomings property.
            boardingRepository.Remove(boarding1);
            boardingRepository.Remove(boarding2);

            costRepository.Remove(boardingCost);
            costRepository.Remove(sundayDaycareCost);
            dogRepository.Remove(dog);
            userRepository.Remove(user);
        }

        [Test]
        public void Can_add_new_user_with_new_boarding_entries()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            Dog dog = new Dog { FirstName = "Wyatt", LastName = "Patel" };
            IDogRepository dogRepository = new DogRepository();
            dogRepository.Add(dog);

            Cost boardingCost = new Cost(20);
            Cost sundayDaycareCost = new Cost(30);
            ICostRepository costRepository = new CostRepository();
            costRepository.Add(boardingCost);
            costRepository.Add(sundayDaycareCost);

            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            Boarding boarding1 = new Boarding(date, true, dog, boardingCost, user);
            Boarding boarding2 = new Boarding(date, true, dog, boardingCost, user);

            IList<Boarding> boardings = new List<Boarding>();
            boardings.Add(boarding1);
            boardings.Add(boarding2);

            //Set boardings list via user object.
            user.Boardings = boardings;

            userRepository.Update(user);

            IBoardingRepository boardingRepository = new BoardingRepository();
            ICollection<Boarding> boardingsFromDb = boardingRepository.GetByUser(user);

            Assert.AreEqual(2, boardingsFromDb.Count);

            //Delete user, will also get rid of user's grooming entries.
            userRepository.Remove(user);
            costRepository.Remove(boardingCost);
            costRepository.Remove(sundayDaycareCost);
            dogRepository.Remove(dog);
        }

        [Test]
        public void Can_add_new_user_with_new_daycare()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            Dog dog = new Dog { FirstName = "Wyatt", LastName = "Patel" };
            IDogRepository dogRepository = new DogRepository();
            dogRepository.Add(dog);

            Cost daycareCost = new Cost(70);
            ICostRepository costRepository = new CostRepository();
            costRepository.Add(daycareCost);

            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            Daycare daycare1 = new Daycare(date, dog, daycareCost, user);
            Daycare daycare2 = new Daycare(date, dog, daycareCost, user);

            IDaycareRepository daycareRepository = new DaycareRepository();
            daycareRepository.Add(daycare1);
            daycareRepository.Add(daycare2);

            User userFromDb = userRepository.GetById(user.UserId);
            ICollection<Daycare> daycares = userFromDb.Daycares;

            Assert.AreEqual(2, daycares.Count);

            // Here we have to explicitly remove the user's groomings, since
            // they were added using the grooming repository and not the user's
            // Groomings property.
            daycareRepository.Remove(daycare1);
            daycareRepository.Remove(daycare2);

            costRepository.Remove(daycareCost);
            dogRepository.Remove(dog);
            userRepository.Remove(user);
        }

        public void Can_add_new_user_with_new_daycare_entries()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            Dog dog = new Dog { FirstName = "Wyatt", LastName = "Patel" };
            IDogRepository dogRepository = new DogRepository();
            dogRepository.Add(dog);

            Cost daycareCost = new Cost(70);
            ICostRepository costRepository = new CostRepository();
            costRepository.Add(daycareCost);

            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            Daycare daycare1 = new Daycare(date, dog, daycareCost, user);
            Daycare daycare2 = new Daycare(date, dog, daycareCost, user);

            IList<Daycare> daycares = new List<Daycare>();
            daycares.Add(daycare1);
            daycares.Add(daycare2);

            //Set daycares list via user object.
            user.Daycares = daycares;

            userRepository.Update(user);

            IDaycareRepository daycareRepository = new DaycareRepository();
            ICollection<Daycare> daycaresFromDb = daycareRepository.GetByUser(user);

            Assert.AreEqual(2, daycaresFromDb.Count);

            //Delete user, will also get rid of user's grooming entries.
            userRepository.Remove(user);
            costRepository.Remove(daycareCost);
            dogRepository.Remove(dog);
        }

        [Test]
        public void Can_add_new_user_with_new_pickup_dropoff()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            Dog dog = new Dog { FirstName = "Wyatt", LastName = "Patel" };
            IDogRepository dogRepository = new DogRepository();
            dogRepository.Add(dog);

            Cost pickupCost = new Cost(6);
            Cost dropoffCost = new Cost(7);
            ICostRepository costRepository = new CostRepository();
            costRepository.Add(pickupCost);
            costRepository.Add(dropoffCost);

            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            PickupDropoff pickupDropoff1 = new PickupDropoff(date, dog, user);
            pickupDropoff1.PickupCost = pickupCost;
            pickupDropoff1.DropoffCost = dropoffCost;
            PickupDropoff pickupDropoff2 = new PickupDropoff(date, dog, user);
            pickupDropoff2.PickupCost = pickupCost;
            pickupDropoff2.DropoffCost = dropoffCost;

            IPickupDropoffRepository pickupDropoffRepository = new PickupDropoffRepository();
            pickupDropoffRepository.Add(pickupDropoff1);
            pickupDropoffRepository.Add(pickupDropoff2);

            User userFromDb = userRepository.GetById(user.UserId);
            ICollection<PickupDropoff> pickupDropoffs = userFromDb.PickupDropoffs;

            Assert.AreEqual(2, pickupDropoffs.Count);

            // Here we have to explicitly remove the user's groomings, since
            // they were added using the grooming repository and not the user's
            // Groomings property.
            pickupDropoffRepository.Remove(pickupDropoff1);
            pickupDropoffRepository.Remove(pickupDropoff2);

            costRepository.Remove(pickupCost);
            costRepository.Remove(dropoffCost);
            dogRepository.Remove(dog);
            userRepository.Remove(user);
        }

        [Test]
        public void Can_add_new_user_with_new_pickup_dropoff_entries()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            Dog dog = new Dog { FirstName = "Wyatt", LastName = "Patel" };
            IDogRepository dogRepository = new DogRepository();
            dogRepository.Add(dog);

            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            PickupDropoff pickupDropoff1 = new PickupDropoff(date, dog, user);
            PickupDropoff pickupDropoff2 = new PickupDropoff(date, dog, user);

            IList<PickupDropoff> pickupDropoffs = new List<PickupDropoff>();
            pickupDropoffs.Add(pickupDropoff1);
            pickupDropoffs.Add(pickupDropoff2);

            //Set pickupDropoffs list via user object.
            user.PickupDropoffs = pickupDropoffs;

            userRepository.Update(user);

            IPickupDropoffRepository pickupDropoffRepository = new PickupDropoffRepository();
            ICollection<PickupDropoff> pickupDropoffsFromDb = pickupDropoffRepository.GetByUser(user);

            Assert.AreEqual(2, pickupDropoffsFromDb.Count);

            //Delete user, will also get rid of user's grooming entries.
            userRepository.Remove(user);
            dogRepository.Remove(dog);
        }

        [Test]
        public void Can_add_new_user_with_new_training()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            Dog dog = new Dog { FirstName = "Wyatt", LastName = "Patel" };
            IDogRepository dogRepository = new DogRepository();
            dogRepository.Add(dog);

            CostType classType = new CostType("AA");
            ICostTypeRepository costTypeRepository = new CostTypeRepository();
            costTypeRepository.Add(classType);

            Cost classCost = new Cost(200);
            Cost preK9DaycareCost = new Cost(30);
            ICostRepository costRepository = new CostRepository();
            costRepository.Add(classCost);
            costRepository.Add(preK9DaycareCost);

            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            Training training1 = new Training(date, classType, classCost, dog, user);
            Training training2 = new Training(date, classType, classCost, dog, user);

            ITrainingRepository trainingRepository = new TrainingRepository();
            trainingRepository.Add(training1);
            trainingRepository.Add(training2);

            User userFromDb = userRepository.GetById(user.UserId);
            ICollection<Training> trainings = userFromDb.Trainings;

            Assert.AreEqual(2, trainings.Count);

            // Here we have to explicitly remove the user's groomings, since
            // they were added using the grooming repository and not the user's
            // Groomings property.
            trainingRepository.Remove(training1);
            trainingRepository.Remove(training2);

            costRepository.Remove(classCost);
            costRepository.Remove(preK9DaycareCost);
            costTypeRepository.Remove(classType);
            dogRepository.Remove(dog);
            userRepository.Remove(user);
        }

        [Test]
        public void Can_add_new_user_with_new_training_entries()
        {
            var user = new User("Jessica", "jessica123", "Jessica", "Barajas", DateTime.Now); ;
            IUserRepository userRepository = new UserRepository();
            userRepository.Add(user);

            // use session to try to load the user
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<User>(user.UserId);
                // Test that the user was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(user, fromDb);
                Assert.AreEqual(user.FirstName, fromDb.FirstName);
                Assert.AreEqual(user.LastName, fromDb.LastName);
            }

            Dog dog = new Dog { FirstName = "Wyatt", LastName = "Patel" };
            IDogRepository dogRepository = new DogRepository();
            dogRepository.Add(dog);

            CostType classType = new CostType("AA");
            ICostTypeRepository costTypeRepository = new CostTypeRepository();
            costTypeRepository.Add(classType);

            Cost classCost = new Cost(20);
            Cost preK9DaycareCost = new Cost(30);
            ICostRepository costRepository = new CostRepository();
            costRepository.Add(classCost);
            costRepository.Add(preK9DaycareCost);

            DateTime date = DateTime.Now;
            // Set the Milliseconds to 0 since MySQL DATETIME does not support milliseconds.
            date = DateTime.ParseExact(date.ToString(), "M/d/yyyy h:mm:ss tt", null);

            Training training1 = new Training(date, classType, classCost, dog, user);
            Training training2 = new Training(date, classType, classCost, dog, user);

            IList<Training> trainings = new List<Training>();
            trainings.Add(training1);
            trainings.Add(training2);

            //Set trainings list via user object.
            user.Trainings = trainings;

            userRepository.Update(user);

            ITrainingRepository trainingRepository = new TrainingRepository();
            ICollection<Training> trainingsFromDb = trainingRepository.GetByUser(user);

            Assert.AreEqual(2, trainingsFromDb.Count);

            //Delete user, will also get rid of user's grooming entries.
            userRepository.Remove(user);
            costRepository.Remove(classCost);
            costRepository.Remove(preK9DaycareCost);
            costTypeRepository.Remove(classType);
            dogRepository.Remove(dog);
        }
    }
}

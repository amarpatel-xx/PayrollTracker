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
    public class CostTypeTest
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

        private readonly CostType[] _costTypes = new[]
                 {
                     new CostType ("Boarding - Boarding Rate"),
                     new CostType ("Boarding - Sunday Daycare"),
                     new CostType ("Daycare - Daycare Rate"),
                     new CostType ("Pickup or Dropoff - Pickup Rate"),
                     new CostType ("Pickup or Dropoff - Dropoff Rate"),
                     new CostType ("Training - Class Rate"),
                     new CostType ("Training - Pre K9 Daycare Rate")
                 };

        private void CreateInitialData()
        {

            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var costType in _costTypes)
                {
                    session.Save(costType);
                }
                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            ICostTypeRepository repository = new CostTypeRepository();
            foreach (var costType in _costTypes)
            {
                CostType fromDb = repository.GetById(costType.CostTypeId);
                if (fromDb != null)
                {
                    repository.Remove(costType);
                }
            }
        }

        [Test]
        public void Can_add_multiple_new_costTypes()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getCostTypeTableSize(session);

            ITransaction tx = session.BeginTransaction();
            CostType costType1 = new CostType();
            costType1.CostName = "test cost name 1";
            session.Save(costType1);

            CostType costType2 = new CostType();
            costType2.CostName = "test cost name 2";
            session.Save(costType2);

            CostType costType3 = new CostType();
            costType3.CostName = "test cost name 3";
            session.Save(costType3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getCostTypeTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(costType1);
            session.Delete(costType2);
            session.Delete(costType3);

            tx.Commit();

            session.Close();
        }

        private int getCostTypeTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from CostType").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_costType()
        {
            var costType = new CostType ("test new cost name");
            ICostTypeRepository repository = new CostTypeRepository();
            repository.Add(costType);

            // use session to try to load the costType
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<CostType>(costType.CostTypeId);
                // Test that the costType was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(costType, fromDb);
                Assert.AreEqual(costType.CostName, fromDb.CostName);
            }

            repository.Remove(costType);
        }

        [Test]
        public void Can_update_existing_costType()
        {
            var costType = _costTypes[1];
            costType.CostName = "test cost name update";
            ICostTypeRepository repository = new CostTypeRepository();
            repository.Update(costType);

            // use session to try to load the costType
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<CostType>(costType.CostTypeId);
                Assert.AreEqual(costType.CostName, fromDb.CostName);
            }
        }

        [Test]
        public void Can_remove_existing_costType()
        {
            var costType = _costTypes[0];
            ICostTypeRepository repository = new CostTypeRepository();
            repository.Remove(costType);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<CostType>(costType.CostTypeId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_costType_by_id()
        {
            ICostTypeRepository repository = new CostTypeRepository();
            var fromDb = repository.GetById(_costTypes[1].CostTypeId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_costTypes[1], fromDb);
            Assert.AreEqual(_costTypes[1].CostName, fromDb.CostName);
        }

        [Test]
        public void Can_add_new_cost_type_with_new_costs()
        {
            var costType = new CostType("new cost type"); 
            ICostTypeRepository costTypeRepository = new CostTypeRepository();
            costTypeRepository.Add(costType);

            var cost1 = new Cost(1000.11);
            var cost2 = new Cost(1500.11);
            ICostRepository costRepository = new CostRepository();
            costRepository.Add(cost1);
            costRepository.Add(cost2);

            IList<Cost> possibleCosts = new List<Cost>();
            possibleCosts.Add(cost1);
            possibleCosts.Add(cost2);
            costType.PossibleCosts = possibleCosts;

            costTypeRepository.Update(costType);

            // use session to try to load the cost type
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<CostType>(costType.CostTypeId);
                // Test that the costType was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(costType, fromDb);
                Assert.AreEqual(costType.CostName, fromDb.CostName);
                Assert.AreEqual(possibleCosts.Count, fromDb.PossibleCosts.Count);
            }

            costTypeRepository.Remove(costType);
            costRepository.Remove(cost1);
            costRepository.Remove(cost2);
        }
    }
}

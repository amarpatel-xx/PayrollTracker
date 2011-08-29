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
    public class CostTest
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

        private readonly Cost[] _costs = new[]
                 {
                     new Cost (10),
                     new Cost (15),
                     new Cost (20),
                     new Cost (25)
                 };

        private void CreateInitialData()
        {

            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var cost in _costs)
                {
                    session.Save(cost);
                }
                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            ICostRepository repository = new CostRepository();
            foreach (var cost in _costs)
            {
                Cost fromDb = repository.GetById(cost.CostId);
                if (fromDb != null)
                {
                    repository.Remove(cost);
                }
            }
        }

        [Test]
        public void Can_add_multiple_new_costs()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getCostTableSize(session);

            ITransaction tx = session.BeginTransaction();
            Cost cost1 = new Cost();
            cost1.CostValue = 30.5;
            session.Save(cost1);

            Cost cost2 = new Cost();
            cost2.CostValue = 40.99;
            session.Save(cost2);

            Cost cost3 = new Cost();
            cost3.CostValue = 52.25;
            session.Save(cost3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getCostTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(cost1);
            session.Delete(cost2);
            session.Delete(cost3);

            tx.Commit();

            session.Close();
        }

        private int getCostTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from Cost").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_cost()
        {
            var cost = new Cost(100.50);
            ICostRepository repository = new CostRepository();
            repository.Add(cost);

            // use session to try to load the cost
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Cost>(cost.CostId);
                // Test that the cost was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(cost, fromDb);
                Assert.AreEqual(cost.CostValue, fromDb.CostValue);
            }

            repository.Remove(cost);
        }

        [Test]
        public void Can_update_existing_cost()
        {
            var cost = _costs[1];
            cost.CostValue = 200.50;
            ICostRepository repository = new CostRepository();
            repository.Update(cost);

            // use session to try to load the cost
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Cost>(cost.CostId);
                Assert.AreEqual(cost.CostValue, fromDb.CostValue);
            }
        }

        [Test]
        public void Can_remove_existing_cost()
        {
            var cost = _costs[0];
            ICostRepository repository = new CostRepository();
            repository.Remove(cost);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Cost>(cost.CostId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_cost_by_id()
        {
            ICostRepository repository = new CostRepository();
            var fromDb = repository.GetById(_costs[1].CostId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_costs[1], fromDb);
            Assert.AreEqual(_costs[1].CostValue, fromDb.CostValue);
        }
    }
}

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
    public class GroomingTypeTest
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

        private readonly GroomingType[] _groomingTypes = new[]
                 {
                     new GroomingType ("Bath"),
                     new GroomingType ("Nose and Ears"),
                     new GroomingType ("Mini Groom"),
                     new GroomingType ("Full Groom")
                 };

        private void CreateInitialData()
        {

            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var groomingType in _groomingTypes)
                {
                    session.Save(groomingType);
                }
                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            IGroomingTypeRepository repository = new GroomingTypeRepository();
            foreach (var groomingType in _groomingTypes)
            {
                GroomingType fromDb = repository.GetById(groomingType.GroomingTypeId);
                if (fromDb != null)
                {
                    repository.Remove(groomingType);
                }
            }
        }

        [Test]
        public void Can_add_multiple_new_groomingTypes()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getGroomingTypeTableSize(session);

            ITransaction tx = session.BeginTransaction();
            GroomingType groomingType1 = new GroomingType();
            groomingType1.TypeName = "test type name 1";
            session.Save(groomingType1);

            GroomingType groomingType2 = new GroomingType();
            groomingType2.TypeName = "test type name 2";
            session.Save(groomingType2);

            GroomingType groomingType3 = new GroomingType();
            groomingType3.TypeName = "test type name 3";
            session.Save(groomingType3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getGroomingTypeTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(groomingType1);
            session.Delete(groomingType2);
            session.Delete(groomingType3);

            tx.Commit();

            session.Close();
        }

        private int getGroomingTypeTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from GroomingType").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_groomingType()
        {
            var groomingType = new GroomingType ("test new type name");
            IGroomingTypeRepository repository = new GroomingTypeRepository();
            repository.Add(groomingType);

            // use session to try to load the groomingType
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<GroomingType>(groomingType.GroomingTypeId);
                // Test that the groomingType was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(groomingType, fromDb);
                Assert.AreEqual(groomingType.TypeName, fromDb.TypeName);
            }

            repository.Remove(groomingType);
        }

        [Test]
        public void Can_update_existing_groomingType()
        {
            var groomingType = _groomingTypes[1];
            groomingType.TypeName = "test type name update";
            IGroomingTypeRepository repository = new GroomingTypeRepository();
            repository.Update(groomingType);

            // use session to try to load the groomingType
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<GroomingType>(groomingType.GroomingTypeId);
                Assert.AreEqual(groomingType.TypeName, fromDb.TypeName);
            }
        }

        [Test]
        public void Can_remove_existing_groomingType()
        {
            var groomingType = _groomingTypes[0];
            IGroomingTypeRepository repository = new GroomingTypeRepository();
            repository.Remove(groomingType);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<GroomingType>(groomingType.GroomingTypeId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_groomingType_by_id()
        {
            IGroomingTypeRepository repository = new GroomingTypeRepository();
            var fromDb = repository.GetById(_groomingTypes[1].GroomingTypeId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_groomingTypes[1], fromDb);
            Assert.AreEqual(_groomingTypes[1].TypeName, fromDb.TypeName);
        }
    }
}

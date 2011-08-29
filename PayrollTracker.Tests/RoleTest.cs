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
    public class RoleTest
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

        private readonly Role[] _roles = new[]
                 {
                     new Role ("Training"),
                     new Role ("PickupAndDropoff"),
                     new Role ("Daycare"),
                     new Role ("Boarding"),
                     new Role ("Grooming")
                 };

        private void CreateInitialData()
        {

            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var role in _roles)
                {
                    session.Save(role);
                }
                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            IRoleRepository repository = new RoleRepository();
            foreach (var role in _roles)
            {
                Role fromDb = repository.GetById(role.RoleId);
                if (fromDb != null)
                {
                    repository.Remove(role);
                }
            }
        }

        [Test]
        public void Can_add_multiple_new_roles()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getRoleTableSize(session);

            ITransaction tx = session.BeginTransaction();
            Role role1 = new Role();
            role1.RoleName = "test role name 1";
            session.Save(role1);

            Role role2 = new Role();
            role2.RoleName = "test role name 2";
            session.Save(role2);

            Role role3 = new Role();
            role3.RoleName = "test role name 3";
            session.Save(role3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getRoleTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(role1);
            session.Delete(role2);
            session.Delete(role3);

            tx.Commit();

            session.Close();
        }

        private int getRoleTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from Role").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_role()
        {
            var role = new Role ("test new role name");
            IRoleRepository repository = new RoleRepository();
            repository.Add(role);

            // use session to try to load the role
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Role>(role.RoleId);
                // Test that the role was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(role, fromDb);
                Assert.AreEqual(role.RoleName, fromDb.RoleName);
            }

            repository.Remove(role);
        }

        [Test]
        public void Can_update_existing_role()
        {
            var role = _roles[1];
            role.RoleName = "test role name update";
            IRoleRepository repository = new RoleRepository();
            repository.Update(role);

            // use session to try to load the role
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Role>(role.RoleId);
                Assert.AreEqual(role.RoleName, fromDb.RoleName);
            }
        }

        [Test]
        public void Can_remove_existing_role()
        {
            var role = _roles[0];
            IRoleRepository repository = new RoleRepository();
            repository.Remove(role);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Role>(role.RoleId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_role_by_id()
        {
            IRoleRepository repository = new RoleRepository();
            var fromDb = repository.GetById(_roles[1].RoleId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_roles[1], fromDb);
            Assert.AreEqual(_roles[1].RoleName, fromDb.RoleName);
        }
    }
}

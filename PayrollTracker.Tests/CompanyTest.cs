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
    public class CompanyTest
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

        private readonly Company[] _companys = new[]
                 {
                     new Company ("University of Doglando"),
                     new Company ("Groom, Grub and Belly Rub"),
                 };

        private void CreateInitialData()
        {
            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var company in _companys)
                {
                    session.Save(company);
                }
                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            ICompanyRepository repository = new CompanyRepository();
            foreach (var company in _companys)
            {
                Company fromDb = repository.GetById(company.CompanyId);
                if (fromDb != null)
                {
                    repository.Remove(company);
                }
            }
        }

        [Test]
        public void Can_add_multiple_new_companys()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getCompanyTableSize(session);

            ITransaction tx = session.BeginTransaction();
            Company company1 = new Company();
            company1.Name = "test company name 1";
            session.Save(company1);

            Company company2 = new Company();
            company2.Name = "test company name 2";
            session.Save(company2);

            Company company3 = new Company();
            company3.Name = "test company name 3";
            session.Save(company3);

            tx.Commit();

            Assert.AreEqual(countBefore + 3, getCompanyTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(company1);
            session.Delete(company2);
            session.Delete(company3);

            tx.Commit();

            session.Close();
        }

        private int getCompanyTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from Company").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_company()
        {
            var company = new Company("test new company name");
            ICompanyRepository repository = new CompanyRepository();
            repository.Add(company);

            // use session to try to load the company
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Company>(company.CompanyId);
                // Test that the company was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(company, fromDb);
                Assert.AreEqual(company.Name, fromDb.Name);
            }

            repository.Remove(company);
        }

        [Test]
        public void Can_update_existing_company()
        {
            var company = _companys[1];
            company.Name = "test company name update";
            ICompanyRepository repository = new CompanyRepository();
            repository.Update(company);

            // use session to try to load the company
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Company>(company.CompanyId);
                Assert.AreEqual(company.Name, fromDb.Name);
            }
        }

        [Test]
        public void Can_remove_existing_company()
        {
            var company = _companys[0];
            ICompanyRepository repository = new CompanyRepository();
            repository.Remove(company);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Company>(company.CompanyId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_company_by_id()
        {
            ICompanyRepository repository = new CompanyRepository();
            var fromDb = repository.GetById(_companys[1].CompanyId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_companys[1], fromDb);
            Assert.AreEqual(_companys[1].Name, fromDb.Name);
        }
    }
}

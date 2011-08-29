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
    public class PayrollTest
    {
        private ISessionFactory _sessionFactory;
        private Configuration _configuration;

        private Company _company1 = new Company("University of Doglando");
        private Company _company2 = new Company("Groom, Grub & Bell Rub");

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

        private readonly Payroll[] _payrolls = new[]
                 {
                     new Payroll {PayrollStartDate = DateTime.Now, PayrollNumberOfWeeks = 2},
                     new Payroll {PayrollStartDate = DateTime.Now, PayrollNumberOfWeeks = 3}
                 };

        private void CreateInitialData()
        {

            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(_company1);
                session.Save(_company2);


                // Get rid of milliseconds to make comparisons work, since MySQL
                // DateTime does not support milliseconds.
                _payrolls[0].PayrollStartDate = DateTime.ParseExact(_payrolls[0].PayrollStartDate.ToString(), "M/d/yyyy h:mm:ss tt", null);

                _payrolls[0].Company = _company1;
                session.Save(_payrolls[0]);

                // Get rid of milliseconds to make comparisons work, since MySQL
                // DateTime does not support milliseconds.
                _payrolls[1].PayrollStartDate = DateTime.ParseExact(_payrolls[1].PayrollStartDate.ToString(), "M/d/yyyy h:mm:ss tt", null);

                _payrolls[1].Company = _company2;
                session.Save(_payrolls[1]);

                transaction.Commit();
            }
        }

        private void DeleteInitialData()
        {
            IPayrollRepository repository = new PayrollRepository();
            foreach (var payroll in _payrolls)
            {
                Payroll fromDb = repository.GetById(payroll.PayrollId);
                if (fromDb != null)
                {
                    repository.Remove(payroll);
                }
            }

            ICompanyRepository companyRepository = new CompanyRepository();
            companyRepository.Remove(_company1);
            companyRepository.Remove(_company2);
        }

        [Test]
        public void Can_add_multiple_new_payrolls()
        {
            ISession session = _sessionFactory.OpenSession();

            int countBefore = getPayrollTableSize(session);

            ITransaction tx = session.BeginTransaction();
            Payroll payroll1 = new Payroll();
            payroll1.PayrollStartDate = DateTime.Now;
            payroll1.PayrollNumberOfWeeks = 2;
            payroll1.Company = _company1;
            session.Save(payroll1);

            Payroll payroll2 = new Payroll();
            payroll2.PayrollStartDate = DateTime.Now;
            payroll2.PayrollNumberOfWeeks = 3;
            payroll2.Company = _company2;
            session.Save(payroll2);

            tx.Commit();

            Assert.AreEqual(countBefore + 2, getPayrollTableSize(session));

            tx = session.BeginTransaction();

            session.Delete(payroll1);
            session.Delete(payroll2);

            tx.Commit();

            session.Close();
        }

        private int getPayrollTableSize(ISession session)
        {
            ITransaction readTx = session.BeginTransaction();
            int size = session.CreateQuery("from Payroll").List().Count;
            readTx.Rollback();
            return size;
        }


        [Test]
        public void Can_add_new_payroll()
        {
            var payroll = new Payroll { PayrollStartDate = DateTime.Now, PayrollNumberOfWeeks = 2 };
            // Get rid of milliseconds to make comparisons work, since MySQL
            // DateTime does not support milliseconds.
            payroll.PayrollStartDate = DateTime.ParseExact(payroll.PayrollStartDate.ToString(), "M/d/yyyy h:mm:ss tt", null);
            payroll.Company = _company1;
            IPayrollRepository repository = new PayrollRepository();
            repository.Add(payroll);

            // use session to try to load the payroll
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Payroll>(payroll.PayrollId);
                // Test that the payroll was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(payroll, fromDb);
                Assert.AreEqual(payroll.PayrollNumberOfWeeks, fromDb.PayrollNumberOfWeeks);
                Assert.AreEqual(payroll.PayrollStartDate, fromDb.PayrollStartDate);
            }

            repository.Remove(payroll);
        }

        [Test]
        public void Can_update_existing_payroll()
        {
            var payroll = _payrolls[1];
            payroll.PayrollNumberOfWeeks = 10;
            IPayrollRepository repository = new PayrollRepository();
            repository.Update(payroll);

            // use session to try to load the payroll
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Payroll>(payroll.PayrollId);
                Assert.AreEqual(payroll.PayrollNumberOfWeeks, fromDb.PayrollNumberOfWeeks);
            }
        }

        [Test]
        public void Can_remove_existing_payroll()
        {
            var payroll = _payrolls[0];
            IPayrollRepository repository = new PayrollRepository();
            repository.Remove(payroll);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Payroll>(payroll.PayrollId);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_payroll_by_id()
        {
            IPayrollRepository repository = new PayrollRepository();
            var fromDb = repository.GetById(_payrolls[1].PayrollId);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_payrolls[1], fromDb);
            Assert.AreEqual(_payrolls[1].PayrollNumberOfWeeks, fromDb.PayrollNumberOfWeeks);
            Assert.AreEqual(_payrolls[1].PayrollStartDate, fromDb.PayrollStartDate);
        }

        [Test]
        public void Can_get_existing_payroll_by_company()
        {
            IPayrollRepository repository = new PayrollRepository();
            Payroll payroll = repository.GetByCompany(_payrolls[1].Company);

            Assert.IsNotNull(payroll);
        }

        private bool IsInCollection(Payroll payroll, ICollection<Payroll> fromDb)
        {
            bool result = false;

            foreach (var item in fromDb)
            {
                if (payroll.PayrollId == item.PayrollId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}

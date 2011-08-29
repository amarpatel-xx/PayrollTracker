using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PayrollTracker.ModelClassLibrary.Domain;
using PayrollTracker.ModelClassLibrary.Repositories;
using NUnit.Framework;

namespace PayrollTracker.Tests
{
    [TestFixture]
    class DatabaseSetupInsertions
    {
        [Test]
        public void DatabaseSetupInsertions_script()
        {
            CompanyRepository companyRepository = new CompanyRepository();
            Company universityOfDoglando = new Company("University of Doglando");
            Company groomGrubAndBellyRub = new Company("Groom, Grub & Belly Rub");
            companyRepository.Add(universityOfDoglando);
            companyRepository.Add(groomGrubAndBellyRub);

            PayrollRepository payrollRepository = new PayrollRepository();
            Payroll universityOfDoglandoPayroll = new Payroll();
            universityOfDoglandoPayroll.Company = universityOfDoglando;
            universityOfDoglandoPayroll.PayrollNumberOfWeeks = 2;
            universityOfDoglandoPayroll.PayrollStartDate = new DateTime(2010, 11, 6);
            Payroll groomGrubAndBellyRubPayroll = new Payroll();
            groomGrubAndBellyRubPayroll.Company = groomGrubAndBellyRub;
            groomGrubAndBellyRubPayroll.PayrollNumberOfWeeks = 2;
            groomGrubAndBellyRubPayroll.PayrollStartDate = new DateTime(2010, 11, 6);
            payrollRepository.Add(universityOfDoglandoPayroll);
            payrollRepository.Add(groomGrubAndBellyRubPayroll);

            RoleRepository roleRepository = new RoleRepository();
            Role training = new Role("Training");
            Role pickupDropoff = new Role("Pickup/Dropoff");
            Role boarding = new Role("Boarding");
            Role grooming = new Role("Grooming");
            Role timeCard = new Role("Time Card");
            Role administrator = new Role("Administrator");
            Role addDog = new Role("Add Dog");
            roleRepository.Add(training);
            roleRepository.Add(pickupDropoff);
            roleRepository.Add(boarding);
            roleRepository.Add(grooming);
            roleRepository.Add(timeCard);
            roleRepository.Add(administrator);
            roleRepository.Add(addDog);

            UserRepository userRepository = new UserRepository();
            User teena = new User("teena", "teena123", "Teena", "Patel", DateTime.Now);
            User nim = new User("nim", "nim123", "Nim", "Patel", DateTime.Now);
            User jessica = new User("jessica", "jessica123", "Jessica", "Barajas", DateTime.Now);
            teena.AssignedRoles.Add(training);
            teena.AssignedRoles.Add(pickupDropoff);
            teena.AssignedRoles.Add(boarding);
            teena.AssignedRoles.Add(grooming);
            teena.AssignedRoles.Add(timeCard);
            teena.AssignedRoles.Add(administrator);
            teena.AssignedRoles.Add(addDog);
            teena.WorksForCompanies.Add(universityOfDoglando);
            teena.WorksForCompanies.Add(groomGrubAndBellyRub);
            nim.AssignedRoles.Add(training);
            nim.AssignedRoles.Add(pickupDropoff);
            nim.AssignedRoles.Add(boarding);
            nim.AssignedRoles.Add(grooming);
            nim.AssignedRoles.Add(timeCard);
            nim.AssignedRoles.Add(administrator);
            nim.AssignedRoles.Add(addDog);
            nim.WorksForCompanies.Add(universityOfDoglando);
            nim.WorksForCompanies.Add(groomGrubAndBellyRub);
            jessica.AssignedRoles.Add(training);
            jessica.AssignedRoles.Add(pickupDropoff);
            jessica.AssignedRoles.Add(boarding);
            jessica.AssignedRoles.Add(timeCard);
            jessica.AssignedRoles.Add(addDog);
            jessica.WorksForCompanies.Add(universityOfDoglando);
            userRepository.Add(teena);
            userRepository.Add(nim);
            userRepository.Add(jessica);

            CostTypeRepository costTypeRepository = new CostTypeRepository();
            CostType costType1 = new CostType("Boarding - Rate");
            CostType costType2 = new CostType("Boarding - Sunday Daycare");
            CostType costType3 = new CostType("Daycare");
            CostType costType4 = new CostType("PickupDropoff - Pickup");
            CostType costType5 = new CostType("PickupDropoff - Dropoff");
            CostType costType6 = new CostType("Training - Class - Pre K9");
            CostType costType7 = new CostType("Training - Class - AA");
            CostType costType8 = new CostType("Training - Class - BS");
            CostType costType9 = new CostType("Training - Class - K9 Nose Work");
            CostType costType10 = new CostType("Training - Class - Agility 1");
            CostType costType11 = new CostType("Training - Class - Agility 2");
            CostType costType12 = new CostType("Training - Pre-K9 Daycare");

            //Boarding costs.

            CostRepository costRepository = new CostRepository();
            Cost cost1 = new Cost(7);
            Cost cost2 = new Cost(10);
            costRepository.Add(cost1);
            costRepository.Add(cost2);

            List<Cost> possibleCosts1 = new List<Cost>();
            possibleCosts1.Add(cost1);
            possibleCosts1.Add(cost2);

            costType1.PossibleCosts = possibleCosts1;
            costTypeRepository.Add(costType1);

            Cost cost3 = new Cost(15);
            costRepository.Add(cost3);

            List<Cost> possibleCosts2 = new List<Cost>();
            possibleCosts2.Add(cost2);
            possibleCosts2.Add(cost3);

            costType2.PossibleCosts = possibleCosts2;
            costTypeRepository.Add(costType2);

            // Daycare costs.
            Cost cost4 = new Cost(20);
            Cost cost5 = new Cost(22);
            Cost cost6 = new Cost(25);
            Cost cost7 = new Cost(28);
            Cost cost8 = new Cost(30);
            Cost cost9 = new Cost(40);
            costRepository.Add(cost4);
            costRepository.Add(cost5);
            costRepository.Add(cost6);
            costRepository.Add(cost7);
            costRepository.Add(cost8);
            costRepository.Add(cost9);

            List<Cost> possibleCosts3 = new List<Cost>();
            possibleCosts3.Add(cost4);
            possibleCosts3.Add(cost5);
            possibleCosts3.Add(cost6);
            possibleCosts3.Add(cost7);
            possibleCosts3.Add(cost8);
            possibleCosts3.Add(cost9);

            costType3.PossibleCosts = possibleCosts3;
            costTypeRepository.Add(costType3);

            //Pickup dropoff costs.

            Cost cost10 = new Cost(3);
            Cost cost11 = new Cost(5);
            costRepository.Add(cost10);
            costRepository.Add(cost11);

            List<Cost> possibleCosts4 = new List<Cost>();
            possibleCosts4.Add(cost10);
            possibleCosts4.Add(cost11);

            costType4.PossibleCosts = possibleCosts4;
            costTypeRepository.Add(costType4);

            costType5.PossibleCosts = possibleCosts4;
            costTypeRepository.Add(costType5);

            //Training costs

            Cost cost12 = new Cost(60);
            Cost cost13 = new Cost(80);
            Cost cost14 = new Cost(100);
            Cost cost15 = new Cost(120);
            Cost cost16 = new Cost(160);
            costRepository.Add(cost12);
            costRepository.Add(cost13);
            costRepository.Add(cost14);
            costRepository.Add(cost15);
            costRepository.Add(cost16);

            List<Cost> possibleCosts6 = new List<Cost>();
            possibleCosts6.Add(cost12);
            possibleCosts6.Add(cost13);
            possibleCosts6.Add(cost14);
            possibleCosts6.Add(cost15);
            possibleCosts6.Add(cost16);

            List<Cost> possibleCosts7 = new List<Cost>();
            possibleCosts7.Add(cost12);
            possibleCosts7.Add(cost13);

            costType6.PossibleCosts = possibleCosts7;
            costTypeRepository.Add(costType6);

            costType7.PossibleCosts = possibleCosts6;
            costTypeRepository.Add(costType7);

            costType8.PossibleCosts = possibleCosts6;
            costTypeRepository.Add(costType8);

            costType9.PossibleCosts = possibleCosts6;
            costTypeRepository.Add(costType9);

            costType10.PossibleCosts = possibleCosts6;
            costTypeRepository.Add(costType10);

            costType11.PossibleCosts = possibleCosts6;
            costTypeRepository.Add(costType11);

            // - Pre K9 Daycare Costss
            List<Cost> possibleCosts8 = new List<Cost>();
            possibleCosts8.Add(cost11); // $5
            possibleCosts8.Add(cost2);  // $10

            costType12.PossibleCosts = possibleCosts8;
            costTypeRepository.Add(costType12);

            // Training Class Types

            // Add Grooming Types
            GroomingTypeRepository groomingTypeRepository = new GroomingTypeRepository();
            GroomingType groomingType1 = new GroomingType("Bath");
            GroomingType groomingType2 = new GroomingType("Nose and Ears");
            GroomingType groomingType3 = new GroomingType("Mini Groom");
            GroomingType groomingType4 = new GroomingType("Full Groom");
            groomingTypeRepository.Add(groomingType1);
            groomingTypeRepository.Add(groomingType2);
            groomingTypeRepository.Add(groomingType3);
            groomingTypeRepository.Add(groomingType4);
        }
    }
}
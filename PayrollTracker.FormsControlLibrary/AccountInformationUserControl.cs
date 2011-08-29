using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PayrollTracker.ModelClassLibrary.Repositories;
using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.FormsControlLibrary
{
    public partial class AccountInformationUserControl : UserControl
    {

        private User user;
        private Company company;
        private DateTime payrollStartDate;
        private DateTime payrollEndDate;

        public AccountInformationUserControl(User theUser, Company theCompany, DateTime thePayrollStartDate, DateTime thePayrollEndDate)
        {
            InitializeComponent();

            this.user = theUser;
            this.company = theCompany;
            this.payrollStartDate = thePayrollStartDate;
            this.payrollEndDate = thePayrollEndDate;

            this.usernameComboBox.SelectedValueChanged += new EventHandler(usernameComboBox_SelectedValueChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            refreshDisplayFromDatabase();
        }

        private void refreshDisplayFromDatabase()
        {
            CompanyRepository companyRepository = new CompanyRepository();
            IList<User> users = company.Users;
            this.usernameComboBox.DataSource = users;
            this.usernameComboBox.DisplayMember = "Username";
        }

        private void usernameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            User user = (User)usernameComboBox.SelectedValue;

            employeeFirstNameTextBox.Text = user.FirstName;
            employeeMiddleInitialTextBox.Text = user.MiddleInitial;
            employeeLastNameTextBox.Text = user.LastName;
            ssnTextBox.Text = user.SocialSecurityNumber;
            dateOfHireDateTimePicker.Value = user.DateOfHire;
            passwordTextBox.Text = user.Password;
            trainingPercentageTextBox.Text = "" + user.TrainingPercentage;
            groomingPercentageTextBox.Text = "" +user.GroomingPercentage;
            perHourPayTextBox.Text = "" + user.HourlyPay;
            phoneNumberTextBox.Text = user.PhoneNumber;
            emergencyContactNameTextBox.Text = user.EmergencyContactName;
            emergencyContactPhoneTextBox.Text = user.EmergencyContactNumber;
            rolesUserControl1.setActualRoles(user.AssignedRoles);
        }

        public void PayrollTracker_SaveButtonClickedEventHandler(object sender, EventArgs e)
        {
            User updatedUser = (User)usernameComboBox.SelectedValue;
            updatedUser.FirstName = employeeFirstNameTextBox.Text;
            updatedUser.MiddleInitial = employeeMiddleInitialTextBox.Text;
            updatedUser.LastName = employeeLastNameTextBox.Text;
            updatedUser.SocialSecurityNumber = ssnTextBox.Text;
            updatedUser.DateOfHire = dateOfHireDateTimePicker.Value;
            updatedUser.Password = passwordTextBox.Text;
            try
            {
                updatedUser.TrainingPercentage = Convert.ToDouble(trainingPercentageTextBox.Text);
                updatedUser.GroomingPercentage = Convert.ToDouble(groomingPercentageTextBox.Text);
                updatedUser.HourlyPay = Convert.ToDouble(perHourPayTextBox.Text);
            }
            catch (FormatException exception)
            {
                //Catch this exception quietly for now.
            }

            updatedUser.PhoneNumber = phoneNumberTextBox.Text;
            updatedUser.EmergencyContactName = emergencyContactNameTextBox.Text;
            updatedUser.EmergencyContactNumber = emergencyContactPhoneTextBox.Text;

            updatedUser.AssignedRoles = rolesUserControl1.getActualRoles();

            UserRepository userRepository = new UserRepository();
            userRepository.Update(updatedUser);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using PayrollTracker.ModelClassLibrary.Repositories;
using PayrollTracker.ModelClassLibrary.Domain;
using PayrollTracker.FormsControlLibrary;

namespace PayrollTracker
{
    public partial class PayrollTracker : Form
    {
        private const int NUMBER_OF_DAYS_IN_A_WEEK = 7;

        public PayrollTracker()
        {
            InitializeComponent();
            session = null;
            companyName = ConfigurationSettings.AppSettings["CompanyName"];

            showLoginUserControl();

            // TODO: Remove this.
            loginButton_Click(this, null);
        }

        private void showLoginUserControl()
        {
            loginUserControl = new LoginUserControl(companyName);
            // 
            // loginUserControl1
            // 
            this.loginUserControl.Location = new System.Drawing.Point(27, 40);
            this.loginUserControl.Name = "loginUserControl1";
            this.loginUserControl.TabIndex = 1;
            this.Controls.Add(this.loginUserControl);

            this.loginUserControl.LoginButtonClickedEventHandler += new System.EventHandler<EventArgs>(this.loginButton_Click);
        }

        private void logoutToolStripButton_Click(object sender, EventArgs e)
        {
            // Destroy session since user is logging out.
            session = null;
            this.Controls.Clear();
            showLoginUserControl();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            UserRepository userRepository = new UserRepository();
            User user = userRepository.GetByUsername(loginUserControl.Username());

            CompanyRepository companyRepository = new CompanyRepository();
            Company company = companyRepository.GetByCompanyName(companyName);

            if (user != null && user.WorksForCompanies.Contains(company) && user.Password.Equals(loginUserControl.Password()))
            {
                // Add payroll time range to session.
                PayrollRepository payrollRepository = new PayrollRepository();
                Payroll payroll = payrollRepository.GetByCompany(company);
                DateTime payrollStartDate = getCurrentPayrollStartDate(payroll);
                int numberOfDaysInPayroll = payroll.PayrollNumberOfWeeks * NUMBER_OF_DAYS_IN_A_WEEK;
                // TimeSpan constructor specifies: days, hours, minutes, seconds,           
                // and milliseconds. The TimeSpan returned has those values.
                TimeSpan oneMillisecondSpan = new TimeSpan(0, 0, 0, 0, 1);
                DateTime payrollEndDate = payrollStartDate.AddDays(numberOfDaysInPayroll).Subtract(oneMillisecondSpan);

                // Create a new session.
                // Associate user and her or his company with session.
                session = new Session(user, company, payrollStartDate, payrollEndDate);

                // Initialize the dashboard user control since we have successfully logged in.
                dashboardUserControl = new DashboardUserControl(session);

                // Remove the login user control/screen.
                this.Controls.Clear();

                // Add the dashboard user control.
                this.Controls.Add(dashboardUserControl);

                this.toolStrip1 = new System.Windows.Forms.ToolStrip();
                this.toolStrip1.SuspendLayout();
                // 
                // toolStrip1
                // 
                this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.logoutToolStripButton,
                    this.welcomeToolStripLabel});
                this.toolStrip1.Location = new System.Drawing.Point(0, 0);
                this.toolStrip1.Name = "toolStrip1";
                this.toolStrip1.Size = new System.Drawing.Size(649, 25);
                this.toolStrip1.TabIndex = 0;
                this.toolStrip1.Text = "Payroll Tracker Tool Strip";

                welcomeToolStripLabel.Text = "Welcome " + user.FirstName + "!";
                this.Controls.Add(this.toolStrip1);
                this.toolStrip1.ResumeLayout(false);
                this.toolStrip1.PerformLayout();

                this.logoutToolStripButton.Click +=new EventHandler(logoutToolStripButton_Click);
            }
            else
            {
                MessageBox.Show("Incorrect username and password combination");
            }
        }

        private DateTime getCurrentPayrollStartDate(Payroll payroll)
        {
            DateTime payrollStartDate = payroll.PayrollStartDate;
            int numberOfDaysInPayroll = payroll.PayrollNumberOfWeeks * NUMBER_OF_DAYS_IN_A_WEEK;
            DateTime payrollEndDate = payrollStartDate.AddDays(numberOfDaysInPayroll);

            DateTime currentDate = DateTime.Now;

            while (currentDate >= payrollStartDate)
            {
                if (currentDate.CompareTo(payrollStartDate) >= 0 && currentDate.CompareTo(payrollEndDate) <= 0)
                {
                    break;
                }

                payrollStartDate = payrollStartDate.AddDays(numberOfDaysInPayroll);
                payrollEndDate = payrollStartDate.AddDays(numberOfDaysInPayroll);
            }

            return payrollStartDate;
        }

        private Session session;
        private string companyName;
        private LoginUserControl loginUserControl;
        private DashboardUserControl dashboardUserControl;
        private System.Windows.Forms.ToolStrip toolStrip1;
    }
}

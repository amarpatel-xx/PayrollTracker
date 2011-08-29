using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PayrollTracker.ModelClassLibrary.Domain;
using PayrollTracker.ModelClassLibrary.Repositories;

namespace PayrollTracker
{
    public partial class DashboardUserControl : UserControl
    {
        private const int SCROLL_BAR_WIDTH = 15;
        private const string TIME_CARD_TAB_NAME = "timeCardTabPage";
        private const string BOARDING_TAB_NAME = "boardingTabPage";
        private const string GROOMING_TAB_NAME = "groomingTabPage";
        private const string PICKUP_DROPOFF_TAB_NAME = "pickupDropoffTabPage";
        private const string TRAINING_TAB_NAME = "trainingTabPage";
        private const string ADMINISTRATOR_ACCOUNT_INFORMATION_TAB_NAME = "administratorAccountInformationTabPage";
        private const string ADMINISTRATOR_PAYROLL_INFORMATION_TAB_NAME = "administratorPayrollInformationTabPage";
        private const string ADD_DOG_TAB_NAME = "addDogTabPage";
        
        public DashboardUserControl(Session session)
        {
            InitializeComponent();

            this.session = session;

            this.deleteButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.timeInButton = new System.Windows.Forms.Button();
            this.timeOutButton = new System.Windows.Forms.Button();

            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            User user = session.GetUser();

            Size dashboardSize = new Size();

            if (user.ContainsRole("Time Card"))
            {
                //
                // timeCard
                //
                this.timeCardTabPage = new System.Windows.Forms.TabPage();
                this.timeCardUserControl = new FormsControlLibrary.TimeCardUserControl(session.GetUser(), session.GetPayrollStartDate(), session.GetPayrollEndDate());
                this.dashboardTabControl.Controls.Add(this.timeCardTabPage);

                // 
                // timeCardTabPage
                // 
                this.timeCardTabPage.Controls.Add(this.timeCardUserControl);
                this.timeCardTabPage.Location = new System.Drawing.Point(22, 22);
                this.timeCardTabPage.Name = TIME_CARD_TAB_NAME;
                this.timeCardTabPage.Padding = new System.Windows.Forms.Padding(3);
                this.timeCardTabPage.TabIndex = 0;
                this.timeCardTabPage.Text = "Time Card";
                this.timeCardTabPage.UseVisualStyleBackColor = true;

                if (timeCardUserControl.Size.Width > dashboardSize.Width)
                {
                    dashboardSize.Width = timeCardUserControl.Size.Width;
                }
                if (timeCardUserControl.Size.Height > dashboardSize.Height)
                {
                    dashboardSize.Height = timeCardUserControl.Size.Height;
                }

                this.dashboardTabControl.SelectedTab = timeCardTabPage;
            }

            if (user.ContainsRole("Add Dog"))
            {
                //
                // addDog
                //
                this.addDogTabPage = new System.Windows.Forms.TabPage();
                this.addDogUserControl = new FormsControlLibrary.DogUserControl();
                this.dashboardTabControl.Controls.Add(this.addDogTabPage);

                // 
                // addDogTabPage
                // 
                this.addDogTabPage.Controls.Add(this.addDogUserControl);
                this.addDogTabPage.Location = new System.Drawing.Point(22, 22);
                this.addDogTabPage.Name = ADD_DOG_TAB_NAME;
                this.addDogTabPage.Padding = new System.Windows.Forms.Padding(3);
                this.addDogTabPage.TabIndex = 0;
                this.addDogTabPage.Text = "Dog";
                this.addDogTabPage.UseVisualStyleBackColor = true;

                if (addDogUserControl.Size.Width > dashboardSize.Width)
                {
                    dashboardSize.Width = addDogUserControl.Size.Width + SCROLL_BAR_WIDTH;
                }
                if (addDogUserControl.Size.Height > dashboardSize.Height)
                {
                    dashboardSize.Height = addDogUserControl.Size.Height;
                }
            }

            if (user.ContainsRole("Boarding"))
            {
                //
                // boarding
                //
                this.boardingTabPage = new System.Windows.Forms.TabPage();
                this.boardingUserControl = new FormsControlLibrary.BoardingUserControl(session.GetUser(), session.GetPayrollStartDate(), session.GetPayrollEndDate());
                this.dashboardTabControl.Controls.Add(this.boardingTabPage);

                // 
                // boardingTabPage
                // 
                this.boardingTabPage.Controls.Add(this.boardingUserControl);
                this.boardingTabPage.Location = new System.Drawing.Point(22, 22);
                this.boardingTabPage.Name = BOARDING_TAB_NAME;
                this.boardingTabPage.Padding = new System.Windows.Forms.Padding(3);
                this.boardingTabPage.TabIndex = 0;
                this.boardingTabPage.Text = "Boarding";
                this.boardingTabPage.UseVisualStyleBackColor = true;

                if (boardingUserControl.Size.Width > dashboardSize.Width)
                {
                    dashboardSize.Width = boardingUserControl.Size.Width + SCROLL_BAR_WIDTH;
                }
                if (boardingUserControl.Size.Height > dashboardSize.Height)
                {
                    dashboardSize.Height = boardingUserControl.Size.Height;
                }

                this.addDogUserControl.DogAddedOrUpdatedEvent += new EventHandler(boardingUserControl.dogAddedOrUpdated);
            }

            if (user.ContainsRole("Grooming"))
            {
                //
                // grooming
                //
                this.groomingTabPage = new System.Windows.Forms.TabPage();
                this.groomingUserControl = new FormsControlLibrary.GroomingUserControl(session.GetUser(), session.GetPayrollStartDate(), session.GetPayrollEndDate());
                this.dashboardTabControl.Controls.Add(this.groomingTabPage);

                // 
                // groomingTabPage
                // 
                this.groomingTabPage.Controls.Add(this.groomingUserControl);
                this.groomingTabPage.Location = new System.Drawing.Point(22, 22);
                this.groomingTabPage.Name = GROOMING_TAB_NAME;
                this.groomingTabPage.Padding = new System.Windows.Forms.Padding(3);
                this.groomingTabPage.TabIndex = 0;
                this.groomingTabPage.Text = "Grooming";
                this.groomingTabPage.UseVisualStyleBackColor = true;

                if (groomingUserControl.Size.Width > dashboardSize.Width)
                {
                    dashboardSize.Width = groomingUserControl.Size.Width + SCROLL_BAR_WIDTH;
                }
                if (groomingUserControl.Size.Height > dashboardSize.Height)
                {
                    dashboardSize.Height = groomingUserControl.Size.Height;
                }

                this.addDogUserControl.DogAddedOrUpdatedEvent += new EventHandler(groomingUserControl.dogAddedOrUpdated);
            }

            if (user.ContainsRole("Pickup/Dropoff"))
            {
                //
                // pickupDropoff
                //
                this.pickupDropoffTabPage = new System.Windows.Forms.TabPage();
                this.pickupDropoffUserControl = new FormsControlLibrary.PickupDropoffUserControl(session.GetUser(), session.GetPayrollStartDate(), session.GetPayrollEndDate());
                this.dashboardTabControl.Controls.Add(this.pickupDropoffTabPage);

                // 
                // pickupDropoffTabPage
                // 
                this.pickupDropoffTabPage.Controls.Add(this.pickupDropoffUserControl);
                this.pickupDropoffTabPage.Location = new System.Drawing.Point(22, 22);
                this.pickupDropoffTabPage.Name = PICKUP_DROPOFF_TAB_NAME;
                this.pickupDropoffTabPage.Padding = new System.Windows.Forms.Padding(3);
                this.pickupDropoffTabPage.TabIndex = 0;
                this.pickupDropoffTabPage.Text = "Pickup/Dropoff";
                this.pickupDropoffTabPage.UseVisualStyleBackColor = true;

                if (pickupDropoffUserControl.Size.Width > dashboardSize.Width)
                {
                    dashboardSize.Width = pickupDropoffUserControl.Size.Width + SCROLL_BAR_WIDTH;
                }
                if (pickupDropoffUserControl.Size.Height > dashboardSize.Height)
                {
                    dashboardSize.Height = pickupDropoffUserControl.Size.Height;
                }

                this.addDogUserControl.DogAddedOrUpdatedEvent += new EventHandler(pickupDropoffUserControl.dogAddedOrUpdated);

            }

            if (user.ContainsRole("Training"))
            {
                //
                // training
                //
                this.trainingTabPage = new System.Windows.Forms.TabPage();
                this.trainingUserControl = new FormsControlLibrary.TrainingUserControl(session.GetUser(), session.GetPayrollStartDate(), session.GetPayrollEndDate());
                this.dashboardTabControl.Controls.Add(this.trainingTabPage);

                // 
                // trainingTabPage
                // 
                this.trainingTabPage.Controls.Add(this.trainingUserControl);
                this.trainingTabPage.Location = new System.Drawing.Point(22, 22);
                this.trainingTabPage.Name = TRAINING_TAB_NAME;
                this.trainingTabPage.Padding = new System.Windows.Forms.Padding(3);
                this.trainingTabPage.TabIndex = 0;
                this.trainingTabPage.Text = "Training";
                this.trainingTabPage.UseVisualStyleBackColor = true;

                if (trainingUserControl.Size.Width > dashboardSize.Width)
                {
                    dashboardSize.Width = trainingUserControl.Size.Width + SCROLL_BAR_WIDTH;
                }
                if (trainingUserControl.Size.Height > dashboardSize.Height)
                {
                    dashboardSize.Height = trainingUserControl.Size.Height;
                }

                this.addDogUserControl.DogAddedOrUpdatedEvent += new EventHandler(trainingUserControl.dogAddedOrUpdated);
            }

            if (user.ContainsRole("Administrator"))
            {
                //
                // accountInfo
                //
                this.accountInfoTabPage = new System.Windows.Forms.TabPage();
                this.accountInfoUserControl = new FormsControlLibrary.AccountInformationUserControl(session.GetUser(), session.GetCompany(), session.GetPayrollStartDate(), session.GetPayrollEndDate());
                this.dashboardTabControl.Controls.Add(this.accountInfoTabPage);

                // 
                // accountInfoTabPage
                // 
                this.accountInfoTabPage.Controls.Add(this.accountInfoUserControl);
                this.accountInfoTabPage.Location = new System.Drawing.Point(22, 22);
                this.accountInfoTabPage.Name = ADMINISTRATOR_ACCOUNT_INFORMATION_TAB_NAME;
                this.accountInfoTabPage.Padding = new System.Windows.Forms.Padding(3);
                this.accountInfoTabPage.TabIndex = 0;
                this.accountInfoTabPage.Text = "Account Information";
                this.accountInfoTabPage.UseVisualStyleBackColor = true;

                if (accountInfoUserControl.Size.Width > dashboardSize.Width)
                {
                    dashboardSize.Width = accountInfoUserControl.Size.Width + SCROLL_BAR_WIDTH;
                }
                if (accountInfoUserControl.Size.Height > dashboardSize.Height)
                {
                    dashboardSize.Height = accountInfoUserControl.Size.Height;
                }
            }

            if (this.addDogUserControl != null)
            {
                this.addDogUserControl.Size = dashboardSize;
            }

            if (this.boardingUserControl != null)
            {
                this.boardingUserControl.Size = dashboardSize;
            }

            if (this.groomingUserControl != null)
            {
                this.groomingUserControl.Size = dashboardSize;
            }

            if (this.pickupDropoffUserControl != null)
            {
                this.pickupDropoffUserControl.Size = dashboardSize;
            }
            
            if (this.trainingUserControl != null)
            {
                this.trainingUserControl.Size = dashboardSize;
            }

            if (this.accountInfoUserControl != null)
            {
                this.accountInfoUserControl.Size = dashboardSize;
            }

            // offset the drawing vertical starting position for the Dashboard
            // User Control by 40 to accomodate the tool strip.
            this.Location = new System.Drawing.Point(4, 40);

            // add an offset to the width of the dashboard Tab Control to
            // accomodate the scrollbars in the usercontrols added to its
            // tab pages.
            this.dashboardTabControl.Width = dashboardSize.Width + 13;

            // add an offset to the height of the dashboard Tab Control to 
            // accomodate the tool strip.
            this.dashboardTabControl.Height = dashboardSize.Height + 30;

            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(3, dashboardTabControl.Height);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 1;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;

            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(deleteButton.Width + 15, dashboardTabControl.Height);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;

            // 
            // timeInButton
            // 
            this.timeInButton.Location = new System.Drawing.Point(3, dashboardTabControl.Height);
            this.timeInButton.Name = "timeInButton";
            this.timeInButton.Size = new System.Drawing.Size(75, 23);
            this.timeInButton.TabIndex = 1;
            this.timeInButton.Text = "Time In";
            this.timeInButton.UseVisualStyleBackColor = true;

            // 
            // timeOutButton
            // 
            this.timeOutButton.Location = new System.Drawing.Point(3, dashboardTabControl.Height);
            this.timeOutButton.Name = "timeOutButton";
            this.timeOutButton.Size = new System.Drawing.Size(75, 23);
            this.timeOutButton.TabIndex = 1;
            this.timeOutButton.Text = "Time Out";
            this.timeOutButton.UseVisualStyleBackColor = true;

            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.timeInButton);
            this.Controls.Add(this.timeOutButton);

            this.dashboardTabControl.SelectedIndexChanged += new EventHandler(dasboardTabControl_SelectedIndexChanged);
            this.timeInButton.Click += new EventHandler(timeInButton_Click);
            this.timeOutButton.Click += new EventHandler(timeOutButton_Click);
            this.saveButton.Click += new EventHandler(saveButton_Click);
            this.deleteButton.Click += new EventHandler(deleteButton_Click);

            dasboardTabControl_SelectedIndexChanged(this, null);
        }

        private void dasboardTabControl_SelectedIndexChanged(object sender, System.EventArgs e)  
        {
            TabPage selectedTab = dashboardTabControl.SelectedTab;
            
            if (TIME_CARD_TAB_NAME.Equals(selectedTab.Name))
            {
                this.deleteButton.Visible = false;
                this.saveButton.Visible = false;

                TimeCardRepository timeCardRepository = new TimeCardRepository();

                TimeCard mostRecentTimeCard = timeCardRepository.GetMostRecentTimeIn(
                    session.GetUser(),
                    session.GetPayrollStartDate(),
                    session.GetPayrollEndDate());

                // check if the most recent time card has a time out.
                if (mostRecentTimeCard != null && mostRecentTimeCard.TimeOut == DateTime.MinValue)
                {
                    // most recent time out is null, so disable time in button,
                    // and enable time out button.
                    this.timeInButton.Visible = false;
                    this.timeOutButton.Visible = true;
                }
                else
                {
                    // most recent time out has a value, so the user has signed 
                    // out her or his timecard and can now sign in a new time card
                    // entry.
                    this.timeOutButton.Visible = false;
                    this.timeInButton.Visible = true;
                }
            }
            else if (ADD_DOG_TAB_NAME.Equals(selectedTab.Name))
            {
                this.timeInButton.Visible = false;
                this.timeOutButton.Visible = false;
                this.deleteButton.Visible = false;
                this.saveButton.Visible = true;
            }
            else
            {
                this.timeInButton.Visible = false;
                this.timeOutButton.Visible = false;
                this.deleteButton.Visible = true;
                this.saveButton.Visible = true;
            }
        }

        private void timeInButton_Click(object sender, System.EventArgs e)
        {
            TimeCardRepository timeCardRepository = new TimeCardRepository();
            TimeCard timeCard = new TimeCard();
            timeCard.User = session.GetUser();
            timeCard.TimeIn = DateTime.Now;
            timeCardRepository.Add(timeCard);

            // disable time in button now that time in has been
            // entered, and enable time out button.
            this.timeInButton.Visible = false;
            this.timeOutButton.Visible = true;

            // call the time card user control's time out
            // button clicked method.
            timeCardUserControl.PayrollTracker_TimeOutButtonClickedEventHandler(this, e);
        }

        private void timeOutButton_Click(object sender, System.EventArgs e)
        {
            TimeCardRepository timeCardRepository = new TimeCardRepository();
            TimeCard timeCard = timeCardRepository.GetMostRecentTimeIn(session.GetUser(), session.GetPayrollStartDate(), session.GetPayrollEndDate());
            timeCard.TimeOut = DateTime.Now;
            timeCardRepository.Update(timeCard);

            // disable time out button now that time out has been
            // entered, and enable time in button.
            this.timeOutButton.Visible = false;
            this.timeInButton.Visible = true;

            // call the time card user control's time in
            // button clicked method.
            timeCardUserControl.PayrollTracker_TimeInButtonClickedEventHandler(this, e);
        }

        private void saveButton_Click(object sender, System.EventArgs e)
        {
            TabPage selectedTab = dashboardTabControl.SelectedTab;

            if (ADD_DOG_TAB_NAME.Equals(selectedTab.Name))
            {
                // Call Add Dog User Control's Save Button Clicked method
                // to handle any necessary operations for saving.
                addDogUserControl.PayrollTracker_SaveButtonClickedEventHandler(this, e);
            }
            else if (BOARDING_TAB_NAME.Equals(selectedTab.Name))
            {
                // Call Boarding User Control's Save Button Clicked method
                // to handle any necessary operations for saving.
                boardingUserControl.PayrollTracker_SaveButtonClickedEventHandler(this, e);
            }
            else if (GROOMING_TAB_NAME.Equals(selectedTab.Name))
            {
                // Call Grooming User Control's Save Button Clicked method
                // to handle any necessary operations for saving.
                groomingUserControl.PayrollTracker_SaveButtonClickedEventHandler(this, e);
            }
            else if (TRAINING_TAB_NAME.Equals(selectedTab.Name))
            {
                // Call Training User Control's Save Button Clicked method
                // to handle any necessary operations for saving.
                trainingUserControl.PayrollTracker_SaveButtonClickedEventHandler(this, e);
            }
            else if (PICKUP_DROPOFF_TAB_NAME.Equals(selectedTab.Name))
            {
                // Call PickupDropoff User Control's Save Button Clicked method
                // to handle any necessary operations for saving.
                pickupDropoffUserControl.PayrollTracker_SaveButtonClickedEventHandler(this, e);
            }
            else if (ADMINISTRATOR_ACCOUNT_INFORMATION_TAB_NAME.Equals(selectedTab.Name))
            {
                // Call Administrator Account Information User Control's Save 
                // Button Clicked method to handle any necessary operations for saving.
                accountInfoUserControl.PayrollTracker_SaveButtonClickedEventHandler(this, e);
            }
        }

        private void deleteButton_Click(object sender, System.EventArgs e)
        {
            TabPage selectedTab = dashboardTabControl.SelectedTab;

            if (BOARDING_TAB_NAME.Equals(selectedTab.Name))
            {
                // Call Boarding User Control's Delete Button Clicked method
                // to handle any necessary operations for saving.
                boardingUserControl.PayrollTracker_DeleteButtonClickedEventHandler(this, e);
            }
            else if (GROOMING_TAB_NAME.Equals(selectedTab.Name))
            {
                // Call Grooming User Control's Delete Button Clicked method
                // to handle any necessary operations for saving.
                groomingUserControl.PayrollTracker_DeleteButtonClickedEventHandler(this, e);
            }
            else if (TRAINING_TAB_NAME.Equals(selectedTab.Name))
            {
                // Call Training User Control's Delete Button Clicked method
                // to handle any necessary operations for saving.
                trainingUserControl.PayrollTracker_DeleteButtonClickedEventHandler(this, e);
            }
            else if (PICKUP_DROPOFF_TAB_NAME.Equals(selectedTab.Name))
            {
                // Call PickupDropoff User Control's Delete Button Clicked method
                // to handle any necessary operations for saving.
                pickupDropoffUserControl.PayrollTracker_DeleteButtonClickedEventHandler(this, e);
            }
        }

        private FormsControlLibrary.TimeCardUserControl timeCardUserControl;
        private System.Windows.Forms.TabPage timeCardTabPage;
        private FormsControlLibrary.BoardingUserControl boardingUserControl;
        private System.Windows.Forms.TabPage boardingTabPage;
        private FormsControlLibrary.DogUserControl addDogUserControl;
        private System.Windows.Forms.TabPage addDogTabPage;
        private FormsControlLibrary.GroomingUserControl groomingUserControl;
        private System.Windows.Forms.TabPage groomingTabPage;
        private FormsControlLibrary.PickupDropoffUserControl pickupDropoffUserControl;
        private System.Windows.Forms.TabPage pickupDropoffTabPage;
        private FormsControlLibrary.TrainingUserControl trainingUserControl;
        private System.Windows.Forms.TabPage trainingTabPage;
        private FormsControlLibrary.AccountInformationUserControl accountInfoUserControl;
        private System.Windows.Forms.TabPage accountInfoTabPage;

        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button timeInButton;
        private System.Windows.Forms.Button timeOutButton;

        private Session session;
    }
}

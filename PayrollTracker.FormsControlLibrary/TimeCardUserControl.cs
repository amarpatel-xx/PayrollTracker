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

namespace PayrollTracker.FormsControlLibrary
{
    public partial class TimeCardUserControl : UserControl
    {
        private System.Windows.Forms.Timer timer1;

        private User user;
        private DateTime payrollStartDate;
        private DateTime payrollEndDate;

        public TimeCardUserControl(User theUser, DateTime thePayrollStartDate, DateTime thePayrollEndDate)
        {
            InitializeComponent();

            this.user = theUser;
            this.payrollStartDate = thePayrollStartDate;
            this.payrollEndDate = thePayrollEndDate;

            this.dataGridView1.ReadOnly = true;

            // 
            // timer1
            // 
            this.timer1 = new System.Windows.Forms.Timer();
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            this.a1Panel2.BorderColor = System.Drawing.SystemColors.HighlightText;
            this.a1Panel2.GradientEndColor = System.Drawing.Color.Silver;
            this.a1Panel2.GradientStartColor = System.Drawing.Color.Silver;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.digitalDisplayControl2.BackColor = System.Drawing.Color.Transparent;
            this.digitalDisplayControl2.DigitColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            this.analogClock1.Time = DateTime.Now;
            this.digitalDisplayControl2.DigitText = DateTime.Now.ToString("hh:mm:ss");
        }

        protected override void OnLoad(EventArgs e)
        {
            refreshDisplayFromDatabase();
        }

        public void PayrollTracker_TimeInButtonClickedEventHandler(object sender, EventArgs e)
        {
            refreshDisplayFromDatabase();
        }

        public void PayrollTracker_TimeOutButtonClickedEventHandler(object sender, EventArgs e)
        {
            refreshDisplayFromDatabase();
        }

        private void refreshDisplayFromDatabase()
        {
            this.dataGridView1.Rows.Clear();

            TimeCardRepository timeCardRepository = new TimeCardRepository();
            IList<TimeCard> timeCards = timeCardRepository.GetCurrentTimeCards(user, payrollStartDate, payrollEndDate);

            foreach (TimeCard timeCard in timeCards)
            {
                int index = this.dataGridView1.Rows.Add();

                dataGridView1.Rows[index].Cells["TimeInColumn"].Value = timeCard.TimeIn.ToString();
                DateTime timeOut = timeCard.TimeOut;
                if (!timeOut.Equals(DateTime.MinValue))
                {
                    dataGridView1.Rows[index].Cells["TimeOutColumn"].Value = timeCard.TimeOut.ToString();
                }
            }
        }
    }
}

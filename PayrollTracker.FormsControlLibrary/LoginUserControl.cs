using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PayrollTracker.FormsControlLibrary
{
    public partial class LoginUserControl : UserControl
    {
        public event EventHandler<EventArgs> LoginButtonClickedEventHandler;

        public LoginUserControl(string companyName)
        {
            InitializeComponent();

            this.companyLabel.Text = companyName;
            
            //TODO: remove from code:
            usernameTextBox.Text = "teena";
            passwordTextBox.Text = "teena123";

            passwordTextBox.KeyDown += new KeyEventHandler(passwordTextBox_KeyDown);
        }

        public string Username()
        {
            return usernameTextBox.Text;
        }

        public string Password()
        {
            return passwordTextBox.Text;
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                // If the Enter key is pressed in the password text box,
                // submit the login information for validation.
                loginButton.PerformClick();
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (LoginButtonClickedEventHandler != null)
            {
                LoginButtonClickedEventHandler(this, e);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            usernameTextBox.Clear();
            passwordTextBox.Clear();
        }
    }
}

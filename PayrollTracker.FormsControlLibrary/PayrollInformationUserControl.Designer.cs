namespace PayrollTracker.FormsControlLibrary
{
    partial class PayrollInformationUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.usernameComboBox = new System.Windows.Forms.ComboBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.payrollFrequencyLabel = new System.Windows.Forms.Label();
            this.PayrollFrequencyValueLabel = new System.Windows.Forms.Label();
            this.payrollPeriodLabel = new System.Windows.Forms.Label();
            this.payrollPeriodComboBox = new System.Windows.Forms.ComboBox();
            this.updateEmployeePayrollInfoButton = new System.Windows.Forms.Button();
            this.generateExcelSpreadsheetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // usernameComboBox
            // 
            this.usernameComboBox.FormattingEnabled = true;
            this.usernameComboBox.Location = new System.Drawing.Point(182, 3);
            this.usernameComboBox.Name = "usernameComboBox";
            this.usernameComboBox.Size = new System.Drawing.Size(200, 21);
            this.usernameComboBox.TabIndex = 12;
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(24, 6);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(55, 13);
            this.usernameLabel.TabIndex = 11;
            this.usernameLabel.Text = "Username";
            // 
            // payrollFrequencyLabel
            // 
            this.payrollFrequencyLabel.AutoSize = true;
            this.payrollFrequencyLabel.Location = new System.Drawing.Point(24, 27);
            this.payrollFrequencyLabel.Name = "payrollFrequencyLabel";
            this.payrollFrequencyLabel.Size = new System.Drawing.Size(91, 13);
            this.payrollFrequencyLabel.TabIndex = 13;
            this.payrollFrequencyLabel.Text = "Payroll Frequency";
            // 
            // PayrollFrequencyValueLabel
            // 
            this.PayrollFrequencyValueLabel.AutoSize = true;
            this.PayrollFrequencyValueLabel.Location = new System.Drawing.Point(179, 27);
            this.PayrollFrequencyValueLabel.Name = "PayrollFrequencyValueLabel";
            this.PayrollFrequencyValueLabel.Size = new System.Drawing.Size(49, 13);
            this.PayrollFrequencyValueLabel.TabIndex = 14;
            this.PayrollFrequencyValueLabel.Text = "Biweekly";
            // 
            // payrollPeriodLabel
            // 
            this.payrollPeriodLabel.AutoSize = true;
            this.payrollPeriodLabel.Location = new System.Drawing.Point(24, 46);
            this.payrollPeriodLabel.Name = "payrollPeriodLabel";
            this.payrollPeriodLabel.Size = new System.Drawing.Size(71, 13);
            this.payrollPeriodLabel.TabIndex = 15;
            this.payrollPeriodLabel.Text = "Payroll Period";
            // 
            // payrollPeriodComboBox
            // 
            this.payrollPeriodComboBox.FormattingEnabled = true;
            this.payrollPeriodComboBox.Location = new System.Drawing.Point(182, 43);
            this.payrollPeriodComboBox.Name = "payrollPeriodComboBox";
            this.payrollPeriodComboBox.Size = new System.Drawing.Size(200, 21);
            this.payrollPeriodComboBox.TabIndex = 16;
            // 
            // updateEmployeePayrollInfoButton
            // 
            this.updateEmployeePayrollInfoButton.Location = new System.Drawing.Point(3, 80);
            this.updateEmployeePayrollInfoButton.Name = "updateEmployeePayrollInfoButton";
            this.updateEmployeePayrollInfoButton.Size = new System.Drawing.Size(225, 23);
            this.updateEmployeePayrollInfoButton.TabIndex = 17;
            this.updateEmployeePayrollInfoButton.Text = "View/Update Employee Payroll Information";
            this.updateEmployeePayrollInfoButton.UseVisualStyleBackColor = true;
            // 
            // generateExcelSpreadsheetButton
            // 
            this.generateExcelSpreadsheetButton.Location = new System.Drawing.Point(234, 80);
            this.generateExcelSpreadsheetButton.Name = "generateExcelSpreadsheetButton";
            this.generateExcelSpreadsheetButton.Size = new System.Drawing.Size(158, 23);
            this.generateExcelSpreadsheetButton.TabIndex = 18;
            this.generateExcelSpreadsheetButton.Text = "Generate Spreadsheet";
            this.generateExcelSpreadsheetButton.UseVisualStyleBackColor = true;
            // 
            // PayrollInformationTabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.generateExcelSpreadsheetButton);
            this.Controls.Add(this.updateEmployeePayrollInfoButton);
            this.Controls.Add(this.payrollPeriodComboBox);
            this.Controls.Add(this.payrollPeriodLabel);
            this.Controls.Add(this.PayrollFrequencyValueLabel);
            this.Controls.Add(this.payrollFrequencyLabel);
            this.Controls.Add(this.usernameComboBox);
            this.Controls.Add(this.usernameLabel);
            this.Name = "PayrollInformationTabPage";
            this.Size = new System.Drawing.Size(397, 107);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox usernameComboBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label payrollFrequencyLabel;
        private System.Windows.Forms.Label PayrollFrequencyValueLabel;
        private System.Windows.Forms.Label payrollPeriodLabel;
        private System.Windows.Forms.ComboBox payrollPeriodComboBox;
        private System.Windows.Forms.Button updateEmployeePayrollInfoButton;
        private System.Windows.Forms.Button generateExcelSpreadsheetButton;
    }
}

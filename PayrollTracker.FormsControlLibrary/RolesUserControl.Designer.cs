namespace PayrollTracker.FormsControlLibrary
{
    partial class RolesUserControl
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
            this.availableRolesListBox = new System.Windows.Forms.ListBox();
            this.actualRolesListBox = new System.Windows.Forms.ListBox();
            this.selectAllButton = new System.Windows.Forms.Button();
            this.selectButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.removeAllButton = new System.Windows.Forms.Button();
            this.availableRolesLabel = new System.Windows.Forms.Label();
            this.actualRolesLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // availableRolesListBox
            // 
            this.availableRolesListBox.FormattingEnabled = true;
            this.availableRolesListBox.Location = new System.Drawing.Point(22, 32);
            this.availableRolesListBox.Name = "availableRolesListBox";
            this.availableRolesListBox.Size = new System.Drawing.Size(120, 134);
            this.availableRolesListBox.TabIndex = 0;
            // 
            // actualRolesListBox
            // 
            this.actualRolesListBox.FormattingEnabled = true;
            this.actualRolesListBox.Location = new System.Drawing.Point(318, 32);
            this.actualRolesListBox.Name = "actualRolesListBox";
            this.actualRolesListBox.Size = new System.Drawing.Size(120, 134);
            this.actualRolesListBox.TabIndex = 1;
            // 
            // selectAllButton
            // 
            this.selectAllButton.Location = new System.Drawing.Point(183, 40);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(91, 23);
            this.selectAllButton.TabIndex = 2;
            this.selectAllButton.Text = ">> | Select All";
            this.selectAllButton.UseVisualStyleBackColor = true;
            this.selectAllButton.Click += new System.EventHandler(this.selectAllButton_Click);
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(183, 69);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(91, 23);
            this.selectButton.TabIndex = 3;
            this.selectButton.Text = "> Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(183, 98);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(91, 23);
            this.removeButton.TabIndex = 4;
            this.removeButton.Text = "< Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // removeAllButton
            // 
            this.removeAllButton.Location = new System.Drawing.Point(183, 127);
            this.removeAllButton.Name = "removeAllButton";
            this.removeAllButton.Size = new System.Drawing.Size(91, 23);
            this.removeAllButton.TabIndex = 5;
            this.removeAllButton.Text = "| << Remove All";
            this.removeAllButton.UseVisualStyleBackColor = true;
            this.removeAllButton.Click += new System.EventHandler(this.removeAllButton_Click);
            // 
            // availableRolesLabel
            // 
            this.availableRolesLabel.AutoSize = true;
            this.availableRolesLabel.Location = new System.Drawing.Point(22, 13);
            this.availableRolesLabel.Name = "availableRolesLabel";
            this.availableRolesLabel.Size = new System.Drawing.Size(80, 13);
            this.availableRolesLabel.TabIndex = 6;
            this.availableRolesLabel.Text = "Available Roles";
            // 
            // actualRolesLabel
            // 
            this.actualRolesLabel.AutoSize = true;
            this.actualRolesLabel.Location = new System.Drawing.Point(318, 13);
            this.actualRolesLabel.Name = "actualRolesLabel";
            this.actualRolesLabel.Size = new System.Drawing.Size(67, 13);
            this.actualRolesLabel.TabIndex = 7;
            this.actualRolesLabel.Text = "Actual Roles";
            // 
            // RolesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.actualRolesLabel);
            this.Controls.Add(this.availableRolesLabel);
            this.Controls.Add(this.removeAllButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.selectAllButton);
            this.Controls.Add(this.actualRolesListBox);
            this.Controls.Add(this.availableRolesListBox);
            this.Name = "RolesUserControl";
            this.Size = new System.Drawing.Size(459, 179);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox availableRolesListBox;
        private System.Windows.Forms.ListBox actualRolesListBox;
        private System.Windows.Forms.Button selectAllButton;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button removeAllButton;
        private System.Windows.Forms.Label availableRolesLabel;
        private System.Windows.Forms.Label actualRolesLabel;

    }
}

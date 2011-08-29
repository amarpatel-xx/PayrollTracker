namespace PayrollTracker
{
    partial class PayrollTracker
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PayrollTracker));
            this.logoutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.welcomeToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.SuspendLayout();

            // 
            // logoutToolStripButton
            // 
            this.logoutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.logoutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("LogOutImage")));
            this.logoutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.logoutToolStripButton.Name = "logoutToolStripButton";
            this.logoutToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.logoutToolStripButton.Text = "Log Out";
            this.logoutToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            // 
            // welcomeToolStripLabel
            // 
            this.welcomeToolStripLabel.Name = "welcomeToolStripLabel";
            this.welcomeToolStripLabel.Size = new System.Drawing.Size(0, 22);
            // 
            // PayrollTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(649, 547);
            this.Name = "PayrollTracker";
            this.Text = "PayrollTracker";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripButton logoutToolStripButton;
        private System.Windows.Forms.ToolStripLabel welcomeToolStripLabel;

    }
}
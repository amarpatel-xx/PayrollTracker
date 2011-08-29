namespace PayrollTracker.FormsControlLibrary
{
    partial class BoardingUserControl
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SelectColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DateColumn = new PayrollTracker.FormsControlLibrary.DataGridViewControls.CalendarColumn();
            this.DogNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DaycareOrNonDaycareColumn = new PayrollTracker.FormsControlLibrary.DataGridViewControls.RadioButtonColumn();
            this.BoardingRateColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SundayDaycareColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TipColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectColumn,
            this.DateColumn,
            this.DogNameColumn,
            this.DaycareOrNonDaycareColumn,
            this.BoardingRateColumn,
            this.SundayDaycareColumn,
            this.TipColumn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(781, 187);
            this.dataGridView1.TabIndex = 0;
            // 
            // SelectColumn
            // 
            this.SelectColumn.HeaderText = "Select";
            this.SelectColumn.Name = "SelectColumn";
            // 
            // Date
            // 
            this.DateColumn.HeaderText = "Date";
            this.DateColumn.Name = "DateColumn";
            // 
            // DogNameColumn
            // 
            this.DogNameColumn.HeaderText = "Dog\'s Name";
            this.DogNameColumn.Name = "DogNameColumn";
            // 
            // DaycareOrNonDaycareColumn
            // 
            this.DaycareOrNonDaycareColumn.HeaderText = "Daycare/Non-daycare";
            this.DaycareOrNonDaycareColumn.Name = "DaycareOrNonDaycareColumn";
            // 
            // BoardingRateColumn
            // 
            this.BoardingRateColumn.HeaderText = "Boarding Rate";
            this.BoardingRateColumn.Name = "BoardingRateColumn";
            // 
            // SundayDaycareColumn
            // 
            this.SundayDaycareColumn.HeaderText = "Sunday Daycare";
            this.SundayDaycareColumn.Name = "SundayDaycareColumn";
            // 
            // TipColumn
            // 
            this.TipColumn.HeaderText = "Tip";
            this.TipColumn.Name = "TipColumn";
            // 
            // BoardingUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.dataGridView1);
            this.Name = "BoardingUserControl";
            this.Size = new System.Drawing.Size(781, 187);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectColumn;
        private PayrollTracker.FormsControlLibrary.DataGridViewControls.CalendarColumn DateColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn DogNameColumn;
        private PayrollTracker.FormsControlLibrary.DataGridViewControls.RadioButtonColumn DaycareOrNonDaycareColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn BoardingRateColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn SundayDaycareColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipColumn;
    }
}

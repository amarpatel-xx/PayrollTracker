namespace PayrollTracker.FormsControlLibrary
{
    partial class TrainingUserControl
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
            this.ClassColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CostOfClassColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PreK9DaycareCostColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
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
            this.ClassColumn,
            this.CostOfClassColumn,
            this.PreK9DaycareCostColumn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(672, 278);
            this.dataGridView1.TabIndex = 0;
            // 
            // SelectColumn
            // 
            this.SelectColumn.HeaderText = "Select";
            this.SelectColumn.Name = "SelectColumn";
            // 
            // DateColumn
            // 
            this.DateColumn.HeaderText = "Date";
            this.DateColumn.Name = "DateColumn";
            // 
            // DogNameColumn
            // 
            this.DogNameColumn.HeaderText = "Dog\'s Name";
            this.DogNameColumn.Name = "DogNameColumn";
            // 
            // ClassColumn
            // 
            this.ClassColumn.HeaderText = "Class";
            this.ClassColumn.Name = "ClassColumn";
            // 
            // CostOfClassColumn
            // 
            this.CostOfClassColumn.HeaderText = "Cost of Class";
            this.CostOfClassColumn.Name = "CostOfClassColumn";
            // 
            // PreK9DaycareCostColumn
            // 
            this.PreK9DaycareCostColumn.HeaderText = "Pre K9 Daycare Cost";
            this.PreK9DaycareCostColumn.Name = "PreK9DaycareCostColumn";
            // 
            // TrainingUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.dataGridView1);
            this.Name = "TrainingUserControl";
            this.Size = new System.Drawing.Size(672, 278);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectColumn;
        private PayrollTracker.FormsControlLibrary.DataGridViewControls.CalendarColumn DateColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn DogNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ClassColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn CostOfClassColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn PreK9DaycareCostColumn;
    }
}

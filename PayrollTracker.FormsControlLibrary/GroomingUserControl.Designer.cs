namespace PayrollTracker.FormsControlLibrary
{
    partial class GroomingUserControl
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
            this.groomingDataGridView = new System.Windows.Forms.DataGridView();
            this.SelectColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DateColumn = new PayrollTracker.FormsControlLibrary.DataGridViewControls.CalendarColumn();
            this.DogNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.GroomTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CostColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.groomingDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groomingDataGridView
            // 
            this.groomingDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.groomingDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.groomingDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.groomingDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectColumn,
            this.DateColumn,
            this.DogNameColumn,
            this.GroomTypeColumn,
            this.CostColumn,
            this.TipColumn});
            this.groomingDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groomingDataGridView.Location = new System.Drawing.Point(0, 0);
            this.groomingDataGridView.Name = "groomingDataGridView";
            this.groomingDataGridView.Size = new System.Drawing.Size(651, 185);
            this.groomingDataGridView.TabIndex = 0;
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
            // GroomTypeColumn
            // 
            this.GroomTypeColumn.HeaderText = "Groom Type";
            this.GroomTypeColumn.Name = "GroomTypeColumn";
            // 
            // CostColumn
            // 
            this.CostColumn.HeaderText = "Cost";
            this.CostColumn.Name = "CostColumn";
            // 
            // TipColumn
            // 
            this.TipColumn.HeaderText = "Tip";
            this.TipColumn.Name = "TipColumn";
            // 
            // GroomingUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.groomingDataGridView);
            this.Name = "GroomingUserControl";
            this.Size = new System.Drawing.Size(651, 185);
            ((System.ComponentModel.ISupportInitialize)(this.groomingDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView groomingDataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectColumn;
        private PayrollTracker.FormsControlLibrary.DataGridViewControls.CalendarColumn DateColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn DogNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn GroomTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipColumn;
    }
}

namespace PayrollTracker.FormsControlLibrary
{
    partial class TimeCardUserControl
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
            this.a1Panel2 = new PayrollTracker.FormsControlLibrary.DigitalClock.Controls.A1Panel();
            this.digitalDisplayControl2 = new PayrollTracker.FormsControlLibrary.DigitalClock.Controls.DigitalDisplayControl();
            this.analogClock1 = new PayrollTracker.FormsControlLibrary.AnalogClockControl.AnalogClock();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.TimeInColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeOutColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.a1Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // a1Panel2
            // 
            this.a1Panel2.BorderColor = System.Drawing.Color.Gray;
            this.a1Panel2.Controls.Add(this.digitalDisplayControl2);
            this.a1Panel2.GradientEndColor = System.Drawing.Color.Gray;
            this.a1Panel2.GradientStartColor = System.Drawing.Color.White;
            this.a1Panel2.Image = null;
            this.a1Panel2.ImageLocation = new System.Drawing.Point(4, 4);
            this.a1Panel2.Location = new System.Drawing.Point(5, 277);
            this.a1Panel2.Name = "a1Panel2";
            this.a1Panel2.Size = new System.Drawing.Size(316, 100);
            this.a1Panel2.TabIndex = 2;
            // 
            // digitalDisplayControl2
            // 
            this.digitalDisplayControl2.BackColor = System.Drawing.Color.Transparent;
            this.digitalDisplayControl2.DigitColor = System.Drawing.Color.GreenYellow;
            this.digitalDisplayControl2.Location = new System.Drawing.Point(22, 0);
            this.digitalDisplayControl2.Name = "digitalDisplayControl2";
            this.digitalDisplayControl2.Size = new System.Drawing.Size(213, 94);
            this.digitalDisplayControl2.TabIndex = 0;
            // 
            // analogClock1
            // 
            this.analogClock1.DrawHourHand = true;
            this.analogClock1.DrawHourHandShadow = true;
            this.analogClock1.DrawMinuteHand = true;
            this.analogClock1.DrawMinuteHandShadow = true;
            this.analogClock1.DrawSecondHand = true;
            this.analogClock1.DropShadowColor = System.Drawing.Color.Black;
            this.analogClock1.DropShadowOffset = new System.Drawing.Point(0, 0);
            this.analogClock1.FaceColorHigh = System.Drawing.Color.RoyalBlue;
            this.analogClock1.FaceColorLow = System.Drawing.Color.SkyBlue;
            this.analogClock1.FaceGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.analogClock1.FaceImage = null;
            this.analogClock1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analogClock1.HourHandColor = System.Drawing.Color.Gainsboro;
            this.analogClock1.HourHandDropShadowColor = System.Drawing.Color.Gray;
            this.analogClock1.Location = new System.Drawing.Point(40, 19);
            this.analogClock1.MinuteHandColor = System.Drawing.Color.WhiteSmoke;
            this.analogClock1.MinuteHandDropShadowColor = System.Drawing.Color.Gray;
            this.analogClock1.MinuteHandTickStyle = PayrollTracker.FormsControlLibrary.AnalogClockControl.TickStyle.Normal;
            this.analogClock1.Name = "analogClock1";
            this.analogClock1.NumeralColor = System.Drawing.Color.WhiteSmoke;
            this.analogClock1.RimColorHigh = System.Drawing.Color.RoyalBlue;
            this.analogClock1.RimColorLow = System.Drawing.Color.SkyBlue;
            this.analogClock1.RimGradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.analogClock1.SecondHandColor = System.Drawing.Color.Tomato;
            this.analogClock1.SecondHandDropShadowColor = System.Drawing.Color.Gray;
            this.analogClock1.SecondHandEndCap = System.Drawing.Drawing2D.LineCap.Round;
            this.analogClock1.SecondHandTickStyle = PayrollTracker.FormsControlLibrary.AnalogClockControl.TickStyle.Normal;
            this.analogClock1.Size = new System.Drawing.Size(232, 232);
            this.analogClock1.TabIndex = 0;
            this.analogClock1.Time = new System.DateTime(((long)(0)));
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TimeInColumn,
            this.TimeOutColumn});
            this.dataGridView1.Location = new System.Drawing.Point(327, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(370, 352);
            this.dataGridView1.TabIndex = 3;
            // 
            // TimeInColumn
            // 
            this.TimeInColumn.HeaderText = "Time In";
            this.TimeInColumn.Name = "TimeInColumn";
            // 
            // TimeOutColumn
            // 
            this.TimeOutColumn.HeaderText = "Time Out";
            this.TimeOutColumn.Name = "TimeOutColumn";
            // 
            // TimeCardUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.a1Panel2);
            this.Controls.Add(this.analogClock1);
            this.Name = "TimeCardUserControl";
            this.Size = new System.Drawing.Size(700, 380);
            this.a1Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PayrollTracker.FormsControlLibrary.AnalogClockControl.AnalogClock analogClock1;
        private PayrollTracker.FormsControlLibrary.DigitalClock.Controls.A1Panel a1Panel2;
        private PayrollTracker.FormsControlLibrary.DigitalClock.Controls.DigitalDisplayControl digitalDisplayControl2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeInColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeOutColumn;
    }
}

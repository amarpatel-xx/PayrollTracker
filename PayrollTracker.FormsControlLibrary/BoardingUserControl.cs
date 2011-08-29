using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PayrollTracker.ModelClassLibrary.Repositories;
using PayrollTracker.ModelClassLibrary.Domain;

namespace PayrollTracker.FormsControlLibrary
{
    public partial class BoardingUserControl : UserControl
    {
        private const string BOARDING_RATE_COST_TYPE = "Boarding - Rate";
        private const string SUNDAY_DAYCARE_COST_TYPE = "Boarding - Sunday Daycare";
        private System.Windows.Forms.DataGridViewTextBoxColumn BoardingIdColumn;

        private Dictionary<int, Boarding> dirtyBoardingsMap = new Dictionary<int, Boarding>();
        private Dictionary<int, Boarding> deleteBoardingsMap = new Dictionary<int, Boarding>();

        private User user;
        private DateTime payrollStartDate;
        private DateTime payrollEndDate;

        public BoardingUserControl(User theUser, DateTime thePayrollStartDate, DateTime thePayrollEndDate)
        {
            InitializeComponent();
            this.BoardingIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BoardingIdColumn.HeaderText = "BoardingIdColumn";
            this.BoardingIdColumn.Name = "BoardingIdColumn";
            this.dataGridView1.Columns.Add(BoardingIdColumn);
            dataGridView1.Columns["BoardingIdColumn"].Visible = false;

            this.user = theUser;
            this.payrollStartDate = thePayrollStartDate;
            this.payrollEndDate = thePayrollEndDate;

            //this.dataGridView1.RowsAdded += new DataGridViewRowsAddedEventHandler(BoardingUserControl_Load);
            this.DaycareOrNonDaycareColumn.Items.AddRange(new object[] {
            "Daycare",
            "Non-daycare"});
            this.DaycareOrNonDaycareColumn.Width = 130;
            this.Load += new System.EventHandler(this.BoardingUserControl_Load);
            this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            ((DataGridViewControls.RadioButtonColumn)this.dataGridView1.Columns[3]).NotifyItemsCollectionChanged();

            this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
        }

        private void BoardingUserControl_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoResizeRows();

            DogRepository dogRepository = new DogRepository();
            IList<Dog> dataSourceDogsList = dogRepository.GetAllDogs();

            DogNameColumn.DataSource = dataSourceDogsList;
            DogNameColumn.DisplayMember = "FullName";
            DogNameColumn.ValueMember = "DogId";

            CostTypeRepository costTypeRepository = new CostTypeRepository();
            CostType boardingRateCostType = costTypeRepository.GetByCostName(BOARDING_RATE_COST_TYPE);

            IList<Cost> possibleCosts1 = boardingRateCostType.PossibleCosts;
            ArrayList.Adapter((IList)possibleCosts1).Sort();

            BoardingRateColumn.DataSource = possibleCosts1;

            BoardingRateColumn.DisplayMember = "CostValue";
            BoardingRateColumn.ValueMember = "CostId";

            CostType sundayDaycareCostType = costTypeRepository.GetByCostName(SUNDAY_DAYCARE_COST_TYPE);

            IList<Cost> possibleCosts2 = sundayDaycareCostType.PossibleCosts;
            ArrayList.Adapter((IList)possibleCosts2).Sort();

            SundayDaycareColumn.DataSource = possibleCosts2;

            SundayDaycareColumn.DisplayMember = "CostValue";
            SundayDaycareColumn.ValueMember = "CostId";

            refreshDisplayFromDatabase();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                if (dirtyBoardingsMap.Keys.Contains(e.RowIndex))
                {
                    dirtyBoardingsMap.Remove(e.RowIndex);
                }

                Boarding boarding = new Boarding();

                DateTime date = (DateTime)dataGridView1.Rows[e.RowIndex].Cells["DateColumn"].Value;
                boarding.Date = date;

                string daycareNonDaycare = (string)dataGridView1.Rows[e.RowIndex].Cells["DaycareOrNonDaycareColumn"].Value;
                bool isDaycare = "Daycare".Equals(daycareNonDaycare) ? true : false;
                boarding.IsDaycare = isDaycare;

                string dogId = (string)dataGridView1.Rows[e.RowIndex].Cells["DogNameColumn"].Value;
                DogRepository dogRepository = new DogRepository();
                if (dogId != null)
                {
                    Dog dog = dogRepository.GetById(dogId);
                    boarding.Dog = dog;
                }

                CostRepository costRepository = new CostRepository();
                string boardingCostId = (string)dataGridView1.Rows[e.RowIndex].Cells["BoardingRateColumn"].Value;
                if (boardingCostId != null)
                {
                    Cost boardingCost = costRepository.GetById(boardingCostId);
                    boarding.BoardingCost = boardingCost;
                }

                string sundayDaycareCostId = (string)dataGridView1.Rows[e.RowIndex].Cells["SundayDaycareColumn"].Value;
                if (sundayDaycareCostId != null)
                {
                    Cost sundayDaycareCost = costRepository.GetById(sundayDaycareCostId);
                    boarding.SundayDaycareCost = sundayDaycareCost;
                }

                string tipStr = (string)dataGridView1.Rows[e.RowIndex].Cells["TipColumn"].Value;
                try
                {
                    Double tip = Convert.ToDouble(tipStr);
                    boarding.Tip = tip;
                }
                catch (FormatException exception)
                {
                    //Catch this exception quietly for now.
                }

                boarding.User = user;

                string boardingId = (string)dataGridView1.Rows[e.RowIndex].Cells["BoardingIdColumn"].Value;

                boarding.BoardingId = boardingId;

                dirtyBoardingsMap.Add(e.RowIndex, boarding);

                // Remove the entry from the delete map, if
                // an entry for the Boarding exists in the
                // delete map already.
                if (deleteBoardingsMap.Keys.Contains(e.RowIndex))
                {
                    deleteBoardingsMap.Remove(e.RowIndex);
                }

                var isSelected = dataGridView1.Rows[e.RowIndex].Cells["SelectColumn"].Value;

                if (isSelected != null && (bool)isSelected)
                {
                    deleteBoardingsMap.Add(e.RowIndex, boarding);
                }
            }
        }

        public void PayrollTracker_SaveButtonClickedEventHandler(object sender, EventArgs e)
        {
            BoardingRepository boardingRepository = new BoardingRepository();
            foreach (KeyValuePair<int, Boarding> entry in dirtyBoardingsMap)
            {
                Boarding boarding = entry.Value;

                bool isValid = dataGridView1_RowValidating(entry.Key, boarding);

                if (isValid)
                {
                    if (boarding.BoardingId == null)
                    {
                        boardingRepository.Add(boarding);
                    }
                    else
                    {
                        boardingRepository.Update(boarding);
                    }

                    dirtyBoardingsMap = new Dictionary<int, Boarding>();
                    refreshDisplayFromDatabase();
                }
            }
        }

        public void PayrollTracker_DeleteButtonClickedEventHandler(object sender, EventArgs e)
        {
            BoardingRepository boardingRepository = new BoardingRepository();
            foreach (KeyValuePair<int, Boarding> entry in deleteBoardingsMap)
            {
                Boarding boarding = entry.Value;

                if (boarding.BoardingId != null)
                {
                    boardingRepository.Remove(boarding);
                }
            }

            deleteBoardingsMap = new Dictionary<int, Boarding>();
            refreshDisplayFromDatabase();
        }

        private void refreshDisplayFromDatabase()
        {
            this.dataGridView1.Rows.Clear();

            BoardingRepository boardingRepository = new BoardingRepository();
            IList<Boarding> boardingsList = boardingRepository.GetRecentBoardings(user, payrollStartDate, payrollEndDate);

            foreach (Boarding boarding in boardingsList)
            {
                int index = this.dataGridView1.Rows.Add();

                dataGridView1.Rows[index].Cells["DateColumn"].Value = boarding.Date;
                dataGridView1.Rows[index].Cells["DogNameColumn"].Value = boarding.Dog.DogId;

                string daycareOrNonDaycare = "Non-daycare";
                if (boarding.IsDaycare)
                {
                    daycareOrNonDaycare = "Daycare";
                }
                dataGridView1.Rows[index].Cells["DaycareOrNonDaycareColumn"].Value = daycareOrNonDaycare;

                dataGridView1.Rows[index].Cells["BoardingRateColumn"].Value = boarding.BoardingCost.CostId;

                if (boarding.SundayDaycareCost != null)
                {
                    dataGridView1.Rows[index].Cells["SundayDaycareColumn"].Value = boarding.SundayDaycareCost.CostId;
                }

                dataGridView1.Rows[index].Cells["TipColumn"].Value = boarding.Tip.ToString();

                dataGridView1.Rows[index].Cells["BoardingIdColumn"].Value = boarding.BoardingId;
            }
        }

        public void dogAddedOrUpdated(object sender, System.EventArgs e)
        {
            DogRepository dogRepository = new DogRepository();
            IList<Dog> dataSourceDogsList = dogRepository.GetAllDogs();

            // Update the DogName column. This updates 
            // after a dog's name is modified or a new dog is added.
            DogNameColumn.DataSource = dataSourceDogsList;
        }

        private bool dataGridView1_RowValidating(int index, Boarding boarding)
        {
            if (boarding.Date < payrollStartDate || boarding.Date > payrollEndDate)
            {
                dataGridView1.Rows[index].ErrorText = "Boarding Date must be for current payroll period (" + payrollStartDate + " - " + payrollEndDate + ").";
                return false;
            }

            if (boarding.Dog == null)
            {
                dataGridView1.Rows[index].ErrorText = "Dog's Name must not be empty.";
                return false;
            }
            else if (dataGridView1.Rows[index].Cells["DaycareOrNonDaycareColumn"].Value == null)
            {
                dataGridView1.Rows[index].ErrorText = "Daycare/Non-daycare must not be empty.";
                return false;
            }
            else if (boarding.BoardingCost == null)
            {
                dataGridView1.Rows[index].ErrorText = "Boarding Rate must not be empty.";
                return false;
            }

            if (dataGridView1.Rows[index].Cells["TipColumn"].Value != null)
            {
                string value = dataGridView1.Rows[index].Cells["TipColumn"].Value.ToString();

                try
                {
                    Double tip = Convert.ToDouble(value);
                }
                catch (FormatException exception)
                {
                    dataGridView1.Rows[index].ErrorText = "Tip must be numeric.";
                    return false;
                }
            }

            return true;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the row error in case the user presses ESC.   
            dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
        }
    }
}

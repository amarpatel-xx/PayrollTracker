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
    public partial class PickupDropoffUserControl : UserControl
    {
        private Dictionary<int, PickupDropoff> dirtyObjectsMap = new Dictionary<int, PickupDropoff>();
        private Dictionary<int, PickupDropoff> deleteObjectsMap = new Dictionary<int, PickupDropoff>();

        private User user;
        private DateTime payrollStartDate;
        private DateTime payrollEndDate;

        private System.Windows.Forms.DataGridViewTextBoxColumn PickupDropoffIdColumn;

        private const string PICKUP_COST_TYPE = "PickupDropoff - Pickup";
        private const string DROPOFF_COST_TYPE = "PickupDropoff - Dropoff";

        public PickupDropoffUserControl(User theUser, DateTime thePayrollStartDate, DateTime thePayrollEndDate)
        {
            InitializeComponent();

            this.user = theUser;
            this.payrollStartDate = thePayrollStartDate;
            this.payrollEndDate = thePayrollEndDate;

            this.PickupDropoffIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PickupDropoffIdColumn.HeaderText = "PickupDropoffIdColumn";
            this.PickupDropoffIdColumn.Name = "PickupDropoffIdColumn";
            this.dataGridView1.Columns.Add(PickupDropoffIdColumn);
            dataGridView1.Columns["PickupDropoffIdColumn"].Visible = false;

            this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            this.dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
        }

        protected override void OnLoad(EventArgs e)
        {
            initializeDataGridComponentsDatasources();
            refreshDisplayFromDatabase();
        }

        private void initializeDataGridComponentsDatasources()
        {
            DogRepository dogRepository = new DogRepository();
            IList<Dog> dataSourceDogsList = dogRepository.GetAllDogs();

            DogNameColumn.DataSource = dataSourceDogsList;
            DogNameColumn.DisplayMember = "FullName";
            DogNameColumn.ValueMember = "DogId";

            CostTypeRepository costTypeRepository = new CostTypeRepository();
            CostType pickupCostType = costTypeRepository.GetByCostName(PICKUP_COST_TYPE);

            IList<Cost> possibleCosts1 = pickupCostType.PossibleCosts;
            ArrayList.Adapter((IList)possibleCosts1).Sort();

            PickupCostColumn.DataSource = possibleCosts1;
            PickupCostColumn.DisplayMember = "CostValue";
            PickupCostColumn.ValueMember = "CostId";

            CostType dropoffCostType = costTypeRepository.GetByCostName(DROPOFF_COST_TYPE);

            IList<Cost> possibleCosts2 = dropoffCostType.PossibleCosts;
            ArrayList.Adapter((IList)possibleCosts2).Sort();

            DropoffCostColumn.DataSource = possibleCosts2;
            DropoffCostColumn.DisplayMember = "CostValue";
            DropoffCostColumn.ValueMember = "CostId";
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                if (dirtyObjectsMap.Keys.Contains(e.RowIndex))
                {
                    dirtyObjectsMap.Remove(e.RowIndex);
                }

                PickupDropoff pickupDropoff = new PickupDropoff();

                DateTime date = (DateTime)dataGridView1.Rows[e.RowIndex].Cells["DateColumn"].Value;
                pickupDropoff.Date = date;

                string dogId = (string)dataGridView1.Rows[e.RowIndex].Cells["DogNameColumn"].Value;
                DogRepository dogRepository = new DogRepository();
                if (dogId != null)
                {
                    Dog dog = dogRepository.GetById(dogId);
                    pickupDropoff.Dog = dog;
                }

                CostRepository costRepository = new CostRepository();
                string pickupCostId = (string)dataGridView1.Rows[e.RowIndex].Cells["PickupCostColumn"].Value;
                if (pickupCostId != null)
                {
                    Cost cost = costRepository.GetById(pickupCostId);
                    pickupDropoff.PickupCost = cost;
                }

                string dropoffCostId = (string)dataGridView1.Rows[e.RowIndex].Cells["DropoffCostColumn"].Value;
                if (dropoffCostId != null)
                {
                    Cost cost = costRepository.GetById(dropoffCostId);
                    pickupDropoff.DropoffCost = cost;
                }

                pickupDropoff.User = user;

                string pickupDropoffId = (string)dataGridView1.Rows[e.RowIndex].Cells["PickupDropoffIdColumn"].Value;

                pickupDropoff.PickupDropoffId = pickupDropoffId;

                // Add object to dirty objects map.
                dirtyObjectsMap.Add(e.RowIndex, pickupDropoff);

                // Remove the entry from the delete map, if
                // an entry for the Daycare exists in the
                // delete map already.
                if (deleteObjectsMap.Keys.Contains(e.RowIndex))
                {
                    deleteObjectsMap.Remove(e.RowIndex);
                }

                var isSelected = dataGridView1.Rows[e.RowIndex].Cells["SelectColumn"].Value;

                if (isSelected != null && (bool)isSelected)
                {
                    deleteObjectsMap.Add(e.RowIndex, pickupDropoff);
                }
            }
        }

        public void PayrollTracker_DeleteButtonClickedEventHandler(object sender, EventArgs e)
        {
            PickupDropoffRepository repository = new PickupDropoffRepository();
            foreach (KeyValuePair<int, PickupDropoff> entry in deleteObjectsMap)
            {
                PickupDropoff pickupDropoff = entry.Value;

                if (pickupDropoff.PickupDropoffId != null)
                {
                    repository.Remove(pickupDropoff);
                }
            }

            deleteObjectsMap = new Dictionary<int, PickupDropoff>();
            refreshDisplayFromDatabase();
        }

        public void PayrollTracker_SaveButtonClickedEventHandler(object sender, EventArgs e)
        {
            PickupDropoffRepository repository = new PickupDropoffRepository();

            foreach (KeyValuePair<int, PickupDropoff> entry in dirtyObjectsMap)
            {
                PickupDropoff pickupDropoff = entry.Value;

                bool isValid = dataGridView1_RowValidating(entry.Key, pickupDropoff);

                if (isValid)
                {
                    if (pickupDropoff.PickupDropoffId == null)
                    {
                        repository.Add(pickupDropoff);
                    }
                    else
                    {
                        repository.Update(pickupDropoff);
                    }

                    dirtyObjectsMap = new Dictionary<int, PickupDropoff>();
                    refreshDisplayFromDatabase();
                }
            }
       }

        private void refreshDisplayFromDatabase()
        {
            this.dataGridView1.Rows.Clear();

            PickupDropoffRepository repository = new PickupDropoffRepository();
            IList<PickupDropoff> pickupsAndDropoffsList = repository.GetRecentPickupsAndDropoffs(user, payrollStartDate, payrollEndDate);

            foreach (PickupDropoff pickupDropoff in pickupsAndDropoffsList)
            {
                int index = this.dataGridView1.Rows.Add();

                dataGridView1.Rows[index].Cells["DateColumn"].Value = pickupDropoff.Date;

                dataGridView1.Rows[index].Cells["DogNameColumn"].Value = pickupDropoff.Dog.DogId;

                if (pickupDropoff.PickupCost != null)
                {
                    dataGridView1.Rows[index].Cells["PickupCostColumn"].Value = pickupDropoff.PickupCost.CostId;
                }

                if (pickupDropoff.DropoffCost != null)
                {
                    dataGridView1.Rows[index].Cells["DropoffCostColumn"].Value = pickupDropoff.DropoffCost.CostId;
                }

                dataGridView1.Rows[index].Cells["PickupDropoffIdColumn"].Value = pickupDropoff.PickupDropoffId;
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

        private bool dataGridView1_RowValidating(int index, PickupDropoff pickupDropoff)
        {
            if (pickupDropoff.Date < payrollStartDate || pickupDropoff.Date > payrollEndDate)
            {
                dataGridView1.Rows[index].ErrorText = "Pickup/Dropoff Date must be for current payroll period (" + payrollStartDate + " - " + payrollEndDate + ").";
                return false;
            }

            if (pickupDropoff.Dog == null)
            {
                dataGridView1.Rows[index].ErrorText = "Dog's Name must not be empty.";
                return false;
            }

            bool pickupOrDropoffCostFilled = false;
            if (pickupDropoff.PickupCost != null)
            {
                pickupOrDropoffCostFilled = true;
            }
            else if (pickupDropoff.DropoffCost != null)
            {
                pickupOrDropoffCostFilled = true;
            }

            if (!pickupOrDropoffCostFilled)
            {
                dataGridView1.Rows[index].ErrorText = "Either Pickup Cost or Dropoff Cost (or both) must filled.";
                return false;
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

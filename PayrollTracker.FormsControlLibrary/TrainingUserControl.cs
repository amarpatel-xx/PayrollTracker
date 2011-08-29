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
    public partial class TrainingUserControl : UserControl
    {
        private Dictionary<int, Training> dirtyObjectsMap = new Dictionary<int, Training>();
        private Dictionary<int, Training> deleteObjectsMap = new Dictionary<int, Training>();

        private User user;
        private DateTime payrollStartDate;
        private DateTime payrollEndDate;

        private System.Windows.Forms.DataGridViewTextBoxColumn TrainingIdColumn;

        private const string CLASS_COST_TYPE = "Training - Class";
        private const string PRE_K9_DAYCARE_COST_TYPE = "Training - Pre-K9 Daycare";
        private const string TRAINING_CLASS_PRE_K9 = "Training - Class - Pre K9";

        private BindingSource bindingSource = new BindingSource();

        public TrainingUserControl(User theUser, DateTime thePayrollStartDate, DateTime thePayrollEndDate)
        {
            InitializeComponent();

            this.user = theUser;
            this.payrollStartDate = thePayrollStartDate;
            this.payrollEndDate = thePayrollEndDate;

            this.TrainingIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TrainingIdColumn.HeaderText = "TrainingIdColumn";
            this.TrainingIdColumn.Name = "TrainingIdColumn";
            this.dataGridView1.Columns.Add(TrainingIdColumn);
            dataGridView1.Columns["TrainingIdColumn"].Visible = false;

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
            IList<CostType> trainingCostTypes = costTypeRepository.GetAllSimilarCostTypes(CLASS_COST_TYPE);

            ClassColumn.DataSource = trainingCostTypes;
            ClassColumn.DisplayMember = "CostName";
            ClassColumn.ValueMember = "CostTypeId";

            CostType preK9DaycareCostType = costTypeRepository.GetByCostName(PRE_K9_DAYCARE_COST_TYPE);

            IList<Cost> possibleCosts1 = preK9DaycareCostType.PossibleCosts;
            ArrayList.Adapter((IList)possibleCosts1).Sort();

            PreK9DaycareCostColumn.DataSource = possibleCosts1;
            PreK9DaycareCostColumn.DisplayMember = "CostValue";
            PreK9DaycareCostColumn.ValueMember = "CostId";

            CostOfClassColumn.DataSource = bindingSource;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                if (dirtyObjectsMap.Keys.Contains(e.RowIndex))
                {
                    dirtyObjectsMap.Remove(e.RowIndex);
                }

                Training training = new Training();

                DateTime date = (DateTime)dataGridView1.Rows[e.RowIndex].Cells["DateColumn"].Value;
                training.Date = date;

                string dogId = (string)dataGridView1.Rows[e.RowIndex].Cells["DogNameColumn"].Value;
                DogRepository dogRepository = new DogRepository();
                if (dogId != null)
                {
                    Dog dog = dogRepository.GetById(dogId);
                    training.Dog = dog;
                }

                CostRepository costRepository = new CostRepository();
                CostTypeRepository costTypeRepository = new CostTypeRepository();
                string classTypeId = (string)dataGridView1.Rows[e.RowIndex].Cells["ClassColumn"].Value;
                if (classTypeId != null)
                {
                    CostType costType = costTypeRepository.GetById(classTypeId);
                    training.ClassType = costType;

                    // If Class column value is Pre-K9, then enable Pre-K9 Daycare
                    // Cost column.
                    if (TRAINING_CLASS_PRE_K9.Equals(costType.CostName))
                    {
                        DataGridViewComboBoxCell preK9DaycareComboBoxCell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells["PreK9DaycareCostColumn"];
                        preK9DaycareComboBoxCell.ReadOnly = false;
                    }
                    else
                    {
                        DataGridViewComboBoxCell preK9DaycareComboBoxCell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells["PreK9DaycareCostColumn"];
                        preK9DaycareComboBoxCell.ReadOnly = true;
                    }

                    // Has Class column combobox value changed?
                    if (e.ColumnIndex == 3)
                    {
                        // Yes, Class column value has changed, so update
                        // Class Cost column combobox with appropriate
                        // values for new Class.

                        // Sort the costs.
                        IList<Cost> possibleCosts1 = costType.PossibleCosts;
                        ArrayList.Adapter((IList)possibleCosts1).Sort();

                        DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[e.RowIndex].Cells["CostOfClassColumn"]);

                        // Now that a class type has been selected, we can populate
                        // the cost of class drop down box appropriately.
                        cell.Value = null;
                        cell.DataSource = null;
                        if (cell.Items != null)
                        {
                            cell.Items.Clear();
                        }
                        cell.DataSource = possibleCosts1;
                        cell.DisplayMember = "CostValue";
                        cell.ValueMember = "CostId";
                    }
                }

                string classCostId = (string)dataGridView1.Rows[e.RowIndex].Cells["CostOfClassColumn"].Value;
                if (classCostId != null)
                {
                    Cost cost = costRepository.GetById(classCostId);
                    training.ClassCost = cost;
                }

                string preK9DaycareCostId = (string)dataGridView1.Rows[e.RowIndex].Cells["PreK9DaycareCostColumn"].Value;
                if (preK9DaycareCostId != null)
                {
                    Cost cost = costRepository.GetById(preK9DaycareCostId);
                    training.PreK9DaycareCost = cost;
                }

                training.User = user;

                string trainingId = (string)dataGridView1.Rows[e.RowIndex].Cells["TrainingIdColumn"].Value;

                training.TrainingId = trainingId;

                // Add object to dirty objects map.
                if (!dirtyObjectsMap.Keys.Contains(e.RowIndex))
                {
                    dirtyObjectsMap.Add(e.RowIndex, training);
                }

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
                    deleteObjectsMap.Add(e.RowIndex, training);
                }
            }
        }

        public void PayrollTracker_DeleteButtonClickedEventHandler(object sender, EventArgs e)
        {
            TrainingRepository repository = new TrainingRepository();
            foreach (KeyValuePair<int, Training> entry in deleteObjectsMap)
            {
                Training training = entry.Value;

                if (training.TrainingId != null)
                {
                    repository.Remove(training);
                }
            }

            deleteObjectsMap = new Dictionary<int, Training>();
            refreshDisplayFromDatabase();
        }

        public void PayrollTracker_SaveButtonClickedEventHandler(object sender, EventArgs e)
        {
            TrainingRepository repository = new TrainingRepository();

            foreach (KeyValuePair<int, Training> entry in dirtyObjectsMap)
            {
                Training training = entry.Value;

                bool isValid = dataGridView1_RowValidating(entry.Key, training);

                if (isValid)
                {
                    if (training.TrainingId == null)
                    {
                        repository.Add(training);
                    }
                    else
                    {
                        repository.Update(training);
                    }

                    dirtyObjectsMap = new Dictionary<int, Training>();
                    refreshDisplayFromDatabase();
                }
            }
        }

        private void refreshDisplayFromDatabase()
        {
            this.dataGridView1.Rows.Clear();

            TrainingRepository repository = new TrainingRepository();
            IList<Training> trainingsList = repository.GetRecentTrainings(user, payrollStartDate, payrollEndDate);

            foreach (Training training in trainingsList)
            {
                int index = this.dataGridView1.Rows.Add();

                dataGridView1.Rows[index].Cells["DateColumn"].Value = training.Date;

                dataGridView1.Rows[index].Cells["DogNameColumn"].Value = training.Dog.DogId;

                dataGridView1.Rows[index].Cells["ClassColumn"].Value = training.ClassType.CostTypeId;

                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[index].Cells["CostOfClassColumn"]); 
                IList<Cost> possibleCosts1 = training.ClassType.PossibleCosts;
                ArrayList.Adapter((IList)possibleCosts1).Sort();
                cell.DataSource = possibleCosts1;
                cell.DisplayMember = "CostValue";
                cell.ValueMember = "CostId";
                cell.Value = training.ClassCost.CostId;

                if (TRAINING_CLASS_PRE_K9.Equals(training.ClassType.CostName))
                {
                    DataGridViewComboBoxCell preK9DaycareComboBoxCell = (DataGridViewComboBoxCell)dataGridView1.Rows[index].Cells["PreK9DaycareCostColumn"];
                    preK9DaycareComboBoxCell.ReadOnly = false;
                }
                else
                {
                    DataGridViewComboBoxCell preK9DaycareComboBoxCell = (DataGridViewComboBoxCell)dataGridView1.Rows[index].Cells["PreK9DaycareCostColumn"];
                    preK9DaycareComboBoxCell.ReadOnly = true;
                }

                if (training.PreK9DaycareCost != null)
                {
                    dataGridView1.Rows[index].Cells["PreK9DaycareCostColumn"].Value = training.PreK9DaycareCost.CostId;
                }

                dataGridView1.Rows[index].Cells["TrainingIdColumn"].Value = training.TrainingId;
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

        private bool dataGridView1_RowValidating(int index, Training training)
        {
            if (training.Date < payrollStartDate || training.Date > payrollEndDate)
            {
                dataGridView1.Rows[index].ErrorText = "Training Date must be for current payroll period (" + payrollStartDate + " - " + payrollEndDate + ").";
                return false;
            }

            if (training.Dog == null)
            {
                dataGridView1.Rows[index].ErrorText = "Dog's Name must not be empty.";
                return false;
            }
            else if (training.ClassType == null)
            {
                dataGridView1.Rows[index].ErrorText = "Class must not be empty.";
                return false;
            }
            else if (training.ClassCost == null)
            {
                dataGridView1.Rows[index].ErrorText = "Cost of Class must not be empty.";
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

using System;
using System.Collections.Generic;
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
    public partial class GroomingUserControl : UserControl
    {
        private Dictionary<int, Grooming> dirtyObjectsMap = new Dictionary<int, Grooming>();
        private Dictionary<int, Grooming> deleteObjectsMap = new Dictionary<int, Grooming>();

        private User user;
        private DateTime payrollStartDate;
        private DateTime payrollEndDate;

        private System.Windows.Forms.DataGridViewTextBoxColumn GroomingIdColumn;

        public GroomingUserControl(User theUser, DateTime thePayrollStartDate, DateTime thePayrollEndDate)
        {
            InitializeComponent();

            this.user = theUser;
            this.payrollStartDate = thePayrollStartDate;
            this.payrollEndDate = thePayrollEndDate;

            this.GroomingIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroomingIdColumn.HeaderText = "GroomingIdColumn";
            this.GroomingIdColumn.Name = "GroomingIdColumn";
            this.groomingDataGridView.Columns.Add(GroomingIdColumn);
            groomingDataGridView.Columns["GroomingIdColumn"].Visible = false;

            this.groomingDataGridView.CellValueChanged += new DataGridViewCellEventHandler(groomingDataGridView_CellValueChanged);
            this.groomingDataGridView.CellEndEdit += new DataGridViewCellEventHandler(groomingDataGridView_CellEndEdit);
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

            GroomingTypeRepository groomingTypeRepository = new GroomingTypeRepository();
            IList<GroomingType> groomingTypes = groomingTypeRepository.GetAllGroomingTypes();
            GroomTypeColumn.DataSource = groomingTypes;
            GroomTypeColumn.DisplayMember = "TypeName";
            GroomTypeColumn.ValueMember = "GroomingTypeId";
        }

        private void groomingDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (groomingDataGridView.IsCurrentCellDirty)
            {
                if (dirtyObjectsMap.Keys.Contains(e.RowIndex))
                {
                    dirtyObjectsMap.Remove(e.RowIndex);
                }

                Grooming grooming = new Grooming();

                DateTime date = (DateTime)groomingDataGridView.Rows[e.RowIndex].Cells["DateColumn"].Value;
                grooming.Date = date;

                string dogId = (string)groomingDataGridView.Rows[e.RowIndex].Cells["DogNameColumn"].Value;
                DogRepository dogRepository = new DogRepository();
                if (dogId != null)
                {
                    Dog dog = dogRepository.GetById(dogId);
                    grooming.Dog = dog;
                }

                GroomingTypeRepository groomingTypeRepository = new GroomingTypeRepository();
                string groomingTypeId = (string)groomingDataGridView.Rows[e.RowIndex].Cells["GroomTypeColumn"].Value;
                if (groomingTypeId != null)
                {
                    GroomingType groomingType = groomingTypeRepository.GetById(groomingTypeId);
                    grooming.GroomingType = groomingType;
                }

                string costStr = (string)groomingDataGridView.Rows[e.RowIndex].Cells["CostColumn"].Value;
                if (costStr != null)
                {
                    try
                    {
                        Double cost = Convert.ToDouble(costStr);
                        grooming.Cost = cost;
                    }
                    catch (FormatException exception)
                    {
                        //Catch this exception quietly.
                    }
                }

                string tipStr = (string)groomingDataGridView.Rows[e.RowIndex].Cells["TipColumn"].Value;
                if (tipStr != null)
                {
                    try
                    {
                        Double tip = Convert.ToDouble(tipStr);
                        grooming.Tip = tip;
                    }
                    catch (FormatException exception)
                    {
                        //Catch this exception quietly.
                    }
                }

                grooming.User = user;

                string groomingId = (string)groomingDataGridView.Rows[e.RowIndex].Cells["GroomingIdColumn"].Value;

                grooming.GroomingId = groomingId;

                // Add object to dirty objects map.
                dirtyObjectsMap.Add(e.RowIndex, grooming);

                // Remove the entry from the delete map, if
                // an entry for the Daycare exists in the
                // delete map already.
                if (deleteObjectsMap.Keys.Contains(e.RowIndex))
                {
                    deleteObjectsMap.Remove(e.RowIndex);
                }

                var isSelected = groomingDataGridView.Rows[e.RowIndex].Cells["SelectColumn"].Value;

                if (isSelected != null && (bool)isSelected)
                {
                    deleteObjectsMap.Add(e.RowIndex, grooming);
                }
            }
        }

        public void PayrollTracker_DeleteButtonClickedEventHandler(object sender, EventArgs e)
        {
            GroomingRepository repository = new GroomingRepository();
            foreach (KeyValuePair<int, Grooming> entry in deleteObjectsMap)
            {
                Grooming grooming = entry.Value;

                if (grooming.GroomingId != null)
                {
                    repository.Remove(grooming);
                }
            }

            deleteObjectsMap = new Dictionary<int, Grooming>();
            refreshDisplayFromDatabase();
        }

        public void PayrollTracker_SaveButtonClickedEventHandler(object sender, EventArgs e)
        {
            GroomingRepository repository = new GroomingRepository();

            foreach (KeyValuePair<int, Grooming> entry in dirtyObjectsMap)
            {
                Grooming grooming = entry.Value;

                bool isValid = groomingDataGridView_RowValidating(entry.Key, grooming);

                if (isValid)
                {
                    if (grooming.GroomingId == null)
                    {
                        repository.Add(grooming);
                    }
                    else
                    {
                        repository.Update(grooming);
                    }

                    dirtyObjectsMap = new Dictionary<int, Grooming>();
                    refreshDisplayFromDatabase();
                }
            }
        }

        private void refreshDisplayFromDatabase()
        {
            this.groomingDataGridView.Rows.Clear();

            GroomingRepository groomingRepository = new GroomingRepository();
            IList<Grooming> groomingsList = groomingRepository.GetRecentGroomings(user, payrollStartDate, payrollEndDate);

            foreach (Grooming grooming in groomingsList)
            {
                int index = this.groomingDataGridView.Rows.Add();

                groomingDataGridView.Rows[index].Cells["DateColumn"].Value = grooming.Date;

                groomingDataGridView.Rows[index].Cells["DogNameColumn"].Value = grooming.Dog.DogId;

                groomingDataGridView.Rows[index].Cells["GroomTypeColumn"].Value = grooming.GroomingType.GroomingTypeId;

                groomingDataGridView.Rows[index].Cells["CostColumn"].Value = grooming.Cost.ToString();

                groomingDataGridView.Rows[index].Cells["TipColumn"].Value = grooming.Tip.ToString();
                groomingDataGridView.Rows[index].Cells["GroomingIdColumn"].Value = grooming.GroomingId;
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

        private bool groomingDataGridView_RowValidating(int index, Grooming grooming)
        {
            if (grooming.Date < payrollStartDate || grooming.Date > payrollEndDate)
            {
                groomingDataGridView.Rows[index].ErrorText = "Grooming Date must be for current payroll period (" + payrollStartDate + " - " + payrollEndDate + ").";
                return false;
            }

            if (grooming.Dog == null)
            {
                groomingDataGridView.Rows[index].ErrorText = "Dog's Name must not be empty.";
                return false;
            }
            else if (grooming.GroomingType == null)
            {
                groomingDataGridView.Rows[index].ErrorText = "Groom Type must not be empty.";
                return false;
            }
            else if (groomingDataGridView.Rows[index].Cells["CostColumn"].Value == null)
            {
                groomingDataGridView.Rows[index].ErrorText = "Cost must not be empty.";
                return false;
            }
            
            if (groomingDataGridView.Rows[index].Cells["CostColumn"].Value != null)
            {
                string value = groomingDataGridView.Rows[index].Cells["CostColumn"].Value.ToString();

                try
                {
                    Double cost = Convert.ToDouble(value);
                }
                catch (FormatException exception)
                {
                    groomingDataGridView.Rows[index].ErrorText = "Cost must be numeric.";
                    return false;
                }
            }
            
            if (groomingDataGridView.Rows[index].Cells["TipColumn"].Value != null)
            {
                string value = groomingDataGridView.Rows[index].Cells["TipColumn"].Value.ToString();

                try
                {
                    Double tip = Convert.ToDouble(value);
                }
                catch (FormatException exception)
                {
                    groomingDataGridView.Rows[index].ErrorText = "Tip must be numeric.";
                    return false;
                }
            }

            return true;
        }

        void groomingDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the row error in case the user presses ESC.   
            groomingDataGridView.Rows[e.RowIndex].ErrorText = String.Empty;
        }
    }
}

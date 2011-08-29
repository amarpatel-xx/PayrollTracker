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
    public partial class DogUserControl : UserControl
    {
        private Dictionary<int, Dog> dirtyDogsMap = new Dictionary<int, Dog>();
        public event EventHandler DogAddedOrUpdatedEvent;

        private System.Windows.Forms.DataGridViewTextBoxColumn DogIdColumn;

        public DogUserControl()
        {
            InitializeComponent();

            this.DogIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DogIdColumn.HeaderText = "DogIdColumn";
            this.DogIdColumn.Name = "DogIdColumn";
            this.dataGridView1.Columns.Add(DogIdColumn);
            dataGridView1.Columns["DogIdColumn"].Visible = false;
            this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            this.dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);

            // Setting this value to above 18 ensures the error 
            // image and label are visible. Also, the Data Grid 
            // View's AutoSizeRowsMode must be disabled for the 
            // RowTemplate.Height to work.
            this.dataGridView1.RowTemplate.Height = 25; 
        }

        protected override void OnLoad(EventArgs e)
        {
            refreshDisplayFromDatabase();
        }

        private void refreshDisplayFromDatabase()
        {
            this.dataGridView1.Rows.Clear();

            DogRepository repository = new DogRepository();
            IList<Dog> dogsList = repository.GetAllDogs();

            foreach (Dog dog in dogsList)
            {
                int index = this.dataGridView1.Rows.Add();

                dataGridView1.Rows[index].Cells["DogFirstNameColumn"].Value = dog.FirstName;

                dataGridView1.Rows[index].Cells["DogLastNameColumn"].Value = dog.LastName;

                dataGridView1.Rows[index].Cells["DogIdColumn"].Value = dog.DogId;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                if (dirtyDogsMap.Keys.Contains(e.RowIndex))
                {
                    dirtyDogsMap.Remove(e.RowIndex);
                }

                Dog dog = new Dog();

                string dogId = (string)dataGridView1.Rows[e.RowIndex].Cells["DogIdColumn"].Value;
                dog.DogId = dogId;

                string dogFirstName = (string)dataGridView1.Rows[e.RowIndex].Cells["DogFirstNameColumn"].Value;
                if (dogFirstName != null)
                {
                    dog.FirstName = dogFirstName;
                }

                string dogLastName = (string)dataGridView1.Rows[e.RowIndex].Cells["DogLastNameColumn"].Value;
                if (dogLastName != null)
                {
                    dog.LastName = dogLastName;
                }

                dirtyDogsMap.Add(e.RowIndex, dog);
            }
        }

        public void PayrollTracker_SaveButtonClickedEventHandler(object sender, EventArgs e)
        {
            DogRepository repository = new DogRepository();

            foreach (KeyValuePair<int, Dog> entry in dirtyDogsMap)
            {
                Dog dog = entry.Value;

                bool isValid = dataGridView1_RowValidating(entry.Key, dog);

                if (isValid)
                {
                    if (dog.DogId == null)
                    {
                        repository.Add(dog);
                    }
                    else
                    {
                        repository.Update(dog);
                    }

                    if (DogAddedOrUpdatedEvent != null)
                    {
                        DogAddedOrUpdatedEvent(this, e);
                    }

                    dirtyDogsMap = new Dictionary<int, Dog>();
                    refreshDisplayFromDatabase();
                }
                else
                {
                    dataGridView1.Refresh();
                }
            }
        }

        private bool dataGridView1_RowValidating(int index, Dog dog)
        {
            if (dog.FirstName == null)
            {
                dataGridView1.Rows[index].ErrorText = "Dog's First Name must not be empty.";
                return false;
            }
            else if (dog.LastName == null)
            {
                dataGridView1.Rows[index].ErrorText = "Dog's Last Name must not be empty.";
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

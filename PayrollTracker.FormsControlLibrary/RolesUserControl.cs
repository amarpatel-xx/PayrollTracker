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
    public partial class RolesUserControl : UserControl
    {
        IList<Role> actualRoles = new List<Role>();
        IList<Role> availableRoles = new List<Role>();
        public RolesUserControl()
        {
            InitializeComponent();
        }

        public void setActualRoles(IList<Role> theActualRoles)
        {
            this.actualRoles = theActualRoles;
            this.actualRolesListBox.DataSource = theActualRoles;
            this.actualRolesListBox.DisplayMember = "RoleName";
            setAvailableRoles(theActualRoles);
        }

        public IList<Role> getActualRoles()
        {
            return this.actualRoles;
        }

        private void setAvailableRoles(IList<Role> actualRoles)
        {
            RoleRepository roleRepository = new RoleRepository();
            IList<Role> theAvailableRoles = roleRepository.GetAllRoles();

            foreach (Role role in actualRoles)
            {
                theAvailableRoles.Remove(role);
            }

            this.availableRoles = theAvailableRoles;
            this.availableRolesListBox.DataSource = theAvailableRoles;
            this.availableRolesListBox.DisplayMember = "RoleName";
        }

        private void selectAllButton_Click(object sender, EventArgs e)
        {
            foreach (Role role in this.availableRoles)
            {
                this.actualRoles.Add(role);
            }

            // Reset the actual roles list box, by setting its
            // datasource to null.
            this.actualRolesListBox.DataSource = null;
            //  Now update the listbox datasource.
            this.actualRolesListBox.DataSource = this.actualRoles;
            this.actualRolesListBox.DisplayMember = "RoleName";

            this.availableRoles.Clear();
            this.availableRolesListBox.DataSource = null;
            this.availableRolesListBox.Items.Clear();
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (this.availableRolesListBox.SelectedItem != null)
            {
                Role selectedRole = (Role)this.availableRolesListBox.SelectedItem;
                this.actualRoles.Add(selectedRole);

                // Reset the actual roles list box, by setting its
                // datasource to null.
                this.actualRolesListBox.DataSource = null;
                //  Now update the listbox datasource.
                this.actualRolesListBox.Items.Clear();
                this.actualRolesListBox.DataSource = this.actualRoles;
                this.actualRolesListBox.DisplayMember = "RoleName";

                this.availableRoles.Remove(selectedRole);
                this.availableRolesListBox.DataSource = null;
                this.availableRolesListBox.Items.Clear();
                this.availableRolesListBox.DataSource = this.availableRoles;
                this.availableRolesListBox.DisplayMember = "RoleName";
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (this.actualRolesListBox.SelectedItem != null)
            {
                Role removeRole = (Role)this.actualRolesListBox.SelectedItem;
                this.availableRoles.Add(removeRole);

                // Reset the available roles list box, by setting its
                // datasource to null.
                this.availableRolesListBox.DataSource = null;
                //  Now update the listbox datasource.
                this.availableRolesListBox.Items.Clear();
                this.availableRolesListBox.DataSource = this.availableRoles;
                this.availableRolesListBox.DisplayMember = "RoleName";

                this.actualRoles.Remove(removeRole);
                this.actualRolesListBox.DataSource = null;
                this.actualRolesListBox.Items.Clear();
                this.actualRolesListBox.DataSource = this.actualRoles;
                this.actualRolesListBox.DisplayMember = "RoleName";
            }
        }

        private void removeAllButton_Click(object sender, EventArgs e)
        {
            foreach(Role role in this.actualRoles)
            {
                this.availableRoles.Add(role);
            }

            // Reset the available roles list box, by setting its
            // datasource to null.
            this.availableRolesListBox.DataSource = null;
            //  Now update the listbox datasource.
            this.availableRolesListBox.DataSource = this.availableRoles;
            this.availableRolesListBox.DisplayMember = "RoleName";

            this.actualRoles.Clear();
            this.actualRolesListBox.DataSource = null;
            this.actualRolesListBox.Items.Clear();
        }
    }
}

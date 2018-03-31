// File: ChooseAddressForm.cs
// This class creates the ChooseAddress dialog box form GUI. It performs validation
// and provides a property for the field. It allows you to select an existing
// address from a comboBox and use the property to get its selected index.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPVApp
{
    public partial class ChooseAddressForm : Form
    {
        public const int MIN_ADDRESSES = 1; // Minimum number of addresses needed

        private List<Address> addressList;  // List of addresses used to fill combo box

        // Precondition:  List addresses is populated with the available
        //                addresses at least 1 to choose from
        // Postcondition: The form's GUI displayed.
        public ChooseAddressForm(List<Address> addresses)  // pass list of addresses
        {
            InitializeComponent();
            addressList = addresses; // assign addresses to new addressList
        }
        // Precondition:  addressList.Count >= MIN_ADDRESSES
        // Postcondition: The list of addresses is used to populate the
        //                address combo box
        private void ChooseAdressForm_Load(object sender, EventArgs e)
        {
            if (addressList.Count < MIN_ADDRESSES) // Violated precondition
            {
                MessageBox.Show("Need " + MIN_ADDRESSES + " address to edit",
                    "Addresses Error");
                this.DialogResult = DialogResult.Abort; // Dismiss immediately
            }
            else
            {
                foreach (Address a in addressList)  // if greater or equal to than min_addresses
                {
                    addressComboBox.Items.Add(a.Name);   // add addresses to comboBox 
                    
                }
            }
        }
        // Precondition:  File has been opened and addresses are loaded to comboBox
        // Postcondition: If no address selected, focus remains and error provider
        //                highlights the field
        private void addressComboBox_Validating(object sender, CancelEventArgs e)
        {
            // Downcast to sender as ComboBox, so make sure you obey precondition!
            ComboBox cbo = sender as ComboBox; // Cast sender as combo box

            if (cbo.SelectedIndex == -1) // -1 means no item selected
            {
                e.Cancel = true;  // stop focus
                errorProvider.SetError(cbo, "Must select an address"); // show error
            }
        }
        // Precondition:  Validating of combobox not cancelled, so data OK
        //                sender is Control
        // Postcondition: Error provider cleared and focus allowed to change
        private void addressComboBox_Validated(object sender, EventArgs e)
        {
            ComboBox cbo = sender as ComboBox; // Cast sender as ComboBox
            
            errorProvider.SetError(cbo, ""); // clear error on comboBox
        }
        internal int AddressIndex
        {
            // Precondition:  User has selected from addressComboBox
            // Postcondition: The index of the selected address returned
            get
            {
                return addressComboBox.SelectedIndex;  // return index
            }

            // Precondition:  -1 <= value < addressList.Count
            // Postcondition: The specified index is selected in addressComobBox
            set
            {
                if ((value >= -1) && (value < addressList.Count))
                    addressComboBox.SelectedIndex = value; // set combobox index to value
                else
                    throw new ArgumentOutOfRangeException("DestinationAddressIndex", value,
                        "Index must be valid");  // throw exception if value is out of range
            }
        }
        // Precondition:  User pressed on cancelButton
        // Postcondition: Form closes
        private void cancelButton_MouseDown(object sender, MouseEventArgs e)
        {
            // This uses MouseDown instead of Click event because
            // Click won't be allowed if field's validation fails

            if (e.Button == MouseButtons.Left) // Was it a left-click
                this.DialogResult = DialogResult.Cancel; // cancel form
        }
        // Precondition:  User clicked on okButton
        // Postcondition: Ifsomething invalid, keep form open and give first invalid
        //                field the focus. Else return OK and close form.
        private void okButton_Click(object sender, EventArgs e)
        {
            // Raise validating event for all enabled controls on form
            // If all pass, ValidateChildren() will be true
            if (ValidateChildren())
                this.DialogResult = DialogResult.OK;
        }
    }
}

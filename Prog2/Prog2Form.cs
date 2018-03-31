// Program 3
// CIS 200
// Fall 2016
// Due: 11/15/2016
// GradingID: C6181
// By: Andrew L. Wright (Students use Grading ID)
// Description: This program adds a Save As and 
// Open option to the File menu while creating an Edit, Addresses in the menu as well. 
// This program allows you to save the upv object as a serializable object then open the file
// back in and edit an existing address by selecting one from the chooseAddressForm.

// File: Prog2Form.cs
// This class creates the main GUI for Program 2. It provides a
// File menu with About and Exit items, an Insert menu with Address and
// Letter items, and a Report menu with List Addresses and List Parcels
// items. Have added Open and Save As to File and Edit, Address to menu.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace UPVApp
{
    [Serializable]  // to make class serializable
    public partial class Prog2Form : Form
    {
        private UserParcelView upv; // The UserParcelView
        private BinaryFormatter reader = new BinaryFormatter();  // to read the serializable object
        private BinaryFormatter formatter = new BinaryFormatter(); // to format the upv object
        private FileStream input; // stream for reading from a file
        private FileStream output; // stream for writing to the file
        // Precondition:  None
        // Postcondition: The form's GUI is prepared for display. 
        //                Will have to load in data from a file.
        public Prog2Form()
        {
            InitializeComponent();

            upv = new UserParcelView();       
        }

        // Precondition:  File, About menu item activated
        // Postcondition: Information about author displayed in dialog box
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string NL = Environment.NewLine; // Newline shorthand

            MessageBox.Show($"Program 3{NL}By: Andrew L. Wright/C6181{NL}CIS 200{NL}Fall 2016",
                "About Program 3");
        }

        // Precondition:  File, Exit menu item activated
        // Postcondition: The application is exited
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Precondition:  Insert, Address menu item activated
        // Postcondition: The Address dialog box is displayed. If data entered
        //                are OK, an Address is created and added to the list
        //                of addresses
        private void addressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddressForm addressForm = new AddressForm();    // The address dialog box form
            DialogResult result = addressForm.ShowDialog(); // Show form as dialog and store result

            if (result == DialogResult.OK) // Only add if OK
            {
                try
                {
                    upv.AddAddress(addressForm.AddressName, addressForm.Address1,
                        addressForm.Address2, addressForm.City, addressForm.State,
                        int.Parse(addressForm.ZipText)); // Use form's properties to create address
                }
                catch (FormatException) // This should never happen if form validation works!
                {
                    MessageBox.Show("Problem with Address Validation!", "Validation Error");
                }
            }

            addressForm.Dispose(); // Best practice for dialog boxes
        }

        // Precondition:  Report, List Addresses menu item activated
        // Postcondition: The list of addresses is displayed in the addressResultsTxt
        //                text box
        private void listAddressesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder(); // Holds text as report being built
                                                        // StringBuilder more efficient than String
            string NL = Environment.NewLine;            // Newline shorthand

            result.Append("Addresses:");
            result.Append(NL); // Remember, \n doesn't always work in GUIs
            result.Append(NL);
            
            

            foreach (Address a in upv.AddressList)
            {
                result.Append(a.ToString());
                result.Append(NL);
                result.Append("------------------------------");
                result.Append(NL);
            }

            reportTxt.Text = result.ToString();

            // Put cursor at start of report
            reportTxt.Focus();
            reportTxt.SelectionStart = 0;
            reportTxt.SelectionLength = 0;
        }

        // Precondition:  Insert, Letter menu item activated
        // Postcondition: The Letter dialog box is displayed. If data entered
        //                are OK, a Letter is created and added to the list
        //                of parcels
        private void letterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LetterForm letterForm; // The letter dialog box form
            DialogResult result;   // The result of showing form as dialog

            if (upv.AddressCount < LetterForm.MIN_ADDRESSES) // Make sure we have enough addresses
            {
                MessageBox.Show("Need " + LetterForm.MIN_ADDRESSES + " addresses to create letter!",
                    "Addresses Error");
                return;
            }

            letterForm = new LetterForm(upv.AddressList); // Send list of addresses
            result = letterForm.ShowDialog();

            if (result == DialogResult.OK) // Only add if OK
            {
                try
                {
                    // For this to work, LetterForm's combo boxes need to be in same
                    // order as upv's AddressList
                    upv.AddLetter(upv.AddressAt(letterForm.OriginAddressIndex),
                        upv.AddressAt(letterForm.DestinationAddressIndex),
                        decimal.Parse(letterForm.FixedCostText)); // Letter to be inserted
                }
                catch (FormatException) // This should never happen if form validation works!
                {
                    MessageBox.Show("Problem with Letter Validation!", "Validation Error");
                }
            }

            letterForm.Dispose(); // Best practice for dialog boxes
        }

        // Precondition:  Report, List Parcels menu item activated
        // Postcondition: The list of parcels is displayed in the parcelResultsTxt
        //                text box
        private void listParcelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder(); // Holds text as report being built
                                                        // StringBuilder more efficient than String
            decimal totalCost = 0;                      // Running total of parcel shipping costs
            string NL = Environment.NewLine;            // Newline shorthand

            result.Append("Parcels:");
            result.Append(NL); // Remember, \n doesn't always work in GUIs
            result.Append(NL);

            foreach (Parcel p in upv.ParcelList)
            {
                result.Append(p.ToString());
                result.Append(NL);
                result.Append("------------------------------");
                result.Append(NL);
                totalCost += p.CalcCost();
            }

            result.Append(NL);
            result.Append($"Total Cost: {totalCost:C}");

            reportTxt.Text = result.ToString();

            // Put cursor at start of report
            reportTxt.Focus();
            reportTxt.SelectionStart = 0;
            reportTxt.SelectionLength = 0;
        }
        // Precondition:  File, Open menu item activated
        // Postcondition: Dialog box opens and allows you to choose file from directory
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // create and show dialog box enabling user to open file
            DialogResult result; // result of OpenFileDialog
            string fileName; // name of file containing data

            using (OpenFileDialog chooseFile=new OpenFileDialog()) // making chooseFile relate to open file dialog
            {
                result = chooseFile.ShowDialog(); // result equal to showing dialog
                fileName = chooseFile.FileName; // get specified name
            } // end using
            
            // ensure that user clicked "OK"
            if (result == DialogResult.OK)
            {


                // show error if user specified invalid file
                if (fileName == string.Empty)
                    MessageBox.Show("Invalid File Name", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    try
                    {
                        // create FileStream to obtain read access to file
                        input = new FileStream(
                       fileName, FileMode.Open, FileAccess.Read);
                    // deserialize upv
                    
                        // get upv for input
                        upv =
                           (UserParcelView)reader.Deserialize(input);
                    } // end try
                      // handle exception when there are no upvs that are serializable in file
                    catch (System.IO.IOException)
                    {
                        // notify user if file could not be opened
                        MessageBox.Show("Error opening file. Close the Application and Run it again.", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (SerializationException)
                    {
                        input.Close(); // close FileStream 

                        // notify user could not open file
                        MessageBox.Show("File not Serializable", string.Empty,
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } // end catch
                }
            }
        }
        // Precondition:  File, Save As menu item activated
        // Postcondition: Dialog Box opened allowing user to save upv object in a file
        //                that is serializable
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result; // create dialog result to hold what user does
            string fileName; // name of file to save data

            using (SaveFileDialog fileChooser = new SaveFileDialog()) // associate fileChooser as new SaveFile Dialog
            {
                fileChooser.CheckFileExists = false; // let user create file

                // retrieve the result of the dialog box
                result = fileChooser.ShowDialog();
                fileName = fileChooser.FileName; // get specified file name
            } // end using

            // ensure that user clicked "OK"
            if (result == DialogResult.OK)
            {
                // show error if user specified invalid file
                if (fileName == string.Empty)
                    MessageBox.Show("Invalid File Name", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    // save file via FileStream if user specified valid file
                    try
                    {
                        // open file with write access
                        output = new FileStream(fileName,
                           FileMode.OpenOrCreate, FileAccess.Write);
                        formatter.Serialize(output, upv);  // save upv object

                    } // end try
                      // handle exception if there is a problem opening the file
                    catch (IOException)
                    {
                        // notify user if file could not be opened
                        MessageBox.Show("Error saving file", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        // Precondition: Edit, Addresses menu item activated
        // Postcondition: Allows you to edit an addresses opened from 
        //                the upv object that you selected in ChooseAddressForm
        private void addressesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseAddressForm chooseForm; // new instnace of ChooseAddressForm
            DialogResult result;  // new dialogresult
            chooseForm = new ChooseAddressForm(upv.AddressList); // Send list of addresses
            result = chooseForm.ShowDialog();  // show chooseform dialog box
            
            if (result == DialogResult.OK) // Only add if OK
            {
                try
                {
                    AddressForm addressForm = new AddressForm(); // new instance of address
                    Address myAddress; // The address being added
                    DialogResult resultTwo; // second dialog result

                    myAddress = upv.AddressAt(chooseForm.AddressIndex); // get address from selected index and 
                                                                        //assign it to variable myAddress
                    addressForm.AddressName=myAddress.Name; // load address information into textbox
                    
                    addressForm.Address1 = myAddress.Address1; // load address information into textbox
                    addressForm.Address2 = myAddress.Address2; // load address information into textbox
                    addressForm.City = myAddress.City;         // load address information into textbox
                    addressForm.State = myAddress.State;       // load address information into textbox
                    addressForm.ZipText = myAddress.Zip.ToString("D5"); // load address information into textbox
                    resultTwo = addressForm.ShowDialog(); // show form with preloaded address to edit
                    if (resultTwo == DialogResult.OK) // Only add if OK
                    {
                        myAddress.Name = addressForm.AddressName; // Change address at selected index with new information
                        myAddress.Address1 = addressForm.Address1; // Change address at selected index with new information
                        myAddress.Address2 = addressForm.Address2; // Change address at selected index with new information
                        myAddress.City = addressForm.City;         // Change address at selected index with new information
                        myAddress.State = addressForm.State;       // Change address at selected index with new information
                        myAddress.Zip = int.Parse(addressForm.ZipText); // Change address at selected index with new information
                    }
                        
                    }
                catch (FormatException) // This should never happen if form validation works
                {
                    MessageBox.Show("Problem with Letter Validation!", "Validation Error"); // if something wrong, show error
                }
            }
        }
    }
}
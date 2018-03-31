namespace UPVApp
{
    partial class ChooseAddressForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.addressComboBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.chooseAddressLabel = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // addressComboBox
            // 
            this.addressComboBox.FormattingEnabled = true;
            this.addressComboBox.Location = new System.Drawing.Point(85, 99);
            this.addressComboBox.Name = "addressComboBox";
            this.addressComboBox.Size = new System.Drawing.Size(121, 21);
            this.addressComboBox.TabIndex = 0;
            this.addressComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.addressComboBox_Validating);
            this.addressComboBox.Validated += new System.EventHandler(this.addressComboBox_Validated);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(54, 153);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(159, 153);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cancelButton_MouseDown);
            // 
            // chooseAddressLabel
            // 
            this.chooseAddressLabel.AutoSize = true;
            this.chooseAddressLabel.Location = new System.Drawing.Point(86, 67);
            this.chooseAddressLabel.Name = "chooseAddressLabel";
            this.chooseAddressLabel.Size = new System.Drawing.Size(120, 13);
            this.chooseAddressLabel.TabIndex = 3;
            this.chooseAddressLabel.Text = "Choose Address to Edit:";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ChooseAddressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.chooseAddressLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.addressComboBox);
            this.Name = "ChooseAddressForm";
            this.Text = "ChooseAddressForm";
            this.Load += new System.EventHandler(this.ChooseAdressForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox addressComboBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label chooseAddressLabel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
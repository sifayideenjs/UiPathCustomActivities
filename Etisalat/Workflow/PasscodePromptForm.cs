using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Etisalat.CIT.OPS.Robotics
{
    public partial class PasscodePromptForm : Form
    {
        public PasscodePromptForm(string[] machineNames, string[] screenSizes)
        {
            InitializeComponent();
            this.comboMachineNames.Items.AddRange(machineNames);
            this.comboMachineNames.SelectedIndex = 0;
            this.comboScreenSize.Items.AddRange(screenSizes);
            this.comboScreenSize.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OnOK();
        }

        public string Passcode { get; private set; }
        public string SelectedMachine { get; private set; }
        public string SelectedScreenSize { get; private set; }

        private void PasscodePromptForm_Load(object sender, EventArgs e)
        {
            this.txtPasscode.Focus();
        }

        private void OnOK()
        {
            if (!string.IsNullOrEmpty(this.txtPasscode.Text))
            {
                this.DialogResult = DialogResult.OK;
                this.Passcode = this.txtPasscode.Text;
                this.SelectedMachine = this.comboMachineNames.SelectedItem.ToString();
                this.SelectedScreenSize = this.comboScreenSize.SelectedItem.ToString();
                this.Close();
            }
            else
            {
                this.alert.Text = "Please enter Passcode";
            }
        }

        private void PasscodePromptForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OnOK();
            }
        }
    }
}

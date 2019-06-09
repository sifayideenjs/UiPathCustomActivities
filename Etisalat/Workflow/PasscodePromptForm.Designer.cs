namespace Etisalat.CIT.OPS.Robotics
{
    partial class PasscodePromptForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtPasscode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboMachineNames = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.alert = new System.Windows.Forms.Label();
            this.comboScreenSize = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Neo Tech Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Passcode:";
            // 
            // txtPasscode
            // 
            this.txtPasscode.Font = new System.Drawing.Font("Neo Tech Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPasscode.Location = new System.Drawing.Point(133, 24);
            this.txtPasscode.Name = "txtPasscode";
            this.txtPasscode.Size = new System.Drawing.Size(248, 24);
            this.txtPasscode.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Neo Tech Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select Machine:";
            // 
            // comboMachineNames
            // 
            this.comboMachineNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMachineNames.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboMachineNames.Font = new System.Drawing.Font("Neo Tech Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboMachineNames.FormattingEnabled = true;
            this.comboMachineNames.Location = new System.Drawing.Point(133, 65);
            this.comboMachineNames.Name = "comboMachineNames";
            this.comboMachineNames.Size = new System.Drawing.Size(248, 24);
            this.comboMachineNames.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Neo Tech", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(306, 159);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Neo Tech Alt", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(212, 159);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // alert
            // 
            this.alert.AutoSize = true;
            this.alert.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.alert.Font = new System.Drawing.Font("Neo Tech Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alert.ForeColor = System.Drawing.Color.Red;
            this.alert.Location = new System.Drawing.Point(0, 191);
            this.alert.Name = "alert";
            this.alert.Size = new System.Drawing.Size(0, 13);
            this.alert.TabIndex = 5;
            this.alert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboScreenSize
            // 
            this.comboScreenSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboScreenSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboScreenSize.Font = new System.Drawing.Font("Neo Tech Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboScreenSize.FormattingEnabled = true;
            this.comboScreenSize.Location = new System.Drawing.Point(133, 110);
            this.comboScreenSize.Name = "comboScreenSize";
            this.comboScreenSize.Size = new System.Drawing.Size(245, 24);
            this.comboScreenSize.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Neo Tech Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Select Screen Size:";
            // 
            // PasscodePromptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 204);
            this.ControlBox = false;
            this.Controls.Add(this.comboScreenSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.alert);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.comboMachineNames);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPasscode);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Neo Tech Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PasscodePromptForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Passcode Prompter";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PasscodePromptForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasscodePromptForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPasscode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboMachineNames;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label alert;
        private System.Windows.Forms.ComboBox comboScreenSize;
        private System.Windows.Forms.Label label3;
    }
}
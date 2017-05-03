namespace ClientTcpChat.Forms
{
    partial class Di_Client_CreateAddAgreement
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
            this.FirstLevelPanel = new System.Windows.Forms.Panel();
            this.FirstLevelMessageLabel = new System.Windows.Forms.Label();
            this.UserName_TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Add_Button = new System.Windows.Forms.Button();
            this.FirstLevelPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FirstLevelPanel
            // 
            this.FirstLevelPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FirstLevelPanel.Controls.Add(this.FirstLevelMessageLabel);
            this.FirstLevelPanel.Controls.Add(this.UserName_TextBox);
            this.FirstLevelPanel.Controls.Add(this.label1);
            this.FirstLevelPanel.Controls.Add(this.Add_Button);
            this.FirstLevelPanel.Location = new System.Drawing.Point(12, 2);
            this.FirstLevelPanel.Name = "FirstLevelPanel";
            this.FirstLevelPanel.Size = new System.Drawing.Size(247, 199);
            this.FirstLevelPanel.TabIndex = 0;
            // 
            // FirstLevelMessageLabel
            // 
            this.FirstLevelMessageLabel.ForeColor = System.Drawing.Color.Red;
            this.FirstLevelMessageLabel.Location = new System.Drawing.Point(4, 55);
            this.FirstLevelMessageLabel.Name = "FirstLevelMessageLabel";
            this.FirstLevelMessageLabel.Size = new System.Drawing.Size(240, 106);
            this.FirstLevelMessageLabel.TabIndex = 3;
            // 
            // UserName_TextBox
            // 
            this.UserName_TextBox.Location = new System.Drawing.Point(90, 22);
            this.UserName_TextBox.MaxLength = 30;
            this.UserName_TextBox.Name = "UserName_TextBox";
            this.UserName_TextBox.Size = new System.Drawing.Size(154, 20);
            this.UserName_TextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Name";
            // 
            // Add_Button
            // 
            this.Add_Button.Location = new System.Drawing.Point(169, 164);
            this.Add_Button.Name = "Add_Button";
            this.Add_Button.Size = new System.Drawing.Size(75, 23);
            this.Add_Button.TabIndex = 0;
            this.Add_Button.Text = "Add";
            this.Add_Button.UseVisualStyleBackColor = true;
            this.Add_Button.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // Di_Client_CreateAddAgreement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 201);
            this.Controls.Add(this.FirstLevelPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Di_Client_CreateAddAgreement";
            this.Text = "Add A User";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Di_Client_CreateAddAgreement_FormClosing);
            this.FirstLevelPanel.ResumeLayout(false);
            this.FirstLevelPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel FirstLevelPanel;
        private System.Windows.Forms.TextBox UserName_TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label FirstLevelMessageLabel;
        public System.Windows.Forms.Button Add_Button;
    }
}
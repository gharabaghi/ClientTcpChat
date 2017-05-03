namespace ClientTcpChat.Forms
{
    partial class Di_Client_ClientSignupRequest
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
            this.FirstLevel_MessageLabel = new System.Windows.Forms.Label();
            this.Password_TextBox = new System.Windows.Forms.TextBox();
            this.UserName_TextBox = new System.Windows.Forms.TextBox();
            this.CreateAccount_Button = new System.Windows.Forms.Button();
            this.Password_Label = new System.Windows.Forms.Label();
            this.UserName_Label = new System.Windows.Forms.Label();
            this.FirstLevelPanel = new System.Windows.Forms.Panel();
            this.FirstLevelPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FirstLevel_MessageLabel
            // 
            this.FirstLevel_MessageLabel.ForeColor = System.Drawing.Color.Red;
            this.FirstLevel_MessageLabel.Location = new System.Drawing.Point(11, 99);
            this.FirstLevel_MessageLabel.Name = "FirstLevel_MessageLabel";
            this.FirstLevel_MessageLabel.Size = new System.Drawing.Size(224, 135);
            this.FirstLevel_MessageLabel.TabIndex = 5;
            // 
            // Password_TextBox
            // 
            this.Password_TextBox.Location = new System.Drawing.Point(82, 47);
            this.Password_TextBox.MaxLength = 30;
            this.Password_TextBox.Name = "Password_TextBox";
            this.Password_TextBox.PasswordChar = '*';
            this.Password_TextBox.Size = new System.Drawing.Size(153, 20);
            this.Password_TextBox.TabIndex = 4;
            // 
            // UserName_TextBox
            // 
            this.UserName_TextBox.Location = new System.Drawing.Point(82, 8);
            this.UserName_TextBox.MaxLength = 30;
            this.UserName_TextBox.Name = "UserName_TextBox";
            this.UserName_TextBox.Size = new System.Drawing.Size(153, 20);
            this.UserName_TextBox.TabIndex = 3;
            // 
            // CreateAccount_Button
            // 
            this.CreateAccount_Button.Location = new System.Drawing.Point(124, 73);
            this.CreateAccount_Button.Name = "CreateAccount_Button";
            this.CreateAccount_Button.Size = new System.Drawing.Size(111, 23);
            this.CreateAccount_Button.TabIndex = 2;
            this.CreateAccount_Button.Text = "Creat Account";
            this.CreateAccount_Button.UseVisualStyleBackColor = true;
            this.CreateAccount_Button.Click += new System.EventHandler(this.CreateAccount_Button_Click);
            // 
            // Password_Label
            // 
            this.Password_Label.AutoSize = true;
            this.Password_Label.Location = new System.Drawing.Point(8, 50);
            this.Password_Label.Name = "Password_Label";
            this.Password_Label.Size = new System.Drawing.Size(53, 13);
            this.Password_Label.TabIndex = 1;
            this.Password_Label.Text = "Password";
            // 
            // UserName_Label
            // 
            this.UserName_Label.AutoSize = true;
            this.UserName_Label.Location = new System.Drawing.Point(8, 15);
            this.UserName_Label.Name = "UserName_Label";
            this.UserName_Label.Size = new System.Drawing.Size(57, 13);
            this.UserName_Label.TabIndex = 0;
            this.UserName_Label.Text = "UserName";
            // 
            // FirstLevelPanel
            // 
            this.FirstLevelPanel.Controls.Add(this.FirstLevel_MessageLabel);
            this.FirstLevelPanel.Controls.Add(this.UserName_Label);
            this.FirstLevelPanel.Controls.Add(this.Password_Label);
            this.FirstLevelPanel.Controls.Add(this.UserName_TextBox);
            this.FirstLevelPanel.Controls.Add(this.CreateAccount_Button);
            this.FirstLevelPanel.Controls.Add(this.Password_TextBox);
            this.FirstLevelPanel.Location = new System.Drawing.Point(13, 13);
            this.FirstLevelPanel.Name = "FirstLevelPanel";
            this.FirstLevelPanel.Size = new System.Drawing.Size(246, 243);
            this.FirstLevelPanel.TabIndex = 6;
            // 
            // Di_Client_ClientSignupRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 268);
            this.Controls.Add(this.FirstLevelPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Di_Client_ClientSignupRequest";
            this.Text = "Create Account";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Di_Client_ClientSignupRequest_FormClosing);
            this.FirstLevelPanel.ResumeLayout(false);
            this.FirstLevelPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CreateAccount_Button;
        private System.Windows.Forms.Label Password_Label;
        private System.Windows.Forms.Label UserName_Label;
        private System.Windows.Forms.TextBox Password_TextBox;
        private System.Windows.Forms.TextBox UserName_TextBox;
        private System.Windows.Forms.Label FirstLevel_MessageLabel;
        private System.Windows.Forms.Panel FirstLevelPanel;
    }
}
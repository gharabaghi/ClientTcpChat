namespace ClientTcpChat.Forms
{
    partial class Di_Client_FormalMessageRequest
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
            this.UserName_TextBox = new System.Windows.Forms.TextBox();
            this.MessageText_TextBox = new System.Windows.Forms.TextBox();
            this.FirstLevel_Panel = new System.Windows.Forms.Panel();
            this.SendMessage_Button = new System.Windows.Forms.Button();
            this.FirstLevelMessageLabel = new System.Windows.Forms.Label();
            this.MessageText_Label = new System.Windows.Forms.Label();
            this.UserName_Label = new System.Windows.Forms.Label();
            this.FirstLevel_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // UserName_TextBox
            // 
            this.UserName_TextBox.Location = new System.Drawing.Point(95, 7);
            this.UserName_TextBox.MaxLength = 30;
            this.UserName_TextBox.Name = "UserName_TextBox";
            this.UserName_TextBox.Size = new System.Drawing.Size(154, 20);
            this.UserName_TextBox.TabIndex = 0;
            // 
            // MessageText_TextBox
            // 
            this.MessageText_TextBox.Location = new System.Drawing.Point(12, 50);
            this.MessageText_TextBox.MaxLength = 200;
            this.MessageText_TextBox.Multiline = true;
            this.MessageText_TextBox.Name = "MessageText_TextBox";
            this.MessageText_TextBox.Size = new System.Drawing.Size(237, 164);
            this.MessageText_TextBox.TabIndex = 1;
            // 
            // FirstLevel_Panel
            // 
            this.FirstLevel_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FirstLevel_Panel.Controls.Add(this.SendMessage_Button);
            this.FirstLevel_Panel.Controls.Add(this.FirstLevelMessageLabel);
            this.FirstLevel_Panel.Controls.Add(this.MessageText_Label);
            this.FirstLevel_Panel.Controls.Add(this.UserName_Label);
            this.FirstLevel_Panel.Controls.Add(this.MessageText_TextBox);
            this.FirstLevel_Panel.Controls.Add(this.UserName_TextBox);
            this.FirstLevel_Panel.Location = new System.Drawing.Point(12, 12);
            this.FirstLevel_Panel.Name = "FirstLevel_Panel";
            this.FirstLevel_Panel.Size = new System.Drawing.Size(260, 352);
            this.FirstLevel_Panel.TabIndex = 2;
            // 
            // SendMessage_Button
            // 
            this.SendMessage_Button.Location = new System.Drawing.Point(155, 326);
            this.SendMessage_Button.Name = "SendMessage_Button";
            this.SendMessage_Button.Size = new System.Drawing.Size(94, 23);
            this.SendMessage_Button.TabIndex = 4;
            this.SendMessage_Button.Text = "SendMessage";
            this.SendMessage_Button.UseVisualStyleBackColor = true;
            this.SendMessage_Button.Click += new System.EventHandler(this.SendMessage_Button_Click);
            // 
            // FirstLevelMessageLabel
            // 
            this.FirstLevelMessageLabel.ForeColor = System.Drawing.Color.Red;
            this.FirstLevelMessageLabel.Location = new System.Drawing.Point(9, 217);
            this.FirstLevelMessageLabel.Name = "FirstLevelMessageLabel";
            this.FirstLevelMessageLabel.Size = new System.Drawing.Size(240, 106);
            this.FirstLevelMessageLabel.TabIndex = 5;
            // 
            // MessageText_Label
            // 
            this.MessageText_Label.AutoSize = true;
            this.MessageText_Label.Location = new System.Drawing.Point(9, 34);
            this.MessageText_Label.Name = "MessageText_Label";
            this.MessageText_Label.Size = new System.Drawing.Size(74, 13);
            this.MessageText_Label.TabIndex = 3;
            this.MessageText_Label.Text = "MessageText:";
            // 
            // UserName_Label
            // 
            this.UserName_Label.AutoSize = true;
            this.UserName_Label.Location = new System.Drawing.Point(9, 10);
            this.UserName_Label.Name = "UserName_Label";
            this.UserName_Label.Size = new System.Drawing.Size(57, 13);
            this.UserName_Label.TabIndex = 2;
            this.UserName_Label.Text = "UserName";
            // 
            // Di_Client_FormalMessageRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 376);
            this.Controls.Add(this.FirstLevel_Panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Di_Client_FormalMessageRequest";
            this.Text = "Di_Client_FormalMessageRequest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Di_Client_FormalMessageRequest_FormClosing);
            this.FirstLevel_Panel.ResumeLayout(false);
            this.FirstLevel_Panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox UserName_TextBox;
        private System.Windows.Forms.TextBox MessageText_TextBox;
        private System.Windows.Forms.Panel FirstLevel_Panel;
        private System.Windows.Forms.Button SendMessage_Button;
        private System.Windows.Forms.Label MessageText_Label;
        private System.Windows.Forms.Label UserName_Label;
        private System.Windows.Forms.Label FirstLevelMessageLabel;
    }
}
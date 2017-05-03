namespace ClientTcpChat.Forms
{
    partial class AgreementInvitation_Form
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
            this.Yes_Button = new System.Windows.Forms.Button();
            this.No_Button = new System.Windows.Forms.Button();
            this.AgreementInviation_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Yes_Button
            // 
            this.Yes_Button.Location = new System.Drawing.Point(12, 192);
            this.Yes_Button.Name = "Yes_Button";
            this.Yes_Button.Size = new System.Drawing.Size(75, 32);
            this.Yes_Button.TabIndex = 0;
            this.Yes_Button.Text = "Yes";
            this.Yes_Button.UseVisualStyleBackColor = true;
            this.Yes_Button.Click += new System.EventHandler(this.Yes_Button_Click);
            // 
            // No_Button
            // 
            this.No_Button.Location = new System.Drawing.Point(190, 192);
            this.No_Button.Name = "No_Button";
            this.No_Button.Size = new System.Drawing.Size(75, 32);
            this.No_Button.TabIndex = 1;
            this.No_Button.Text = "No";
            this.No_Button.UseVisualStyleBackColor = true;
            this.No_Button.Click += new System.EventHandler(this.No_Button_Click);
            // 
            // AgreementInviation_RichTextBox
            // 
            this.AgreementInviation_RichTextBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.AgreementInviation_RichTextBox.Location = new System.Drawing.Point(12, 2);
            this.AgreementInviation_RichTextBox.Name = "AgreementInviation_RichTextBox";
            this.AgreementInviation_RichTextBox.ReadOnly = true;
            this.AgreementInviation_RichTextBox.Size = new System.Drawing.Size(253, 184);
            this.AgreementInviation_RichTextBox.TabIndex = 2;
            this.AgreementInviation_RichTextBox.Text = "";
            // 
            // AgreementInvitation_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 235);
            this.Controls.Add(this.AgreementInviation_RichTextBox);
            this.Controls.Add(this.No_Button);
            this.Controls.Add(this.Yes_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AgreementInvitation_Form";
            this.Text = "An Invitation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AgreementInvitation_Form_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Yes_Button;
        private System.Windows.Forms.Button No_Button;
        private System.Windows.Forms.RichTextBox AgreementInviation_RichTextBox;
    }
}
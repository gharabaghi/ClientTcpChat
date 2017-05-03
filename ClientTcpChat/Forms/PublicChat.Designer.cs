namespace ClientTcpChat.Forms
{
    partial class PublicChat
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.MessagesList_TextBox = new System.Windows.Forms.RichTextBox();
            this.send_button = new System.Windows.Forms.Button();
            this.ChatPersons_list = new System.Windows.Forms.ListBox();
            this.MyMessage_TextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.MessagesList_TextBox);
            this.panel1.Controls.Add(this.send_button);
            this.panel1.Controls.Add(this.ChatPersons_list);
            this.panel1.Controls.Add(this.MyMessage_TextBox);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(648, 458);
            this.panel1.TabIndex = 1;
            // 
            // MessagesList_TextBox
            // 
            this.MessagesList_TextBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.MessagesList_TextBox.Location = new System.Drawing.Point(4, 3);
            this.MessagesList_TextBox.Name = "MessagesList_TextBox";
            this.MessagesList_TextBox.ReadOnly = true;
            this.MessagesList_TextBox.Size = new System.Drawing.Size(494, 352);
            this.MessagesList_TextBox.TabIndex = 4;
            this.MessagesList_TextBox.Text = "";
            // 
            // send_button
            // 
            this.send_button.Location = new System.Drawing.Point(504, 420);
            this.send_button.Name = "send_button";
            this.send_button.Size = new System.Drawing.Size(71, 35);
            this.send_button.TabIndex = 3;
            this.send_button.Text = "Send";
            this.send_button.UseVisualStyleBackColor = true;
            this.send_button.Click += new System.EventHandler(this.send_button_Click);
            // 
            // ChatPersons_list
            // 
            this.ChatPersons_list.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ChatPersons_list.FormattingEnabled = true;
            this.ChatPersons_list.HorizontalScrollbar = true;
            this.ChatPersons_list.Location = new System.Drawing.Point(504, 16);
            this.ChatPersons_list.Name = "ChatPersons_list";
            this.ChatPersons_list.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.ChatPersons_list.Size = new System.Drawing.Size(141, 327);
            this.ChatPersons_list.TabIndex = 2;
            // 
            // MyMessage_TextBox
            // 
            this.MyMessage_TextBox.Location = new System.Drawing.Point(3, 361);
            this.MyMessage_TextBox.Multiline = true;
            this.MyMessage_TextBox.Name = "MyMessage_TextBox";
            this.MyMessage_TextBox.Size = new System.Drawing.Size(495, 94);
            this.MyMessage_TextBox.TabIndex = 1;
            // 
            // PublicChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 482);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "PublicChat";
            this.Text = "PublicChat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PublicChat_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox MessagesList_TextBox;
        private System.Windows.Forms.Button send_button;
        private System.Windows.Forms.ListBox ChatPersons_list;
        private System.Windows.Forms.TextBox MyMessage_TextBox;
    }
}
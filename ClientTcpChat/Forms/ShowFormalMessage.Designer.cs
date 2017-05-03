namespace ClientTcpChat.Forms
{
    partial class ShowFormalMessage_Form
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Messages", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("FormalMessage", System.Windows.Forms.HorizontalAlignment.Center);
            this.Messages_ListView = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Messages_ListView
            // 
            this.Messages_ListView.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.Messages_ListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            listViewGroup1.Header = "Messages";
            listViewGroup1.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup1.Name = "offline_messages";
            listViewGroup2.Header = "FormalMessage";
            listViewGroup2.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup2.Name = "formal_message";
            this.Messages_ListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.Messages_ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Messages_ListView.HideSelection = false;
            this.Messages_ListView.Location = new System.Drawing.Point(13, 13);
            this.Messages_ListView.MultiSelect = false;
            this.Messages_ListView.Name = "Messages_ListView";
            this.Messages_ListView.Size = new System.Drawing.Size(348, 270);
            this.Messages_ListView.TabIndex = 0;
            this.Messages_ListView.UseCompatibleStateImageBehavior = false;
            this.Messages_ListView.View = System.Windows.Forms.View.Tile;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(286, 289);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Read";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ShowFormalMessage_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 324);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Messages_ListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ShowFormalMessage_Form";
            this.Text = "Messages";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShowFormalMessage_Form_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView Messages_ListView;
        private System.Windows.Forms.Button button1;
    }
}
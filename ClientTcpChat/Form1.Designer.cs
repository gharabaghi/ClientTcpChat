namespace ClientTcpChat
{
    partial class Form1
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
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Friends List", System.Windows.Forms.HorizontalAlignment.Center);
            this.OwnUserName_Label = new System.Windows.Forms.Label();
            this.StartChat_Button = new System.Windows.Forms.Button();
            this.FriendList_ListView = new System.Windows.Forms.ListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinAPublicChatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoginSignup_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.LoginFormPassword_Label = new System.Windows.Forms.Label();
            this.LoginFormUserName_Label = new System.Windows.Forms.Label();
            this.LoginFormError_Label = new System.Windows.Forms.Label();
            this.LoginForm_Label = new System.Windows.Forms.Label();
            this.Login_Button = new System.Windows.Forms.Button();
            this.LoginPassword_TextBox = new System.Windows.Forms.TextBox();
            this.LoginUserName_TextBox = new System.Windows.Forms.TextBox();
            this.DisconnectedConnect_Button = new System.Windows.Forms.Button();
            this.UdpTcpStatus_Label = new System.Windows.Forms.Label();
            this.AutheMode_Panel = new System.Windows.Forms.Panel();
            this.UnAutheMode_Panel = new System.Windows.Forms.Panel();
            this.DisConnectedMode_Panel = new System.Windows.Forms.Panel();
            this.TcpUdpMode_Panel = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.AutheMode_Panel.SuspendLayout();
            this.UnAutheMode_Panel.SuspendLayout();
            this.DisConnectedMode_Panel.SuspendLayout();
            this.TcpUdpMode_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // OwnUserName_Label
            // 
            this.OwnUserName_Label.Font = new System.Drawing.Font("Adobe Arabic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.OwnUserName_Label.ForeColor = System.Drawing.Color.Navy;
            this.OwnUserName_Label.Location = new System.Drawing.Point(3, 0);
            this.OwnUserName_Label.Name = "OwnUserName_Label";
            this.OwnUserName_Label.Size = new System.Drawing.Size(280, 43);
            this.OwnUserName_Label.TabIndex = 3;
            // 
            // StartChat_Button
            // 
            this.StartChat_Button.Location = new System.Drawing.Point(208, 365);
            this.StartChat_Button.Name = "StartChat_Button";
            this.StartChat_Button.Size = new System.Drawing.Size(75, 23);
            this.StartChat_Button.TabIndex = 2;
            this.StartChat_Button.Text = "Start Chat";
            this.StartChat_Button.UseVisualStyleBackColor = true;
            this.StartChat_Button.Click += new System.EventHandler(this.StartChat_Button_Click);
            // 
            // FriendList_ListView
            // 
            this.FriendList_ListView.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.FriendList_ListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FriendList_ListView.Font = new System.Drawing.Font("Adobe Arabic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FriendList_ListView.FullRowSelect = true;
            listViewGroup5.Header = "Friends List";
            listViewGroup5.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup5.Name = "Friends";
            this.FriendList_ListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup5});
            this.FriendList_ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.FriendList_ListView.HideSelection = false;
            this.FriendList_ListView.Location = new System.Drawing.Point(3, 46);
            this.FriendList_ListView.MultiSelect = false;
            this.FriendList_ListView.Name = "FriendList_ListView";
            this.FriendList_ListView.Size = new System.Drawing.Size(280, 313);
            this.FriendList_ListView.TabIndex = 1;
            this.FriendList_ListView.TileSize = new System.Drawing.Size(280, 30);
            this.FriendList_ListView.UseCompatibleStateImageBehavior = false;
            this.FriendList_ListView.View = System.Windows.Forms.View.Tile;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.userToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(312, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAUserToolStripMenuItem,
            this.joinAPublicChatToolStripMenuItem,
            this.sendMessageToolStripMenuItem});
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.userToolStripMenuItem.Text = "User";
            // 
            // addAUserToolStripMenuItem
            // 
            this.addAUserToolStripMenuItem.Name = "addAUserToolStripMenuItem";
            this.addAUserToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.addAUserToolStripMenuItem.Text = "Add A User";
            this.addAUserToolStripMenuItem.Click += new System.EventHandler(this.addAUserToolStripMenuItem_Click);
            // 
            // joinAPublicChatToolStripMenuItem
            // 
            this.joinAPublicChatToolStripMenuItem.Name = "joinAPublicChatToolStripMenuItem";
            this.joinAPublicChatToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.joinAPublicChatToolStripMenuItem.Text = "Join A Public Chat";
            this.joinAPublicChatToolStripMenuItem.Click += new System.EventHandler(this.joinAPublicChatToolStripMenuItem_Click);
            // 
            // sendMessageToolStripMenuItem
            // 
            this.sendMessageToolStripMenuItem.Name = "sendMessageToolStripMenuItem";
            this.sendMessageToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.sendMessageToolStripMenuItem.Text = "Send Message";
            this.sendMessageToolStripMenuItem.Click += new System.EventHandler(this.sendMessageToolStripMenuItem_Click);
            // 
            // LoginSignupLink_Label
            // 
            this.LoginSignup_LinkLabel.AutoSize = true;
            this.LoginSignup_LinkLabel.Location = new System.Drawing.Point(15, 153);
            this.LoginSignup_LinkLabel.Name = "LoginSignupLink_Label";
            this.LoginSignup_LinkLabel.Size = new System.Drawing.Size(97, 13);
            this.LoginSignup_LinkLabel.TabIndex = 7;
            this.LoginSignup_LinkLabel.TabStop = true;
            this.LoginSignup_LinkLabel.Text = "Create An Account";
            this.LoginSignup_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // LoginFormPassword_Label
            // 
            this.LoginFormPassword_Label.AutoSize = true;
            this.LoginFormPassword_Label.Location = new System.Drawing.Point(10, 109);
            this.LoginFormPassword_Label.Name = "LoginFormPassword_Label";
            this.LoginFormPassword_Label.Size = new System.Drawing.Size(53, 13);
            this.LoginFormPassword_Label.TabIndex = 6;
            this.LoginFormPassword_Label.Text = "Password";
            // 
            // LoginFormUserName_Label
            // 
            this.LoginFormUserName_Label.AutoSize = true;
            this.LoginFormUserName_Label.Location = new System.Drawing.Point(10, 70);
            this.LoginFormUserName_Label.Name = "LoginFormUserName_Label";
            this.LoginFormUserName_Label.Size = new System.Drawing.Size(60, 13);
            this.LoginFormUserName_Label.TabIndex = 5;
            this.LoginFormUserName_Label.Text = "User Name";
            // 
            // LoginFormError_Label
            // 
            this.LoginFormError_Label.ForeColor = System.Drawing.Color.Red;
            this.LoginFormError_Label.Location = new System.Drawing.Point(13, 194);
            this.LoginFormError_Label.Name = "LoginFormError_Label";
            this.LoginFormError_Label.Size = new System.Drawing.Size(252, 183);
            this.LoginFormError_Label.TabIndex = 4;
            // 
            // LoginForm_Label
            // 
            this.LoginForm_Label.AutoSize = true;
            this.LoginForm_Label.Font = new System.Drawing.Font("Adobe Arabic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.LoginForm_Label.Location = new System.Drawing.Point(8, 9);
            this.LoginForm_Label.Name = "LoginForm_Label";
            this.LoginForm_Label.Size = new System.Drawing.Size(56, 26);
            this.LoginForm_Label.TabIndex = 3;
            this.LoginForm_Label.Text = "Login";
            // 
            // Login_Button
            // 
            this.Login_Button.Font = new System.Drawing.Font("Adobe Arabic", 15.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Login_Button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Login_Button.Location = new System.Drawing.Point(186, 142);
            this.Login_Button.Name = "Login_Button";
            this.Login_Button.Size = new System.Drawing.Size(79, 34);
            this.Login_Button.TabIndex = 2;
            this.Login_Button.Text = "Login";
            this.Login_Button.UseVisualStyleBackColor = true;
            this.Login_Button.Click += new System.EventHandler(this.Login_Button_Click);
            // 
            // LoginPassword_TextBox
            // 
            this.LoginPassword_TextBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.LoginPassword_TextBox.Location = new System.Drawing.Point(103, 106);
            this.LoginPassword_TextBox.Name = "LoginPassword_TextBox";
            this.LoginPassword_TextBox.PasswordChar = '*';
            this.LoginPassword_TextBox.Size = new System.Drawing.Size(162, 26);
            this.LoginPassword_TextBox.TabIndex = 1;
            // 
            // LoginUserName_TextBox
            // 
            this.LoginUserName_TextBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.LoginUserName_TextBox.Location = new System.Drawing.Point(103, 67);
            this.LoginUserName_TextBox.Name = "LoginUserName_TextBox";
            this.LoginUserName_TextBox.Size = new System.Drawing.Size(162, 26);
            this.LoginUserName_TextBox.TabIndex = 0;
            // 
            // DisconnectedConnect_Button
            // 
            this.DisconnectedConnect_Button.Location = new System.Drawing.Point(109, 153);
            this.DisconnectedConnect_Button.Name = "DisconnectedConnect_Button";
            this.DisconnectedConnect_Button.Size = new System.Drawing.Size(75, 23);
            this.DisconnectedConnect_Button.TabIndex = 0;
            this.DisconnectedConnect_Button.Text = "Connect";
            this.DisconnectedConnect_Button.UseVisualStyleBackColor = true;
            this.DisconnectedConnect_Button.Click += new System.EventHandler(this.DisconnectedConnect_Button_Click);
            // 
            // UdpTcpStatus_Label
            // 
            this.UdpTcpStatus_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.UdpTcpStatus_Label.ForeColor = System.Drawing.Color.Gray;
            this.UdpTcpStatus_Label.Location = new System.Drawing.Point(52, 85);
            this.UdpTcpStatus_Label.Name = "UdpTcpStatus_Label";
            this.UdpTcpStatus_Label.Size = new System.Drawing.Size(183, 161);
            this.UdpTcpStatus_Label.TabIndex = 0;
            // 
            // AutheMode_Panel
            // 
            this.AutheMode_Panel.Controls.Add(this.StartChat_Button);
            this.AutheMode_Panel.Controls.Add(this.FriendList_ListView);
            this.AutheMode_Panel.Controls.Add(this.OwnUserName_Label);
            this.AutheMode_Panel.Location = new System.Drawing.Point(11, 38);
            this.AutheMode_Panel.Name = "AutheMode_Panel";
            this.AutheMode_Panel.Size = new System.Drawing.Size(290, 390);
            this.AutheMode_Panel.TabIndex = 8;
            // 
            // UnAutheMode_Panel
            // 
            this.UnAutheMode_Panel.Controls.Add(this.LoginUserName_TextBox);
            this.UnAutheMode_Panel.Controls.Add(this.LoginPassword_TextBox);
            this.UnAutheMode_Panel.Controls.Add(this.Login_Button);
            this.UnAutheMode_Panel.Controls.Add(this.LoginForm_Label);
            this.UnAutheMode_Panel.Controls.Add(this.LoginSignup_LinkLabel);
            this.UnAutheMode_Panel.Controls.Add(this.LoginFormError_Label);
            this.UnAutheMode_Panel.Controls.Add(this.LoginFormPassword_Label);
            this.UnAutheMode_Panel.Controls.Add(this.LoginFormUserName_Label);
            this.UnAutheMode_Panel.Location = new System.Drawing.Point(11, 38);
            this.UnAutheMode_Panel.Name = "UnAutheMode_Panel";
            this.UnAutheMode_Panel.Size = new System.Drawing.Size(290, 390);
            this.UnAutheMode_Panel.TabIndex = 9;
            // 
            // DisConnectedMode_Panel
            // 
            this.DisConnectedMode_Panel.Controls.Add(this.TcpUdpMode_Panel);
            this.DisConnectedMode_Panel.Controls.Add(this.DisconnectedConnect_Button);
            this.DisConnectedMode_Panel.Location = new System.Drawing.Point(11, 38);
            this.DisConnectedMode_Panel.Name = "DisConnectedMode_Panel";
            this.DisConnectedMode_Panel.Size = new System.Drawing.Size(290, 390);
            this.DisConnectedMode_Panel.TabIndex = 10;
            // 
            // TcpUdpMode_Panel
            // 
            this.TcpUdpMode_Panel.Controls.Add(this.UdpTcpStatus_Label);
            this.TcpUdpMode_Panel.Location = new System.Drawing.Point(0, 0);
            this.TcpUdpMode_Panel.Name = "TcpUdpMode_Panel";
            this.TcpUdpMode_Panel.Size = new System.Drawing.Size(290, 390);
            this.TcpUdpMode_Panel.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 434);
            this.Controls.Add(this.DisConnectedMode_Panel);
            this.Controls.Add(this.UnAutheMode_Panel);
            this.Controls.Add(this.AutheMode_Panel);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.AutheMode_Panel.ResumeLayout(false);
            this.UnAutheMode_Panel.ResumeLayout(false);
            this.UnAutheMode_Panel.PerformLayout();
            this.DisConnectedMode_Panel.ResumeLayout(false);
            this.TcpUdpMode_Panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView FriendList_ListView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addAUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joinAPublicChatToolStripMenuItem;
        private System.Windows.Forms.Button StartChat_Button;
        private System.Windows.Forms.ToolStripMenuItem sendMessageToolStripMenuItem;
        private System.Windows.Forms.Label OwnUserName_Label;
        private System.Windows.Forms.Label LoginFormPassword_Label;
        private System.Windows.Forms.Label LoginFormUserName_Label;
        private System.Windows.Forms.Label LoginFormError_Label;
        private System.Windows.Forms.Label LoginForm_Label;
        private System.Windows.Forms.Button Login_Button;
        private System.Windows.Forms.TextBox LoginPassword_TextBox;
        private System.Windows.Forms.TextBox LoginUserName_TextBox;
        private System.Windows.Forms.LinkLabel LoginSignup_LinkLabel;
        private System.Windows.Forms.Label UdpTcpStatus_Label;
        private System.Windows.Forms.Button DisconnectedConnect_Button;
        private System.Windows.Forms.Panel AutheMode_Panel;
        private System.Windows.Forms.Panel UnAutheMode_Panel;
        private System.Windows.Forms.Panel DisConnectedMode_Panel;
        private System.Windows.Forms.Panel TcpUdpMode_Panel;



    }
}


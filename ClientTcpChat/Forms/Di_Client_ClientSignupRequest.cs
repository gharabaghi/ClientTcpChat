using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientTcpChat.Classes;
using ClientTcpChat.Forms;
using CommonChatTypes;

namespace ClientTcpChat.Forms
{
    public partial class Di_Client_ClientSignupRequest : BaseFormedDialog
    {
        public Di_Client_ClientSignupRequest(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send)
        {
            InitializeComponent();

            base.BaseConstruct(p_dialog_id, TypeOfDialog.ClientSignupRequest, p_remove_dialog_from_manager, 20, p_send);
            
            level_counts = 1;
            all_dialog_levels.Add(1, new FormedDialogLevelsInformation(new ALevelOfDialog(FirstlevelFunction), FormedDialogLevelType.UserSendingMessage));

            FirstLevelPanel = new System.Windows.Forms.Panel();
            all_form_levels_panels.Add(1, FirstLevelPanel);
            all_form_level_message_labels.Add(1, FirstLevel_MessageLabel);
            Start();
        }

        private void FirstlevelFunction()
        {
            if (string.IsNullOrWhiteSpace(UserName_TextBox.Text) || string.IsNullOrWhiteSpace(Password_TextBox.Text))
            {
                all_form_level_message_labels[1].Text = "fill both fields.";
                ResetFields();
                UnwaitForm();
                return;
            }
            if (!HelperFunctions.UserNameStringCheck(UserName_TextBox.Text) || !HelperFunctions.UserNameStringCheck(Password_TextBox.Text))
            {
                all_form_level_message_labels[1].Text = "illegal characters for username and/or password";
                ResetFields();
                UnwaitForm();
                return;
            }

            Di_Mess_SignUpRequestData login_request_message_object = new Di_Mess_SignUpRequestData(UserName_TextBox.Text, Password_TextBox.Text);
            Send(TypeOfDialogMessage.SignUpRequestData, login_request_message_object);
        }

        protected override void CreateUserStrings()
        {
            result_message_string = "your request to create an account accepted";
            reject_message_string = "your request to create an account didnt accept";
        }

        protected override void EnableAndDisableControls(bool p_flag)
        {
            base.EnableAndDisableControls(p_flag);
            UserName_TextBox.Enabled = p_flag;
            Password_TextBox.Enabled = p_flag;
            CreateAccount_Button.Enabled = p_flag;
        }
        protected override void ResetFields()
        {
            base.ResetFields();
            UserName_TextBox.Text = string.Empty;
            Password_TextBox.Text = string.Empty;
        }

        private void CreateAccount_Button_Click(object sender, EventArgs e)
        {
            UserNextLevelRequest();
        }

        private void Di_Client_ClientSignupRequest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag == false)
            {
                CancelDialog("user closed dialog.");
                return;
            }
        }

        public override bool Equals(object obj)
        {
            Di_Client_ClientSignupRequest temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_ClientSignupRequest)obj;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientTcpChat.Forms;
using ClientTcpChat.Classes;
using CommonChatTypes;

namespace ClientTcpChat.Forms
{
    public partial class Di_Client_FormalMessageRequest : BaseFormedDialog
    {
        public Di_Client_FormalMessageRequest(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send)
        {
            InitializeComponent();

            base.BaseConstruct(p_dialog_id, TypeOfDialog.FormalMessageRequest, p_remove_dialog_from_manager, 3, p_send);

            level_counts = 1;
            all_dialog_levels.Add(1, new FormedDialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), FormedDialogLevelType.UserSendingMessage));

            all_form_level_message_labels.Add(1, FirstLevelMessageLabel);
            all_form_levels_panels.Add(1, FirstLevel_Panel);

            Start();
        }

        protected override void CreateUserStrings()
        {
            result_message_string = "your message to '" + UserName_TextBox.Text + "' sent";
            reject_message_string = "your message to '" + UserName_TextBox.Text + "' didnt sent";
        }

        private void FirstLevelFunction()
        {
            if (string.IsNullOrWhiteSpace(UserName_TextBox.Text) || string.IsNullOrWhiteSpace(MessageText_TextBox.Text))
            {
                all_form_level_message_labels[1].Text = " username and/or password field is-are empty.";
                ResetFields();
                UnwaitForm();
                return;
            }
            if (!HelperFunctions.UserNameStringCheck(UserName_TextBox.Text))
            {
                all_form_level_message_labels[1].Text = "illegal characters form username";
                ResetFields();
                UnwaitForm();
                return;
            }

            Di_Mess_ClientFormalMessageRequest formal_message_request_message_object = new Di_Mess_ClientFormalMessageRequest(UserName_TextBox.Text, MessageText_TextBox.Text);
            Send(TypeOfDialogMessage.ClientFormalMessageRequest, formal_message_request_message_object);
            return;
        }

        private void SendMessage_Button_Click(object sender, EventArgs e)
        {
            UserNextLevelRequest();
        }

        protected override void ResetFields()
        {
            base.ResetFields();
            UserName_TextBox.Text = string.Empty;
            MessageText_TextBox.Text = string.Empty;
        }

        protected override void EnableAndDisableControls(bool p_flag)
        {
            base.EnableAndDisableControls(p_flag);
            UserName_TextBox.Enabled = p_flag;
            MessageText_TextBox.Enabled = p_flag;
            SendMessage_Button.Enabled = p_flag;
        }

        private void Di_Client_FormalMessageRequest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag == false)
            {
                CancelDialog("user closed dialog.");
                return;
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
            Di_Client_FormalMessageRequest temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_FormalMessageRequest)obj;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}

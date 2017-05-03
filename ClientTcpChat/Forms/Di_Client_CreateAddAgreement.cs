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
    public partial class Di_Client_CreateAddAgreement : BaseFormedDialog
    {
        string invited_user_name;

        public Di_Client_CreateAddAgreement(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send)
        {
            InitializeComponent();

            invited_user_name = "";

            base.BaseConstruct(p_dialog_id, TypeOfDialog.CreateAddAgreement, p_remove_dialog_from_manager, 3, p_send);

            level_counts = 1;
            all_dialog_levels.Add(1, new FormedDialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), FormedDialogLevelType.UserSendingMessage));

            all_form_levels_panels.Add(1, FirstLevelPanel);
            all_form_level_message_labels.Add(1, FirstLevelMessageLabel);

            Start();
        }

        protected override void CreateUserStrings()
        {
            result_message_string = "your request to add '" + invited_user_name + "' sent to server.";
            reject_message_string = "your request to add '" + invited_user_name + "' didnt send to server. please try again.";
        }

        private void FirstLevelFunction()
        {
            if (string.IsNullOrWhiteSpace(UserName_TextBox.Text))
            {
                all_form_level_message_labels[1].Text = "Enter A UserName";
                ResetFields();
                UnwaitForm();
                return;
            }
            if (!HelperFunctions.UserNameStringCheck(UserName_TextBox.Text))
            {
                all_form_level_message_labels[1].Text = "illegal characters";
                ResetFields();
                UnwaitForm();
                return;
            }
            Di_Mess_CreateAddAgreementRequest create_add_agreement_request_message_object = new Di_Mess_CreateAddAgreementRequest(invited_user_name);
            Send(TypeOfDialogMessage.CreateAddAgreementRequest, create_add_agreement_request_message_object);
            return;
        }

        
        protected override void EnableAndDisableControls(bool p_flag)
        {
            base.EnableAndDisableControls(p_flag);
            Add_Button.Enabled = p_flag;
            UserName_TextBox.Enabled = p_flag;
        }

        protected override void ResetFields()
        {
            if (current_level == 1)
            {
                UserName_TextBox.Text = string.Empty;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            invited_user_name = UserName_TextBox.Text; 
            all_form_level_message_labels[1].Text = "";
            UserNextLevelRequest();
        }

        private void Di_Client_CreateAddAgreement_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag == false)
            {
                CancelDialog("user closed dialog.");
                return;
            }
        }

        public override bool Equals(object obj)
        {
            Di_Client_CreateAddAgreement temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_CreateAddAgreement)obj;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}

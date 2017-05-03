using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientTcpChat.Classes;
using ClientTcpChat.Forms;
using System.Windows.Forms;
using System.Drawing;
using CommonChatTypes;

namespace ClientTcpChat
{

    public enum TypeOfMode
    {
        Dissconnected,
        UdpConnection,
        TcpAllocation,
        UnAuthMode,
        AuthMode,
    }

    partial class Form1
    {

        /// <summary>
        /// for AuthMode
        /// </summary>

        string own_user_name;
        List<OfflineMessage> received_offline_messages;
        List<PersonStatus> friends_and_status;
        AllDiaogs all_dialogs;
        AllChats all_chats;
        List<int> public_chat_ids;
        List<ShowFormalMessage_Form> all_show_formal_message_forms;
        List<AgreementInvitationInfo> all_agreement_invitations;
        List<AgreementInvitation_Form> all_show_agreement_invitations_forms;
        SelectPublicChat select_public_chat_form;
        bool select_public_chat_is_open;

        public void AuthModeReceive(FinalMessageForClient p_message)
        {
            if (p_message.Get_message_type == TypeOfMessage.Dialog)
            {
                DialogMessageForClient t_dialog_message = null;
                try
                {
                    t_dialog_message = ((DialogMessageForClient)p_message.Get_message_object);
                }
                catch (Exception)
                {
                    return;
                }
                all_dialogs.Receive(t_dialog_message);
            }
            else if (p_message.Get_message_type == TypeOfMessage.Chat)
            {
                ChatMessageForClient t_chat_message = null;
                try
                {
                    t_chat_message = ((ChatMessageForClient)p_message.Get_message_object);
                }
                catch (Exception)
                {
                    return;
                }
                all_chats.Receive(t_chat_message);
            }
        }

        private void ShowFriends()
        {
            ListViewGroup friends_list_view_group = FriendList_ListView.Groups["Friends"];
            FriendList_ListView.Items.Clear();

            for (int i = 0; i < friends_and_status.Count; i++)
            {
                if (friends_and_status[i].Get_user_status == UserStatus.Online)
                {
                    FriendList_ListView.Items.Add(friends_and_status[i].Get_user_name + ": online");
                    FriendList_ListView.Items[i].ForeColor = Color.Blue;
                    FriendList_ListView.Items[i].Group = friends_list_view_group;
                }
                else if (friends_and_status[i].Get_user_status == UserStatus.Offline)
                {
                    FriendList_ListView.Items.Add(friends_and_status[i].Get_user_name + ": Offline");
                    FriendList_ListView.Items[i].ForeColor = Color.Red;
                    FriendList_ListView.Items[i].Group = friends_list_view_group;
                }
            }
            int bbb = FriendList_ListView.Items.Count;
            return;
        }

        private void AuthModeStart(LoginData p_login_data)
        {

            AutheMode_Panel.Visible = true;

            own_user_name = p_login_data.Get_user_name;
            friends_and_status = p_login_data.Get_friends_list_and_status;
            public_chat_ids = p_login_data.Get_public_chat_ids;
            all_agreement_invitations = p_login_data.Get_agreement_invitations;
            received_offline_messages = p_login_data.Get_offline_messages;

            OwnUserName_Label.Text = "LoggedAs: " + own_user_name;

            select_public_chat_is_open = false;

            menuStrip1.Visible = true;

            all_show_agreement_invitations_forms = new List<AgreementInvitation_Form>();
            all_show_formal_message_forms = new List<ShowFormalMessage_Form>();

            ShowFriends();
            foreach (AgreementInvitationInfo t_agreement_invitation_info in all_agreement_invitations)
            {
                AgreementInvitation_Form t_new_agreement_invitation_show_form = new AgreementInvitation_Form(t_agreement_invitation_info
                    , new RemoveShowAgreementInvitationForm(RemoveShowAgreementInvitationForm), new CreateGetAgreementAnswerDialog(Mo_CreateGetAgreementAnswerDialog));
                if (!all_show_agreement_invitations_forms.Contains(t_new_agreement_invitation_show_form))
                {
                    all_show_agreement_invitations_forms.Add(t_new_agreement_invitation_show_form);
                    t_new_agreement_invitation_show_form.StartForm();
                }
                else
                {
                    t_new_agreement_invitation_show_form.DontStartForm();
                }
            }
            if (received_offline_messages.Count > 0)
            {
                ShowFormalMessage_Form show_offline_messages_form = new ShowFormalMessage_Form(received_offline_messages, new OfflienMesssageRead(Mo_ServerOfflineMessagesReadInform)
                    , new RemoveShowOfflineMessagesForm(RemoveShowOfflineMessagesFormFromList));
                if (!all_show_formal_message_forms.Contains(show_offline_messages_form))
                {
                    all_show_formal_message_forms.Add(show_offline_messages_form);
                    show_offline_messages_form.StartForm();
                }
                else
                {
                    show_offline_messages_form.DontStartForm();
                }
            }

            Cl_ClientDelegatesForDialogs all_dialogs_delegates = new Cl_ClientDelegatesForDialogs(new SendDialogMessage(Mo_SendDialogMessage), new MessageShow(Mo_MessageShow)
            , new SomeoneLeftChat(Mo_SomeoneLeftChat), new InformEjectFromChat(Mo_InformEjectFromChat), new CreateEmptyPrivateChat(Mo_CreateEmptyPrivateChat)
            , new InformJoinToPublicChat(Mo_InformJoinToPublicChat), new SomeoneJoinedChat(Mo_SomeoneJoinedChat), new InformFriendChangedStatus(Mo_InformFriendChangedStatus)
            , new ShowFormalMessage(Mo_ShowFormalMessage), new InformFriendListChanhged(Mo_InformFriendListChanhged), new InformCreatedPrivateChat(Mo_InformCreatedPrivateChat)
            ,new ShowAnAgreementInvitation(Mo_ShowAnAgreementInvitation));
            all_dialogs = new AllDiaogs(all_dialogs_delegates);

            all_chats = new AllChats(new SendChatMessage(Mo_SendChatMessage), new StartLeaveChatRequest(mo_StartLeaveChatRequest));

        }

        private void AuthModeEnd()
        {
            for (int i = 0; i < all_show_formal_message_forms.Count; i++)
            {
                all_show_formal_message_forms[i].Close();
            }
            all_show_formal_message_forms.Clear();

            for (int i = 0; i < all_show_agreement_invitations_forms.Count; i++)
            {
                all_show_agreement_invitations_forms[i].Close();
            }
            all_show_agreement_invitations_forms.Clear();

            if (select_public_chat_is_open)
            {
                select_public_chat_form.Close();
            }

            own_user_name = "";
            received_offline_messages = new List<OfflineMessage>();
            friends_and_status = new List<PersonStatus>();
            all_agreement_invitations = new List<AgreementInvitationInfo>();

            OwnUserName_Label.Text = "";
            FriendList_ListView.Items.Clear();
            FriendList_ListView.Refresh();
            menuStrip1.Visible = false;
            menuStrip1.Hide();
            AutheMode_Panel.Visible = false;
            AutheMode_Panel.Hide();

            all_dialogs.Clear();
            all_chats.Cleare();
        }

        public void RemoveShowOfflineMessagesFormFromList(ShowFormalMessage_Form p_form_to_remove)
        {
            all_show_formal_message_forms.Remove(p_form_to_remove);
        }

        public void RemoveShowAgreementInvitationForm(AgreementInvitation_Form p_form_to_remove)
        {
            all_show_agreement_invitations_forms.Remove(p_form_to_remove);
        }

        public void Mo_StartClientJoinPublicChatRequestDialog(int p_chat_id)
        {
            all_dialogs.CreateClientJoinPublicChatRequestDialog(p_chat_id);
        }

        public void Mo_SendDialogMessage(DialogMessageForServer p_message)
        {
            SendMessagesToServer(new FinalMessageForServer(TypeOfMessage.Dialog, p_message));
        }

        public void Mo_SomeoneLeftChat(string p_user_left_chat_name, int p_chat_id, TypeOfChat p_chat_type)
        {
            all_chats.SomeoneLeftChat(p_user_left_chat_name, p_chat_id, p_chat_type);
        }

        public void Mo_InformEjectFromChat(int p_chat_id, string p_ejecting_comment, TypeOfChat p_chat_type)
        {
            bool ejected = all_chats.EjectChat(p_chat_id, p_chat_type);
            if (ejected)
            {
                MessageBox.Show("chat closed because: " + p_ejecting_comment);
            }
        }

        public void Mo_CreateEmptyPrivateChat(int p_chat_id)
        {
            all_chats.CreateEmptyPrivateChat(own_user_name, p_chat_id);
        }

        public void Mo_InformJoinToPublicChat(int p_chat_id, List<string> p_public_chat_persons)
        {
            all_chats.CreatePublicChat(own_user_name, p_public_chat_persons, p_chat_id);
        }

        public void Mo_SomeoneJoinedChat(string p_user_joined_chat_name, int p_chat_id, TypeOfChat p_chat_type)
        {
            all_chats.SomeoneJoinedChat(p_user_joined_chat_name, p_chat_id, p_chat_type);
        }

        public void Mo_ShowFormalMessage(FormalMessage p_formal_message, int p_message_id)
        {
            ShowFormalMessage_Form show_formal_message_form = new ShowFormalMessage_Form(p_formal_message, p_message_id, new OfflienMesssageRead(Mo_ServerOfflineMessagesReadInform)
                , new RemoveShowOfflineMessagesForm(RemoveShowOfflineMessagesFormFromList));
            if (!all_show_formal_message_forms.Contains(show_formal_message_form))
            {
                all_show_formal_message_forms.Add(show_formal_message_form);
                show_formal_message_form.StartForm();
            }
            else
            {
                show_formal_message_form.DontStartForm();
            }
        }

        public void Mo_InformFriendListChanhged(List<PersonStatus> p_new_friends_and_status)
        {
            friends_and_status = p_new_friends_and_status;
            ShowFriends();
        }

        public void Mo_InformCreatedPrivateChat(string p_user_name, int p_chat_id)
        {
            all_chats.CreatePrivateChat(own_user_name, p_user_name, p_chat_id);
        }

        public void Mo_ShowAnAgreementInvitation(AgreementInvitationInfo p_agreement_invitation_info)
        {
            AgreementInvitation_Form t_new_agreement_invitation_show_form = new AgreementInvitation_Form(p_agreement_invitation_info
                    , new RemoveShowAgreementInvitationForm(RemoveShowAgreementInvitationForm), new CreateGetAgreementAnswerDialog(Mo_CreateGetAgreementAnswerDialog));
            if (!all_show_agreement_invitations_forms.Contains(t_new_agreement_invitation_show_form))
            {
                all_show_agreement_invitations_forms.Add(t_new_agreement_invitation_show_form);
                t_new_agreement_invitation_show_form.StartForm();
            }
            else
            {
                t_new_agreement_invitation_show_form.DontStartForm();
            }
        }


        public void Mo_ServerOfflineMessagesReadInform(List<int> p_messages_ids)
        {
            all_dialogs.CreateServerOfflineMessageReadInformDialog(p_messages_ids);
        }

        public void Mo_InformFriendChangedStatus(PersonStatus p_user_and_new_status)
        {
            for (int i = 0; i < friends_and_status.Count; i++)
            {
                if (friends_and_status[i].Get_user_name == p_user_and_new_status.Get_user_name)
                {
                    friends_and_status.RemoveAt(i);
                    friends_and_status.Add(p_user_and_new_status);
                    ShowFriends();
                    return;
                }
            }
        }

        public void Mo_SendChatMessage(ChatMessageForServer p_message)
        {
            SendMessagesToServer(new FinalMessageForServer(TypeOfMessage.Chat, p_message));
        }

        public void mo_StartLeaveChatRequest(int p_chat_id, TypeOfChat p_chat_type)
        {
            all_dialogs.CreateClientLeaveChatRequestDialog(p_chat_id, p_chat_type);
        }

        public void Mo_StartCreatePrivateChatRequestDialog(string p_invited_person_user_name)
        {
            all_dialogs.CreateCreatePrivateChatRequestDialog(p_invited_person_user_name);
        }

        private void joinAPublicChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (select_public_chat_is_open == true)
            {
                MessageBox.Show("This form is open. you cannot open another");
                return;
            }
            else
            {
                select_public_chat_is_open = true;
                SelectPublicChat select_public_chat = new SelectPublicChat(public_chat_ids, new StartClientJoinPublicChatRequestDialog(Mo_StartClientJoinPublicChatRequestDialog)
                , new SelectPublicChatFormCloesd(Mo_SelectPublicChatFormClosed));
                select_public_chat_form = select_public_chat;
                select_public_chat.Show();
            }

        }

        public void Mo_SelectPublicChatFormClosed()
        {
            select_public_chat_is_open = false;
        }

        private void addAUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            all_dialogs.CreateCreateAddAgreementDialog();
        }

        private void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            all_dialogs.CreateFormalMessageRequestDialog();
        }

        public void Mo_CreateGetAgreementAnswerDialog(int p_agreement_id, TypeOfAgreement p_agreement_type, bool p_answer)
        {
            all_dialogs.CreateGetAgreementAnswerDialog(p_agreement_id, p_agreement_type, p_answer);
        }

        private void StartChat_Button_Click(object sender, EventArgs e)
        {
            if (FriendList_ListView.SelectedItems.Count < 1)
            {
                return;
            }
            else if (FriendList_ListView.SelectedItems.Count > 0)
            {
                string selected_friend = FriendList_ListView.SelectedItems[0].Text;


                bool is_there_such_person = false;
                string invited_user_name = "";

                foreach (PersonStatus t_person_status in friends_and_status)
                {
                    if (selected_friend.Contains(t_person_status.Get_user_name))
                    {
                        is_there_such_person = true;
                        invited_user_name = t_person_status.Get_user_name;
                        break;
                    }
                }
                if (!is_there_such_person)
                {
                    MessageBox.Show("there is not such person");
                    return;
                }
                Mo_StartCreatePrivateChatRequestDialog(invited_user_name);
                return;
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelWorker();
            ChangeMode(TypeOfMode.Dissconnected);
        }

        void LoginUserName_TextBox_Enter(object sender, EventArgs e)
        {
            if (LoginUserName_TextBox.Text.Length > 0)
            {
                LoginUserName_TextBox.SelectAll();
            }
        }

        void LoginPassword_TextBox_Enter(object sender, EventArgs e)
        {
            if (LoginPassword_TextBox.Text.Length > 0)
            {
                LoginPassword_TextBox.SelectAll();
            }
        }
    }

    partial class Form1
    {
        /// <summary>
        /// for UnAuthMode
        /// </sumamry>
        /// 
        int Unauth_mode_level_counts;
        int current_level;
        int login_data_request_retry_counts;
        bool signup_dialog_created;

        Di_Client_LoginDataRequest login_data_request_dialog;
        Di_ClientLoginRequest login_request_dialog;
        Di_Client_ClientSignupRequest signup_dialog;

        public void UnAuthModeStart()
        {
            ResetFields();
            LoginFormError_Label.Text = "";
            UnAuthModeConstructDialogs();   

            Unauth_mode_level_counts = 2;
            current_level = 1;
            login_data_request_retry_counts = 0;

            signup_dialog_created = false;
            signup_dialog = null;

            LoginFormWaitAndUnWait(true);
            UnAutheMode_Panel.Visible = true;
        }

        public void UnAuthModeEnd()
        {
            LoginFormWaitAndUnWait(true);
            current_level = 0;

            UnAutheMode_Panel.Visible = false;

            if (login_data_request_dialog.Get_status != DialogStatus.End)
            {
                if (login_data_request_dialog.Get_current_level > 0)
                {
                    login_data_request_dialog.ManagerRemoveRequestPath();
                }
            }
            if (login_request_dialog.Get_status != DialogStatus.End)
            {
                login_request_dialog.ManagerRemoveRequestPath();
            }

            if (signup_dialog_created == true)
            {
                signup_dialog.Close();
            }
        }

        public void UnAuthModeConstructDialogs()
        {
            login_data_request_dialog = new Di_Client_LoginDataRequest(HelperFunctions.GetGUID(), new Remove(RemoveDialog), new SendDialogMessage(Mo_SendDialogMessage)
           , new MessageShow(Mo_MessageShow), new ModeLevelRejected(LevelReject), new GetLoginData(Mo_GetLoginData));

            login_request_dialog = new Di_ClientLoginRequest(HelperFunctions.GetGUID(), new Remove(RemoveDialog), new SendDialogMessage(Mo_SendDialogMessage)
            , new MessageShow(Mo_MessageShow), new ModeLevelRejected(LevelReject), new ModeLevelReset(LevelReset), new ModeLevelAccepted(LevelAccept));

        }

        public void LevelReject(string p_comment)
        {
            if (current_level == 1)
            {
                LoginFormWaitAndUnWait(true);
                MessageBox.Show(p_comment);
                ChangeMode(TypeOfMode.Dissconnected);
                return;
            }
            else if (current_level == 2)
            {
                login_data_request_retry_counts++;
                ResetFields();
                if (login_data_request_retry_counts > 3)
                {
                    LoginFormWaitAndUnWait(true);
                    MessageBox.Show("Server didnt respond");
                    ChangeMode(TypeOfMode.Dissconnected);
                    return;
                }

                current_level = 1;
                UnAuthModeConstructDialogs();

                LoginFormError_Label.Text = p_comment;
                LoginFormWaitAndUnWait(true);
                LevelAccept();
            }
        }
        public void LevelReset(string p_comment)
        {
            if (current_level == 1)
            {
                ResetFields();
                LoginFormWaitAndUnWait(true);
                LoginFormError_Label.Text = p_comment;
            }
        }
        public void LevelAccept()
        {
            if (current_level == 1)
            {
                current_level++;
                login_data_request_dialog.StartDialog();
            }
            else if (current_level == 2)
            {
                login_data_request_retry_counts = 0;
                ChangeMode(TypeOfMode.AuthMode);
                return;
            }
        }

        void Mo_GetLoginData(string p_user_name, List<PersonStatus> p_frineds_list_and_status, List<int> p_public_chat_ids
                           , List<AgreementInvitationInfo> p_agreement_invitations, List<OfflineMessage> p_offline_messages)
        {
            login_data = new LoginData(p_user_name, p_frineds_list_and_status, p_public_chat_ids, p_agreement_invitations, p_offline_messages);
            ChangeMode(TypeOfMode.AuthMode);
            return;
        }

        public void UnAuthModeReceive(DialogMessageForClient p_message)
        {
            if (p_message.Get_dialog_type == TypeOfDialog.LoginRequest && p_message.Get_dialog_id == login_request_dialog.Get_dialog_id
                && current_level == 1)
            {
                login_request_dialog.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.LoginDataRequest && p_message.Get_dialog_id == login_data_request_dialog.Get_dialog_id
                && current_level == 2)
            {
                login_data_request_dialog.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.ClientSignupRequest && p_message.Get_dialog_id == signup_dialog.Get_dialog_id)
            {
                if (signup_dialog_created == true)
                {
                    signup_dialog.ReceiveMessage(p_message);
                }
            }

        }

        private void ResetFields()
        {
            LoginUserName_TextBox.Text = string.Empty;
            LoginPassword_TextBox.Text = string.Empty;
        }

        public void RemoveDialog(int p_dialog_id)
        {
        }

        public void RemoveSignupDialog(int p_dialog_id)
        {
            signup_dialog_created = false;
        }

        private void Login_Button_Click(object sender, EventArgs e)
        {
            string user_name = LoginUserName_TextBox.Text;
            string password = LoginPassword_TextBox.Text;
            if (string.IsNullOrWhiteSpace(user_name) || string.IsNullOrWhiteSpace(password))
            {
                LoginFormError_Label.Text = "Username and/or password field is empty.";
                return;
            }
            LoginFormWaitAndUnWait(false);
            login_request_dialog.StartLogin(user_name, password);
        }

        private void LoginFormWaitAndUnWait(bool p_flag)
        {
            LoginUserName_TextBox.Enabled = p_flag;
            LoginPassword_TextBox.Enabled = p_flag;
            Login_Button.Enabled = p_flag;
            LoginSignup_LinkLabel.Enabled = p_flag;
            if (signup_dialog_created == true)
            {
                if (p_flag == false)
                {
                    signup_dialog.Hide();
                }
                else if (p_flag == true)
                {
                    signup_dialog.Show();
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (signup_dialog_created == false)
            {
                signup_dialog_created = true;
                signup_dialog = new Di_Client_ClientSignupRequest(HelperFunctions.GetGUID(), new Remove(RemoveSignupDialog), new SendDialogMessage(Mo_SendDialogMessage));
                signup_dialog.StartDialog();
            }
        }

    }

    public abstract class BaseMode
    {
        protected TypeOfMode p_mode_type;
        protected Panel mode_panel;

        public void BaseConstruct(TypeOfMode p_mode_type, Panel p_mode_panel)
        {
        }

        public abstract void Receive(FinalMessageForClient p_message);

    }

    public class AuthMode
    {
        AllDiaogs all_dialogs;
        AllChats all_chats;
        Panel modde_panel;


        MessageShow show_message;

        public AuthMode()
        {

        }

        public void MessageShow(string p_message)
        {
        }

    }



}

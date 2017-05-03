using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientTcpChat.Forms;
using ClientTcpChat.Classes;
using CommonChatTypes;

namespace ClientTcpChat.Classes
{
    public delegate void ALevelOfDialog();
    public delegate void Remove(int p_dialog_id);
    public delegate void SendDialogMessage(DialogMessageForServer p_message);
    public delegate void GetLoginData(string p_user_name, List<PersonStatus> p_frineds_list_and_status, List<int> p_public_chat_ids
                , List<AgreementInvitationInfo> p_agreement_invitations, List<OfflineMessage> p_offline_messages);

    public delegate void ChangeMode(ClientApplicationMode p_application_mode);
    public delegate void MessageShow(string p_message_text);
    public delegate void ModeLevelReset(string p_reset_comment);
    public delegate void ModeLevelAccepted();
    public delegate void ModeLevelRejected(string p_rejecting_comment);

    public delegate void SomeoneLeftChat(string p_user_left_chat_name, int p_chat_id, TypeOfChat p_chat_type);
    public delegate void SomeoneJoinedChat(string p_user_joined_chat_name, int p_chat_id, TypeOfChat p_chat_type);
    public delegate void InformEjectFromChat(int p_chat_id, string p_ejecting_comment, TypeOfChat p_chat_type);
    public delegate void InformCreatedPrivateChat(string p_user_name, int p_chat_id);
    public delegate void CreateEmptyPrivateChat(int p_chat_id);
    public delegate void InformJoinToPublicChat(int p_chat_id, List<string> p_public_chat_persons);

    public delegate void InformFriendChangedStatus(PersonStatus p_user_and_new_status);
    public delegate void ShowFormalMessage(FormalMessage p_formal_message, int p_message_id);
    public delegate void InformFriendListChanhged(List<PersonStatus> p_new_friends_and_status);
    public delegate void OfflienMesssageRead(List<int> p_messages_ids);
    public delegate void StartClientJoinPublicChatRequestDialog(int p_chat_id);

    public delegate void ShowAnAgreementInvitation(AgreementInvitationInfo p_agreement_invitation_info);

    public class DialogLevelsInformation
    {
        ALevelOfDialog level_function;
        DialogLevelType level_type;
        bool level_executed;
        public DialogLevelsInformation(ALevelOfDialog p_level_function, DialogLevelType p_level_type)
        {
            level_function = p_level_function;
            level_type = p_level_type;
            level_executed = false;
        }
        public ALevelOfDialog Get_level_function
        {
            get
            {
                return level_function;
            }
        }
        public DialogLevelType Get_level_type
        {
            get
            {
                return level_type;
            }
        }
        public bool Set_Get_level_executed
        {
            set
            {
                level_executed = value;
            }
            get
            {
                return level_executed;
            }
        }
    }

    public class ExceptionOccurenceStruct
    {
        bool exception_occured;
        public bool Get_exception_occured
        {
            get { return exception_occured; }
        }

        List<string> exception_message;
        public List<string> Get_exception_message
        {
            get { return exception_message; }
        }

        public void RegisterAnException(string p_exception_message)
        {
            exception_occured = true;
            exception_message.Add(p_exception_message);
        }

        public ExceptionOccurenceStruct()
        {
            exception_occured = false;
            exception_message = new List<string>();
        }
    }


    public abstract class BaseDialog
    {
        protected int dialog_id;
        protected int last_message_number;
        protected int current_level;
        protected int level_counts;
        protected DialogMessageForClient last_message_received;
        protected TypeOfDialog dialog_type;
        protected Dictionary<int, DialogLevelsInformation> all_dialog_levels;
        protected int last_message_sent_id;
        protected Remove remove_dialog_from_manager;
        protected List<DialogMessageForClient> cache;
        protected DialogStatus status;
        protected SendDialogMessage send_delegate;
        protected MessageShow message_show;

        protected ExceptionOccurenceStruct exception_occurance;
        protected int retry_counts;
        protected int max_retry_count;

        protected bool manager_removed_dialog;

        public ExceptionOccurenceStruct Get_exception_occurance
        {
            get { return exception_occurance; }
        }
        public int Get_retry_counts
        {
            get { return retry_counts; }
        }
        public int Get_last_message_number
        {
            get { return last_message_number; }
        }
        public int Get_current_level
        {
            get { return current_level; }
        }
        public int Get_level_counts
        {
            get { return level_counts; }
        }
        public DialogMessageForClient Get_last_message_received
        {
            get { return last_message_received; }
        }
        public TypeOfDialog Get_dialog_type
        {
            get { return dialog_type; }
        }
        public Dictionary<int, DialogLevelsInformation> Get_all_dialog_levels
        {
            get { return all_dialog_levels; }
        }
        public int Get_last_message_sent_id
        {
            get { return last_message_sent_id; }
        }
        public List<DialogMessageForClient> Get_cache
        {
            get { return cache; }
        }
        public DialogStatus Get_status
        {
            get { return status; }
        }
        public int Get_dialog_id
        {
            get { return dialog_id; }
        }
        public int Get_max_retry_count
        {
            get { return max_retry_count; }
        }


        protected void BaseConstruct(int p_dialog_id, TypeOfDialog p_dialog_type, Remove p_remove_dialog_from_manager, int p_max_retry_counts
            , SendDialogMessage p_send, MessageShow p_message_show)
        {
            dialog_type = p_dialog_type;
            dialog_id = p_dialog_id;
            remove_dialog_from_manager = p_remove_dialog_from_manager;
            send_delegate = p_send;
            message_show = p_message_show;
            current_level = 0;
            last_message_number = 0;
            last_message_received = null;
            all_dialog_levels = new Dictionary<int, DialogLevelsInformation>();
            cache = new List<DialogMessageForClient>();
            status = DialogStatus.Running;
            last_message_sent_id = 0;
            exception_occurance = new ExceptionOccurenceStruct();
            retry_counts = 0;
            max_retry_count = p_max_retry_counts;
            manager_removed_dialog = false;
        }

        protected void Start() 
        {
            LevelAccept();
        }

        public void ReceiveMessage(DialogMessageForClient p_received_message)
        {
            ProcessMessage(p_received_message);
        }   

        public void ManagerRemoveRequestPath() 
        {
            manager_removed_dialog = true;
            CancelDialog("manager removed dialog");
            return;
        }

        protected void ProcessMessage(DialogMessageForClient p_message)
        {
            if (!HelperFunctions.DialogMessageObjectInvestigate(p_message))
            {
                exception_occurance.RegisterAnException("Received message object is  not valid");
                return;
            }
            if (p_message.Get_message_number_in_dialog == last_message_number + 1)
            {
                if (p_message.Get_message_object_type == TypeOfDialogMessage.CancelDialog)
                {
                    if (!(current_level == level_counts && all_dialog_levels[current_level].Set_Get_level_executed))
                    {
                        End(((Di_Mess_CancelDialogMessage)p_message.Get_message_object).Get_cancel_comment);
                    }
                    return;
                }
                if (status == DialogStatus.WaitingForAMessage || status == DialogStatus.WaitingForAReceipt)
                {

                    last_message_number = p_message.Get_message_number_in_dialog;
                    if (status == DialogStatus.WaitingForAMessage && p_message.Get_message_object_type != TypeOfDialogMessage.ReceiptMessage &&
                        Get_all_dialog_levels[current_level].Get_level_type == DialogLevelType.WaitingForMessageReceive)
                    {
                        status = DialogStatus.MessageInvestigation;
                        last_message_received = p_message;
                        all_dialog_levels[current_level].Get_level_function();
                    }
                    else if (status == DialogStatus.WaitingForAReceipt && p_message.Get_message_object_type == TypeOfDialogMessage.ReceiptMessage &&
                        all_dialog_levels[current_level].Get_level_type == DialogLevelType.SendingAMessage)
                    {
                        Di_Mess_ReceiptMessage temp_receipt = (Di_Mess_ReceiptMessage)p_message.Get_message_object;
                        ReceiptInvestigate(temp_receipt);
                    }
                }
            }

            else if (p_message.Get_message_number_in_dialog > last_message_number + 1 && p_message.Get_dialog_id == this.dialog_id)
            {
                cache.Add(p_message);
                return;
            }
        }

        protected void LevelAccept()
        {
            if (current_level == 0)
            {
                if (all_dialog_levels.Count != level_counts)
                {
                    exception_occurance.RegisterAnException("get_all_dialog_levels.Count != level_counts");
                    return;
                }
            }
            if (status != DialogStatus.ReceipptRejected && status != DialogStatus.MessageRejected)
            {
                retry_counts = 0;
            }

            if (status == DialogStatus.MessageInvestigation)
            {
                status = DialogStatus.MessageAccepted;
                AccMessage(last_message_received);
            }
            else if (status == DialogStatus.ReceiptAccepted)
            {

            }


            if (current_level != 0)
            {
                all_dialog_levels[current_level].Set_Get_level_executed = true;
                LevelAccActs();
            }

            if (current_level == level_counts)
            {
                all_dialog_levels[current_level].Set_Get_level_executed = true;
                End();
            }
            else if (all_dialog_levels[current_level + 1].Get_level_type == DialogLevelType.SendingAMessage)
            {
                status = DialogStatus.Running;
                current_level++;
                all_dialog_levels[current_level].Get_level_function();
            }
            else if (all_dialog_levels[current_level + 1].Get_level_type == DialogLevelType.WaitingForMessageReceive)
            {
                current_level++;
                status = DialogStatus.WaitingForAMessage;
                TryCache();
            }

            if (all_dialog_levels[current_level].Get_level_type == DialogLevelType.WaitingForMessageReceive) 
                CheckCache();
        }
        protected void LevelReject(string p_reject_comment)
        {
            LevelRejActs(p_reject_comment);
            if (retry_counts > max_retry_count)
            {
                Send(TypeOfDialogMessage.CancelDialog, new Di_Mess_CancelDialogMessage("Retry counts became more than " + max_retry_count.ToString() + " times. server reason:" + p_reject_comment));
                End("no more retry is available");
            }

            if (status == DialogStatus.MessageInvestigation)
            {
                status = DialogStatus.MessageRejected;
                retry_counts++;
                RejMessage(last_message_received, p_reject_comment);
                RollBack();
                return;
            }
            else if (status == DialogStatus.ReceipptRejected)
            {
                status = DialogStatus.ReceipptRejected;
                retry_counts++;
                RollBack();
                return;
            }

            return;


        }

        protected virtual void LevelAccActs()
        {
        }
        protected virtual void LevelRejActs(string p_reject_comment)
        {
        }
        protected virtual void WaitingStateActs()
        {
        }

        protected virtual void RollBack()
        {
            if (current_level > 1)
            {
                all_dialog_levels[current_level - 1].Set_Get_level_executed = false;
            }
            current_level--;
            LevelAccept();
        }

        protected virtual void End()
        {
            if (current_level == level_counts && all_dialog_levels[current_level].Set_Get_level_executed)
                Result();
            retry_counts = 0;
            status = DialogStatus.End;
            RemoveThisDialog();
        }

        protected virtual void End(string p_reject_comment)
        {
            DialogRejectingActs(p_reject_comment);
            retry_counts = 0;
            status = DialogStatus.End;
            RemoveThisDialog();
        }

        protected void ReceiptInvestigate(Di_Mess_ReceiptMessage p_receipt_to_be_investigated)
        {
            status = DialogStatus.ReceiptInvestigation;
            if (p_receipt_to_be_investigated.Get_message_rec_status == ReceiptStatus.Accepted)
            {
                if (((Di_Mess_Rec_AcceptMessage)p_receipt_to_be_investigated.Get_rec_message).Get_accpepted_message_id == last_message_sent_id)
                {
                    status = DialogStatus.ReceiptAccepted;
                    LevelAccept();
                    return;
                }
                else
                {
                    exception_occurance.RegisterAnException("Receipt tells message has been accepted but id is not valid");
                    return;
                }
            }

            else if (p_receipt_to_be_investigated.Get_message_rec_status == ReceiptStatus.Rejected)
            {
                if (((Di_Mess_Rec_RejectMessage)p_receipt_to_be_investigated.Get_rec_message).Get_rejected_message_id == last_message_sent_id)
                {
                    status = DialogStatus.ReceipptRejected;
                    LevelReject(((Di_Mess_Rec_RejectMessage)p_receipt_to_be_investigated.Get_rec_message).Get_comment_for_rejecting);
                }
                else
                {
                    exception_occurance.RegisterAnException("Receipt tells message has been rejected but id is not valid");
                    return;
                }
            }
        }

        protected virtual void DialogRejectingActs(string p_reject_comment)
        {
        }
        protected virtual void Result()
        {
        }

        protected void CheckCache()
        {
            for (int i = 0; i < cache.Count; i++)
            {
                if (cache[i].Get_message_number_in_dialog <= last_message_number)
                {
                    cache.RemoveAt(i);  
                }
            }
        }
        protected virtual void TryCache()
        {
            int level_before_try = current_level;
            int last_message_number_before_try = last_message_number;
            DialogStatus status_before_try = status;
            for (int i = 0; i < cache.Count; i++)
            {
                if (level_before_try == current_level && last_message_number_before_try == last_message_number && status_before_try == status)
                {
                    DialogMessageForClient temp_message = cache[i];
                    cache.RemoveAt(i);
                    ProcessMessage(temp_message);
                }
                else
                {
                    return;
                }
            }

        }

        protected virtual void CancelDialog(string p_cancel_comment)
        {
            if (!(current_level == 1 && all_dialog_levels[1].Get_level_type == DialogLevelType.SendingAMessage && retry_counts == 0))
            {
                Send(TypeOfDialogMessage.CancelDialog, new Di_Mess_CancelDialogMessage(p_cancel_comment));
            }
            End(p_cancel_comment);
            return;
        }

        protected void Send(TypeOfDialogMessage p_message_object_type, object p_message_object)
        {

            DialogMessageForServer dialog_message_to_be_sent = new DialogMessageForServer(HelperFunctions.GetGUID(), dialog_id, last_message_number + 1
                , dialog_type, p_message_object, p_message_object_type);
            if (!HelperFunctions.DialogMessageObjectInvestigate(dialog_message_to_be_sent))
            {
                exception_occurance.RegisterAnException("send message object is not valid");
                return;
            }

            send_delegate(dialog_message_to_be_sent);
            if (all_dialog_levels[current_level].Get_level_type == DialogLevelType.SendingAMessage && p_message_object_type != TypeOfDialogMessage.CancelDialog)
            {
                status = DialogStatus.WaitingForAReceipt;
                WaitingStateActs();
            }

            if (p_message_object_type == TypeOfDialogMessage.CancelDialog)
            {
                status = DialogStatus.Canceling;
            }
            last_message_number = dialog_message_to_be_sent.Get_message_number_in_dialog;
            last_message_sent_id = dialog_message_to_be_sent.Get_message_id;

        }
        protected void AccMessage(DialogMessageForClient p_message_to_accept)
        {
            status = DialogStatus.MessageAccepted;
            Di_Mess_Rec_AcceptMessage temp_accept_message = new Di_Mess_Rec_AcceptMessage(p_message_to_accept.Get_message_id);
            Di_Mess_ReceiptMessage receipt_object = new Di_Mess_ReceiptMessage(ReceiptStatus.Accepted, temp_accept_message);
            DialogMessageForServer accept_message = new DialogMessageForServer(HelperFunctions.GetGUID(), Get_dialog_id, Get_last_message_number + 1, Get_dialog_type, receipt_object, TypeOfDialogMessage.ReceiptMessage);
            last_message_number = accept_message.Get_message_number_in_dialog;
            send_delegate(accept_message);
        }
        protected void RejMessage(DialogMessageForClient p_message_to_reject, string p_rejecting_comment)
        {
            status = DialogStatus.ReceipptRejected;
            Di_Mess_Rec_RejectMessage temp_reject_message = new Di_Mess_Rec_RejectMessage(p_message_to_reject.Get_message_id, p_rejecting_comment);
            Di_Mess_ReceiptMessage receipt_object = new Di_Mess_ReceiptMessage(ReceiptStatus.Rejected, temp_reject_message);
            DialogMessageForServer reject_message = new DialogMessageForServer(HelperFunctions.GetGUID(), Get_dialog_id, Get_last_message_number + 1, Get_dialog_type, receipt_object, TypeOfDialogMessage.ReceiptMessage);
            last_message_number = reject_message.Get_message_number_in_dialog;
            send_delegate(reject_message);
        }

        public override abstract bool Equals(object obj);   

        protected void RemoveThisDialog()
        {
            remove_dialog_from_manager(dialog_id);
        }
    }

    public enum FormedDialogLevelType
    {
        WaitingForMessageReceive,
        SendingAMessage,
        UserSendingMessage,
    };
    public class FormedDialogLevelsInformation
    {
        ALevelOfDialog level_function;
        FormedDialogLevelType level_type;
        bool level_executed;
        public FormedDialogLevelsInformation(ALevelOfDialog p_level_function, FormedDialogLevelType p_level_type)
        {
            level_function = p_level_function;
            level_type = p_level_type;
            level_executed = false;
        }
        public ALevelOfDialog Get_level_function
        {
            get
            {
                return level_function;
            }
        }
        public FormedDialogLevelType Get_level_type
        {
            get
            {
                return level_type;
            }
        }
        public bool Set_Get_level_executed
        {
            set
            {
                level_executed = value;
            }
            get
            {
                return level_executed;
            }
        }
    }

    public class BaseFormedDialog : Form
    {

        protected int dialog_id;
        protected int last_message_number;
        protected int current_level;
        protected int level_counts;
        protected DialogMessageForClient last_message_received;
        protected TypeOfDialog dialog_type;
        protected Dictionary<int, FormedDialogLevelsInformation> all_dialog_levels;
        protected int last_message_sent_id;
        protected Remove remove_dialog_from_manager;
        protected List<DialogMessageForClient> cache;
        protected DialogStatus status;
        protected SendDialogMessage send_delegate;
        protected ExceptionOccurenceStruct exception_occurance;
        protected int retry_counts;
        protected int max_retry_count;

        protected bool flag;

        public ExceptionOccurenceStruct Get_exception_occurance
        {
            get { return exception_occurance; }
        }
        public int Get_retry_counts
        {
            get { return retry_counts; }
        }
        public int Get_last_message_number
        {
            get { return last_message_number; }
        }
        public int Get_current_level
        {
            get { return current_level; }
        }
        public int Get_level_counts
        {
            get { return level_counts; }
        }
        public DialogMessageForClient Get_last_message_received
        {
            get { return last_message_received; }
        }
        public TypeOfDialog Get_dialog_type
        {
            get { return dialog_type; }
        }
        public Dictionary<int, FormedDialogLevelsInformation> Get_all_dialog_levels
        {
            get { return all_dialog_levels; }
        }
        public int Get_last_message_sent_id
        {
            get { return last_message_sent_id; }
        }
        public List<DialogMessageForClient> Get_cache
        {
            get { return cache; }
        }
        public DialogStatus Get_status
        {
            get { return status; }
        }
        public int Get_dialog_id
        {
            get { return dialog_id; }
        }
        public int Get_max_retry_count
        {
            get { return max_retry_count; }
        }


        protected void BaseConstruct(int p_dialog_id, TypeOfDialog p_dialog_type, Remove p_remove_dialog_from_manager, int p_max_retry_counts
            , SendDialogMessage p_send)
        {
            dialog_type = p_dialog_type;
            dialog_id = p_dialog_id;
            remove_dialog_from_manager = p_remove_dialog_from_manager;
            send_delegate = p_send;
            current_level = 0;
            last_message_number = 0;
            last_message_received = null;
            all_dialog_levels = new Dictionary<int, FormedDialogLevelsInformation>();
            cache = new List<DialogMessageForClient>();
            status = DialogStatus.Running;
            last_message_sent_id = 0;
            exception_occurance = new ExceptionOccurenceStruct();
            retry_counts = 0;
            max_retry_count = p_max_retry_counts;

            result_message_string = string.Empty;
            reject_message_string = string.Empty;

            all_form_levels_panels = new Dictionary<int, Panel>();
            all_form_level_message_labels = new Dictionary<int, Label>();

            flag = false;
            this.Visible = false;
            this.Hide();

        }

        protected void Start()  
        {
            LevelAccept();
        }

        public void StartDialog() 
        {
            flag = false;
            this.Visible = true;
            this.Show();
        }
        public void DontStartDialog()
        {
            flag = true;
            this.Close();
        }

        public void ReceiveMessage(DialogMessageForClient p_received_message)
        {
            ProcessMessage(p_received_message);
        }   

        public void ManagerRemoveRequestPath()  
        {
            CancelDialog("manager removed dialog");
            return;
        }

        protected void ProcessMessage(DialogMessageForClient p_message)
        {
            if (!HelperFunctions.DialogMessageObjectInvestigate(p_message))
            {
                exception_occurance.RegisterAnException("Received message object is  not valid");
                return;
            }
            if (p_message.Get_message_number_in_dialog == last_message_number + 1)
            {
                if (p_message.Get_message_object_type == TypeOfDialogMessage.CancelDialog)
                {
                    if (!(current_level == level_counts && all_dialog_levels[current_level].Set_Get_level_executed))
                    {
                        End(((Di_Mess_CancelDialogMessage)p_message.Get_message_object).Get_cancel_comment);
                    }
                    return;
                }
                if (status == DialogStatus.WaitingForAMessage || status == DialogStatus.WaitingForAReceipt)
                {

                    last_message_number = p_message.Get_message_number_in_dialog;
                    if (status == DialogStatus.WaitingForAMessage && p_message.Get_message_object_type != TypeOfDialogMessage.ReceiptMessage &&
                        Get_all_dialog_levels[current_level].Get_level_type == FormedDialogLevelType.WaitingForMessageReceive)
                    {
                        status = DialogStatus.MessageInvestigation;
                        last_message_received = p_message;
                        UnwaitForm();
                        all_dialog_levels[current_level].Get_level_function();
                    }
                    else if (status == DialogStatus.WaitingForAReceipt && p_message.Get_message_object_type == TypeOfDialogMessage.ReceiptMessage &&
                        (all_dialog_levels[current_level].Get_level_type == FormedDialogLevelType.SendingAMessage
                        || all_dialog_levels[current_level].Get_level_type == FormedDialogLevelType.UserSendingMessage))
                    {
                        Di_Mess_ReceiptMessage temp_receipt = (Di_Mess_ReceiptMessage)p_message.Get_message_object;
                        ReceiptInvestigate(temp_receipt);
                    }
                }
            }

            else if (p_message.Get_message_number_in_dialog > last_message_number + 1 && p_message.Get_dialog_id == this.dialog_id)
            {
                cache.Add(p_message);
                return;
            }
        }

        protected void LevelAccept()
        {
            UnwaitForm();
            if (current_level == 0)
            {
                if (all_dialog_levels.Count != level_counts)
                {
                    exception_occurance.RegisterAnException("get_all_dialog_levels.Count != level_counts");
                    return;
                }
            }
            if (status != DialogStatus.ReceipptRejected && status != DialogStatus.MessageRejected && status != DialogStatus.UserRequestRejected)
            {
                retry_counts = 0;
            }

            if (status == DialogStatus.MessageInvestigation)
            {
                status = DialogStatus.MessageAccepted;
                AccMessage(last_message_received);
            }
            else if (status == DialogStatus.ReceiptAccepted)
            {

            }

            if (current_level != 0)
            {
                all_dialog_levels[current_level].Set_Get_level_executed = true;
                LevelAccActs();
            }

            if (current_level == level_counts)
            {
                all_dialog_levels[current_level].Set_Get_level_executed = true;
                End();
            }
            else
            {

                if (all_dialog_levels[current_level + 1].Get_level_type == FormedDialogLevelType.SendingAMessage)
                {
                    status = DialogStatus.Running;
                    current_level++;
                    FormNextLevel();
                    all_dialog_levels[current_level].Get_level_function();
                }
                else if (all_dialog_levels[current_level + 1].Get_level_type == FormedDialogLevelType.UserSendingMessage)
                {
                    status = DialogStatus.Running;
                    current_level++;
                    FormNextLevel();
                }
                else if (all_dialog_levels[current_level + 1].Get_level_type == FormedDialogLevelType.WaitingForMessageReceive)
                {
                    current_level++;
                    status = DialogStatus.WaitingForAMessage;
                    FormNextLevel();
                    FormWaitState();
                    if (cache.Count > 0)
                        TryCache();
                }
                if (all_dialog_levels[current_level].Get_level_type == FormedDialogLevelType.WaitingForMessageReceive)
                {
                    if (cache.Count > 0)
                        CheckCache();
                }
            }
        }
        protected void LevelReject(string p_reject_comment)
        {
            LevelRejActs(p_reject_comment);
            if (retry_counts > max_retry_count)
            {
                Send(TypeOfDialogMessage.CancelDialog, new Di_Mess_CancelDialogMessage("Retry counts became more than " + max_retry_count.ToString() + " times. server reason:" + p_reject_comment));
                End("no more retry is available");
                return;
            }

            if (status == DialogStatus.MessageInvestigation)
            {
                status = DialogStatus.MessageRejected;
                retry_counts++;
                RejMessage(last_message_received, p_reject_comment);
                all_form_level_message_labels[current_level].Text = "message received from a server didnt accepted: " + p_reject_comment + ". retry.";
                RollBack();
                return;
            }
            else if (status == DialogStatus.ReceipptRejected)
            {
                status = DialogStatus.ReceipptRejected;
                retry_counts++;
                all_form_level_message_labels[current_level].Text = "server rejected message: " + p_reject_comment;
                RollBack();
                return;
            }

            if (all_dialog_levels[current_level].Get_level_type == FormedDialogLevelType.UserSendingMessage && status == DialogStatus.Running)
            {
                status = DialogStatus.UserRequestRejected;
                all_form_level_message_labels[current_level].Text = "Invalid Operation: " + p_reject_comment;
                RollBack();
                return;
            }

            return;
        }

        protected virtual void LevelAccActs()
        {
        }
        protected virtual void LevelRejActs(string p_reject_comment)
        {
        }
        protected virtual void WaitingStateActs()
        {
        }

        protected virtual void RollBack()
        {
            if (current_level > 1)     
            {
                all_dialog_levels[current_level - 1].Set_Get_level_executed = false;
            }
            current_level--;
            LevelAccept();
        }

        protected virtual void End()
        {
            if (current_level == level_counts && all_dialog_levels[current_level].Set_Get_level_executed)
                Result();
            retry_counts = 0;
            flag = true;
            status = DialogStatus.End;
            MessageBox.Show(result_message_string);
            this.Close();
            RemoveThisDialog();
        }

        protected virtual void End(string p_reject_comment)
        {
            DialogRejectingActs(p_reject_comment);
            retry_counts = 0;
            status = DialogStatus.End;
            flag = true;
            MessageBox.Show(reject_message_string + ": " + p_reject_comment);
            this.Close();   
            RemoveThisDialog();
        }

        protected void UserEndRequest()
        {
            retry_counts = 0;
            status = DialogStatus.End;
            RemoveThisDialog();

            if (!flag)
            {
                flag = true;
                this.Close();
            }
        }

        protected void ReceiptInvestigate(Di_Mess_ReceiptMessage p_receipt_to_be_investigated)
        {
            status = DialogStatus.ReceiptInvestigation;
            if (p_receipt_to_be_investigated.Get_message_rec_status == ReceiptStatus.Accepted)
            {
                if (((Di_Mess_Rec_AcceptMessage)p_receipt_to_be_investigated.Get_rec_message).Get_accpepted_message_id == last_message_sent_id)
                {
                    status = DialogStatus.ReceiptAccepted;
                    LevelAccept();
                    return;
                }
                else
                {
                    exception_occurance.RegisterAnException("Receipt tells message has been accepted but id is not valid");
                    return;
                }
            }

            else if (p_receipt_to_be_investigated.Get_message_rec_status == ReceiptStatus.Rejected)
            {
                if (((Di_Mess_Rec_RejectMessage)p_receipt_to_be_investigated.Get_rec_message).Get_rejected_message_id == last_message_sent_id)
                {
                    status = DialogStatus.ReceipptRejected;
                    LevelReject(((Di_Mess_Rec_RejectMessage)p_receipt_to_be_investigated.Get_rec_message).Get_comment_for_rejecting);
                    return;
                }
                else
                {
                    exception_occurance.RegisterAnException("Receipt tells message has been rejected but id is not valid");
                    return;
                }
            }
        }

        protected virtual void DialogRejectingActs(string p_reject_comment)
        {
        }
        protected virtual void Result()
        {
        }

        protected void CheckCache()
        {
            for (int i = 0; i < cache.Count; i++)
            {
                if (cache[i].Get_message_number_in_dialog <= last_message_number)
                {
                    cache.RemoveAt(i); 
                }
            }
        }
        protected virtual void TryCache()
        {
            int level_before_try = current_level;
            int last_message_number_before_try = last_message_number;
            DialogStatus status_before_try = status;
            for (int i = 0; i < cache.Count; i++)
            {
                if (level_before_try == current_level && last_message_number_before_try == last_message_number && status_before_try == status)
                {
                    DialogMessageForClient temp_message = cache[i];
                    cache.RemoveAt(i);
                    ProcessMessage(temp_message);
                }
                else
                {
                    return;
                }
            }

        }

        protected virtual void CancelDialog(string p_cancel_comment) 
        {
            if (!(current_level == 1 && (all_dialog_levels[1].Get_level_type == FormedDialogLevelType.SendingAMessage
                || all_dialog_levels[1].Get_level_type == FormedDialogLevelType.UserSendingMessage) && retry_counts == 0))
            {
                Send(TypeOfDialogMessage.CancelDialog, new Di_Mess_CancelDialogMessage(p_cancel_comment));
            }
            UserEndRequest();
            return;
        }

        protected void Send(TypeOfDialogMessage p_message_object_type, object p_message_object)
        {

            DialogMessageForServer dialog_message_to_be_sent = new DialogMessageForServer(HelperFunctions.GetGUID(), dialog_id, last_message_number + 1, dialog_type, p_message_object, p_message_object_type);
            if (!HelperFunctions.DialogMessageObjectInvestigate(dialog_message_to_be_sent))
            {
                exception_occurance.RegisterAnException("send message object is not valid");
                return;
            }

            send_delegate(dialog_message_to_be_sent);
            if ((all_dialog_levels[current_level].Get_level_type == FormedDialogLevelType.SendingAMessage
                || all_dialog_levels[current_level].Get_level_type == FormedDialogLevelType.UserSendingMessage)
                && p_message_object_type != TypeOfDialogMessage.CancelDialog)
            {
                status = DialogStatus.WaitingForAReceipt;
                FormWaitState();
                WaitingStateActs();
            }

            if (p_message_object_type == TypeOfDialogMessage.CancelDialog)
            {
                status = DialogStatus.Canceling;
            }
            last_message_number = dialog_message_to_be_sent.Get_message_number_in_dialog;
            last_message_sent_id = dialog_message_to_be_sent.Get_message_id;

        }
        protected void AccMessage(DialogMessageForClient p_message_to_accept)
        {
            status = DialogStatus.MessageAccepted;
            Di_Mess_Rec_AcceptMessage temp_accept_message = new Di_Mess_Rec_AcceptMessage(p_message_to_accept.Get_message_id);
            Di_Mess_ReceiptMessage receipt_object = new Di_Mess_ReceiptMessage(ReceiptStatus.Accepted, temp_accept_message);
            DialogMessageForServer accept_message = new DialogMessageForServer(HelperFunctions.GetGUID(), Get_dialog_id, Get_last_message_number + 1, Get_dialog_type, receipt_object, TypeOfDialogMessage.ReceiptMessage);
            last_message_number = accept_message.Get_message_number_in_dialog;
            send_delegate(accept_message);
        }
        protected void RejMessage(DialogMessageForClient p_message_to_reject, string p_rejecting_comment)
        {
            status = DialogStatus.ReceipptRejected;
            Di_Mess_Rec_RejectMessage temp_reject_message = new Di_Mess_Rec_RejectMessage(p_message_to_reject.Get_message_id, p_rejecting_comment);
            Di_Mess_ReceiptMessage receipt_object = new Di_Mess_ReceiptMessage(ReceiptStatus.Rejected, temp_reject_message);
            DialogMessageForServer reject_message = new DialogMessageForServer(HelperFunctions.GetGUID(), Get_dialog_id, Get_last_message_number + 1, Get_dialog_type, receipt_object, TypeOfDialogMessage.ReceiptMessage);
            last_message_number = reject_message.Get_message_number_in_dialog;
            send_delegate(reject_message);
        }

        protected Dictionary<int, Panel> all_form_levels_panels;
        protected Dictionary<int, Label> all_form_level_message_labels;
        protected string result_message_string;
        protected string reject_message_string;
        protected virtual void FormWaitState()
        {
            EnableAndDisableControls(false);
        }
        protected virtual void UnwaitForm()
        {
            EnableAndDisableControls(true);
        }
        protected virtual void FormNextLevel() 
        {
            UnwaitForm();
            if (current_level <= level_counts)
            {
                if (retry_counts == 0)
                {
                    all_form_level_message_labels[current_level].Text = "";
                }
                ResetFields();
                for (int i = 1; i < all_form_levels_panels.Count + 1; i++)
                {
                    all_form_levels_panels[i].Visible = false;
                }
                all_form_levels_panels[current_level].Visible = true;
            }
        }
        protected virtual void ResetFields()
        {
        }

        protected void UserNextLevelRequest() 
        {
            CreateUserStrings();
            if (all_dialog_levels[current_level].Get_level_type == FormedDialogLevelType.UserSendingMessage)
            {
                all_dialog_levels[current_level].Get_level_function();
            }
        }

        protected virtual void EnableAndDisableControls(bool p_flag)
        {
        }
        protected virtual void CreateUserStrings()
        {
        }

        protected void RemoveThisDialog()
        {
            remove_dialog_from_manager(dialog_id);
        }
    }


    public class Di_Client_LoginDataRequest : BaseDialog
    {
        ModeLevelRejected level_rejected;
        GetLoginData get_login_data;

        string user_name;
        List<PersonStatus> all_friends_and_status;
        List<OfflineMessage> offline_messages;
        List<int> public_chat_ids;
        List<AgreementInvitationInfo> agreement_invitations;

        public Di_Client_LoginDataRequest(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show,
            ModeLevelRejected p_level_rejected, GetLoginData p_get_login_data)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.LoginDataRequest, p_remove_dialog_from_manager, 3, p_send, p_message_show);
            level_rejected = p_level_rejected;
            get_login_data = p_get_login_data;

            user_name = string.Empty;
            all_friends_and_status = new List<PersonStatus>();
            offline_messages = new List<OfflineMessage>();
            public_chat_ids = new List<int>();
            agreement_invitations = new List<AgreementInvitationInfo>();

            level_counts = 6;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.SendingAMessage));
            all_dialog_levels.Add(2, new DialogLevelsInformation(new ALevelOfDialog(SecondLevelFunction), DialogLevelType.WaitingForMessageReceive));
            all_dialog_levels.Add(3, new DialogLevelsInformation(new ALevelOfDialog(ThirdLevelFunction), DialogLevelType.WaitingForMessageReceive));
            all_dialog_levels.Add(4, new DialogLevelsInformation(new ALevelOfDialog(FourthLevelFunction), DialogLevelType.WaitingForMessageReceive));
            all_dialog_levels.Add(5, new DialogLevelsInformation(new ALevelOfDialog(FifthLevelFunction), DialogLevelType.WaitingForMessageReceive));
            all_dialog_levels.Add(6, new DialogLevelsInformation(new ALevelOfDialog(SixthLevelFunction), DialogLevelType.WaitingForMessageReceive));

        }

        public void StartDialog()
        {
            Start();
        }


        private void FirstLevelFunction()
        {
            Di_Mess_LoginDataRequestMessage login_data_request_message_object = new Di_Mess_LoginDataRequestMessage();
            Send(TypeOfDialogMessage.LoginDataRequestMessage, login_data_request_message_object);
            return;
        }

        private void SecondLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.AccountInformation)
            {
                LevelReject("message objects not correct");
                return;
            }

            user_name = ((Di_Mess_AccountInformation)last_message_received.Get_message_object).Get_user_name;
            LevelAccept();
            return;
        }

        private void ThirdLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.FriendsListAndStatus)
            {
                level_rejected("message object is not valid");
                return;
            }
            all_friends_and_status = ((Di_Mess_FriendsListAndStatus)last_message_received.Get_message_object).Get_all_friends_and_status;
            LevelAccept();
            return;
        }

        private void FourthLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.PublicChatIds)
            {
                level_rejected("message object is not valid");
                return;
            }
            public_chat_ids = ((Di_Mess_PublicChatIds)last_message_received.Get_message_object).Get_public_chat_ids;
            LevelAccept();
            return;
        }

        private void FifthLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.InviteToAgreementInfo)
            {
                level_rejected("message object is not valid");
                return;
            }
            agreement_invitations = ((Di_Mess_InviteToAgreemenstInfo)last_message_received.Get_message_object).Get_all_user_agreement_invitations;
            LevelAccept();
            return;
        }

        private void SixthLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.OfflineMessages)
            {
                level_rejected("message object is not valid");
                return;
            }
            offline_messages = ((Di_Mess_OfflineMessages)last_message_received.Get_message_object).Get_all_offline_messages;
            LevelAccept();
            return;
        }


        protected override void DialogRejectingActs(string p_reject_comment)
        {
            level_rejected(p_reject_comment);
            return;
        }
        protected override void Result()
        {
            get_login_data(user_name, all_friends_and_status, public_chat_ids, agreement_invitations, offline_messages);
            return;
        }


        public override bool Equals(object obj)
        {
            Di_Client_LoginDataRequest dialog = null;
            try
            {
                dialog = (Di_Client_LoginDataRequest)obj;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

    public class Di_ClientLoginRequest : BaseDialog
    {
        ModeLevelAccepted level_accpeted;
        ModeLevelReset level_reset;
        ModeLevelRejected level_rejected;

        string user_name;
        string password;
        bool flag;

        public Di_ClientLoginRequest(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show,
            ModeLevelRejected p_level_rejected, ModeLevelReset p_level_reset, ModeLevelAccepted p_level_accepted)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.LoginRequest, p_remove_dialog_from_manager, 20, p_send, p_message_show);

            level_accpeted = p_level_accepted;
            level_rejected = p_level_rejected;
            level_reset = p_level_reset;

            user_name = "";
            password = "";

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(LevelOneFunction), DialogLevelType.SendingAMessage));
            flag = false; ;
            Start();
        }

        private void LevelOneFunction()
        {
            if (flag == true)
            {
                Di_Mess_LoginRequestData login_data_message_object = new Di_Mess_LoginRequestData(user_name, password);
                Send(TypeOfDialogMessage.LoginRequestData, login_data_message_object);
                return;
            }
        }

        public void StartLogin(string p_user_name, string p_password)
        {
            flag = true;

            user_name = p_user_name;
            password = p_password;

            all_dialog_levels[1].Get_level_function();
        }

        protected override void DialogRejectingActs(string p_reject_comment)
        {
            flag = false;
            level_rejected(p_reject_comment);
            return;
        }
        protected override void Result()
        {
            level_accpeted();
            return;
        }
        protected override void LevelRejActs(string p_reject_comment)
        {
            flag = false;
            level_reset(p_reject_comment);
            return;
        }

        public override bool Equals(object obj)
        {
            Di_ClientLoginRequest dialog = null;
            try
            {
                dialog = (Di_ClientLoginRequest)obj;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

    public class Di_Client_GetAgreementAnswer : BaseDialog
    {
        int agreement_id;
        bool agreement_answer;
        TypeOfAgreement agreement_type;

        public Di_Client_GetAgreementAnswer(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , int p_agreement_id, bool p_agreement_answer, TypeOfAgreement p_agreement_type)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.GetAgreementAnswer, p_remove_dialog_from_manager, 3, p_send, p_message_show);

            agreement_id = p_agreement_id;
            agreement_answer = p_agreement_answer;
            agreement_type = p_agreement_type;

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.SendingAMessage));
        }

        public void StartDialog()
        {
            Start();
        }

        private void FirstLevelFunction()
        {
            Di_Mess_AgreementAnswer agreement_answer_message_object = new Di_Mess_AgreementAnswer(agreement_id, agreement_answer, agreement_type);
            Send(TypeOfDialogMessage.AgreementAnswer, agreement_answer_message_object);
            return;
        }

        protected override void DialogRejectingActs(string p_reject_comment)
        {
            if (!manager_removed_dialog)
            {
                message_show("your request to answer agreement: " + agreement_id.ToString() + "  didnt accept: " + p_reject_comment);
                return;
            }
        }
        protected override void Result()
        {
            message_show("your request to answer agreement: " + agreement_id.ToString() + " accepted");
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_GetAgreementAnswer dialog = null;
            try
            {
                dialog = (Di_Client_GetAgreementAnswer)obj;
            }
            catch (Exception)
            {
                return false;
            }
            if (dialog.agreement_id == agreement_id)
            {
                return true;
            }
            return false;
        }
    }

    public class Di_Client_ServerOfflineMessageReadInform : BaseDialog
    {
        List<int> offline_messages_ids;
        public Di_Client_ServerOfflineMessageReadInform(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , List<int> p_offline_messages_ids)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.ServerReadOfflineMessagesInform, p_remove_dialog_from_manager, 3, p_send, p_message_show);

            offline_messages_ids = p_offline_messages_ids;

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.SendingAMessage));
        }


        public void StartDialog()
        {
            Start();
        }

        private void FirstLevelFunction()
        {
            Di_Mess_ServerReadOfflineMessagesInform server_read_offline_messages_inform_message_object = new Di_Mess_ServerReadOfflineMessagesInform(offline_messages_ids);
            Send(TypeOfDialogMessage.ServerReadOfflineMessagesInform, server_read_offline_messages_inform_message_object);
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_ServerOfflineMessageReadInform temp_dialog = null;

            try
            {
                temp_dialog = (Di_Client_ServerOfflineMessageReadInform)obj;
            }
            catch (Exception)
            {
                return false;
            }

            if (temp_dialog.offline_messages_ids.Count == offline_messages_ids.Count)
            {
                foreach (int message_id in temp_dialog.offline_messages_ids)
                {
                    if (!offline_messages_ids.Contains(message_id))
                        return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }

    public class Di_Client_ClientLeaveChatRequest : BaseDialog
    {

        int chat_id;
        TypeOfChat chat_type;

        public Di_Client_ClientLeaveChatRequest(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , int p_chat_id, TypeOfChat p_chat_type)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.ClientLeaveChatRequest, p_remove_dialog_from_manager, 3, p_send, p_message_show);
            chat_id = p_chat_id;
            chat_type = p_chat_type;

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.SendingAMessage));
        }

        public void StartDialog()
        {
            Start();
        }

        private void FirstLevelFunction()
        {
            Di_Mess_ClientLeaveChatRequest leave_chat_request_message_object = new Di_Mess_ClientLeaveChatRequest(chat_id, chat_type);
            Send(TypeOfDialogMessage.ClientLeaveChatRequest, leave_chat_request_message_object);
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_ClientLeaveChatRequest temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_ClientLeaveChatRequest)obj;
            }
            catch (Exception)
            {
                return false;
            }
            if (temp_dialog.chat_id == chat_id && temp_dialog.chat_type == chat_type)
            {
                return true;
            }
            return false;
        }
    }

    public class Di_Client_ClientSomeoneLeftChatInform : BaseDialog
    {
        SomeoneLeftChat inform_someone_left_chat;
        string person_left_chat_user_name;
        int chat_id;
        TypeOfChat chat_type;

        public Di_Client_ClientSomeoneLeftChatInform(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , SomeoneLeftChat p_inform_someone_left_chat)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.ClientSomeoneLeftChatInform, p_remove_dialog_from_manager, 3, p_send, p_message_show);
            inform_someone_left_chat = p_inform_someone_left_chat;

            person_left_chat_user_name = "";
            chat_id = 0;

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.WaitingForMessageReceive));
            Start();
        }

        private void FirstLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.SomeoneLeftTheChat)
            {
                LevelReject("message object is not valid");
                return;
            }
            Di_Mess_SomeoneLeftTheChat someone_left_chat_message_object = ((Di_Mess_SomeoneLeftTheChat)last_message_received.Get_message_object);
            person_left_chat_user_name = someone_left_chat_message_object.Get_user_name;
            chat_id = someone_left_chat_message_object.Get_chat_id;
            chat_type = someone_left_chat_message_object.Get_chat_type;
            inform_someone_left_chat(person_left_chat_user_name, chat_id, chat_type);
            LevelAccept();
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_ClientSomeoneLeftChatInform temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_ClientSomeoneLeftChatInform)obj;
            }
            catch (Exception)
            {
                return false;
            }
            if (temp_dialog.person_left_chat_user_name == person_left_chat_user_name && temp_dialog.chat_id == chat_id)
                return true;
            return false;

        }
    }

    public class Di_Client_InformEjectedChatUser : BaseDialog
    {
        InformEjectFromChat inform_eject_from_chat;
        string ejecting_comment;
        int closed_chat_id;
        TypeOfChat chat_type;

        public Di_Client_InformEjectedChatUser(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , InformEjectFromChat p_inform_eject_from_chat)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.InformEjectedChatUser, p_remove_dialog_from_manager, 3, p_send, p_message_show);
            inform_eject_from_chat = p_inform_eject_from_chat;

            ejecting_comment = "";
            closed_chat_id = 0;

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.WaitingForMessageReceive));
            Start();
        }

        private void FirstLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.InformEjectedChatUser)
            {
                LevelReject("message object is not correct");
                return;
            }

            closed_chat_id = ((Di_Mess_InformEjectedChatUser)last_message_received.Get_message_object).Get_id_of_closed_chat;
            ejecting_comment = ((Di_Mess_InformEjectedChatUser)last_message_received.Get_message_object).Get_ejecting_coment;
            chat_type = ((Di_Mess_InformEjectedChatUser)last_message_received.Get_message_object).Get_chat_type;

            inform_eject_from_chat(closed_chat_id, ejecting_comment, chat_type);
            LevelAccept();
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_InformEjectedChatUser temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_InformEjectedChatUser)obj;
            }
            catch (Exception)
            {
                return false;
            }

            if (temp_dialog.closed_chat_id == closed_chat_id && chat_type == temp_dialog.chat_type)
            {
                return true;
            }

            return false;
        }
    }

    public class Di_Client_CreatePrivateChatRequest : BaseDialog
    {
        string invited_person_user_name;
        CreateEmptyPrivateChat create_empty_private_chat;

        public Di_Client_CreatePrivateChatRequest(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , string p_invited_person_user_name, CreateEmptyPrivateChat p_create_empty_private_chat)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.CreatePrivateChatRequest, p_remove_dialog_from_manager, 3, p_send, p_message_show);

            invited_person_user_name = p_invited_person_user_name;
            create_empty_private_chat = p_create_empty_private_chat;

            level_counts = 2;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLeveFunction), DialogLevelType.SendingAMessage));
            all_dialog_levels.Add(2, new DialogLevelsInformation(new ALevelOfDialog(SecondLevelFunction), DialogLevelType.WaitingForMessageReceive));

        }


        public void StartDialog()
        {
            Start();
        }


        private void FirstLeveFunction()
        {
            Di_Mess_StartPrivateChatRequest start_private_chat_request_message_object = new Di_Mess_StartPrivateChatRequest(invited_person_user_name);
            Send(TypeOfDialogMessage.StartPrivateChatRequest, start_private_chat_request_message_object);
            return;
        }

        private void SecondLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.PrivateChatInfo)
            {
                LevelReject("message object is not correct");
                return;
            }

            Di_Mess_PrivateChatInfo private_chat_info = ((Di_Mess_PrivateChatInfo)last_message_received.Get_message_object);
            create_empty_private_chat(private_chat_info.Get_chat_id);
            LevelAccept();
            return;
        }


        protected override void DialogRejectingActs(string p_reject_comment)
        {
            if (!manager_removed_dialog)
            {
                message_show("your request to start private chat with '" + invited_person_user_name + "' rejected: " + p_reject_comment);
            }
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_CreatePrivateChatRequest temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_CreatePrivateChatRequest)obj;
            }
            catch (Exception)
            {
                return false;
            }
            if (temp_dialog.invited_person_user_name == invited_person_user_name)
            {
                return true;
            }
            return false;
        }
    }

    public class Di_Client_ClientJoinPublicChatRequest : BaseDialog
    {
        int chat_id;
        InformJoinToPublicChat inform_join_to_public_chat;

        public Di_Client_ClientJoinPublicChatRequest(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , int p_chat_id, InformJoinToPublicChat p_inform_join_to_public_chat)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.ClientJoinPublicChatRequest, p_remove_dialog_from_manager, 3, p_send, p_message_show);

            chat_id = p_chat_id;
            inform_join_to_public_chat = p_inform_join_to_public_chat;

            level_counts = 2;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.SendingAMessage));
            all_dialog_levels.Add(2, new DialogLevelsInformation(new ALevelOfDialog(SecondLevelFunction), DialogLevelType.WaitingForMessageReceive));
        }


        public void StartDialog()
        {
            Start();
        }

        private void FirstLevelFunction()
        {
            Di_Mess_JoinPublicChatRequest join_public_chat_request_message_object = new Di_Mess_JoinPublicChatRequest(chat_id);
            Send(TypeOfDialogMessage.JoinPublicChatRequest, join_public_chat_request_message_object);
            return;
        }

        private void SecondLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.PublicChatUsersIds)
            {
                LevelReject("message object is not correct.");
                return;
            }

            List<string> public_chat_persons = ((Di_Mess_PublicChatUsersIds)last_message_received.Get_message_object).Get_users_ids;
            inform_join_to_public_chat(chat_id, public_chat_persons);
            LevelAccept();
            return;
        }

        protected override void DialogRejectingActs(string p_reject_comment)
        {
            if (!manager_removed_dialog)
            {
                message_show("your request to join to public chat with id '" + chat_id + "' rejected: " + p_reject_comment);
            }
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_ClientJoinPublicChatRequest temp_dialog = null;

            try
            {
                temp_dialog = (Di_Client_ClientJoinPublicChatRequest)obj;
            }
            catch (Exception)
            {
                return false;
            }

            if (temp_dialog.chat_id == chat_id)
            {
                return true;
            }

            return false;
        }
    }

    public class Di_Client_ClientSomeoneJoinedChatInform : BaseDialog   
    {
        SomeoneJoinedChat someone_joined_chat;

        string joined_chat_person_user_name;
        int chat_id;
        TypeOfChat chat_type;

        public Di_Client_ClientSomeoneJoinedChatInform(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , SomeoneJoinedChat p_someone_joined_chat)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.ClientSomeoneJoinedChatInform, p_remove_dialog_from_manager, 3, p_send, p_message_show);

            someone_joined_chat = p_someone_joined_chat;

            joined_chat_person_user_name = "";
            chat_id = 0;

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.WaitingForMessageReceive));
            Start();
        }

        private void FirstLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.SomeoneJoinedTheChat)
            {
                LevelReject("message object is not correct");
                return;
            }

            joined_chat_person_user_name = ((Di_Mess_SomeoneJoinedTheChat)last_message_received.Get_message_object).Get_user_name;
            chat_id = ((Di_Mess_SomeoneJoinedTheChat)last_message_received.Get_message_object).Get_chat_id;
            chat_type = ((Di_Mess_SomeoneJoinedTheChat)last_message_received.Get_message_object).Get_chat_type;
            someone_joined_chat(joined_chat_person_user_name, chat_id, chat_type);

            LevelAccept();
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_ClientSomeoneJoinedChatInform temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_ClientSomeoneJoinedChatInform)obj;
            }
            catch (Exception)
            {
                return false;
            }

            if (temp_dialog.chat_id == chat_id && temp_dialog.joined_chat_person_user_name == joined_chat_person_user_name && chat_type == temp_dialog.chat_type)
            {
                return true;
            }

            return false;
        }

    }

    public class Di_Client_ClientFriendChangedStatusInform : BaseDialog
    {
        PersonStatus friend_changed_status;
        InformFriendChangedStatus inform_friend_changed_status;

        public Di_Client_ClientFriendChangedStatusInform(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , InformFriendChangedStatus p_inform_friend_changed_status)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.ClientFriendChangedStatusInform, p_remove_dialog_from_manager, 3, p_send, p_message_show);

            friend_changed_status = null;
            inform_friend_changed_status = p_inform_friend_changed_status;

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.WaitingForMessageReceive));
            Start();
        }

        private void FirstLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.FriendChangeStatus)
            {
                LevelReject("message object is not correct");
                return;
            }

            friend_changed_status = ((Di_Mess_FriendChangeStatus)last_message_received.Get_message_object).Get_person_status_changed;
            inform_friend_changed_status(friend_changed_status);

            LevelAccept();
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_ClientFriendChangedStatusInform temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_ClientFriendChangedStatusInform)obj;
            }
            catch (Exception)
            {
                return false;
            }

            if (temp_dialog.friend_changed_status == friend_changed_status)
            {
                return true;
            }
            return false;
        }
    }

    public class Di_Client_SendToClientFormalMessage : BaseDialog
    {
        ShowFormalMessage show_formal_message;
        FormalMessage user_formal_message;
        int message_id;

        public Di_Client_SendToClientFormalMessage(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , ShowFormalMessage p_show_formal_message)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.SendToClinetFormalMessage, p_remove_dialog_from_manager, 3, p_send, p_message_show);

            show_formal_message = p_show_formal_message;
            user_formal_message = new FormalMessage("", "");
            message_id = 0;

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.WaitingForMessageReceive));
            Start();
        }

        private void FirstLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.ClientInformFormalMessage)
            {
                LevelReject("message object is not correct");
                return;
            }

            Di_Mess_ClientInformFormalMessage received_formal_message = (Di_Mess_ClientInformFormalMessage)last_message_received.Get_message_object;
            user_formal_message = received_formal_message.Get_message;
            show_formal_message(received_formal_message.Get_message, received_formal_message.Get_message_id);

            LevelAccept();
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_SendToClientFormalMessage temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_SendToClientFormalMessage)obj;
            }
            catch (Exception)
            {
                return false;
            }

            if (temp_dialog.user_formal_message == user_formal_message && temp_dialog.message_id == message_id)
            {
                return true;
            }
            return false;
        }
    }

    public class Di_Client_ClientInvitedAgreementInform : BaseDialog
    {
        ShowAnAgreementInvitation show_an_agreement_invitation;
        AgreementInvitationInfo agreement_invitation_info;


        public Di_Client_ClientInvitedAgreementInform(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , ShowAnAgreementInvitation p_show_an_agreement_invitation)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.ClientInvitedAgreementInform, p_remove_dialog_from_manager, 3, p_send, p_message_show);

            show_an_agreement_invitation = p_show_an_agreement_invitation;
            agreement_invitation_info = new AgreementInvitationInfo("", "", 0, TypeOfAgreement.Add);

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.WaitingForMessageReceive));

            Start();
        }

        private void FirstLevelFunction()
        {

            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.InformInviteToAgreementInfo)
            {
                LevelReject("message object is not valid.");
                return;
            }

            Di_Mess_InformInviteToAgreementInfo inform_invite_to_agreement = (Di_Mess_InformInviteToAgreementInfo)last_message_received.Get_message_object;
            agreement_invitation_info = inform_invite_to_agreement.Get_user_agreement_invitation_info;
            show_an_agreement_invitation(agreement_invitation_info);
            LevelAccept();
        }

        public override bool Equals(object obj)
        {
            Di_Client_ClientInvitedAgreementInform temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_ClientInvitedAgreementInform)obj;
            }
            catch
            {
                return false;
            }

            if (agreement_invitation_info.Get_agreement_id == temp_dialog.agreement_invitation_info.Get_agreement_id)
            {
                return true;
            }
            return false;
        }
    }

    public class Di_Client_ClientFriendListChangedInform : BaseDialog
    {
        List<PersonStatus> new_friends_and_status;
        InformFriendListChanhged inform_friendlist_changed;

        public Di_Client_ClientFriendListChangedInform(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , InformFriendListChanhged p_inform_friendlist_changed)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.ClientFriendListChangedInform, p_remove_dialog_from_manager, 3, p_send, p_message_show);
            inform_friendlist_changed = p_inform_friendlist_changed;

            level_counts = 1;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.WaitingForMessageReceive));
            Start();
        }

        private void FirstLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.FriendsListAndStatus)
            {
                LevelReject("message object is not correct.");
                return;
            }

            new_friends_and_status = ((Di_Mess_FriendsListAndStatus)last_message_received.Get_message_object).Get_all_friends_and_status;
            inform_friendlist_changed(new_friends_and_status);

            LevelAccept();
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_ClientFriendListChangedInform temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_ClientFriendListChangedInform)obj;
            }
            catch (Exception)
            {
                return false;
            }

            if (new_friends_and_status.Count == temp_dialog.new_friends_and_status.Count)
            {
                foreach (PersonStatus t_person_status in temp_dialog.new_friends_and_status)
                {
                    if (!new_friends_and_status.Contains(t_person_status))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    public class Di_Client_ClientCreatedPrivateChatInform : BaseDialog
    {
        string user_name;
        int chat_id;

        InformCreatedPrivateChat inform_created_private_chat;

        public Di_Client_ClientCreatedPrivateChatInform(int p_dialog_id, Remove p_remove_dialog_from_manager, SendDialogMessage p_send, MessageShow p_message_show
            , InformCreatedPrivateChat p_inform_created_private_chat)
        {
            base.BaseConstruct(p_dialog_id, TypeOfDialog.ClientCreatedPrivateChatInform, p_remove_dialog_from_manager, 3, p_send, p_message_show);

            inform_created_private_chat = p_inform_created_private_chat;

            user_name = "";
            chat_id = 0;

            level_counts = 2;
            all_dialog_levels.Add(1, new DialogLevelsInformation(new ALevelOfDialog(FirstLevelFunction), DialogLevelType.WaitingForMessageReceive));
            all_dialog_levels.Add(2, new DialogLevelsInformation(new ALevelOfDialog(SecondLevelFunction), DialogLevelType.SendingAMessage));
            Start();
        }

        private void FirstLevelFunction()
        {
            if (last_message_received.Get_message_object_type != TypeOfDialogMessage.CreatedPrivateChatInform)
            {
                LevelReject("message object is not correct.");
                return;
            }

            user_name = ((Di_Mess_CreatedPrivateChatInform)last_message_received.Get_message_object).Get_starter_user_name;
            chat_id = ((Di_Mess_CreatedPrivateChatInform)last_message_received.Get_message_object).Get_chat_id;

            LevelAccept();
            return;
        }

        private void SecondLevelFunction()
        {
            Send(TypeOfDialogMessage.PrivateChatInvitationAnswer, new Di_Mess_PrivateChatInvitationAnswer(true));
            inform_created_private_chat(user_name, chat_id);
            return;
        }

        public override bool Equals(object obj)
        {
            Di_Client_ClientCreatedPrivateChatInform temp_dialog = null;
            try
            {
                temp_dialog = (Di_Client_ClientCreatedPrivateChatInform)obj;
            }
            catch (Exception)
            {
                return false;
            }

            if (temp_dialog.user_name == user_name)
            {
                return true;
            }

            return false;
        }
    }


    public abstract class BaseDialogManager
    {
        protected Dictionary<int, BaseDialog> all_dialogs;
        protected TypeOfDialog dialog_type;

        protected SendDialogMessage send_messages;
        protected MessageShow message_show;

        public void BaseConstruct(TypeOfDialog p_dialog_type, SendDialogMessage p_send_messages, MessageShow p_message_show)
        {
            all_dialogs = new Dictionary<int, BaseDialog>();

            dialog_type = p_dialog_type;
            send_messages = p_send_messages;
            message_show = p_message_show;
        }

        public void ReceiveMessage(DialogMessageForClient p_message)
        {
            if (HelperFunctions.DialogMessageObjectInvestigate(p_message))
            {
                InitialProcessMessage(p_message);
            }

        }

        protected void InitialProcessMessage(DialogMessageForClient p_message)
        {
            if (p_message.Get_dialog_type == dialog_type)
            {
                SecondaryProcessMessage(p_message);
            }
        }

        protected abstract void SecondaryProcessMessage(DialogMessageForClient p_message);

        public void Clear()   
        {
            List<int> all_dialog_ids = all_dialogs.Keys.ToList();
            foreach (int p_dialog_id in all_dialog_ids)
            {
                if (all_dialogs.ContainsKey(p_dialog_id))
                {
                    RemoveADilaog(p_dialog_id);
                }
            }
        }

        public void DialogRemoveItselfRequest(int p_dialog_id)
        {
            all_dialogs.Remove(p_dialog_id);
        }

        public void RemoveADilaog(int p_dialog_id)
        {
            if (all_dialogs.ContainsKey(p_dialog_id))
            {
                all_dialogs[p_dialog_id].ManagerRemoveRequestPath();
            }
        }

        public void Ma_Send(DialogMessageForServer p_message)
        {
            send_messages(p_message);
        }
        public void Ma_MessageShow(string p_message_text)
        {
            message_show(p_message_text);
        }
    }

    public abstract class Cs_DialogManager : BaseDialogManager
    {
        protected override void SecondaryProcessMessage(DialogMessageForClient p_message)
        {
            if (all_dialogs.ContainsKey(p_message.Get_dialog_id))
            {
                all_dialogs[p_message.Get_dialog_id].ReceiveMessage(p_message);
            }
        }
    }

    public abstract class Ss_DialogManager : BaseDialogManager
    {
        protected abstract BaseDialog CreateDialog(DialogMessageForClient p_dialog_starting_message);

        protected override void SecondaryProcessMessage(DialogMessageForClient p_message)
        {
            if (p_message.Get_message_number_in_dialog == 1)
            {
                BaseDialog new_dialog = CreateDialog(p_message);   
                if (!all_dialogs.ContainsValue(new_dialog))
                {
                    all_dialogs.Add(new_dialog.Get_dialog_id, new_dialog);
                }
            }
            if (all_dialogs.ContainsKey(p_message.Get_dialog_id))
            {
                all_dialogs[p_message.Get_dialog_id].ReceiveMessage(p_message);
            }
        }
    }


    public class Ma_GetAgreementAnswerDialogManager : Cs_DialogManager
    {
        public Ma_GetAgreementAnswerDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show)
        {
            base.BaseConstruct(TypeOfDialog.GetAgreementAnswer, p_send_messages, p_message_show);
        }

        public void Create(int p_agreement_id, TypeOfAgreement p_agreement_type, bool p_answer)
        {
            int random_dialog_id = HelperFunctions.GetGUID();

            Di_Client_GetAgreementAnswer temp_dialog = (new Di_Client_GetAgreementAnswer(random_dialog_id, new Remove(DialogRemoveItselfRequest), new SendDialogMessage(Ma_Send)
            , new MessageShow(Ma_MessageShow), p_agreement_id, p_answer, p_agreement_type));

            if (!all_dialogs.ContainsValue(temp_dialog))
            {
                all_dialogs.Add(random_dialog_id, temp_dialog);
                temp_dialog.StartDialog();
            }

        }
    }   

    public class Ma_ServerOfflineMessageReadInformDialogManager : Cs_DialogManager
    {
        public Ma_ServerOfflineMessageReadInformDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show)
        {
            base.BaseConstruct(TypeOfDialog.ServerReadOfflineMessagesInform, p_send_messages, p_message_show);
        }

        public void Create(List<int> p_offline_messages_id)
        {
            int random_dialog_id = HelperFunctions.GetGUID();
            Di_Client_ServerOfflineMessageReadInform temp_dialog = new Di_Client_ServerOfflineMessageReadInform(random_dialog_id, new Remove(DialogRemoveItselfRequest)
            , new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), p_offline_messages_id);
            if (!all_dialogs.ContainsValue(temp_dialog))
            {
                all_dialogs.Add(random_dialog_id, temp_dialog);
                temp_dialog.StartDialog();
            }
        }
    }

    public class Ma_ClientLeaveChatRequestDialogManager : Cs_DialogManager
    {
        public Ma_ClientLeaveChatRequestDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show)
        {
            base.BaseConstruct(TypeOfDialog.ClientLeaveChatRequest, p_send_messages, p_message_show);
        }

        public void Create(int p_chat_id, TypeOfChat p_chat_type)
        {
            int random_dialog_id = HelperFunctions.GetGUID();
            Di_Client_ClientLeaveChatRequest temp_dialog = new Di_Client_ClientLeaveChatRequest(random_dialog_id, new Remove(DialogRemoveItselfRequest)
            , new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), p_chat_id, p_chat_type);
            if (!all_dialogs.ContainsValue(temp_dialog))
            {
                all_dialogs.Add(random_dialog_id, temp_dialog);
                temp_dialog.StartDialog();
            }
        }
    }

    public class Ma_ClientSomeoneLeftChatInformDialogManager : Ss_DialogManager
    {
        SomeoneLeftChat inform_someone_left_chat;
        public Ma_ClientSomeoneLeftChatInformDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show, SomeoneLeftChat p_inform_someone_left_chat)
        {
            base.BaseConstruct(TypeOfDialog.ClientSomeoneLeftChatInform, p_send_messages, p_message_show);

            inform_someone_left_chat = p_inform_someone_left_chat;
        }

        protected override BaseDialog CreateDialog(DialogMessageForClient p_dialog_starting_message)
        {
            Di_Client_ClientSomeoneLeftChatInform temp_dialog = new Di_Client_ClientSomeoneLeftChatInform(p_dialog_starting_message.Get_dialog_id
                , new Remove(DialogRemoveItselfRequest), new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), new SomeoneLeftChat(Ma_SomeoneLeftChat));
            return temp_dialog;
        }

        public void Ma_SomeoneLeftChat(string p_user_left_chat_name, int p_chat_id, TypeOfChat p_chat_type)
        {
            inform_someone_left_chat(p_user_left_chat_name, p_chat_id, p_chat_type);
        }

    }

    public class Ma_InformEjectedChatUserDialogManager : Ss_DialogManager
    {
        InformEjectFromChat inform_eject_from_chat;

        public Ma_InformEjectedChatUserDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show, InformEjectFromChat p_inform_eject_from_chat)
        {
            base.BaseConstruct(TypeOfDialog.InformEjectedChatUser, p_send_messages, p_message_show);

            inform_eject_from_chat = p_inform_eject_from_chat;
        }

        protected override BaseDialog CreateDialog(DialogMessageForClient p_dialog_starting_message)
        {
            Di_Client_InformEjectedChatUser temp_dialog = new Di_Client_InformEjectedChatUser(p_dialog_starting_message.Get_dialog_id, new Remove(DialogRemoveItselfRequest)
            , new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), new InformEjectFromChat(Ma_InformEjectFromChat));
            return temp_dialog;
        }

        public void Ma_InformEjectFromChat(int p_chat_id, string p_ejecting_comment, TypeOfChat p_chat_type)
        {
            inform_eject_from_chat(p_chat_id, p_ejecting_comment, p_chat_type);
        }
    }

    public class Ma_CreatePrivateChatRequestDialogManager : Cs_DialogManager
    {
        CreateEmptyPrivateChat create_empty_private_chat;

        public Ma_CreatePrivateChatRequestDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show, CreateEmptyPrivateChat p_create_empty_private_chat)
        {
            base.BaseConstruct(TypeOfDialog.CreatePrivateChatRequest, p_send_messages, p_message_show);
            create_empty_private_chat = p_create_empty_private_chat;
        }

        public void Create(string p_invited_person_user_name)
        {
            int random_dialog_id = HelperFunctions.GetGUID();
            Di_Client_CreatePrivateChatRequest temp_dialog = new Di_Client_CreatePrivateChatRequest(random_dialog_id, new Remove(DialogRemoveItselfRequest)
            , new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), p_invited_person_user_name, new CreateEmptyPrivateChat(Ma_CreateEmptyPrivateChat));
            if (!all_dialogs.ContainsValue(temp_dialog))
            {
                all_dialogs.Add(random_dialog_id, temp_dialog);
                temp_dialog.StartDialog();
            }
        }

        public void Ma_CreateEmptyPrivateChat(int p_chat_id)
        {
            create_empty_private_chat(p_chat_id);
        }

    }

    public class Ma_ClientJoinPublicChatRequestDialogManager : Cs_DialogManager
    {
        InformJoinToPublicChat inform_join_to_public_chat;
        public Ma_ClientJoinPublicChatRequestDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show, InformJoinToPublicChat p_inform_join_to_public_chat)
        {
            base.BaseConstruct(TypeOfDialog.ClientJoinPublicChatRequest, p_send_messages, p_message_show);
            inform_join_to_public_chat = p_inform_join_to_public_chat;
        }

        public void Create(int p_chat_id)
        {
            int random_dialog_id = HelperFunctions.GetGUID();
            Di_Client_ClientJoinPublicChatRequest temp_dialog = new Di_Client_ClientJoinPublicChatRequest(random_dialog_id, new Remove(DialogRemoveItselfRequest)
            , new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), p_chat_id, new InformJoinToPublicChat(Ma_InformJoinToPublicChat));
            if (!all_dialogs.ContainsValue(temp_dialog))
            {
                all_dialogs.Add(random_dialog_id, temp_dialog);
                temp_dialog.StartDialog();
            }
        }

        public void Ma_InformJoinToPublicChat(int p_chat_id, List<string> p_public_chat_persons)
        {
            inform_join_to_public_chat(p_chat_id, p_public_chat_persons);
        }
    }

    public class Ma_ClientSomeoneJoinedChatInformDialogManager : Ss_DialogManager
    {
        SomeoneJoinedChat someone_joined_chat;
        public Ma_ClientSomeoneJoinedChatInformDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show, SomeoneJoinedChat p_someone_joined_chat)
        {
            base.BaseConstruct(TypeOfDialog.ClientSomeoneJoinedChatInform, p_send_messages, p_message_show);
            someone_joined_chat = p_someone_joined_chat;
        }

        protected override BaseDialog CreateDialog(DialogMessageForClient p_dialog_starting_message)
        {
            Di_Client_ClientSomeoneJoinedChatInform temp_dialog = new Di_Client_ClientSomeoneJoinedChatInform(p_dialog_starting_message.Get_dialog_id
                , new Remove(DialogRemoveItselfRequest), new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), new SomeoneJoinedChat(Ma_SomeoneJoinedChat));
            return temp_dialog;
        }

        public void Ma_SomeoneJoinedChat(string p_user_joined_chat_name, int p_chat_id, TypeOfChat p_chat_type)
        {
            someone_joined_chat(p_user_joined_chat_name, p_chat_id, p_chat_type);
        }
    }

    public class Ma_ClientFriendChangedStatusInformDialogManager : Ss_DialogManager
    {
        InformFriendChangedStatus inform_friend_changed_status;

        public Ma_ClientFriendChangedStatusInformDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show
            , InformFriendChangedStatus p_inform_friend_changed_status)
        {
            base.BaseConstruct(TypeOfDialog.ClientFriendChangedStatusInform, p_send_messages, p_message_show);
            inform_friend_changed_status = p_inform_friend_changed_status;
        }

        protected override BaseDialog CreateDialog(DialogMessageForClient p_dialog_starting_message)
        {
            Di_Client_ClientFriendChangedStatusInform temp_dialog = new Di_Client_ClientFriendChangedStatusInform(p_dialog_starting_message.Get_dialog_id
                , new Remove(DialogRemoveItselfRequest), new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow)
                , new InformFriendChangedStatus(Ma_InformFriendChangedStatus));
            return temp_dialog;
        }

        public void Ma_InformFriendChangedStatus(PersonStatus p_user_and_new_status)
        {
            inform_friend_changed_status(p_user_and_new_status);
        }
    }

    public class Ma_SendToClientFormalMessageDialogManager : Ss_DialogManager
    {
        ShowFormalMessage show_formal_message;

        public Ma_SendToClientFormalMessageDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show, ShowFormalMessage p_show_formal_message)
        {
            base.BaseConstruct(TypeOfDialog.SendToClinetFormalMessage, p_send_messages, p_message_show);
            show_formal_message = p_show_formal_message;
        }

        protected override BaseDialog CreateDialog(DialogMessageForClient p_dialog_starting_message)
        {
            Di_Client_SendToClientFormalMessage temp_diaog = new Di_Client_SendToClientFormalMessage(p_dialog_starting_message.Get_dialog_id
                , new Remove(DialogRemoveItselfRequest), new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), new ShowFormalMessage(Ma_ShowFormalMessage));
            return temp_diaog;
        }

        public void Ma_ShowFormalMessage(FormalMessage p_formal_message, int p_message_id)
        {
            show_formal_message(p_formal_message, p_message_id);
        }
    }

    public class Ma_ClientFriendListChangedInformDialogManager : Ss_DialogManager
    {
        InformFriendListChanhged inform_friendlist_changed;

        public Ma_ClientFriendListChangedInformDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show, InformFriendListChanhged p_inform_friendlist_changed)
        {
            base.BaseConstruct(TypeOfDialog.ClientFriendListChangedInform, p_send_messages, p_message_show);
            inform_friendlist_changed = p_inform_friendlist_changed;
        }

        protected override BaseDialog CreateDialog(DialogMessageForClient p_dialog_starting_message)
        {
            Di_Client_ClientFriendListChangedInform temp_dialog = new Di_Client_ClientFriendListChangedInform(p_dialog_starting_message.Get_dialog_id
                , new Remove(DialogRemoveItselfRequest), new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), new InformFriendListChanhged(Ma_InformFriendListChanhged));
            return temp_dialog;
        }

        public void Ma_InformFriendListChanhged(List<PersonStatus> p_new_friends_and_status)
        {
            inform_friendlist_changed(p_new_friends_and_status);
        }
    }

    public class Ma_ClientCreatedPrivateChatInformDialogManager : Ss_DialogManager
    {
        InformCreatedPrivateChat inform_created_private_chat;

        public Ma_ClientCreatedPrivateChatInformDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show
            , InformCreatedPrivateChat p_inform_created_private_chat)
        {
            base.BaseConstruct(TypeOfDialog.ClientCreatedPrivateChatInform, p_send_messages, p_message_show);
            inform_created_private_chat = p_inform_created_private_chat;
        }

        protected override BaseDialog CreateDialog(DialogMessageForClient p_dialog_starting_message)
        {
            Di_Client_ClientCreatedPrivateChatInform temp_dialog = new Di_Client_ClientCreatedPrivateChatInform(p_dialog_starting_message.Get_dialog_id
                , new Remove(DialogRemoveItselfRequest), new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), new InformCreatedPrivateChat(Ma_InformCreatedPrivateChat));
            return temp_dialog;
        }

        public void Ma_InformCreatedPrivateChat(string p_user_name, int p_chat_id)
        {
            inform_created_private_chat(p_user_name, p_chat_id);
        }
    }

    public class Ma_ClientInvitedAgreementInfoDialogManager : Ss_DialogManager
    {
        ShowAnAgreementInvitation show_an_agreement_invitation;

        public Ma_ClientInvitedAgreementInfoDialogManager(SendDialogMessage p_send_messages, MessageShow p_message_show, ShowAnAgreementInvitation p_show_an_agreement_invitation)
        {
            base.BaseConstruct(TypeOfDialog.ClientInvitedAgreementInform, p_send_messages, p_message_show);
            show_an_agreement_invitation = p_show_an_agreement_invitation;
        }

        protected override BaseDialog CreateDialog(DialogMessageForClient p_dialog_starting_message)
        {
            Di_Client_ClientInvitedAgreementInform temp_dialog = new Di_Client_ClientInvitedAgreementInform(p_dialog_starting_message.Get_dialog_id
                , new Remove(DialogRemoveItselfRequest), new SendDialogMessage(Ma_Send), new MessageShow(Ma_MessageShow), new ShowAnAgreementInvitation(Ma_ShowAnAgreementInvitation));
            return temp_dialog;
        }

        public void Ma_ShowAnAgreementInvitation(AgreementInvitationInfo p_agreement_invitation_info)
        {
            show_an_agreement_invitation(p_agreement_invitation_info);
        }
    }


    public class BaseFormedDialogManagaer
    {
        protected Dictionary<int, BaseFormedDialog> all_dialogs;
        protected TypeOfDialog dialog_type;

        protected SendDialogMessage send_messages;

        public void BaseConstruct(TypeOfDialog p_dialog_type, SendDialogMessage p_send_messages)
        {
            all_dialogs = new Dictionary<int, BaseFormedDialog>();
            dialog_type = p_dialog_type;
            send_messages = p_send_messages;
        }

        public void ReceiveMessage(DialogMessageForClient p_message)
        {
            if (HelperFunctions.DialogMessageObjectInvestigate(p_message))
            {
                InitialProcessMessage(p_message);
            }

        }

        protected void InitialProcessMessage(DialogMessageForClient p_message)
        {
            if (p_message.Get_dialog_type == dialog_type)
            {
                SecondaryProcessMessage(p_message);
            }
        }

        public void DialogRemoveItselfRequest(int p_dialog_id) 
        {
            all_dialogs.Remove(p_dialog_id);
        }

        public void RemoveADilaog(int p_dialog_id)
        {
            if (all_dialogs.ContainsKey(p_dialog_id))
            {
                all_dialogs[p_dialog_id].ManagerRemoveRequestPath();
            }
        }

        public void Clear()
        {
            List<int> all_dialog_ids = new List<int>();
            all_dialog_ids = all_dialogs.Keys.ToList();
            foreach (int p_dialog_id in all_dialog_ids)
            {
                if (all_dialogs.ContainsKey(p_dialog_id))
                {
                    RemoveADilaog(p_dialog_id);
                }
            }
        }

        public void Ma_Send(DialogMessageForServer p_message)
        {
            send_messages(p_message);
        }

        protected void SecondaryProcessMessage(DialogMessageForClient p_message)
        {
            if (all_dialogs.ContainsKey(p_message.Get_dialog_id))
            {
                all_dialogs[p_message.Get_dialog_id].ReceiveMessage(p_message);
            }
        }
    } 

    public class Ma_CreateAddAgreementDialogManager : BaseFormedDialogManagaer
    {
        public Ma_CreateAddAgreementDialogManager(SendDialogMessage p_send_messages)
        {
            base.BaseConstruct(TypeOfDialog.CreateAddAgreement, p_send_messages);
        }

        public void Create()
        {
            int random_dialog_id = HelperFunctions.GetGUID();
            Di_Client_CreateAddAgreement temp_dialog = new Di_Client_CreateAddAgreement(random_dialog_id, new Remove(DialogRemoveItselfRequest)
                , new SendDialogMessage(Ma_Send));
            if (!all_dialogs.ContainsValue(temp_dialog))
            {
                all_dialogs.Add(random_dialog_id, temp_dialog);
                temp_dialog.StartDialog();
            }
            else
            {
                temp_dialog.DontStartDialog();
                MessageBox.Show("you cannot start another add request dialog. because another one is open.");
            }
        }
    }

    public class Ma_FormalMessageRequestDialogManager : BaseFormedDialogManagaer
    {
        public Ma_FormalMessageRequestDialogManager(SendDialogMessage p_send_messages)
        {
            base.BaseConstruct(TypeOfDialog.FormalMessageRequest, p_send_messages);
        }

        public void Create()
        {
            int random_dialog_id = HelperFunctions.GetGUID();
            Di_Client_FormalMessageRequest temp_dialog = new Di_Client_FormalMessageRequest(random_dialog_id, new Remove(DialogRemoveItselfRequest)
                , new SendDialogMessage(Ma_Send));
            if (!all_dialogs.ContainsValue(temp_dialog))
            {
                all_dialogs.Add(random_dialog_id, temp_dialog);
                temp_dialog.StartDialog();
            }
            else
            {
                temp_dialog.DontStartDialog();
                MessageBox.Show("you cannot start another formal message request dialog. because another one is open.");
            }
        }
    }

    public class Ma_ClientSignupRequestDialogManager : BaseFormedDialogManagaer
    {
        public Ma_ClientSignupRequestDialogManager(SendDialogMessage p_send_messages)
        {
            base.BaseConstruct(TypeOfDialog.ClientSignupRequest, p_send_messages);
        }

        public void Create()
        {
            int random_dialog_id = HelperFunctions.GetGUID();
            Di_Client_ClientSignupRequest temp_dialog = new Di_Client_ClientSignupRequest(random_dialog_id, new Remove(DialogRemoveItselfRequest)
                , new SendDialogMessage(Ma_Send));
            if (!all_dialogs.ContainsValue(temp_dialog))
            {
                all_dialogs.Add(random_dialog_id, temp_dialog);
                temp_dialog.StartDialog();
            }
            else
            {
                temp_dialog.DontStartDialog();
                MessageBox.Show("you cannot start another signup request dialog. because another one is open.");
            }
        }
    }

    public class AllDiaogs
    {
        SendDialogMessage send_messages;
        MessageShow message_show;
        SomeoneLeftChat inform_someone_left_chat;
        InformEjectFromChat inform_eject_from_chat;
        CreateEmptyPrivateChat create_empty_private_chat;
        InformJoinToPublicChat inform_join_to_public_chat;
        SomeoneJoinedChat someone_joined_chat;
        InformFriendChangedStatus inform_friend_changed_status;
        ShowFormalMessage show_formal_message;
        InformFriendListChanhged inform_friendlist_changed;
        InformCreatedPrivateChat inform_created_private_chat;
        ShowAnAgreementInvitation show_an_agreement_invitation;

        Ma_GetAgreementAnswerDialogManager ma_get_agreement_answer_dialog_manager;
        Ma_ServerOfflineMessageReadInformDialogManager ma_server_offline_message_read_inform_dialog_manager;
        Ma_ClientLeaveChatRequestDialogManager ma_client_leave_chat_request_dialog_manager;
        Ma_ClientSomeoneLeftChatInformDialogManager ma_client_someone_left_chat_inform_dialog_manager;
        Ma_InformEjectedChatUserDialogManager ma_inform_ejected_user_dialog_manager;
        Ma_CreatePrivateChatRequestDialogManager ma_create_private_chat_request_dialog_manager;
        Ma_ClientJoinPublicChatRequestDialogManager ma_client_join_public_chat_request_dialog_manager;
        Ma_ClientSomeoneJoinedChatInformDialogManager ma_client_someone_joined_chat_inform_dialog_manager;
        Ma_ClientFriendChangedStatusInformDialogManager ma_client_friend_changed_status_inform_dialog_manager;
        Ma_SendToClientFormalMessageDialogManager ma_send_to_client_formal_message_dialog_manager;
        Ma_ClientFriendListChangedInformDialogManager ma_client_friend_list_changed_inform_dialog_manager;
        Ma_ClientCreatedPrivateChatInformDialogManager ma_client_created_private_chat_inform_dialog_manager;
        Ma_CreateAddAgreementDialogManager ma_create_add_agreement_dialog_manager;
        Ma_FormalMessageRequestDialogManager ma_formal_message_request_dialog_manager;
        Ma_ClientInvitedAgreementInfoDialogManager ma_client_invited_agreement_inform_dialog_manager;


        public AllDiaogs(Cl_ClientDelegatesForDialogs p_client_delegates_for_dialog)
        {
            send_messages = p_client_delegates_for_dialog.send_messages;
            message_show = p_client_delegates_for_dialog.message_show;
            inform_someone_left_chat = p_client_delegates_for_dialog.inform_someone_left_chat;
            inform_eject_from_chat = p_client_delegates_for_dialog.inform_eject_from_chat;
            create_empty_private_chat = p_client_delegates_for_dialog.create_empty_private_chat;
            inform_join_to_public_chat = p_client_delegates_for_dialog.inform_join_to_public_chat;
            someone_joined_chat = p_client_delegates_for_dialog.someone_joined_chat;
            inform_friend_changed_status = p_client_delegates_for_dialog.inform_friend_changed_status;
            show_formal_message = p_client_delegates_for_dialog.show_formal_message;
            inform_friendlist_changed = p_client_delegates_for_dialog.inform_friendlist_changed;
            inform_created_private_chat = p_client_delegates_for_dialog.inform_created_private_chat;
            show_an_agreement_invitation = p_client_delegates_for_dialog.show_an_agreement_invitation;


            ma_get_agreement_answer_dialog_manager = new Ma_GetAgreementAnswerDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow));

            ma_server_offline_message_read_inform_dialog_manager = new Ma_ServerOfflineMessageReadInformDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow));

            ma_client_leave_chat_request_dialog_manager = new Ma_ClientLeaveChatRequestDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow));

            ma_client_someone_left_chat_inform_dialog_manager = new Ma_ClientSomeoneLeftChatInformDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow)
            , new SomeoneLeftChat(AllMa_SomeoneLeftChat));

            ma_inform_ejected_user_dialog_manager = new Ma_InformEjectedChatUserDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow)
            , new InformEjectFromChat(AllMa_InformEjectFromChat));

            ma_create_private_chat_request_dialog_manager = new Ma_CreatePrivateChatRequestDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow)
            , new CreateEmptyPrivateChat(AllMa_CreateEmptyPrivateChat));

            ma_client_join_public_chat_request_dialog_manager = new Ma_ClientJoinPublicChatRequestDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow)
            , new InformJoinToPublicChat(AllMa_InformJoinToPublicChat));

            ma_client_someone_joined_chat_inform_dialog_manager = new Ma_ClientSomeoneJoinedChatInformDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow)
            , new SomeoneJoinedChat(AllMa_SomeoneJoinedChat));

            ma_client_friend_changed_status_inform_dialog_manager = new Ma_ClientFriendChangedStatusInformDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow)
            , new InformFriendChangedStatus(AllMa_InformFriendChangedStatus));

            ma_send_to_client_formal_message_dialog_manager = new Ma_SendToClientFormalMessageDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow)
            , new ShowFormalMessage(AllMa_ShowFormalMessage));

            ma_client_friend_list_changed_inform_dialog_manager = new Ma_ClientFriendListChangedInformDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow)
            , new InformFriendListChanhged(AllMa_InformFriendListChanhged));

            ma_client_created_private_chat_inform_dialog_manager = new Ma_ClientCreatedPrivateChatInformDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow)
            , new InformCreatedPrivateChat(AllMa_InformCreatedPrivateChat));

            ma_client_invited_agreement_inform_dialog_manager = new Ma_ClientInvitedAgreementInfoDialogManager(new SendDialogMessage(AllMa_Send), new MessageShow(AllMa_MessageShow)
            , new ShowAnAgreementInvitation(AllMa_ShowAnAgreementInvitation));

            ma_create_add_agreement_dialog_manager = new Ma_CreateAddAgreementDialogManager(new SendDialogMessage(AllMa_Send));

            ma_formal_message_request_dialog_manager = new Ma_FormalMessageRequestDialogManager(new SendDialogMessage(AllMa_Send));


        }


        public void CreateGetAgreementAnswerDialog(int p_agreement_id, TypeOfAgreement p_agreement_type, bool p_answer)
        {
            ma_get_agreement_answer_dialog_manager.Create(p_agreement_id, p_agreement_type, p_answer);
        }

        public void CreateServerOfflineMessageReadInformDialog(List<int> p_offline_messages_id)
        {
            ma_server_offline_message_read_inform_dialog_manager.Create(p_offline_messages_id);
        }

        public void CreateClientLeaveChatRequestDialog(int p_chat_id, TypeOfChat p_chat_type)
        {
            ma_client_leave_chat_request_dialog_manager.Create(p_chat_id, p_chat_type);
        }

        public void CreateCreatePrivateChatRequestDialog(string p_invited_person_user_name)
        {
            ma_create_private_chat_request_dialog_manager.Create(p_invited_person_user_name);
        }

        public void CreateClientJoinPublicChatRequestDialog(int p_chat_id)
        {
            ma_client_join_public_chat_request_dialog_manager.Create(p_chat_id);
        }

        public void CreateCreateAddAgreementDialog()
        {
            ma_create_add_agreement_dialog_manager.Create();
        }

        public void CreateFormalMessageRequestDialog()
        {
            ma_formal_message_request_dialog_manager.Create();
        }


        public void Receive(DialogMessageForClient p_message)
        {
            if (p_message.Get_dialog_type == TypeOfDialog.ClientFriendChangedStatusInform)
            {
                ma_client_friend_changed_status_inform_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.ServerReadOfflineMessagesInform)
            {
                ma_server_offline_message_read_inform_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.SendToClinetFormalMessage)
            {
                ma_send_to_client_formal_message_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.ClientJoinPublicChatRequest)
            {
                ma_client_join_public_chat_request_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.ClientLeaveChatRequest)
            {
                ma_client_leave_chat_request_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.ClientSomeoneJoinedChatInform)
            {
                ma_client_someone_joined_chat_inform_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.ClientSomeoneLeftChatInform)
            {
                ma_client_someone_left_chat_inform_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.CreateAddAgreement)
            {
                ma_create_add_agreement_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.CreatePrivateChatRequest)
            {
                ma_create_private_chat_request_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.FormalMessageRequest)
            {
                ma_formal_message_request_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.GetAgreementAnswer)
            {
                ma_get_agreement_answer_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.InformEjectedChatUser)
            {
                ma_inform_ejected_user_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.ClientFriendListChangedInform)
            {
                ma_client_friend_list_changed_inform_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.ClientCreatedPrivateChatInform)
            {
                ma_client_created_private_chat_inform_dialog_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_dialog_type == TypeOfDialog.ClientInvitedAgreementInform)
            {
                ma_client_invited_agreement_inform_dialog_manager.ReceiveMessage(p_message);
            }
        }

        public void Clear()
        {
            ma_get_agreement_answer_dialog_manager.Clear();
            ma_server_offline_message_read_inform_dialog_manager.Clear();
            ma_client_leave_chat_request_dialog_manager.Clear();
            ma_client_someone_left_chat_inform_dialog_manager.Clear();
            ma_inform_ejected_user_dialog_manager.Clear();
            ma_create_private_chat_request_dialog_manager.Clear();
            ma_client_join_public_chat_request_dialog_manager.Clear();
            ma_client_someone_joined_chat_inform_dialog_manager.Clear();
            ma_client_friend_changed_status_inform_dialog_manager.Clear();
            ma_send_to_client_formal_message_dialog_manager.Clear();
            ma_client_friend_list_changed_inform_dialog_manager.Clear();
            ma_client_created_private_chat_inform_dialog_manager.Clear();
            ma_create_add_agreement_dialog_manager.Clear();
            ma_formal_message_request_dialog_manager.Clear();
            ma_client_invited_agreement_inform_dialog_manager.Clear();
        }


        public void AllMa_Send(DialogMessageForServer p_message)
        {
            send_messages(p_message);
        }
        public void AllMa_MessageShow(string p_message_text)
        {
            message_show(p_message_text);
        }
        public void AllMa_SomeoneLeftChat(string p_user_left_chat_name, int p_chat_id, TypeOfChat p_chat_type)
        {
            inform_someone_left_chat(p_user_left_chat_name, p_chat_id, p_chat_type);
        }
        public void AllMa_InformEjectFromChat(int p_chat_id, string p_ejecting_comment, TypeOfChat p_chat_type)
        {
            inform_eject_from_chat(p_chat_id, p_ejecting_comment, p_chat_type);
        }
        public void AllMa_CreateEmptyPrivateChat(int p_chat_id)
        {
            create_empty_private_chat(p_chat_id);
        }
        public void AllMa_InformJoinToPublicChat(int p_chat_id, List<string> p_public_chat_persons)
        {
            inform_join_to_public_chat(p_chat_id, p_public_chat_persons);
        }
        public void AllMa_SomeoneJoinedChat(string p_user_joined_chat_name, int p_chat_id, TypeOfChat p_chat_type)
        {
            someone_joined_chat(p_user_joined_chat_name, p_chat_id, p_chat_type);
        }
        public void AllMa_InformFriendChangedStatus(PersonStatus p_user_and_new_status)
        {
            inform_friend_changed_status(p_user_and_new_status);
        }
        public void AllMa_ShowFormalMessage(FormalMessage p_formal_message, int p_message_id)
        {
            show_formal_message(p_formal_message, p_message_id);
        }
        public void AllMa_InformFriendListChanhged(List<PersonStatus> p_new_friends_and_status)
        {
            inform_friendlist_changed(p_new_friends_and_status);
        }
        public void AllMa_InformCreatedPrivateChat(string p_user_name, int p_chat_id)
        {
            inform_created_private_chat(p_user_name, p_chat_id);
        }
        public void AllMa_ShowAnAgreementInvitation(AgreementInvitationInfo p_agreement_invitation_info)
        {
            show_an_agreement_invitation(p_agreement_invitation_info);
        }
    }

    public class Cl_ClientDelegatesForDialogs
    {
        public SendDialogMessage send_messages;
        public MessageShow message_show;
        public SomeoneLeftChat inform_someone_left_chat;
        public InformEjectFromChat inform_eject_from_chat;
        public CreateEmptyPrivateChat create_empty_private_chat;
        public InformJoinToPublicChat inform_join_to_public_chat;
        public SomeoneJoinedChat someone_joined_chat;
        public InformFriendChangedStatus inform_friend_changed_status;
        public ShowFormalMessage show_formal_message;
        public InformFriendListChanhged inform_friendlist_changed;
        public InformCreatedPrivateChat inform_created_private_chat;
        public ShowAnAgreementInvitation show_an_agreement_invitation;

        public Cl_ClientDelegatesForDialogs(SendDialogMessage p_send_messages, MessageShow p_message_show, SomeoneLeftChat p_inform_someone_left_chat
            , InformEjectFromChat p_inform_eject_from_chat, CreateEmptyPrivateChat p_create_empty_private_chat, InformJoinToPublicChat p_inform_join_to_public_chat
        , SomeoneJoinedChat p_someone_joined_chat, InformFriendChangedStatus p_inform_friend_changed_status, ShowFormalMessage p_show_formal_message
        , InformFriendListChanhged p_inform_friendlist_changed, InformCreatedPrivateChat p_inform_created_private_chat, ShowAnAgreementInvitation p_show_an_agreement_invitation)
        {
            send_messages = p_send_messages;
            message_show = p_message_show;
            inform_someone_left_chat = p_inform_someone_left_chat;
            inform_eject_from_chat = p_inform_eject_from_chat;
            create_empty_private_chat = p_create_empty_private_chat;
            inform_join_to_public_chat = p_inform_join_to_public_chat;
            someone_joined_chat = p_someone_joined_chat;
            inform_friend_changed_status = p_inform_friend_changed_status;
            show_formal_message = p_show_formal_message;
            inform_friendlist_changed = p_inform_friendlist_changed;
            inform_created_private_chat = p_inform_created_private_chat;
            show_an_agreement_invitation = p_show_an_agreement_invitation;
        }
    }


    public enum DialogStatus
    {
        Running,
        WaitingForAMessage,
        WaitingForAReceipt,
        ReceiptInvestigation,
        ReceipptRejected,
        ReceiptAccepted,
        MessageInvestigation,
        MessageAccepted,
        MessageRejected,
        End,
        Canceling,
        UserRequestRejected
    }

    public abstract class Se_BaseBooleanFunctionResult
    {
        protected bool function_result;

    }
    public class Se_BooleanFunctionAccResult : Se_BaseBooleanFunctionResult
    {
        public Se_BooleanFunctionAccResult()
        {
            function_result = true;
        }
        public bool get_function_result
        {
            get
            {
                return function_result;
            }
        }
    }
    public class Se_BooleanFunctionRejResult : Se_BaseBooleanFunctionResult
    {
        string reject_comment;
        public Se_BooleanFunctionRejResult(string p_reject_comment)
        {
            function_result = false;
            reject_comment = p_reject_comment;
        }
        public string get_reject_comment
        {
            get
            {
                return reject_comment;
            }
        }
        public bool get_function_result
        {
            get
            {
                return function_result;
            }
        }
    }

    public abstract class Se_BaseIntFunctionResult
    {
        protected bool function_result;

    }
    public class Se_IntFunctionRejResult : Se_BaseIntFunctionResult
    {
        string reject_comment;
        public string Get_reject_comment
        {
            get { return reject_comment; }
        }

        public Se_IntFunctionRejResult(string p_reject_comment)
        {
            reject_comment = p_reject_comment;
            function_result = true;
        }
        public bool get_function_result
        {
            get
            {
                return function_result;
            }
        }
    }
    public class Se_IntFunctionAccResult : Se_BaseIntFunctionResult
    {
        int message_content;
        public Se_IntFunctionAccResult(int p_message_content)
        {
            function_result = false;
            message_content = p_message_content;
        }
        public int get_message_content
        {
            get
            {
                return message_content;
            }
        }
        public bool get_function_result
        {
            get
            {
                return function_result;
            }
        }
    }

    public enum ClientApplicationMode
    {
        Dissconnected,
        UdpAllocate,
        TcpAllocate,
        UnAuthorized,
        Authorized
    }

    public static class HelperFunctions
    {
        public static int GetGUID()
        {
            Guid t_guid = new Guid();
            t_guid = Guid.NewGuid();
            return BitConverter.ToInt32(t_guid.ToByteArray(), 0);
        }

        public static bool DialogMessageObjectInvestigate(DialogMessageForServer p_dialog_message)
        {
            try
            {

                if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.ReceiptMessage)
                {
                    Di_Mess_ReceiptMessage temp = (Di_Mess_ReceiptMessage)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.AgreementAnswer)
                {
                    Di_Mess_AgreementAnswer temp = (Di_Mess_AgreementAnswer)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.InformEjectedChatUser)
                {
                    Di_Mess_InformEjectedChatUser temp = (Di_Mess_InformEjectedChatUser)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.CreateAddAgreementRequest)
                {
                    Di_Mess_CreateAddAgreementRequest temp = (Di_Mess_CreateAddAgreementRequest)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.CreatePrivateChatCommand)
                {
                    Di_Mess_CreatePrivateChatCommand temp = (Di_Mess_CreatePrivateChatCommand)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.FriendChangeStatus)
                {
                    Di_Mess_FriendChangeStatus temp = (Di_Mess_FriendChangeStatus)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.FriendsListAndStatus)
                {
                    Di_Mess_FriendsListAndStatus temp = (Di_Mess_FriendsListAndStatus)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.InviteToAgreementInfo)
                {
                    Di_Mess_InviteToAgreemenstInfo temp = (Di_Mess_InviteToAgreemenstInfo)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.JoinPublicChatRequest)
                {
                    Di_Mess_JoinPublicChatRequest temp = (Di_Mess_JoinPublicChatRequest)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.LoginRequestData)
                {
                    Di_Mess_LoginRequestData temp = (Di_Mess_LoginRequestData)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.OfflineMessages)
                {
                    Di_Mess_OfflineMessages temp = (Di_Mess_OfflineMessages)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.PrivateChatInfo)
                {
                    Di_Mess_PrivateChatInfo temp = (Di_Mess_PrivateChatInfo)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.PublicChatIds)
                {
                    Di_Mess_PublicChatIds temp = (Di_Mess_PublicChatIds)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.SignUpRequestData)
                {
                    Di_Mess_SignUpRequestData temp = (Di_Mess_SignUpRequestData)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.StartPrivateChatRequest)
                {
                    Di_Mess_StartPrivateChatRequest temp = (Di_Mess_StartPrivateChatRequest)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.LoginDataRequestMessage)
                {
                    Di_Mess_LoginDataRequestMessage temp = (Di_Mess_LoginDataRequestMessage)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.CancelDialog)
                {
                    Di_Mess_CancelDialogMessage temp = (Di_Mess_CancelDialogMessage)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.ClientLeaveChatRequest)
                {
                    Di_Mess_ClientLeaveChatRequest temp = (Di_Mess_ClientLeaveChatRequest)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.SomeoneLeftTheChat)
                {
                    Di_Mess_SomeoneLeftTheChat temp = (Di_Mess_SomeoneLeftTheChat)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.ClientInformFormalMessage)
                {
                    Di_Mess_ClientInformFormalMessage temp = (Di_Mess_ClientInformFormalMessage)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.ClientFormalMessageRequest)
                {
                    Di_Mess_ClientFormalMessageRequest temp = (Di_Mess_ClientFormalMessageRequest)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.SomeoneJoinedTheChat)
                {
                    Di_Mess_SomeoneJoinedTheChat temp = (Di_Mess_SomeoneJoinedTheChat)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.PublicChatUsersIds)
                {
                    Di_Mess_PublicChatUsersIds temp = (Di_Mess_PublicChatUsersIds)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.PrivateChatInvitationAnswer)
                {
                    Di_Mess_PrivateChatInvitationAnswer temp = (Di_Mess_PrivateChatInvitationAnswer)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.CreatedPrivateChatInform)
                {
                    Di_Mess_CreatedPrivateChatInform temp = (Di_Mess_CreatedPrivateChatInform)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.InformInviteToAgreementInfo)
                {
                    Di_Mess_InformInviteToAgreementInfo temp = (Di_Mess_InformInviteToAgreementInfo)p_dialog_message.Get_message_object;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool DialogMessageObjectInvestigate(DialogMessageForClient p_dialog_message)
        {
            try
            {

                if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.ReceiptMessage)
                {
                    Di_Mess_ReceiptMessage temp = (Di_Mess_ReceiptMessage)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.AgreementAnswer)
                {
                    Di_Mess_AgreementAnswer temp = (Di_Mess_AgreementAnswer)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.InformEjectedChatUser)
                {
                    Di_Mess_InformEjectedChatUser temp = (Di_Mess_InformEjectedChatUser)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.CreateAddAgreementRequest)
                {
                    Di_Mess_CreateAddAgreementRequest temp = (Di_Mess_CreateAddAgreementRequest)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.CreatePrivateChatCommand)
                {
                    Di_Mess_CreatePrivateChatCommand temp = (Di_Mess_CreatePrivateChatCommand)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.FriendChangeStatus)
                {
                    Di_Mess_FriendChangeStatus temp = (Di_Mess_FriendChangeStatus)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.FriendsListAndStatus)
                {
                    Di_Mess_FriendsListAndStatus temp = (Di_Mess_FriendsListAndStatus)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.InviteToAgreementInfo)
                {
                    Di_Mess_InviteToAgreemenstInfo temp = (Di_Mess_InviteToAgreemenstInfo)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.JoinPublicChatRequest)
                {
                    Di_Mess_JoinPublicChatRequest temp = (Di_Mess_JoinPublicChatRequest)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.LoginRequestData)
                {
                    Di_Mess_LoginRequestData temp = (Di_Mess_LoginRequestData)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.OfflineMessages)
                {
                    Di_Mess_OfflineMessages temp = (Di_Mess_OfflineMessages)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.PrivateChatInfo)
                {
                    Di_Mess_PrivateChatInfo temp = (Di_Mess_PrivateChatInfo)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.PublicChatIds)
                {
                    Di_Mess_PublicChatIds temp = (Di_Mess_PublicChatIds)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.SignUpRequestData)
                {
                    Di_Mess_SignUpRequestData temp = (Di_Mess_SignUpRequestData)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.StartPrivateChatRequest)
                {
                    Di_Mess_StartPrivateChatRequest temp = (Di_Mess_StartPrivateChatRequest)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.LoginDataRequestMessage)
                {
                    Di_Mess_LoginDataRequestMessage temp = (Di_Mess_LoginDataRequestMessage)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.CancelDialog)
                {
                    Di_Mess_CancelDialogMessage temp = (Di_Mess_CancelDialogMessage)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.ClientLeaveChatRequest)
                {
                    Di_Mess_ClientLeaveChatRequest temp = (Di_Mess_ClientLeaveChatRequest)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.SomeoneLeftTheChat)
                {
                    Di_Mess_SomeoneLeftTheChat temp = (Di_Mess_SomeoneLeftTheChat)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.ClientInformFormalMessage)
                {
                    Di_Mess_ClientInformFormalMessage temp = (Di_Mess_ClientInformFormalMessage)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.ClientFormalMessageRequest)
                {
                    Di_Mess_ClientFormalMessageRequest temp = (Di_Mess_ClientFormalMessageRequest)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.SomeoneJoinedTheChat)
                {
                    Di_Mess_SomeoneJoinedTheChat temp = (Di_Mess_SomeoneJoinedTheChat)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.PublicChatUsersIds)
                {
                    Di_Mess_PublicChatUsersIds temp = (Di_Mess_PublicChatUsersIds)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.PrivateChatInvitationAnswer)
                {
                    Di_Mess_PrivateChatInvitationAnswer temp = (Di_Mess_PrivateChatInvitationAnswer)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.CreatedPrivateChatInform)
                {
                    Di_Mess_CreatedPrivateChatInform temp = (Di_Mess_CreatedPrivateChatInform)p_dialog_message.Get_message_object;
                }
                else if (p_dialog_message.Get_message_object_type == TypeOfDialogMessage.InformInviteToAgreementInfo)
                {
                    Di_Mess_InformInviteToAgreementInfo temp = (Di_Mess_InformInviteToAgreementInfo)p_dialog_message.Get_message_object;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool MessageFromWorkerInvestigate(MessageFromWorker p_message)
        {
            if (p_message.Get_type_of_message_from_worker_object == TypeOfMessageFromWorker.FinalMessage)
            {
                FinalMessageForClient t_final_message = null;
                try
                {
                    t_final_message = ((FinalMessageForClient)p_message.Get_message_from_worker_object);
                }
                catch (Exception)
                {
                    return false;
                }
                if (t_final_message.Get_message_type == TypeOfMessage.Chat)
                {
                    try
                    {
                        ChatMessageForClient t_chat_message = ((ChatMessageForClient)t_final_message.Get_message_object);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else if (t_final_message.Get_message_type == TypeOfMessage.Dialog)
                {
                    try
                    {
                        DialogMessageForClient t_dialog_message = ((DialogMessageForClient)t_final_message.Get_message_object);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            else if (p_message.Get_type_of_message_from_worker_object == TypeOfMessageFromWorker.SignalMessage)
            {
                try
                {
                    ClientWorkerSignal t_siganl_message = ((ClientWorkerSignal)p_message.Get_message_from_worker_object);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (p_message.Get_type_of_message_from_worker_object == TypeOfMessageFromWorker.OfflineInform)
            {
                try
                {
                    if (p_message.Get_message_from_worker_object == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }

            }
            return false;
        }

        public static bool UserNameStringCheck(string p_inpout_string)
        {
            foreach (char t_char in p_inpout_string)
            {
                if (!(char.IsLetterOrDigit(t_char) || t_char == '_'))
                    return false;
            }
            return true;
        }

    }
}

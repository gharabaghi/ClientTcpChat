using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientTcpChat.Forms;
using ClientTcpChat.Classes;

namespace ClientTcpChat.Classes
{

    [Serializable]
    public enum UserStatus
    {
        Online,
        Offline
    }
    [Serializable]
    public class PersonStatus
    {
        string user_name;
        UserStatus user_status;
        public PersonStatus(string name, UserStatus status)
        {
            user_name = name;
            user_status = status;
        }
        public string Get_user_name
        {
            get
            {
                return user_name;
            }
        }
        public UserStatus Get_user_status
        {
            get
            {
                return user_status;
            }
        }

        public override bool Equals(object obj)
        {
            PersonStatus temp_user_status_object = null;
            try
            {
                temp_user_status_object = (PersonStatus)obj;
            }
            catch (Exception)
            {
                return false;
            }
            if (user_name == temp_user_status_object.user_name && user_status == temp_user_status_object.user_status)
            {
                return true;
            }
            return false;
        }
    }
    [Serializable]
    public class Di_Mess_FriendsListAndStatus
    {
        List<PersonStatus> all_friends_and_status;
        public Di_Mess_FriendsListAndStatus(List<PersonStatus> p_all_friends_and_status)
        {
            all_friends_and_status = p_all_friends_and_status;
        }
        public List<PersonStatus> Get_all_friends_and_status
        {
            get
            {
                return all_friends_and_status;
            }
        }
    }

    [Serializable]
    public class OfflineMessage
    {
        string sender_user_id;
        public string Get_sender_user_id
        {
            get { return sender_user_id; }
        }

        string message_text;
        public string Get_message_text
        {
            get { return message_text; }
        }

        int message_id;
        public int Get_message_id
        {
            get { return message_id; }
        }

        //maybe date//
        public OfflineMessage(int p_message_id, string p_sender_user_id, string p_context)
        {
            message_id = p_message_id;
            sender_user_id = p_sender_user_id;
            message_text = p_context;
        }

        public override bool Equals(object obj)
        {
            OfflineMessage t_message = null;
            try
            {
                t_message = (OfflineMessage)obj;
            }
            catch (Exception)
            {
                return false;
            }
            if (t_message.Get_message_id == message_id && t_message.message_text == message_text && t_message.sender_user_id == sender_user_id)
                return true;
            return false;
        }
    }
    [Serializable]
    public class Di_Mess_OfflineMessages
    {
        List<OfflineMessage> all_offline_messages;
        public Di_Mess_OfflineMessages(List<OfflineMessage> p_all_offline_messages)
        {
            all_offline_messages = p_all_offline_messages;
        }
        public List<OfflineMessage> Get_all_offline_messages
        {
            get
            {
                return all_offline_messages;
            }
        }
    }

    [Serializable]
    public class Di_Mess_FriendChangeStatus
    {
        PersonStatus person_status_changed;
        public Di_Mess_FriendChangeStatus(PersonStatus p_person_status_changed)
        {
            person_status_changed = p_person_status_changed;
        }
        public PersonStatus Get_person_status_changed
        {
            get
            {
                return person_status_changed;
            }
        }
    }

    [Serializable]
    public class Di_Mess_LoginRequestData
    {
        string user_name;
        string password;
        public Di_Mess_LoginRequestData(string p_user_name, string p_password)
        {
            user_name = p_user_name;
            password = p_password;
        }
        public string Get_user_name
        {
            get
            {
                return user_name;
            }
        }
        public string Get_password
        {
            get
            {
                return password;
            }
        }
    }

    [Serializable]
    public class Di_Mess_StartPrivateChatRequest
    {
        string inivted_person_user_name;
        public Di_Mess_StartPrivateChatRequest(string p_invited_person_user_name)
        {
            inivted_person_user_name = p_invited_person_user_name;
        }
        public string Get_invited_person_user_name
        {
            get
            {
                return inivted_person_user_name;
            }
        }
    }

    [Serializable]
    public class Di_Mess_JoinPublicChatRequest
    {
        int public_chat_id;
        public Di_Mess_JoinPublicChatRequest(int p_public_chat_id)
        {
            public_chat_id = p_public_chat_id;
        }
        public int Get_public_chat_id
        {
            get
            {
                return public_chat_id;
            }
        }
    }

    [Serializable]
    public class Di_Mess_PrivateChatInfo
    {
        int chat_id;
        public Di_Mess_PrivateChatInfo(int p_chat_id)
        {
            chat_id = p_chat_id;
        }
        public int Get_chat_id
        {
            get
            {
                return chat_id;
            }
        }
    }

    [Serializable]
    public class Di_Mess_CreatePrivateChatCommand
    {
        string friend_user_id;

        public string Get_friend_user_id
        {
            get { return friend_user_id; }
        }
        int chat_id;

        public int Get_chat_id
        {
            get { return chat_id; }
        }
        public Di_Mess_CreatePrivateChatCommand(string p_friend_user_id, int p_chat_id)
        {
            friend_user_id = p_friend_user_id;
            chat_id = p_chat_id;
        }
    }

    [Serializable]
    public class Di_Mess_PublicChatIds
    {
        List<int> public_chat_ids;

        public Di_Mess_PublicChatIds(List<int> chat_ids)
        {
            public_chat_ids = chat_ids;
        }

        public List<int> Get_public_chat_ids
        {
            get
            {
                return public_chat_ids;
            }
        }
    }

    [Serializable]
    public class Di_Mess_PublicChatUsersIds
    {
        List<string> users_ids;

        public List<string> Get_users_ids
        {
            get { return users_ids; }
        }
        public Di_Mess_PublicChatUsersIds(List<string> p_users_ids)
        {
            users_ids = p_users_ids;
        }
    }

    [Serializable]
    public class Di_Mess_AccountInformation
    {
        string user_name;
        public string Get_user_name
        {
            get { return user_name; }
        }

        public Di_Mess_AccountInformation(string p_user_name)
        {
            user_name = p_user_name;
        }
    }

    //Someone left chat
    //leave chat inform

    //new
    [Serializable]
    public class Di_Mess_ClientLeaveChatRequest
    {
        //string user_name;
        int chat_id;
        TypeOfChat chat_type;
        public TypeOfChat Get_chat_type
        {
            get { return chat_type; }
        }

        public int Get_chat_id
        {
            get { return chat_id; }
        }
        public Di_Mess_ClientLeaveChatRequest(int p_chat_id, TypeOfChat p_chat_type)
        {
            chat_id = p_chat_id;
            chat_type = p_chat_type;
        }
    }       //from client to server
    //new 

    [Serializable]
    public class Di_Mess_SomeoneLeftTheChat
    {
        string user_name;
        public string Get_user_name
        {
            get { return user_name; }
        }

        TypeOfChat chat_type;
        public TypeOfChat Get_chat_type
        {
            get { return chat_type; }
        }

        int chat_id;
        public int Get_chat_id
        {
            get { return chat_id; }
        }

        public Di_Mess_SomeoneLeftTheChat(string p_user_name, int p_chat_id, TypeOfChat p_chat_type)
        {
            user_name = p_user_name;
            chat_id = p_chat_id;
            chat_type = p_chat_type;
        }

    }       //from server to client

    [Serializable]
    public class Di_Mess_SomeoneJoinedTheChat
    {
        string user_name;
        public string Get_user_name
        {
            get { return user_name; }
        }

        TypeOfChat chat_type;
        public TypeOfChat Get_chat_type
        {
            get { return chat_type; }
        }


        int chat_id;
        public int Get_chat_id
        {
            get { return chat_id; }
        }

        public Di_Mess_SomeoneJoinedTheChat(string p_user_name, int p_chat_id, TypeOfChat p_chat_type)
        {
            user_name = p_user_name;
            chat_id = p_chat_id;
            chat_type = p_chat_type;
        }
    }

    [Serializable]
    public class Di_Mess_CreatedPrivateChatInform
    {
        string starter_user_name;
        public string Get_starter_user_name
        {
            get { return starter_user_name; }
        }

        int chat_id;
        public int Get_chat_id
        {
            get { return chat_id; }
        }

        public Di_Mess_CreatedPrivateChatInform(string p_starter_user_name, int p_chat_id)
        {
            starter_user_name = p_starter_user_name;
            chat_id = p_chat_id;
        }
    }

    [Serializable]
    public class Di_Mess_ClientInformFormalMessage
    {
        FormalMessage message;
        public FormalMessage Get_message
        {
            get { return message; }
        }

        int message_id;
        public int Get_message_id
        {
            get { return message_id; }
        }

        public Di_Mess_ClientInformFormalMessage(FormalMessage p_message, int p_message_id)
        {
            message = p_message;
            message_id = p_message_id;
        }
    }           //From client to server

    [Serializable]
    public class Di_Mess_ClientFormalMessageRequest
    {
        string receiver_user_id;
        string message_text;
        public string Get_message_text
        {
            get { return message_text; }
        }

        public string Get_receiver_user_id
        {
            get { return receiver_user_id; }
        }

        public Di_Mess_ClientFormalMessageRequest(string p_sender_user_id, string p_message_text)
        {
            receiver_user_id = p_sender_user_id;
            message_text = p_message_text;
        }
    }       //from server to client

    [Serializable]
    public class Di_Mess_ServerReadOfflineMessagesInform
    {
        List<int> message_ids;
        public List<int> Get_message_ids
        {
            get { return message_ids; }
        }

        public Di_Mess_ServerReadOfflineMessagesInform(List<int> p_message_ids)
        {
            message_ids = p_message_ids;
        }
    }

    /*public class Di_Mess_RequestRejected        //برای اینکه ممکن است در دیالوگی پیام تایید اولیه به معنای تایید کل نباشد مثلا در لانه ای ها
    {
        string reject_comment;

        public string Get_reject_comment
        {
            get { return reject_comment; }
        }
        public Di_Mess_RequestRejected(string p_reject_comment)
        {
            reject_comment = p_reject_comment;
        }
    }*/

    [Serializable]
    public class Di_Mess_InformEjectedChatUser
    {
        //could add string ejectcomment
        int id_of_closed_chat;

        TypeOfChat chat_type;
        public TypeOfChat Get_chat_type
        {
            get { return chat_type; }
        }

        string ejecting_coment;
        public string Get_ejecting_coment
        {
            get { return ejecting_coment; }
        }

        public int Get_id_of_closed_chat
        {
            get { return id_of_closed_chat; }
        }
        public Di_Mess_InformEjectedChatUser(int p_id_of_closed_chat, string p_ejecting_comment, TypeOfChat p_chat_type)
        {
            id_of_closed_chat = p_id_of_closed_chat;
            ejecting_coment = p_ejecting_comment;
        }
    }

    [Serializable]
    public class Di_Mess_CreateAddAgreementRequest
    {
        string user_id_to_add;

        TypeOfAgreement type_of_agreement;
        public TypeOfAgreement Get_type_of_agreement
        {
            get { return type_of_agreement; }
        }

        public string Get_user_id_to_add
        {
            get { return user_id_to_add; }
        }
        public Di_Mess_CreateAddAgreementRequest(string p_user_id_to_add)
        {
            user_id_to_add = p_user_id_to_add;
            type_of_agreement = TypeOfAgreement.Add;
        }
    }

    [Serializable]
    public class Di_Mess_SignUpRequestData
    {
        string user_id_to_Register;

        public string Get_user_id_to_Register
        {
            get { return user_id_to_Register; }
        }
        string password_to_use;

        public string Get_password_to_use
        {
            get { return password_to_use; }
        }
        public Di_Mess_SignUpRequestData(string p_user_id_to_Register, string p_password_to_use)
        {
            user_id_to_Register = p_user_id_to_Register;
            password_to_use = p_password_to_use;
        }
    }

    [Serializable]
    public class AgreementInvitationInfo
    {
        TypeOfAgreement agreement_type;
        public TypeOfAgreement Get_agreement_type
        {
            get { return agreement_type; }
        }

        string inviting_user_id;
        public string Get_inviting_user_id
        {
            get { return inviting_user_id; }
        }

        string agreement_text;
        public string Get_agreement_text
        {
            get { return agreement_text; }
        }

        int agreement_id;
        public int Get_agreement_id
        {
            get { return agreement_id; }
        }

        public AgreementInvitationInfo(string p_inviting_user_id, string p_agreement_text, int p_agreement_id, TypeOfAgreement p_agreement_type)
        {
            agreement_type = p_agreement_type;
            inviting_user_id = p_inviting_user_id;
            agreement_id = p_agreement_id;
            agreement_text = p_agreement_text;
        }

        public override bool Equals(object obj)
        {
            AgreementInvitationInfo t_agreement_invitation_info = null;
            try
            {
                t_agreement_invitation_info = (AgreementInvitationInfo)obj;
            }
            catch (Exception)
            {
                return false;
            }
            if (t_agreement_invitation_info.Get_agreement_id == agreement_id && t_agreement_invitation_info.Get_agreement_text == agreement_text
                && t_agreement_invitation_info.Get_agreement_type == agreement_type && t_agreement_invitation_info.Get_inviting_user_id == inviting_user_id)
                return true;
            return false;
        }
    }
    [Serializable]
    public class Di_Mess_InviteToAgreemenstInfo
    {
        List<AgreementInvitationInfo> all_user_agreement_invitations;
        public List<AgreementInvitationInfo> Get_all_user_agreement_invitations
        {
            get { return all_user_agreement_invitations; }
        }

        public Di_Mess_InviteToAgreemenstInfo(List<AgreementInvitationInfo> p_all_user_agreement_invitations)
        {
            all_user_agreement_invitations = p_all_user_agreement_invitations;
        }

    }

    [Serializable]
    public class Di_Mess_AgreementAnswer
    {
        int agreement_id;

        TypeOfAgreement agreement_type;
        public TypeOfAgreement Get_agreement_type
        {
            get { return agreement_type; }
        }

        public int Get_agreement_id
        {
            get { return agreement_id; }
        }
        bool answer;

        public bool Get_answer
        {
            get { return answer; }
        }
        public Di_Mess_AgreementAnswer(int p_agreement_id, bool p_answer, TypeOfAgreement p_agreement_type)
        {
            agreement_type = p_agreement_type;
            agreement_id = p_agreement_id;
            answer = p_answer;
        }
    }

    [Serializable]
    public class Di_Mess_LoginDataRequestMessage
    {
        bool request_flag;
        public bool Get_request_flag
        {
            get { return request_flag; }
        }

        public Di_Mess_LoginDataRequestMessage()
        {
            request_flag = true;
        }
    }

    [Serializable]
    public class Di_Mess_CancelDialogMessage
    {
        string cancel_comment;
        public string Get_cancel_comment
        {
            get { return cancel_comment; }
        }

        public Di_Mess_CancelDialogMessage(string p_cancel_comment)
        {
            cancel_comment = p_cancel_comment;
        }
    }

    [Serializable]
    public class Di_Mess_PrivateChatInvitationAnswer
    {
        bool answer;
        public bool Get_answer
        {
            get { return answer; }
        }

        public Di_Mess_PrivateChatInvitationAnswer(bool p_answer)
        {
            answer = p_answer;
        }
    }

    //25

    [Serializable]
    public enum ReceiptStatus
    {
        Accepted,
        Rejected
    };

    [Serializable]
    public abstract class Di_Mess_Rec_AccRejParent
    {
    }

    [Serializable]
    public class Di_Mess_Rec_AcceptMessage : Di_Mess_Rec_AccRejParent    //mitavan baraye tabee masalan logi ham az in sakhtar estefade kard
    {
        int accpepted_message_id;
        public Di_Mess_Rec_AcceptMessage(int p_accpepted_message_id)
        {
            accpepted_message_id = p_accpepted_message_id;
        }

        public int Get_accpepted_message_id
        {
            get
            {
                return accpepted_message_id;
            }
        }
    }

    [Serializable]
    public class Di_Mess_Rec_RejectMessage : Di_Mess_Rec_AccRejParent
    {
        int rejected_message_id;
        string comment_for_rejecting;
        public Di_Mess_Rec_RejectMessage(int p_rejected_message_id, string p_comment_for_rejecting)
        {
            rejected_message_id = p_rejected_message_id;
            comment_for_rejecting = p_comment_for_rejecting;
        }

        public int Get_rejected_message_id
        {
            get
            {
                return rejected_message_id;
            }
        }

        public string Get_comment_for_rejecting
        {
            get
            {
                return comment_for_rejecting;
            }
        }
    }

    [Serializable]
    public class Di_Mess_ReceiptMessage
    {
        ReceiptStatus message_rec_status;
        Di_Mess_Rec_AccRejParent rec_message;
        public Di_Mess_ReceiptMessage(ReceiptStatus p_message_rec_status, Di_Mess_Rec_AccRejParent p_rec_message)
        {
            message_rec_status = p_message_rec_status;
            rec_message = p_rec_message;
        }

        public Di_Mess_Rec_AccRejParent Get_rec_message
        {
            get
            {
                return rec_message;
            }
        }
        public ReceiptStatus Get_message_rec_status
        {
            get
            {
                return message_rec_status;
            }
        }
    }


    [Serializable]
    public class FormalMessage      //for transport data for send to user if is onnline and made offline message if user is offline
    {
        string sender_user_id;
        public string Get_sender_user_id
        {
            get { return sender_user_id; }
        }

        string message_text;
        public string Get_message_text
        {
            get { return message_text; }
        }

        public FormalMessage(string p_sender_user_id, string p_message_text)
        {
            sender_user_id = p_sender_user_id;
            message_text = p_message_text;
        }

        public override bool Equals(object obj)
        {
            FormalMessage t_message = null;
            try
            {
                t_message = (FormalMessage)obj;
            }
            catch (Exception)
            {
                return false;
            }
            if (t_message.Get_sender_user_id == sender_user_id && t_message.message_text == message_text)
                return true;
            return false;
        }
    }

    /*public class Ch_Mess_TextChatMessage
    {
        string text_of_message;

        public string Get_text_of_message
        {
            get { return text_of_message; }
        }
        public Ch_Mess_TextChatMessage(string p_text_of_message)
        {
            text_of_message = p_text_of_message;
        }
    }*/



}

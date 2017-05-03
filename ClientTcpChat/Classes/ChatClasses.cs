using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientTcpChat.Forms;
using ClientTcpChat.Classes;
using System.Drawing;
using CommonChatTypes;

namespace ClientTcpChat.Classes
{
    public delegate void SendChatMessage(ChatMessageForServer p_message);
    public delegate void RemoveChatFormManager(int p_chat_id);
    public delegate void StartLeaveChatRequest(int p_chat_id, TypeOfChat p_chat_type);

    public class BaseChat : Form
    {
        protected int chat_id;
        protected TypeOfChat chat_type;
        protected List<string> chat_persons;
        protected ListBox chat_persons_list_box;
        protected RichTextBox messages_list_text_box;
        protected bool close_flag;

        protected SendChatMessage send_message;
        protected RemoveChatFormManager remove;
        protected StartLeaveChatRequest start_leave_chat_request;

        protected void BaseConstruct(int p_chat_id, TypeOfChat p_chat_type, List<string> p_chat_persons, SendChatMessage p_send_message
            , RemoveChatFormManager p_remove, StartLeaveChatRequest p_start_leave_chat_request, ListBox p_chat_persons_list_box
            , RichTextBox p_message_list_text_box)
        {
            chat_id = p_chat_id;
            chat_type = p_chat_type;
            chat_persons = p_chat_persons;

            send_message = p_send_message;
            start_leave_chat_request = p_start_leave_chat_request;
            remove = p_remove;

            chat_persons_list_box = p_chat_persons_list_box;
            messages_list_text_box = p_message_list_text_box;

            close_flag = false;
            chat_persons_list_box.Items.Clear();

            foreach (string t_user_name in p_chat_persons)
            {
                chat_persons_list_box.Items.Add(t_user_name);
            }
            this.Show();
        }

        public void ReceiveChatMessage(ChatMessageObjectToClient p_message)
        {
            string sender_user_name = p_message.Get_sender_user_name;
            string message_text = p_message.Get_chat_message.Get_text_of_message;
            ShowMessage(sender_user_name, message_text);
        }

        private void ShowMessage(string p_sender_user_name, string p_message_text)
        {
            messages_list_text_box.SelectionColor = Color.Blue;
            messages_list_text_box.AppendText(p_sender_user_name);
            messages_list_text_box.SelectionColor = Color.Black;
            messages_list_text_box.AppendText(": " + p_message_text + "\n");
        }

        public void UserLeftChat(string p_left_user_name)
        {
            if (chat_persons.Contains(p_left_user_name))
            {
                chat_persons.Remove(p_left_user_name);

                if (chat_persons_list_box.Items.Contains(p_left_user_name))
                    chat_persons_list_box.Items.Remove(p_left_user_name); 
            }
        }

        public void UserJoinChat(string p_new_user_name)
        {
            if (!chat_persons.Contains(p_new_user_name))
            {
                chat_persons.Add(p_new_user_name);
                chat_persons_list_box.Items.Add(p_new_user_name);
            }
        }

        public void Eeject()
        {
            close_flag = true;
            remove(chat_id);
            this.Close(); 
        }
    }

    public abstract class BaseChatManager
    {
        protected TypeOfChat chat_type;

        protected SendChatMessage send_message;
        protected StartLeaveChatRequest start_leave_chat_request;

        public void BaseConstruct(TypeOfChat p_chat_type, SendChatMessage p_send_message, StartLeaveChatRequest p_start_leave_chat_request)
        {
            chat_type = p_chat_type;
            send_message = p_send_message;
            start_leave_chat_request = p_start_leave_chat_request;
        }

        public abstract void ReceiveMessage(ChatMessageForClient p_message);

        public abstract void ChatRemoveItselfRequest(int p_chat_id);

        public abstract void SomeoneJoinedChat(string p_joined_user_name, int p_chat_id);

        public abstract bool Eject(int p_chat_id);

        public abstract void SomeoneLeftChat(string p_left_user_name, int p_chat_id);

        public abstract void Clear();

        public void Ma_SendChatMessage(ChatMessageForServer p_message)
        {
            send_message(p_message);
        }
        public void Ma_StartLeaveChatRequest(int p_chat_id, TypeOfChat p_chat_type)
        {
            start_leave_chat_request(p_chat_id, p_chat_type);
        }
    }

    public class Ma_PrivateChatManager : BaseChatManager
    {
        Dictionary<int, PrivateChat> all_chats;

        public Ma_PrivateChatManager(SendChatMessage p_send_message, StartLeaveChatRequest p_start_leave_chat_request)
        {
            base.BaseConstruct(TypeOfChat.Private, p_send_message, p_start_leave_chat_request);
            all_chats = new Dictionary<int, PrivateChat>();
        }

        public void CreateEmptyPrivateChat(string p_own_user_name, int p_chat_id)
        {
            all_chats.Add(p_chat_id, new PrivateChat(p_chat_id, p_own_user_name, new SendChatMessage(Ma_SendChatMessage), new RemoveChatFormManager(ChatRemoveItselfRequest)
            , new StartLeaveChatRequest(Ma_StartLeaveChatRequest)));
        }

        public void CreatePrivateChat(string p_own_user_name, string p_second_person_user_name, int p_chat_id)
        {
            all_chats.Add(p_chat_id, new PrivateChat(p_chat_id, p_own_user_name, p_second_person_user_name, new SendChatMessage(Ma_SendChatMessage)
                , new RemoveChatFormManager(ChatRemoveItselfRequest), new StartLeaveChatRequest(Ma_StartLeaveChatRequest)));
        }

        public override void ReceiveMessage(ChatMessageForClient p_message)
        {
            if (all_chats.ContainsKey(p_message.Get_chat_id))
            {
                all_chats[p_message.Get_chat_id].ReceiveChatMessage(p_message.Get_text_message_object);
            }
        }

        public override void SomeoneJoinedChat(string p_joined_user_name, int p_chat_id)
        {
            if (all_chats.ContainsKey(p_chat_id))
            {
                all_chats[p_chat_id].UserJoinChat(p_joined_user_name);
            }
        }

        public override void SomeoneLeftChat(string p_left_user_name, int p_chat_id)
        {
            if (all_chats.ContainsKey(p_chat_id))
            {
                all_chats[p_chat_id].UserLeftChat(p_left_user_name);
            }
        }

        public override bool Eject(int p_chat_id)
        {
            if (all_chats.ContainsKey(p_chat_id))
            {
                all_chats[p_chat_id].Eeject();
                return true;
            }
            return false;
        }

        public override void ChatRemoveItselfRequest(int p_chat_id)
        {
            if (all_chats.ContainsKey(p_chat_id))
            {
                all_chats.Remove(p_chat_id);
            }
        }

        public override void Clear()
        {
            List<int> all_chat_ids = new List<int>();
            all_chat_ids = all_chats.Keys.ToList();
            foreach (int p_chat_id in all_chat_ids)
            {
                if (all_chats.ContainsKey(p_chat_id))
                {
                    Eject(p_chat_id);
                }
            }
        }
    }

    public class Ma_PublicChatManager : BaseChatManager
    {
        Dictionary<int, PublicChat> all_chats;

        public Ma_PublicChatManager(SendChatMessage p_send_message, StartLeaveChatRequest p_start_leave_chat_request)
        {
            base.BaseConstruct(TypeOfChat.Public, p_send_message, p_start_leave_chat_request);
            all_chats = new Dictionary<int, PublicChat>();
        }

        public void CreatePublicChat(string p_own_user_name, List<string> p_chat_persons, int p_chat_id)
        {
            if (!p_chat_persons.Contains(p_own_user_name))
            {
                p_chat_persons.Add(p_own_user_name);
            }
            all_chats.Add(p_chat_id, new PublicChat(p_chat_id, p_chat_persons, new SendChatMessage(Ma_SendChatMessage), new RemoveChatFormManager(ChatRemoveItselfRequest)
            , new StartLeaveChatRequest(Ma_StartLeaveChatRequest)));
        }

        public override void ChatRemoveItselfRequest(int p_chat_id)
        {
            if (all_chats.ContainsKey(p_chat_id))
            {
                all_chats.Remove(p_chat_id);
            }
        }

        public override void ReceiveMessage(ChatMessageForClient p_message)
        {
            if (all_chats.ContainsKey(p_message.Get_chat_id))
            {
                all_chats[p_message.Get_chat_id].ReceiveChatMessage(p_message.Get_text_message_object);
            }
        }

        public override void SomeoneJoinedChat(string p_joined_user_name, int p_chat_id)
        {
            if (all_chats.ContainsKey(p_chat_id))
            {
                all_chats[p_chat_id].UserJoinChat(p_joined_user_name);
            }
        }

        public override void SomeoneLeftChat(string p_left_user_name, int p_chat_id)
        {
            if (all_chats.ContainsKey(p_chat_id))
            {
                all_chats[p_chat_id].UserLeftChat(p_left_user_name);
            }
        }

        public override bool Eject(int p_chat_id)
        {
            if (all_chats.ContainsKey(p_chat_id))
            {
                all_chats[p_chat_id].Eeject();
                return true;
            }
            return false;
        }

        public override void Clear()  
        {
            List<int> all_chat_ids = new List<int>();
            all_chat_ids = all_chats.Keys.ToList();
            foreach (int p_chat_id in all_chat_ids)
            {
                if (all_chats.ContainsKey(p_chat_id))
                {
                    Eject(p_chat_id);
                }
            }
        }

    }

    public class AllChats
    {
        Ma_PublicChatManager public_chat_manager;
        Ma_PrivateChatManager private_chat_manager;

        SendChatMessage send_message;
        StartLeaveChatRequest start_leave_chat_request;

        public AllChats(SendChatMessage p_send_message, StartLeaveChatRequest p_start_leave_chat_request)
        {
            send_message = p_send_message;
            start_leave_chat_request = p_start_leave_chat_request;

            public_chat_manager = new Ma_PublicChatManager(new SendChatMessage(AllCh_SendChatMessage), new StartLeaveChatRequest(AllCh_StartLeaveChatRequest));
            private_chat_manager = new Ma_PrivateChatManager(new SendChatMessage(AllCh_SendChatMessage), new StartLeaveChatRequest(AllCh_StartLeaveChatRequest));
        }

        public void Receive(ChatMessageForClient p_message)
        {
            if (p_message.Get_chat_type == TypeOfChat.Private)
            {
                private_chat_manager.ReceiveMessage(p_message);
            }
            else if (p_message.Get_chat_type == TypeOfChat.Public)
            {
                public_chat_manager.ReceiveMessage(p_message);
            }
        }

        public void CreateEmptyPrivateChat(string p_own_user_name, int p_chat_id)
        {
            private_chat_manager.CreateEmptyPrivateChat(p_own_user_name, p_chat_id);
        }

        public void CreatePrivateChat(string p_own_user_name, string p_second_person_user_name, int p_chat_id)
        {
            private_chat_manager.CreatePrivateChat(p_own_user_name, p_second_person_user_name, p_chat_id);
        }

        public void CreatePublicChat(string p_own_user_name, List<string> p_chat_persons, int p_chat_id)
        {
            public_chat_manager.CreatePublicChat(p_own_user_name, p_chat_persons, p_chat_id);
        }

        public void SomeoneJoinedChat(string p_user_joined_chat_name, int p_chat_id, TypeOfChat p_chat_type)
        {
            if (p_chat_type == TypeOfChat.Private)
            {
                private_chat_manager.SomeoneJoinedChat(p_user_joined_chat_name, p_chat_id);
            }
            else if (p_chat_type == TypeOfChat.Public)
            {
                public_chat_manager.SomeoneJoinedChat(p_user_joined_chat_name, p_chat_id);
            }
        }

        public void SomeoneLeftChat(string p_user_left_chat_name, int p_chat_id, TypeOfChat p_chat_type)
        {
            if (p_chat_type == TypeOfChat.Private)
            {
                private_chat_manager.SomeoneLeftChat(p_user_left_chat_name, p_chat_id);
            }
            else if (p_chat_type == TypeOfChat.Public)
            {
                public_chat_manager.SomeoneLeftChat(p_user_left_chat_name, p_chat_id);
            }
        }

        public bool EjectChat(int p_chat_id, TypeOfChat p_chat_type)
        {
            if (p_chat_type == TypeOfChat.Private)
            {
                return private_chat_manager.Eject(p_chat_id);
            }
            else if (p_chat_type == TypeOfChat.Public)
            {
                return public_chat_manager.Eject(p_chat_id);
            }
            return false;
        }

        public void Cleare()
        {
            public_chat_manager.Clear();
            private_chat_manager.Clear();
        }

        public void AllCh_SendChatMessage(ChatMessageForServer p_message)
        {
            send_message(p_message);
        }
        public void AllCh_StartLeaveChatRequest(int p_chat_id, TypeOfChat p_chat_type)
        {
            start_leave_chat_request(p_chat_id, p_chat_type);
        }
    }


}

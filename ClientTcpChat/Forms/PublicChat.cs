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
    public partial class PublicChat : BaseChat
    {
        public PublicChat(int p_chat_id, List<string> p_chat_persons, SendChatMessage p_send_message
            , RemoveChatFormManager p_remove, StartLeaveChatRequest p_start_leave_chat_request)
        {
            InitializeComponent();
            base.BaseConstruct(p_chat_id, TypeOfChat.Public, p_chat_persons, p_send_message, p_remove
                , p_start_leave_chat_request, ChatPersons_list, MessagesList_TextBox);
        }


        private void send_button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MyMessage_TextBox.Text))
            {
                send_message(new ChatMessageForServer(HelperFunctions.GetGUID(), chat_id, chat_type, new Ch_Mess_TextChatMessage(MyMessage_TextBox.Text)));
                MyMessage_TextBox.Text = string.Empty;
            }
            MyMessage_TextBox.Focus();
        }

        private void PublicChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close_flag == false)
            {
                start_leave_chat_request(chat_id, chat_type); 
                remove(chat_id);                                                                    
            }
        }
    }
}

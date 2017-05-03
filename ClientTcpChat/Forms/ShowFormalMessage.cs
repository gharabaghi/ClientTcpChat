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
    public partial class ShowFormalMessage_Form : Form
    {
        List<OfflineMessage> all_offline_messages;

        FormalMessage formal_message;
        int formal_message_id;
        bool flag;

        OfflienMesssageRead offlien_messages_read_inform;
        ShowFormalMessageFormMode show_formal_message_form_mode;
        RemoveShowOfflineMessagesForm remove_form_from_list;

        private void BaseConstruct(OfflienMesssageRead p_offlien_messages_read_inform, RemoveShowOfflineMessagesForm p_remove_form_from_list)
        {
            InitializeComponent();
            offlien_messages_read_inform = p_offlien_messages_read_inform;
            remove_form_from_list = p_remove_form_from_list;
            flag = false;
        }

        public ShowFormalMessage_Form(List<OfflineMessage> p_all_offline_messages, OfflienMesssageRead p_offlien_messages_read_inform
            , RemoveShowOfflineMessagesForm p_remove_form_from_list)
        {
            BaseConstruct(p_offlien_messages_read_inform, p_remove_form_from_list);
            all_offline_messages = p_all_offline_messages;
            show_formal_message_form_mode = ShowFormalMessageFormMode.ShowOfflineMessages;
            ShowMessages();
            this.Visible = false;
            this.Hide();
        }

        public ShowFormalMessage_Form(FormalMessage p_formal_message, int p_formal_message_id, OfflienMesssageRead p_offlien_messages_read_inform
            , RemoveShowOfflineMessagesForm p_remove_form_from_list)
        {
            BaseConstruct(p_offlien_messages_read_inform, p_remove_form_from_list);
            formal_message = p_formal_message;
            formal_message_id = p_formal_message_id;
            show_formal_message_form_mode = ShowFormalMessageFormMode.ShowAFormalMessage;
            ShowMessages();
            this.Visible = false;
            this.Hide();
        }

        public void StartForm()
        {
            this.Visible = true;
            this.Show();
        }
        public void DontStartForm()
        {
            this.Close();
        }


        private void ShowMessages()
        {
            if (show_formal_message_form_mode == ShowFormalMessageFormMode.ShowAFormalMessage)
            {
                ListViewGroup Messages_ListView_group = Messages_ListView.Groups["formal_message"];
                Messages_ListView.Items.Clear();
                Messages_ListView.Items.Add(formal_message.Get_sender_user_id + ": " + formal_message.Get_message_text);
                Messages_ListView.Items[0].Group = Messages_ListView_group;
            }

            else if (show_formal_message_form_mode == ShowFormalMessageFormMode.ShowOfflineMessages)
            {
                ListViewGroup Messages_ListView_group = Messages_ListView.Groups["offline_messages"];
                Messages_ListView.Items.Clear();
                for (int i = 0; i < all_offline_messages.Count; i++)
                {
                    Messages_ListView.Items.Add(all_offline_messages[i].Get_sender_user_id + ": " + all_offline_messages[i].Get_message_text);
                    Messages_ListView.Items[i].Group = Messages_ListView_group;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            flag = true;
            if (show_formal_message_form_mode == ShowFormalMessageFormMode.ShowAFormalMessage)
            {
                List<int> messages_id = new List<int>();
                messages_id.Add(formal_message_id);
                offlien_messages_read_inform(messages_id);
                this.Close();
                remove_form_from_list(this);
            }

            else if (show_formal_message_form_mode == ShowFormalMessageFormMode.ShowOfflineMessages)
            {
                List<int> messages_id = new List<int>();
                foreach (OfflineMessage t_message in all_offline_messages)
                {
                    messages_id.Add(t_message.Get_message_id);
                }
                offlien_messages_read_inform(messages_id);
                this.Close();
                remove_form_from_list(this);
            }

        }

        public override bool Equals(object obj)
        {
            ShowFormalMessage_Form t_form = null;
            try
            {
                t_form = (ShowFormalMessage_Form)obj;
            }
            catch (Exception)
            {
                return false;
            }

            if (t_form.show_formal_message_form_mode == show_formal_message_form_mode)
            {
                if (show_formal_message_form_mode == ShowFormalMessageFormMode.ShowAFormalMessage)
                {
                    if (t_form.formal_message == formal_message && t_form.formal_message_id == formal_message_id)
                        return true;
                }
                else if (show_formal_message_form_mode == ShowFormalMessageFormMode.ShowOfflineMessages)
                {
                    if (t_form.all_offline_messages.Count == all_offline_messages.Count)
                    {
                        foreach (OfflineMessage t_offline_message in t_form.all_offline_messages)
                        {
                            if (!all_offline_messages.Contains(t_offline_message))
                                return false;
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        private void ShowFormalMessage_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag == false)
            {
                remove_form_from_list(this);
            }
        }
    }

    public enum ShowFormalMessageFormMode
    {
        ShowAFormalMessage,
        ShowOfflineMessages,
    }


}

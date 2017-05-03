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

namespace ClientTcpChat.Forms
{
    public partial class SelectPublicChat : Form
    {
        StartClientJoinPublicChatRequestDialog start_client_join_public_chat_request_dialog;
        SelectPublicChatFormCloesd select_public_chat_form_closed;
        List<int> public_chats_ids;
        Dictionary<string, int> all_public_chats_and_names;
        bool flag;
        public SelectPublicChat(List<int> p_public_chats_ids, StartClientJoinPublicChatRequestDialog p_start_client_join_public_chat_request_dialog
            , SelectPublicChatFormCloesd p_select_public_chat_form_closed)
        {
            InitializeComponent();

            flag = false;
            all_public_chats_and_names = new Dictionary<string, int>();
            start_client_join_public_chat_request_dialog = p_start_client_join_public_chat_request_dialog;
            select_public_chat_form_closed = p_select_public_chat_form_closed;
            public_chats_ids = p_public_chats_ids;

            PublicChatList_dataGridView.Rows.Clear();
            for (int i = 0; i < public_chats_ids.Count; i++)
            {
                all_public_chats_and_names.Add("PublicChat" + (i+1).ToString(), public_chats_ids[i]);
                PublicChatList_dataGridView.Rows.Add();
                PublicChatList_dataGridView.Rows[i].Cells[0].Value = all_public_chats_and_names.ElementAt(i).Key;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selected_chat_id = 0;
            string selected_chat_string = PublicChatList_dataGridView.SelectedRows[0].Cells[0].Value.ToString();
            if (all_public_chats_and_names.ContainsKey(selected_chat_string))
            {
                selected_chat_id = all_public_chats_and_names[selected_chat_string];
            }
            else
            {
                MessageBox.Show("error occured");
                flag = true;
                select_public_chat_form_closed();
                this.Close();
            }
            start_client_join_public_chat_request_dialog(selected_chat_id);
            flag = true;
            select_public_chat_form_closed();
            this.Close();
        }

        private void SelectPublicChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag == false)
            {
                select_public_chat_form_closed();
            }
        }
    }
}

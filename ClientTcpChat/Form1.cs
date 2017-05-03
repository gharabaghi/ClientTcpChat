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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;
using CommonChatTypes;
using System.Windows;

namespace ClientTcpChat
{
    public delegate void RemoveShowOfflineMessagesForm(ShowFormalMessage_Form p_show_offline_message_form);
    public delegate void RemoveShowAgreementInvitationForm(AgreementInvitation_Form p_form_to_remove);
    public delegate void SelectPublicChatFormCloesd();
    public delegate void CreateGetAgreementAnswerDialog(int p_agreement_id, TypeOfAgreement p_agreement_type, bool p_answer);



    public partial class Form1 : Form
    {
        Thread client_worker_thread;
        SendToWorkerConstruct send_to_worker_construct;
        ReceiveFromWorkerConstruct receive_from_worker_construct;
        object client_worker_pulse_object;
        ClientWorkerCancelConstruct client_worker_cancel_construct;
        bool is_there_worker;       //این متغیر باید قبل از شروع و پایان ورکز در هر جا و به هر طریق مقداردهی شود

        TypeOfMode mode;
        LoginData login_data;

        System.Net.IPEndPoint server_udp_ip_endpoint;       //مقداردهی با سازنده
        int server_check_data;
        IPAddress server_ip_address;

        IPEndPoint server_tcp_ip_endpoint;

        int tcp_alloction_mode_retry_counts;

        public Form1()
        {
            InitializeComponent();
            tcp_alloction_mode_retry_counts = 0;
            is_there_worker = false;
            //udp_mode_tcp_port_allocation_retry_counts = 0;
        }

        public void ConstructThreadData()
        {
            send_to_worker_construct = new SendToWorkerConstruct(new Queue<MessageToWorker>());//, new object());
            receive_from_worker_construct = new ReceiveFromWorkerConstruct(new Queue<MessageFromWorker>(), new object());
            client_worker_pulse_object = new object();
            client_worker_cancel_construct = new ClientWorkerCancelConstruct();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConfigs();

            //this.IsMdiContainer = true;


            DisconnectedConnect_Button.Parent = DisConnectedMode_Panel;

            StartChat_Button.Parent = AutheMode_Panel;
            FriendList_ListView.Parent = AutheMode_Panel;
            OwnUserName_Label.Parent = AutheMode_Panel;

            UdpTcpStatus_Label.Parent = TcpUdpMode_Panel;

            LoginForm_Label.Parent = UnAutheMode_Panel;
            LoginFormUserName_Label.Parent = UnAutheMode_Panel;
            LoginFormPassword_Label.Parent = UnAutheMode_Panel;
            LoginUserName_TextBox.Parent = UnAutheMode_Panel;
            LoginPassword_TextBox.Parent = UnAutheMode_Panel;
            Login_Button.Parent = UnAutheMode_Panel;
            LoginFormError_Label.Parent = UnAutheMode_Panel;

            //DisConnectedMode_Panel.Enabled = false;
            //AutheMode_Panel.Enabled = false;
            //UnAutheMode_Panel.Enabled = false;
            //TcpUdpMode_Panel.Enabled = false;

            //menuStrip1.Visible = false;

            menuStrip1.Hide();
            //DisconnectedConnect_Button.Parent = DisConnectedMode_Panel;

            //DisConnectedMode_Panel.Enabled = false;
            DisConnectedMode_Panel.Visible = false;
            //DisConnectedMode_Panel.Select();
            //DisConnectedMode_Panel.Hide();
            //AutheMode_Panel.Select();
            //AutheMode_Panel.Enabled = false;
            AutheMode_Panel.Visible = false;
            //AutheMode_Panel.Hide();
            //UnAutheMode_Panel.Select();
            //UnAutheMode_Panel.Enabled = false;
            UnAutheMode_Panel.Visible = false;
            //UnAutheMode_Panel.Hide();
            //TcpUdpMode_Panel.Select();
            //TcpUdpMode_Panel.Enabled = false;
            TcpUdpMode_Panel.Visible = false;
            //TcpUdpMode_Panel.Hide();


            this.Refresh();
            this.Update();

            mode = TypeOfMode.Dissconnected;
            DisconnectedModeStart();

            LoginPassword_TextBox.Enter += LoginPassword_TextBox_Enter;
            LoginUserName_TextBox.Enter += LoginUserName_TextBox_Enter;
            LoginUserName_TextBox.TabIndex = 0;
            LoginPassword_TextBox.TabIndex = 1;
            Login_Button.TabIndex = 2;

            //TestMethde();
        }

        List<DialogMessageForServer> messages_sent_from_dialog;
        int last_message_number;
        int auth_send_calls_counts;
        int remove_calls_counts;
        int dialog_id;
        int last_message_recv_id;
        public void TestMethde()
        {
            last_message_number = 0;
            dialog_id = 0;
            auth_send_calls_counts = 0;
            remove_calls_counts = 0;
            last_message_recv_id = 0;
            messages_sent_from_dialog = new List<DialogMessageForServer>();

            int random_dialog_id = HelperFunctions.GetGUID();
            dialog_id = random_dialog_id;

            Di_Client_CreateAddAgreement test_dialog = new Di_Client_CreateAddAgreement(random_dialog_id, new Remove(Remove), new SendDialogMessage(AuthSend));
            test_dialog.Add_Button.Click += new EventHandler((sender, e) => Add_Button_Click(sender, e, test_dialog));
            test_dialog.StartDialog();
        }
        void Add_Button_Click(object sender, EventArgs e, Di_Client_CreateAddAgreement p_test_dialog)
        {
            int last_message_received_from_dialog_id = messages_sent_from_dialog[0].Get_message_id;

            DialogMessageForClient temp_dialog_message = null;

            last_message_number++;

            Di_Mess_ReceiptMessage rec_message = new Di_Mess_ReceiptMessage(ReceiptStatus.Rejected, new Di_Mess_Rec_RejectMessage(last_message_recv_id, "he he"));
            temp_dialog_message = new DialogMessageForClient(HelperFunctions.GetGUID(), dialog_id, last_message_number, TypeOfDialog.CreateAddAgreement
                , rec_message, TypeOfDialogMessage.ReceiptMessage);

            p_test_dialog.ReceiveMessage(temp_dialog_message);



        }
        public void Remove(int dialog_id)
        {
            remove_calls_counts++;
        }
        public void AuthSend(DialogMessageForServer message)
        {
            messages_sent_from_dialog.Add(message);
            last_message_recv_id = message.Get_message_id;
            last_message_number++;
            auth_send_calls_counts++;
        }


        private void LoadConfigs()
        {
            System.Configuration.Configuration AppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            try
            {
                string server_ip_string = AppConfig.AppSettings.Settings["ServerIP"].Value;
                string server_udp_port_string = AppConfig.AppSettings.Settings["ServerUdpPort"].Value;
                string udp_check_data_string = AppConfig.AppSettings.Settings["UdpCheckData"].Value;

                server_ip_address = IPAddress.Parse(server_ip_string);
                int server_udp_port_number = Convert.ToInt32(server_udp_port_string);
                int server_udp_check_data = Convert.ToInt32(udp_check_data_string);
                server_udp_ip_endpoint = new IPEndPoint(server_ip_address, server_udp_port_number);
                server_check_data = server_udp_check_data;
            }
            catch (Exception)
            {
                MessageBox.Show("erro in loading config settings.");
                Application.Exit();
                return;
            }
        }

        public void Mo_MessageShow(string p_message)
        {
            MessageBox.Show(p_message);
        }

        public void ChangeMode(TypeOfMode p_mode)
        {
            if (mode == TypeOfMode.AuthMode)
            {
                if (p_mode == TypeOfMode.Dissconnected)
                {
                    AuthModeEnd();
                    mode = TypeOfMode.Dissconnected;
                    DisconnectedModeStart();
                    return;
                }
                else if (p_mode == TypeOfMode.UnAuthMode)
                {
                    AuthModeEnd();
                    mode = TypeOfMode.UnAuthMode;
                    UnAuthModeStart();
                    return;
                }
            }
            else if (mode == TypeOfMode.UnAuthMode)
            {
                if (p_mode == TypeOfMode.AuthMode)
                {
                    try
                    {
                        string test_user_name = login_data.Get_user_name;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Problem with login data received from server.");
                        ChangeMode(TypeOfMode.Dissconnected);
                        return;
                    }
                    UnAuthModeEnd();
                    mode = TypeOfMode.AuthMode;
                    AuthModeStart(login_data);
                    return;

                }
                else if (p_mode == TypeOfMode.Dissconnected)
                {
                    UnAuthModeEnd();
                    mode = TypeOfMode.Dissconnected;
                    DisconnectedModeStart();
                    return;
                }
            }
            else if (mode == TypeOfMode.Dissconnected)
            {
                if (p_mode == TypeOfMode.UdpConnection)
                {
                    DisConnectedModeEnd();
                    mode = TypeOfMode.UdpConnection;
                    UdpModeStart();
                    return;
                }
            }
            else if (mode == TypeOfMode.TcpAllocation)
            {
                if (p_mode == TypeOfMode.Dissconnected)
                {
                    TcpModeEnd();
                    mode = TypeOfMode.Dissconnected;
                    DisconnectedModeStart();
                    return;
                }
                else if (p_mode == TypeOfMode.UnAuthMode)
                {
                    TcpModeEnd();
                    mode = TypeOfMode.UnAuthMode;
                    UnAuthModeStart();
                    return;
                }
                else if (p_mode == TypeOfMode.UdpConnection)
                {
                    TcpModeEnd();
                    mode = TypeOfMode.UdpConnection;
                    UdpModeStart();
                    return;
                }
            }
            else if (mode == TypeOfMode.UdpConnection)
            {
                if (p_mode == TypeOfMode.Dissconnected)
                {
                    UdpModeEnd();
                    mode = TypeOfMode.Dissconnected;
                    DisconnectedModeStart();
                    return;
                }
                else if (p_mode == TypeOfMode.TcpAllocation)
                {
                    UdpModeEnd();
                    mode = TypeOfMode.TcpAllocation;
                    TcpModeStart();
                    return;
                }
            }
        }

        Socket udp_socket;
        byte[] check_data_buffer;

        public void UdpModeStart()
        {
            TcpUdpMode_Panel.Visible = true;

            UdpTcpStatus_Label.Text = "Starting";
            server_tcp_ip_endpoint = new IPEndPoint(server_ip_address, 9999); 
            udp_socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            udp_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2000); 
            check_data_buffer = new byte[32];
            check_data_buffer = BitConverter.GetBytes(server_check_data);
            EndPoint temp_server_endpoint = (EndPoint)server_udp_ip_endpoint;
            int receive_buffer_size = 32;
            byte[] receive_buffer = new byte[receive_buffer_size];
            int j = 0;
            int i = 0;
            int k = 0;
            UdpTcpStatus_Label.Text = "Start Udp Communicate With Server";
            while (i < 4 && k < 4)
            {

                receive_buffer = new byte[receive_buffer_size];
                while (j < 4)
                {
                    try
                    {
                        udp_socket.SendTo(check_data_buffer, server_udp_ip_endpoint);
                        j = 0;
                        break;
                    }
                    catch (SocketException)
                    {
                        j++;
                        continue;
                    }
                }
                if (j > 3)
                {
                    MessageBox.Show("error in Udp communicating with server.");
                    UdpTcpStatus_Label.Text = "Error";
                    ChangeMode(TypeOfMode.Dissconnected);
                    return;
                }

                try
                {
                    udp_socket.ReceiveFrom(receive_buffer, ref temp_server_endpoint);
                    i = 0;
                    k = 0;
                    break;
                }
                catch (SocketException exception)
                {
                    if (exception.ErrorCode == 10054)
                    {
                        i++;
                        continue;
                    }
                    if (exception.ErrorCode == 10040) 
                    {
                        receive_buffer_size = receive_buffer_size * 2; 
                        i = 0;
                        continue;
                    }
                    else
                    {
                        k++;
                        //i = 0;
                        continue;
                    }
                }
            }
            if (k > 3)
            {
                MessageBox.Show("error occured in receiving Udp data from server");
                UdpTcpStatus_Label.Text = "Error";
                ChangeMode(TypeOfMode.Dissconnected);
                return;
            }
            if (i > 3)
            {
                MessageBox.Show("server didnt respond in Udp Communication");
                UdpTcpStatus_Label.Text = "Error";
                ChangeMode(TypeOfMode.Dissconnected);
                return;
            }

            int tcp_port = 0;
            try
            {
                tcp_port = BitConverter.ToInt32(receive_buffer, 0);
            }
            catch
            {
                MessageBox.Show("incorrect data received");
                UdpTcpStatus_Label.Text = "Error";
                ChangeMode(TypeOfMode.Dissconnected);
                return;
            }
            
            server_tcp_ip_endpoint.Port = tcp_port;
            UdpTcpStatus_Label.Text = "Starting Tcp Communication";
            ChangeMode(TypeOfMode.TcpAllocation);
            return;
        }

        public void UdpModeEnd()
        {
            UdpTcpStatus_Label.Text = "";
            TcpUdpMode_Panel.Visible = false;
            check_data_buffer = new byte[32];
            ConstructThreadData();
        }

        public void TcpModeStart()
        {

            tcp_alloction_mode_retry_counts++; 
            if (tcp_alloction_mode_retry_counts > 3)
            {
                MessageBox.Show("tcp alloctaed prots didnt work.");
                ChangeMode(TypeOfMode.Dissconnected);
                return;
            }

            TcpUdpMode_Panel.Visible = true;
            UdpTcpStatus_Label.Text = "Connecting To Allocated Server Port";

            ClientWorkerData client_worker_data = new ClientWorkerData(new MessageReceivedInform(MessageReceivedInform), new MainFormInvoke(this.Invoke)
            , send_to_worker_construct, receive_from_worker_construct, client_worker_pulse_object, client_worker_cancel_construct);

            client_worker_thread = new Thread(() => ClientWorker.WorkerMainThread(client_worker_data, server_tcp_ip_endpoint));
            is_there_worker = true;
            client_worker_thread.Start();
        }
        public void TcpModeEnd()
        {
           
            UdpTcpStatus_Label.Text = "";
            TcpUdpMode_Panel.Visible = false;

        }

        public void TcpModeReceive(ClientWorkerSignal p_signal_message)
        {
            if (p_signal_message.Get_signal == true)
            {
                tcp_alloction_mode_retry_counts = 0;
                is_there_worker = true;
                ChangeMode(TypeOfMode.UnAuthMode);
                return;
            }
            else if (p_signal_message.Get_signal == false)
            {
                is_there_worker = false;   
                ChangeMode(TypeOfMode.UdpConnection);
                return;
            }
        }

        private void DisconnectedConnect_Button_Click(object sender, EventArgs e)
        {
            WaitUnwaitDisconnectedMode(false);
            ChangeMode(TypeOfMode.UdpConnection);
            return;
        }
        private void DisconnectedModeStart()
        {
            if (is_there_worker)
            {
                CancelWorker();
            }

            WaitUnwaitDisconnectedMode(true);
            DisConnectedMode_Panel.Visible = true;
        }
        private void DisConnectedModeEnd()
        {
            DisConnectedMode_Panel.Visible = false;
        }
        private void WaitUnwaitDisconnectedMode(bool p_flag)
        {
            DisconnectedConnect_Button.Enabled = p_flag;
        }
        //
        public void MessageReceivedInform()
        {
            bool flag = false;
            MessageFromWorker received_message = null;
            lock (receive_from_worker_construct.receive_from_worker_queue_lock) 
            {
                if (receive_from_worker_construct.receive_from_worker_queue.Count > 0)
                {
                    received_message = receive_from_worker_construct.receive_from_worker_queue.Dequeue();
                    flag = true;
                    if (receive_from_worker_construct.receive_from_worker_queue.Count == 0)
                    {
                        receive_from_worker_construct.receive_from_worker_queue_flag = false;
                    }
                }
                else
                {
                    receive_from_worker_construct.receive_from_worker_queue_flag = false;
                }
            }
            if (flag == true)
            {
                bool message_investigate = HelperFunctions.MessageFromWorkerInvestigate(received_message);
                if (message_investigate == false)
                {
                    MessageBox.Show("Invalid Message Object Rejected.");  
                    return;
                }
                else if (message_investigate == true)
                {
                    if (received_message.Get_type_of_message_from_worker_object == TypeOfMessageFromWorker.SignalMessage && mode == TypeOfMode.TcpAllocation)
                    {
                        TcpModeReceive(((ClientWorkerSignal)received_message.Get_message_from_worker_object));
                        return;
                    }
                    else if (received_message.Get_type_of_message_from_worker_object == TypeOfMessageFromWorker.FinalMessage)
                    {
                        FinalMessageForClient final_message_received = (FinalMessageForClient)received_message.Get_message_from_worker_object;
                        if (mode == TypeOfMode.UnAuthMode && final_message_received.Get_message_type == TypeOfMessage.Dialog)
                        {
                            UnAuthModeReceive((DialogMessageForClient)final_message_received.Get_message_object);
                            return;
                        }
                        else if (mode == TypeOfMode.AuthMode)
                        {
                            AuthModeReceive(final_message_received);
                            return;
                        }
                    }
                    else if (received_message.Get_type_of_message_from_worker_object == TypeOfMessageFromWorker.OfflineInform)
                    {
                        is_there_worker = false;
                        OfflineApplication();
                        return;
                    }

                }
                else
                {
                    return;
                }
            }
            return;
        }

        public void SendMessagesToServer(FinalMessageForServer p_message)
        {
            lock (client_worker_pulse_object)  
            {
                send_to_worker_construct.send_to_worker_queue.Enqueue(new MessageToWorker(p_message, TypeOfMessageToWorker.FinalMessage));
                send_to_worker_construct.send_to_worker_queue_flag = true;
                Monitor.Pulse(client_worker_pulse_object);
                return;
            }
        }

        private void OfflineApplication() 
        {
            ChangeMode(TypeOfMode.Dissconnected);
            return;
        }

        private void CancelWorker()
        {
            if (is_there_worker)
            {
                is_there_worker = false;
                lock (client_worker_pulse_object)
                {
                    client_worker_cancel_construct.cancel_construct_flag = true;
                    Monitor.Pulse(client_worker_pulse_object);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mode == TypeOfMode.AuthMode || mode == TypeOfMode.UnAuthMode || mode == TypeOfMode.TcpAllocation)
            {
                if (is_there_worker)
                {
                    is_there_worker = false;
                    lock (client_worker_pulse_object)
                    {
                        client_worker_cancel_construct.cancel_construct_flag = true;
                        Monitor.Pulse(client_worker_pulse_object);
                    }
                    client_worker_thread.Join(); 
                }
            }
        }


    }

    public class LoginData
    {
        string user_name;
        public string Get_user_name
        {
            get { return user_name; }
        }

        List<PersonStatus> friends_list_and_status;
        public List<PersonStatus> Get_friends_list_and_status
        {
            get { return friends_list_and_status; }
        }

        List<int> public_chat_ids;
        public List<int> Get_public_chat_ids
        {
            get { return public_chat_ids; }
        }

        List<AgreementInvitationInfo> agreement_invitations;
        public List<AgreementInvitationInfo> Get_agreement_invitations
        {
            get { return agreement_invitations; }
        }

        List<OfflineMessage> offline_messages;
        public List<OfflineMessage> Get_offline_messages
        {
            get { return offline_messages; }
        }

        public LoginData(string p_user_name, List<PersonStatus> p_friends_list_and_status, List<int> p_public_chat_ids
                            , List<AgreementInvitationInfo> p_agreement_invitations, List<OfflineMessage> p_offline_messages)
        {
            user_name = p_user_name;
            friends_list_and_status = p_friends_list_and_status;
            public_chat_ids = p_public_chat_ids;
            agreement_invitations = p_agreement_invitations;
            offline_messages = p_offline_messages;
        }

    }
}

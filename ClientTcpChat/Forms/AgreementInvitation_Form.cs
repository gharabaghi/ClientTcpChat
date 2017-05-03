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
    public partial class AgreementInvitation_Form : Form
    {
        AgreementInvitationInfo agreement_invitaion;
        CreateGetAgreementAnswerDialog create_get_agreement_answer_dialog;
        RemoveShowAgreementInvitationForm remove_show_agreement_invitation_form;
        bool flag = false;
        public AgreementInvitation_Form(AgreementInvitationInfo p_agreement_invitaion, RemoveShowAgreementInvitationForm p_remove_show_agreement_invitation_form
            , CreateGetAgreementAnswerDialog p_create_get_agreement_answer_dialog)
        {
            InitializeComponent();
            agreement_invitaion = p_agreement_invitaion;
            remove_show_agreement_invitation_form = p_remove_show_agreement_invitation_form;
            create_get_agreement_answer_dialog = p_create_get_agreement_answer_dialog;

            AgreementInviation_RichTextBox.AppendText("An Inviiation sent from: ");
            AgreementInviation_RichTextBox.SelectionColor = Color.Red;
            AgreementInviation_RichTextBox.AppendText(agreement_invitaion.Get_inviting_user_id);
            AgreementInviation_RichTextBox.SelectionColor = Color.Black;
            AgreementInviation_RichTextBox.AppendText(": " + "\n");
            AgreementInviation_RichTextBox.AppendText("Invitation Text:");
            AgreementInviation_RichTextBox.SelectionColor = Color.Blue;
            AgreementInviation_RichTextBox.AppendText(agreement_invitaion.Get_agreement_text);
            AgreementInviation_RichTextBox.SelectionColor = Color.Black;
            AgreementInviation_RichTextBox.AppendText("\n");
            AgreementInviation_RichTextBox.AppendText("Do you accept this invitation?");
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

        public override bool Equals(object obj)
        {
            AgreementInvitation_Form t_form = null;
            try
            {
                t_form = (AgreementInvitation_Form)obj;
            }
            catch (Exception)
            {
                return false;
            }
            if (agreement_invitaion == t_form.agreement_invitaion)
            {
                return true;
            }
            return false;
        }

        private void AgreementInvitation_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag == false)
            {
                remove_show_agreement_invitation_form(this);
            }
        }

        private void No_Button_Click(object sender, EventArgs e)
        {
            flag = true;
            create_get_agreement_answer_dialog(agreement_invitaion.Get_agreement_id, agreement_invitaion.Get_agreement_type, false);
            this.Close();
            remove_show_agreement_invitation_form(this);
        }

        private void Yes_Button_Click(object sender, EventArgs e)
        {
            flag = true;
            create_get_agreement_answer_dialog(agreement_invitaion.Get_agreement_id, agreement_invitaion.Get_agreement_type, true);
            this.Close();
            remove_show_agreement_invitation_form(this);
        }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace EncryptionProgram
{
    public partial class EmailWindow : Form
    {
        //This class is created as the emailwindow form and it takes in a form1 type so that when it closes it can go back to the previous form.

        private static LoginWindow log = new LoginWindow();
        private Form1 Main_page = new Form1(log);
        public EmailWindow(Form1 prev)
        {
            //This is emailwindow constructor.

            InitializeComponent();
            Main_page = prev;
        }

        //Declearation of Variables
        String[] attachmentPath = new String[5];
        int count = 0, len = 5;

        private void button1_Click(object sender, EventArgs e)
        {
            //The Send button is used to actually send the email. It reads ithe text from the from textbox and determines which email domain is required.
            //Then the smtp severs are set the message is createdby reading the other fields attachments are added and the email is sent. The code for gmail
            //is there but it seems to crashed all the time to it is commented out.

            String smtphost = "";

            if (From.Text.Contains("hotmail.com") || From.Text.Contains("outlook.com") || From.Text.Contains("msn.com"))
            {
                smtphost = "smtp.live.com";
            }
            if (From.Text.Contains("yahoo.com"))
            {
                smtphost = "smtp.mail.yahoo.com";
            }
            if (From.Text.Contains("office365.com"))
            {
                smtphost = "smtp.office365.com";
            }
            if (From.Text.Contains("aol.com"))
            {
                smtphost = "smtp.aol.com";
            }
            if (From.Text.Contains("@mail.com"))
            {
                smtphost = "smtp.mail.com";
            }
            //if(From.Text.Contains("gmail.com"))
            //{
            //    smtphost = "smtp.gmail.com";
            //}
            if (smtphost != "")
            {
                try
                {
                    int port = 587;
                    SmtpClient smtp = new SmtpClient(smtphost, port);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(From.Text, Pass.Text);
                    smtp.EnableSsl = true;
                    smtp.Timeout = 20000;

                    MailAddress frm = new MailAddress(From.Text);
                    MailAddress too = new MailAddress(To.Text);
                    MailMessage msg = new MailMessage(frm, too);
                    msg.Subject = Subject.Text;
                    msg.Body = Body.Text;

                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Attachment attach = new Attachment(attachmentPath[i]);
                            msg.Attachments.Add(attach);
                        }
                    }

                    smtp.Send(msg);
                    MessageBox.Show("Your message has been sent.");
                    Main_page.Show();
                    this.Close();
                }
                catch (Exception s)
                {
                    MessageBox.Show("Message failed to send!\nTry again");
                }
            }
            else
            {
                MessageBox.Show("This program does not support your email domian.");
            }
        }
       

        private void button3_Click(object sender, EventArgs e)
        {
            //The back button is used to close the email window form and go back to the previous encryption form.

            Main_page.Show();
            this.Hide();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //The attach button is used to attach files to the email. It allows for people to add a maximum of 5 attachments. 
            //It opens a openfiledialog and selects the filepath form that and stores it in a string array that would later 
            //be used to covert into attachments on the email message itself.

            if (len != 0)
            {
                MessageBox.Show("You can add " + len + " more attachments.");
                OpenFileDialog openFile = new OpenFileDialog();
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    attachmentPath[count] = openFile.FileName;
                    count++;
                    len--;
                }
            }
            else
            {
                MessageBox.Show("You can't add any more attachments.");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            //This makes sure that the x button closes the entire program.

            base.OnClosed(e);
            Application.Exit();
        }
    }
}

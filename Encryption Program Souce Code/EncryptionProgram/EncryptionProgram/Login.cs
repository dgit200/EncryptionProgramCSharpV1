using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptionProgram
{
    public partial class LoginWindow : Form
    {
        //This creates a login window class that is used to validate whether you can access the program or not.

        public LoginWindow()
        {
            //This is the login window construtor.

            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //The exit button is used to close the program, is creates a messagebox that says bye and then close the window.

            MessageBox.Show("Okay GoodBye!");
            this.Close();
        }

        String User = "Dwight";
        String Pass = "Thomas";

        private void button1_Click(object sender, EventArgs e)
        {
            //The login button is used to login in and create a form1 that passes this login window in the form1 constructor so 
            //That when the user signs out it would bring them  right back to this login window. It also checks the login credentials to ensure they
            //are ccorrect before giving you access to the program if your credentials are wrong a messagebox enforms you of this.

            if(textBox1.Text == User && textBox2.Text == Pass)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                this.Hide();
                Form1 form = new Form1(this);
                form.Show();
            }
            else
            {
                MessageBox.Show("You have entered the wrong Username or Password\nPlease Try Again.");
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

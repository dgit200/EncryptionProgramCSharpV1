using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace EncryptionProgram
{
    public partial class Form1 : Form
    {
        //This create the form1 class, which was the first form created for this project, hence its lame/simple name.
        //This form passes in a loginwindow form so that when you are finished you can sign out and go back to the login window.

        private LoginWindow login = new LoginWindow();

        public Form1(LoginWindow log)
        {
            //This is form1 constructor.

            InitializeComponent();
            login = log;
        }

        //Declaration of Variables
        List<int> encryptKey;
        int len;
        bool encrytmode = false;
        String keyString = "";
        

        private void button2_Click(object sender, EventArgs e)
        {
            //This is the encrypt button, it is responsible for encrypting the text in the text box.
            //It encrypts the texts by offseting each character by a random value and these values are stored in the encryption key.
            //It also ensure that there is text to encrypt before it tries to encrypt anything and when finish it sets encryptmode to true
            //So that the decrypt button will work.

            if (textBox1.Text != "")
            {
                if (!encrytmode)
                {
                    String msg = textBox1.Text;
                    StringBuilder tempString = new StringBuilder(msg);
                    len = msg.Length;
                    encryptKey = new List<int>(len);

                    Random ranGen = new Random();
                    for (int i = 0; i < len; i++)
                    {
                        int encrypt_factor = ranGen.Next(21);
                        tempString[i] = (char)(tempString[i] + encrypt_factor);
                        encryptKey.Add(encrypt_factor);
                    }
                    msg = tempString.ToString();
                    textBox1.Text = msg;
                    encrytmode = true;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //TThe decryption button is responible for decrypting the text, by using the encryptionkey and resseting all the charaters
            //to their original forms. It also sets encryptmode to false so that if anyone tries to press it again notting will change.
            //It all checks at the beginning to ensure that their is text to decrypt.
             
            if (textBox1.Text != "" && encrytmode)
            {
                String msg = textBox1.Text;
                StringBuilder tempString = new StringBuilder(msg);

                for (int i = 0; i < len; i++)
                {
                    tempString[i] = (char)(tempString[i] - encryptKey.ElementAt(i));
                }
                msg = tempString.ToString();
                textBox1.Text = msg;
                encrytmode = false;
                encryptKey.Clear();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            //The clear button, clears all text. It is their incase you need ti=o clear all your data quickly.

            textBox1.Text = "";
            encrytmode = false;
            encryptKey.Clear();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            //The sign out button is used to close this form and open back the login form that was passed at constrution, which would be
            //the login form we just cam from. So it effectively signs you out and closes this window.

            login.Show();
            this.Close();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            //The save button is used to save encrypted text to textfiles for future use or to be transferred via email or some other form.
            //It checks to ensure that the text that you are trying to save is encrypted first then it saves the text along with the encryption key
            //It all creates some random decouy text and two decouy encryption keys to put off anybody of opens the encryted file.
            //It is also set to only allow you to save .txt files as this version of the program only works with those file types

            if (encrytmode)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "Text File | *.txt";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter write = new StreamWriter(File.Create(saveFile.FileName));
                    String decouyKey = "", decouy = "This is a decouy line you thought you could figure out this encryption!!";
                    StringBuilder tempdecoy = new StringBuilder(decouy);
                    Random ranGen = new Random();
                    int count = encryptKey.Count;
                    foreach (int num in encryptKey)
                    {
                        keyString += num.ToString();
                        decouyKey += ranGen.Next(101);
                        if (--count > 0)
                        {
                            keyString += ',';
                            decouyKey += ',';
                        }
                    }
                    for (int i = 0; i < decouy.Length; i++)
                    {
                        tempdecoy[i] = (char)(tempdecoy[i] + ranGen.Next(101));
                    }
                    decouy = tempdecoy.ToString();

                    write.WriteLine("0");
                    write.WriteLine(decouy);
                    write.WriteLine(decouyKey);
                    write.WriteLine(textBox1.Text);
                    write.WriteLine(keyString);
                    write.WriteLine(decouyKey);
                    write.WriteLine(decouy);

                    keyString = "";
                    write.Dispose();
                }
            }
            else
            {
                MessageBox.Show("The file you are trying to save is not encrypted, please encrypt first.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        { 
            //The open file button is used to open encrypted files. It checks for a special marker that is put in when saving and encrypted file
            //This is so that is doesn't open a file that was not encrypted but this program. When the file is read it disposed of the decout text.
            //Then it sets the key back to key and the encrypted text is set to the textbox and encrypt mode is turned on so that it can be decrypted.

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Text files only |*.txt";
            openFile.InitialDirectory = "C:\\Program Files (x86)\\Thomas Inc\\EncryptionProgramSetup\\Text";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                StreamReader read = new StreamReader(File.OpenRead(openFile.FileName));
                String random = "";

                random = read.ReadLine();
                if (random != "0")
                {
                    MessageBox.Show("This file can not be opened becasue it was not encrypted by this program!!");
                }
                else
                {
                    random = read.ReadLine();
                    random = read.ReadLine();
                    textBox1.Text = read.ReadLine();
                    keyString = read.ReadLine();
                    random = read.ReadLine();
                    random = read.ReadLine();

                    encrytmode = true;
                    len = textBox1.Text.Length;
                    encryptKey = new List<int>(Array.ConvertAll(keyString.Split(','), int.Parse));
                    keyString = "";
                }
                read.Dispose();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //The send email button is used to open a new window where you can send an email from. It passes this form into the
            //construtor of the emailwindow so that when that is finish and is close with will open back up this form.

            EmailWindow email = new EmailWindow(this);
            this.Hide();
            email.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            //This makes sure that the x button closes the entire program.

            base.OnClosed(e);
            Application.Exit();
        }
    }
}

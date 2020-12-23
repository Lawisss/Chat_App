using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Client
{
    public partial class RegistrationForm : Form
    {
        List<string> login = new List<string>(); // Get list of all username in txt
        List<string> passwords = new List<string>(); // Get list of all password in txt
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

                string filepath = "connexion.txt";
                string username = textBox1.Text;
                string password = textBox2.Text;

                if (login.Contains(username) && passwords.Contains(password)) // Login and password checking (if they're already exist)
            {
                    MessageBox.Show("Login and password exist");
                }
                else if (username == "" && password == "")
                {
                    MessageBox.Show("Insert username and password !");
                }
                else
                {
                    backupadd(username, password, filepath);
                }
                
            
        }

        private void Register_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("connexion.txt"); // Read txt to analyse and check data inside
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] components = line.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries); // Login and Password retrieve

                login.Add(components[0]); // Login distribution
                passwords.Add(components[1]); // Password distribution
            }
        }

        // Use StreamWriter to insert in txt login and password
        public static void backupadd(string username, string password, string filepath)
        {
            try
            {
                    using (StreamWriter file = new StreamWriter(@filepath, true))
                    {
                        file.WriteLine(username + "," + password);
                        MessageBox.Show("Username and Password are saved !");
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error : " + e);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Client
{
    public partial class HomePageForm : Form
    {
        public HomePageForm()
        {
            InitializeComponent();
        }

        public RegistrationForm RegistrationForm
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
            }
        }

        public Form1 Form1
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
            }
        }


        List<string> login = new List<string>(); // Get list of all username in txt
        List<string> passwords = new List<string>(); // Get list of all password in txt

        private void FirstPage_Load(object sender, EventArgs e)
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

        // Connexion button
        private void button1_Click(object sender, EventArgs e)
        {

            if (login.Contains(textBox1.Text) && passwords.Contains(textBox2.Text)) // If lists already contain login and password go to PseudonymForm
            {
                Form1 Pseudo = new Form1();
                Pseudo.Show();
            }
            else
            {
                MessageBox.Show("Wrong username and password !");
            }
        }

        // Inscription button
        private void button2_Click(object sender, EventArgs e)
        {
            RegistrationForm register = new RegistrationForm();
            register.Show();
        }
    }
}

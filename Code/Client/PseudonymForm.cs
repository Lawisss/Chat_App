using System;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        public ClientForm ClientForm
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
            }
        }

        public string TextT()
        {
            return textBox1.Text;
        }

        public void LabelTitle(string title)
        {
            label1.Text = title;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter a Pseudonym");
            }
            else
            {
                ClientForm clientInterface = new ClientForm(textBox1.Text);
                clientInterface.Show();
            }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

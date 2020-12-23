using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public partial class PrivateChannelForm : Form
    {
        TcpClient clientSocket = new TcpClient();
        string counterpart, myPseudo; // couterpart and our pseudo initialisation

        Thread newThread; // For retrieve messages
        NetworkStream serverStream = default(NetworkStream); // Get data stream

        List<string> privatechannel = new List<string>();
        ObjSerialization obj = new ObjSerialization();

        public PrivateChannelForm(string userFriend, string name, TcpClient client)
        {
            InitializeComponent();

            clientSocket = client;
            this.counterpart = userFriend; 
            this.myPseudo = name;

            serverStream = clientSocket.GetStream();
            newThread = new Thread(sendPrivateMessage);
            newThread.Start();

        }

        public ObjSerialization ObjSerialization
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
            }
        }

        // Counterpart name
        public string getCounterPart()
        {
            return this.counterpart;
        }

        // Read private message
        public string getProcess()
        {
            return process.Text;
        }

        // Receive private message
        public void setProcess(string message)
        {
            this.Invoke((MethodInvoker)delegate 
            {
                process.Text = process.Text + Environment.NewLine + counterpart + " ‣ " + message;
            });
        }

        // Send private message
        public void sendPrivateMessage()
        {
            try
            {
                while (true)
                {
                    byte[] inStream = new byte[10000];
                    serverStream.Read(inStream, 0, inStream.Length);
                    List<string> parts = (List<string>)obj.ByteArrayToObject(inStream); // Array parts listing

                    if ((parts[2].Equals(counterpart)))
                    {
                        setProcess(parts[3]);
                    }
                    else if (parts[0].Equals('\0')) // Nothing read => User left
                    {
                        setProcess("User left");
                        newThread.Abort();
                        clientSocket.Close();
                        break;
                    }
                    parts.Clear();
                }
            }
            catch (Exception e)
            {
                newThread.Abort();
                clientSocket.Close();
                MessageBox.Show(e.ToString());
            }

        }

        // Private message button
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {   
                if (!messagePrivate.Text.Equals("")) // if textBox not empty then add some information
                {
                    privatechannel.Clear();
                    privatechannel.Add("private");
                    privatechannel.Add(counterpart);
                    privatechannel.Add(myPseudo);
                    privatechannel.Add(messagePrivate.Text);

                    byte[] outStream = obj.ObjectToByteArray(privatechannel);

                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();

                    this.Invoke((MethodInvoker)delegate // Write private message
                    {

                        process.Text = process.Text + Environment.NewLine + messagePrivate.Text;
                        messagePrivate.Text = "";
                    });
                }
            }
            catch (Exception er)
            {

            }
        }

        private void PrivateMessage_Load(object sender, EventArgs e)
        {

        }
    }
}

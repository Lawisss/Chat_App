using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public partial class ClientForm : Form
    {
        public TcpClient clientSocket;
        public NetworkStream serverStream = default(NetworkStream); // Get all data stream

        string readData = null;
        Thread createThread;
        string name = null;

        List<string> UserConnected = new List<string>(); // User connected to server
        List<string> PublicChannel = new List<string>(); // Message in public channel
        List<string> topics = new List<string>();  // List of topics

        ObjSerialization obj = new ObjSerialization();

        public ClientForm( string label)
        {
            InitializeComponent();
            name = label;
            topics.Add("Informatique");
            topics.Add("COVID-19");
            topics.Add("Culture");

            foreach (var item in topics)
            {
                listBox2.Items.Add(item.ToString());
            }
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

        public PrivateChannelForm PrivateChannelForm
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
            }
        }

        // List of users connected
        public void getUsersConnected(List<string> parts)
        {
            this.Invoke((MethodInvoker)delegate
           {
               listBox1.Items.Clear();
               for (int i = 0; i < parts.Count; i++)
               {
                   listBox1.Items.Add(parts[i]);
               }
           });
        }

        // Display data in public channel thanks to delegate
        public void PublicDisplay()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(PublicDisplay));
            else
                process.Text = process.Text + Environment.NewLine + "‣ " + readData;
        }

        // Retrieve private, public messages or list of users
        private void getPublicMessage()
        {
            try
            {
                while (true)
                {
                    serverStream = clientSocket.GetStream();
                    byte[] inStream = new byte[10025];
                    serverStream.Read(inStream, 0, inStream.Length);
                    List<string> parts = null;

                    if (!ConnectedSocket(clientSocket)) // no socket => no connexion
                    {
                        MessageBox.Show("You have been disconnected ");
                        createThread.Abort();
                        clientSocket.Close();
                        btnConnect.Enabled = true;
                    }


                    parts = (List<string>)obj.ByteArrayToObject(inStream);
                    switch (parts[0]) // Display messages in the right channel or list
                    {
                        case "UserList":
                            getUsersConnected(parts);
                            break;

                        case "public":
                            readData = "" + parts[1];
                            PublicDisplay();
                            break;

                        case "private":
                            GotoPrivateChannel(parts);
                            break;
                    }

                    if (readData[0].Equals('\0')) // Retry connexion possibility thanks to delegate
                    {
                        readData = "Retry connexion";
                        PublicDisplay();

                        this.Invoke((MethodInvoker)delegate
                        {
                            btnConnect.Enabled = true;
                        });

                        createThread.Abort();
                        clientSocket.Close();
                        break;
                    }
                    PublicChannel.Clear();
                }
            }
            catch (Exception e)
            {
                createThread.Abort();
                clientSocket.Close();
                btnConnect.Enabled = true;
                Console.WriteLine(e);
            }

        }

        // User server connexion checking
        bool ConnectedSocket(TcpClient socket) 
        {
            bool flag = false;
            try
            {
                // Poll determines the state of the Socket
                // Waiting time for response is 10 microseconds
                //SelectMode.SelectRead returns true if the connection has been closed, reset or terminated
                bool verification1 = socket.Client.Poll(10, SelectMode.SelectRead);

                // if Socket.Available == 0  => disconnection
                //Gets the amount of data received from the network and available for reading
                bool verification2 = (socket.Available == 0);


                if (verification1 && verification2)
                {
                    isConnected.Text = "Disconnected"; // Client disconnected
                    isConnected.BackColor = Color.Red; 
                    this.Invoke((MethodInvoker)delegate // Delegate activation when disconnected client => LoginButton activation
                    {
                        btnConnect.Enabled = true;
                    });
                    flag = false;
                }
                else
                {
                    isConnected.Text = "Connected"; // Client connected
                    isConnected.BackColor = Color.Green;
                    flag = true;
                }
            }
            catch (Exception er)
            {
                Console.WriteLine(er);
            }
            return flag;
        }

        // Public message button (Add + serialize + send to server message)
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!inputText.Text.Equals(""))
                {
                    PublicChannel.Add("public");
                    PublicChannel.Add(inputText.Text);
                    byte[] outStream = obj.ObjectToByteArray(PublicChannel);
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                    inputText.Text = "";
                    PublicChannel.Clear();
                }
            }
            catch (Exception er)
            {

                btnConnect.Enabled = true;
                MessageBox.Show(er.ToString());
            }
        }
        // Display in public channel thanks to delegate
        private void PublicMessage()
        {
            if (this.InvokeRequired)
            {

                this.Invoke(new MethodInvoker(PublicMessage));
            }
            else
            {
                process.Text = process.Text + Environment.NewLine + "‣ " + readData;
            }
        }
        // Create private channel with two users
        public void GotoPrivateChannel(List<string> parts)
        {

            this.Invoke((MethodInvoker)delegate  // Write received data
            {
                if (parts[3].Equals("new"))
                {
                    PrivateChannelForm privateMessage = new PrivateChannelForm(parts[2], name, clientSocket);
                    UserConnected.Add(parts[2]);
                    privateMessage.Text = "Private Conversation with " + parts[2];
                    privateMessage.Show();
                }
                else
                {
                    if (Application.OpenForms["PrivateMessage"] != null)
                    {
                        (Application.OpenForms["PrivateMessage"] as PrivateChannelForm).setProcess(parts[3]);
                    }
                }

            });

        }
        // Disconnect users by closing the application (in server app, disconnected user will remove to the list)
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you really want to exit the application ?", "Exit", MessageBoxButtons.YesNoCancel);
            if (dialog ==DialogResult.Yes)
            {
                try
                {
                    createThread.Abort(); // Close thread
                    clientSocket.Close(); // Close socket
                }
                catch (Exception)
                {

                    Application.ExitThread();
                }
            }
            else if(dialog == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        // To scroll public channel
        private void process_TextChanged(object sender, EventArgs e)
        {
            process.SelectionStart = process.TextLength;
            process.ScrollToCaret();
        }
        // Server Connexion button to launch connxion
        private void btnConnect_Click(object sender, EventArgs e)
        {
            clientSocket = new TcpClient();
            try
            {
                clientSocket.Connect("127.0.0.1", 5000);
                readData = "Connected to the server";
                PublicDisplay(); // Display connexion

                serverStream = clientSocket.GetStream();


                byte[] outStream = Encoding.ASCII.GetBytes(name + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                btnConnect.Enabled = false;// If the connection is made then the button cannot be used.

                createThread = new Thread(getPublicMessage); // Thread for messages
                createThread.Start();
            }
            catch (Exception er)
            {
                MessageBox.Show("the server did not start" + er.ToString());
            }
        }

        // Private Channel Button
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                String clientName = listBox1.GetItemText(listBox1.SelectedItem); // Selection of user in users list
                PublicChannel.Clear();
                PublicChannel.Add("private");
                PublicChannel.Add(clientName);
                PublicChannel.Add(name);
                PublicChannel.Add("new");

                byte[] outStream = obj.ObjectToByteArray(PublicChannel);
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                PrivateChannelForm privateChat = new PrivateChannelForm(clientName, name, clientSocket); // Go to private channel
                UserConnected.Add(clientName);
                privateChat.Text = "Private Conversation with " + clientName;
                privateChat.Show();
                PublicChannel.Clear();
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }
    }
}

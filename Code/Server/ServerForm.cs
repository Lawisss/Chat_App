using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class ServerForm : Form
    {

        TcpListener TCPlistener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000); // TCP Listerner to connect with the server
        TcpClient client;

        Dictionary<string, TcpClient> clientDico = new Dictionary<string, TcpClient>(); // Dictionary of clients connected
        List<string> MsgList = new List<string>(); // List of clients messages
        ObjSerialization obj = new ObjSerialization();

        public ServerForm()
        {
            InitializeComponent();
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

        // Display data with delegate
        public void DataDisplay(string m)
        {
            this.Invoke((MethodInvoker)delegate 
            {
                process.AppendText("‣ " + m + Environment.NewLine);
            });
        }

        // Publication in Public channel with broadcasting and when flag is True
        public void ClientChannel(string msg, string username, bool flag)
        {
            try
            {
                foreach (var Item in clientDico)
                {
                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;
                    NetworkStream broadcastStream = broadcastSocket.GetStream(); // Send and receive data with NetworkStream
                    byte[] broadcastBytes = null;

                    if (flag) // Check active connexion and tag = Public => public message 
                    {
                        MsgList.Add("public"); // Publication in public channel
                        MsgList.Add(username + " : " + msg);
                        broadcastBytes = obj.ObjectToByteArray(MsgList);
                    }
                    else
                    {
                        MsgList.Add("public");
                        MsgList.Add(msg);
                        broadcastBytes = obj.ObjectToByteArray(MsgList);

                    }

                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                    broadcastStream.Flush();
                    MsgList.Clear();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        // Get UsersList
        public void GetUserList()
        {
            try
            {
                byte[] userList = new byte[1024];
                string[] clientlist = listBox1.Items.OfType<string>().ToArray();
                List<string> users = new List<string>();

                users.Add("UserList");
                foreach (string name in clientlist)
                {
                    users.Add(name);
                }
                userList = obj.ObjectToByteArray(users); // Users List serialize

                foreach (var Item in clientDico)
                {
                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value; // Take the issuance value
                    NetworkStream broadcastStream = broadcastSocket.GetStream(); // Return NetworkStream
                    broadcastStream.Write(userList, 0, userList.Length); // Write the data
                    broadcastStream.Flush(); // Clean data
                    users.Clear();
                }
            }
            catch (SocketException se)
            {
            }
        }

        // Send private message
        private void PrivateChannel(List<string> text)
        {
            try
            {

                byte[] byData = obj.ObjectToByteArray(text);

                TcpClient workerSocket = null;
                workerSocket = (TcpClient)clientDico.FirstOrDefault(x => x.Key == text[1]).Value; //Linq query to find dynamically the client in Dictionary

                NetworkStream stm = workerSocket.GetStream();
                stm.Write(byData, 0, byData.Length);
                stm.Flush();

            }
            catch (SocketException se)
            {
                MessageBox.Show(se.ToString());
            }
        }

        // Server Connexion Initialisation
        public async void ServerConnexion()
        {
            TCPlistener.Start();
            DataDisplay("Server connexion starts here : " + TCPlistener.LocalEndpoint); // Display localhost

            DataDisplay("Users Waiting... ");
            try
            {
             
                while (true) // While active server => asychronous task
                {

                    client = await Task.Run(() => TCPlistener.AcceptTcpClientAsync()); // Start a task without blocking the thread and to pursuit the execution after this task is completed

                    byte[] name = new byte[50];
                    NetworkStream flux = client.GetStream();  // Get the data stream
                    flux.Read(name, 0, name.Length); 
                    string username = Encoding.ASCII.GetString(name); 
                    username = username.Substring(0, username.IndexOf("$"));

                    clientDico.Add(username, client); // Add client in dico
                    listBox1.Items.Add(username); // Add client in Users List

                    DataDisplay(username + " is connected on " + client.Client.RemoteEndPoint); // Server App display
                    ClientChannel(username + " is connected ! ", username, false); // Client App display


                    await Task.Delay(1000).ContinueWith(t => GetUserList());
                    var cs = new Thread(() => ServerReception(client, username));
                    cs.Start();

                }
            }
            catch (Exception)
            {
                TCPlistener.Stop();
            }

        }

        // Server receives data
        public void ServerReception(TcpClient nclient, String username)
        {
            byte[] data = new byte[1000];
            string text = null;
            while (true)
            {
                try
                {
                    NetworkStream stream = nclient.GetStream(); // Get the data stream
                    stream.Read(data, 0, data.Length); 
                    List<string> parts = (List<string>)obj.ByteArrayToObject(data);

                    switch (parts[0]) // Write the received data for specified channel
                    {
                        case "public":
                            this.Invoke((MethodInvoker)delegate 
                            {
                                process.Text += username + ": " + parts[1] + Environment.NewLine;
                            });
                            ClientChannel(parts[1], username, true);
                            break;

                        case "private":
                            PrivateChannel(parts);
                            break;
                    }

                    parts.Clear();
                }
                catch (Exception r) // Remove clients when they leave
                {
                    DataDisplay(username + " is disconnected ");
                    ClientChannel("The user is disconnected: " + username + "-", username, false);
                    clientDico.Remove(username);

                    this.Invoke((MethodInvoker)delegate
                    {
                        listBox1.Items.Remove(username);
                    });
                        GetUserList();
                    break;
                }
            }
        }

        // Start button for connect the server to localhost
        private void button1_Click(object sender, EventArgs e)
        {
            ServerConnexion();
        }

        // Close Button
        // Server shutdown = client disconnected
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                TCPlistener.Stop();
                DataDisplay("Server Shutdown");
                foreach (var Item in clientDico)
                {
                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;
                    broadcastSocket.Close();
                }
            }
            catch (SocketException er)
            {
                MessageBox.Show(er.ToString());
            }
        }

        // Private Channel Button
        private void btnPrivate_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string clientName = listBox1.GetItemText(listBox1.SelectedItem);

                MsgList.Clear();
                MsgList.Add("public");
                MsgList.Add("Administrator : " + textBox1.Text);

                byte[] byData = obj.ObjectToByteArray(MsgList);

                TcpClient workerSocket = null;
                workerSocket = (TcpClient)clientDico.FirstOrDefault(x => x.Key == clientName).Value; // Find the customer in the dictionary thanks to his name
                NetworkStream stream = workerSocket.GetStream();
                stream.Write(byData, 0, byData.Length);
                stream.Flush();
                MsgList.Clear();

            }
        }

        private void process_TextChanged(object sender, EventArgs e)
        {
            process.SelectionStart = process.TextLength;
            process.ScrollToCaret();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }
    }
}

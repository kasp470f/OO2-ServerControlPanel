using Renci.SshNet;
using ServerControlPanel.Components;
using System;
using System.Windows;

namespace ServerControlPanel.Views.Windows
{
    public partial class CreateServer : Window
    {
        public CreateServer()
        {
            InitializeComponent();
        }


        private void Create_Button(object sender, RoutedEventArgs e)
        {

            if (IP.Text == "") MessageBox.Show("Field cannot be empty, please enter correct info in the Server IP field");
            else if (Username.Text == "") MessageBox.Show("Field cannot be empty, please enter correct info in the Username field");
            else if (Password.Text == "") MessageBox.Show("Field cannot be empty, please enter correct info in the Password field");
            else if (ServerName.Text == "") MessageBox.Show("Field cannot be empty, please enter correct info in the Server Name field");


            if (!string.IsNullOrEmpty(IP.Text) && !string.IsNullOrEmpty(Username.Text) && !string.IsNullOrEmpty(Password.Text) && !string.IsNullOrEmpty(ServerName.Text))
            {
                try
                {
                    using (var client = (Port.Text == string.Empty) ? new SshClient(IP.Text, Username.Text, Password.Text) : new SshClient(IP.Text, int.Parse(Port.Text), Username.Text, Password.Text))
                    {
                        client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(5);
                        client.Connect();
                        Server.AddServer(ServerName.Text, client.IsConnected, IP.Text, (Port.Text == string.Empty) ? client.ConnectionInfo.Port.ToString() : Port.Text, Username.Text, Password.Text, client);
                        client.Disconnect();
                        Clear_Fields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Clear_Fields()
        {
            IP.Text = String.Empty;
            Port.Text = String.Empty;
            Username.Text = String.Empty;
            Password.Text = String.Empty;
            ServerName.Text = String.Empty;
        }


        private void Clear_Button(object sender, RoutedEventArgs e)
        {
            Clear_Fields();
        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

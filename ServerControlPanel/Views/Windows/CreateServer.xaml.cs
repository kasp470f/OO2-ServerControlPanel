using System;
using System.Windows;
using Renci.SshNet;
using ServerControlPanel.Components;

namespace ServerControlPanel.Views.Windows
{
    public partial class CreateServer : Window
    {
        public CreateServer()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if(Port.Text == string.Empty)
                {
                    using (var client = new SshClient(IP.Text, Username.Text, Password.Text))
                    {
                        client.Connect();

                        MessageBox.Show("Connected: " + client.IsConnected);
                        Server.AddServer(ServerName.Text, client.IsConnected, IP.Text, client.ConnectionInfo.Port.ToString(), client);

                        client.Disconnect();
                    }
                }
                else
                {
                    using (var client = new SshClient(IP.Text, int.Parse(Port.Text), Username.Text, Password.Text))
                    {
                        client.Connect();

                        MessageBox.Show("Connected: " + client.IsConnected);
                        Server.AddServer(ServerName.Text, client.IsConnected, IP.Text, Port.Text, client);
                        client.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}

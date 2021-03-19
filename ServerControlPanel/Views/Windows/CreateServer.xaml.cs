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
                using (var client = (Port.Text == string.Empty) ? new SshClient(IP.Text, Username.Text, Password.Text) : new SshClient(IP.Text, int.Parse(Port.Text), Username.Text, Password.Text))
                {
                    client.Connect();
                    Server.AddServer(ServerName.Text, client.IsConnected, IP.Text, (Port.Text == string.Empty) ? client.ConnectionInfo.Port.ToString() : Port.Text, Username.Text, Password.Text, client);
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

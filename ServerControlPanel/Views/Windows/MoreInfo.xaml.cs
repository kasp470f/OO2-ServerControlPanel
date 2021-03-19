using System.Windows;
using ServerControlPanel.Components;
using System.Timers;
using System;

namespace ServerControlPanel.Views.Windows
{
    public partial class MoreInfo : Window
    {
        public Server currentServer { get; set; }
        public MoreInfo(Server Source)
        {
            InitializeComponent();

            currentServer = Source;

            Title = Source.ServerId;
            serverName.Text = Source.ServerId;
            IP.Text = Source.IP;
            Uptime.Text = Source.Uptime;
            DiskSpace.Text = Source.Disk;
            RAM.Text = Source.RAM;
            CPU.Text = Source.CPU;

            Timer timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 4000;
            timer.Enabled = true;
            timer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            int index = Server.serverStats.IndexOf(currentServer);
            currentServer = Server.serverStats[index];
            Update();
        }

        private void Update()
        {
            Dispatcher.Invoke(() =>
            {
                Uptime.Text = currentServer.Uptime;
                DiskSpace.Text = currentServer.Disk;
                RAM.Text = currentServer.RAM;
                CPU.Text = currentServer.CPU;
            });
        }
    }
}

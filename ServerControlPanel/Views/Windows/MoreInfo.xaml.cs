using System.Windows;
using ServerControlPanel.Components;
using System.Timers;
using System;

namespace ServerControlPanel.Views.Windows
{
    public partial class MoreInfo : Window
    {
        public Server CurrentServer { get; set; }
        public MoreInfo(Server Source)
        {
            InitializeComponent();

            CurrentServer = Source;

            Title = Source.ServerId + " - Server";
            serverName.Text = Source.ServerId;
            IP.Text = Source.IP;
            Uptime.Text = Source.Uptime;
            DiskSpace.Text = Source.Disk;
            RAM.Text = Source.RAM;
            CPU.Text = Source.CPU;
            Processes.Text = Source.Processes;

            Timer timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 4000;
            timer.Enabled = true;
            timer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            int index = Server.serverStats.IndexOf(CurrentServer);
            CurrentServer = Server.serverStats[index];
            Update();
        }

        private void Update()
        {
            Dispatcher.Invoke(() =>
            {
                Uptime.Text = CurrentServer.Uptime;
                DiskSpace.Text = CurrentServer.Disk;
                RAM.Text = CurrentServer.RAM;
                CPU.Text = CurrentServer.CPU;
                Processes.Text = CurrentServer.Processes;
            });
        }
    }
}

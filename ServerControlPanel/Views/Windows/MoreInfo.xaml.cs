using System.Windows;
using ServerControlPanel.Components;

namespace ServerControlPanel.Views.Windows
{
    public partial class MoreInfo : Window
    {
        public MoreInfo(Server Source)
        {
            InitializeComponent();

            Title = Source.ServerId;
            serverName.Text = Source.ServerId;
            IP.Text = Source.IP;
            Uptime.Text = Source.Uptime;
            DiskSpace.Text = Source.Disk;
            RAM.Text = Source.RAM;
            CPU.Text = Source.CPU;
        }
    }
}

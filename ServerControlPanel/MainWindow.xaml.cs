using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ServerControlPanel.Views.Windows;

namespace ServerControlPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //new MoreInfo().Show();
            List<ServerStat> items = new List<ServerStat>();
            items.Add(new ServerStat() { ServerId = "Janus Server", StatusText = "Status", StatusColor = Brushes.Green, ServerIp="255.255.255.255" });
            items.Add(new ServerStat() { ServerId = "Kasper Server", StatusText = "Status", StatusColor = Brushes.Gray, ServerIp = "192.1" });
            items.Add(new ServerStat() { ServerId = "Martin Server", StatusText = "Status", StatusColor = Brushes.Red, ServerIp = "192.2" });


			icSomething.ItemsSource = items;
		}

        public class ServerStat
        {
            public string ServerId { get; set; }
            public string StatusText { get; set; }
            public Brush StatusColor { get; set; }
            public string ServerIp { get; set; }
        }

		private void RebootClick(object sender, RoutedEventArgs e)
		{

		}

		private void InfoClick(object sender, RoutedEventArgs e)
		{

		}
	}
}

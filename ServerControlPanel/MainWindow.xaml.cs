using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using ServerControlPanel.Components;
using ServerControlPanel.Views.Windows;

namespace ServerControlPanel
{
    public partial class MainWindow : Window
    {
		Timer reloadTimer = new Timer();
		public MainWindow()
        {
            InitializeComponent();
            new CreateServer().Show();
			reloadTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			reloadTimer.Interval = 1000;
			reloadTimer.Enabled = true;
			reloadTimer.Start();
		}

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
			reloadTimer.Stop();
			Server.ReloadServers();
			Dispatcher.Invoke(() =>
			{
				icSomething.ItemsSource = null;
				icSomething.Items.Clear();
				icSomething.ItemsSource = Server.serverStats;
			});
			reloadTimer.Start();
		}


        private void RebootClick(object sender, RoutedEventArgs e)
		{
			
		}

		private void InfoClick(object sender, RoutedEventArgs e)
		{
			var baseSource = (Button)e.Source;
			Server Source = (Server)baseSource.DataContext;
			int a = Server.serverStats.IndexOf(Source);
			MoreInfo moreInfo = new MoreInfo(Server.serverStats[a]);
			moreInfo.Show();
		}
	}
}

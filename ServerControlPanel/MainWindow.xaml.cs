using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Renci.SshNet;
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
            
			reloadTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			reloadTimer.Interval = 1000;
			reloadTimer.Enabled = true;
			reloadTimer.Start();
		}

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
			Server.ReloadServers();
			Dispatcher.Invoke(() =>
			{
				ServerContainer.ItemsSource = null;
				ServerContainer.Items.Clear();
				ServerContainer.ItemsSource = Server.serverStats;
			});
		}

        private void RebootClick(object sender, RoutedEventArgs e)
		{
			Button rebootButton= (Button)e.Source;
			Server Source = (Server)rebootButton.DataContext;
			Server.Reboot(Source);
		}

		private void InfoClick(object sender, RoutedEventArgs e)
		{
			var baseSource = (Button)e.Source;
			Server Source = (Server)baseSource.DataContext;
			int a = Server.serverStats.IndexOf(Source);
			MoreInfo moreInfo = new MoreInfo(Server.serverStats[a]);
			moreInfo.Show();
		}

		private void CreateServerClick(object sender, RoutedEventArgs e)
		{
			new CreateServer().Show();
		}

		private void ExitClick(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}		
	}
}

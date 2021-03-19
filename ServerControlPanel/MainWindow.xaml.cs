using System.Windows;
using System.Windows.Controls;
using ServerControlPanel.Components;
using ServerControlPanel.Views.Windows;

namespace ServerControlPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            new CreateServer().Show();
		}

		private void RebootClick(object sender, RoutedEventArgs e)
		{
			Server.ReloadServers();
			icSomething.ItemsSource = null;
			icSomething.Items.Clear();
			icSomething.ItemsSource = Server.serverStats;
		}

		private void InfoClick(object sender, RoutedEventArgs e)
		{
			var baseSource = (Button)e.Source;
			Server Source = (Server)baseSource.DataContext;
			MoreInfo moreInfo = new MoreInfo(Source);
			moreInfo.Show();
		}
	}
}


﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
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


			icSomething.ItemsSource = Server.serverStats;
		}

		private void RebootClick(object sender, RoutedEventArgs e)
		{
			icSomething.ItemsSource = null;
			icSomething.Items.Clear();
			icSomething.ItemsSource = Server.serverStats;
		}

		private void InfoClick(object sender, RoutedEventArgs e)
		{

		}
	}
}

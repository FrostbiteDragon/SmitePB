using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using Domain;

namespace SmitePB.Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenScreen(object sender, RoutedEventArgs e)
        {
            var process = new Process();

            process.StartInfo.FileName = @"..\..\screen\SmitePnB.Screen.exe";
            process.StartInfo.Arguments = "-single-instance -screen-fullscreen 0";
            process.Start();
        }

        private void SelectGod(object sender, RoutedEventArgs e)
        {
        }
    }
}

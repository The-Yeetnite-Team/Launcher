using ModernWpf;
using System.Diagnostics;
using System.Windows;

namespace Yeetnite_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Settings.Init();
        }

        private void OnLaunched(object sender, RoutedEventArgs e)
        {
            // TODO Read settings values instead
            if (Settings.Username() != string.Empty) RootFrame.Navigate(new HomePage());
            else RootFrame.Navigate(new LoginPage());
        }
    }
}

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
        }

        private void OnLaunched(object sender, RoutedEventArgs e)
        {
            // TODO Read settings values instead
            if (true) RootFrame.Navigate(new HomePage());
            else RootFrame.Navigate(new LoginPage());
        }
    }
}

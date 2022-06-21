using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Yeetnite_Launcher
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            Fortnite_Background_Image.Source = new BitmapImage(new System.Uri(string.Format("/Assets/Home-Background/{0}.jpg", new System.Random().Next(1, 10)), System.UriKind.Relative));
        }
    }
}

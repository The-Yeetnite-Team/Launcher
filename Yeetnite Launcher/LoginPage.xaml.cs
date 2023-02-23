using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Yeetnite_Launcher
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        static readonly HttpClient httpClient = new();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            if (UsernameField.Text == "" || PasswordField.Password == "")
            {
                Toast.ShowError("Please fill in all required fields and try again");
                return;
            }
            LoadingElement.Visibility = Visibility.Visible;

            try
            {
                string response = await httpClient.GetStringAsync(string.Format("https://api.yeetnite.ml:444/api/v3/launcher_login.php?username={0}&password={1}", UsernameField.Text, PasswordField.Password));
                if (response.Contains("Invalid username or password"))
                {
                    Toast.ShowError("Invalid username or password");
                    LoadingElement.Visibility = Visibility.Hidden;
                    return;
                }
                User? user = JsonConvert.DeserializeObject<User>(response);
                Settings.Username(user?.GetUsername() ?? string.Empty);
                Settings.AccessToken(user?.GetAccessToken() ?? string.Empty);

                Process.Start(System.Environment.ProcessPath ?? string.Empty);
                Application.Current.Shutdown();
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                Toast.ShowError("Please check your internet connection and try again");
                LoadingElement.Visibility = Visibility.Hidden;
                return;
            }

            LoadingElement.Visibility = Visibility.Hidden;
        }

        private void SetHoverMouse(object sender, MouseEventArgs e)
        {
            ContentArea.Cursor = Cursors.Hand;
        }

        private void SetNormalMouse(object sender, MouseEventArgs e)
        {
            ContentArea.Cursor = Cursors.Arrow;
        }
    }
}

using FontAwesome.WPF;
using Ookii.Dialogs.Wpf;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Yeetnite_Launcher
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private static readonly string[] _fn_versions = new[] { "1.7.2", "1.8", "1.8.1", "1.8.2", "1.9", "1.9.1", "1.1.0", "6.00", "6.01", "6.02", "6.10", "6.20", "6.21", "6.30", "7.00", "7.10", "7.20", "7.30", "7.40" };

        private string _version = "", _install_path = "";

        Window? popup;
        TextBox? Install_Location_Entry;
        Button? Add_Version_Button;

        public HomePage()
        {
            InitializeComponent();
            Fortnite_Background_Image.Source = new BitmapImage(new System.Uri(string.Format("/Assets/Home-Background/{0}.jpg", new System.Random().Next(1, 10)), System.UriKind.Relative));
        }

        private void SetHoverMouse(object sender, MouseEventArgs e)
        {
            ContentGrid.Cursor = Cursors.Hand;
            e.Handled = true;
        }

        private void SetRegularMouse(object sender, MouseEventArgs e)
        {
            ContentGrid.Cursor = Cursors.Arrow;
            e.Handled = true;
        }

        private void OpenLink(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            })?.Dispose();
            e.Handled = true;
        }

        private void SetHoveredDiscordLink(object sender, MouseEventArgs e)
        {
            SetHoverMouse(sender, e);
            Discord_Link.Foreground = SystemParameters.WindowGlassBrush;
            e.Handled = true;
        }

        private void SetRegularDiscordLink(object sender, MouseEventArgs e)
        {
            SetRegularMouse(sender, e);
            Discord_Link.Foreground = new BrushConverter().ConvertFrom("#ffffff") as Brush;
            e.Handled = true;
        }

        private void SetHoveredPlusButton(object sender, MouseEventArgs e)
        {
            SetHoverMouse(sender, e);
            Plus_Icon.Foreground = SystemParameters.WindowGlassBrush;
            e.Handled = true;
        }

        private void SetRegularPlusButton(object sender, MouseEventArgs e)
        {
            SetRegularMouse(sender, e);
            Plus_Icon.Foreground = new BrushConverter().ConvertFrom("#ffffff") as Brush;
            e.Handled = true;
        }

        private void EditVersion(object sender, RoutedEventArgs e)
        {
            if (Settings.FortniteSelectedIndex() == -1)
            {
                Toast.ShowError("Please select a version first");
                e.Handled = true;
                return;
            }
            e.Handled = true;
        }

        private void SetPopupHoverMouse(object sender, MouseEventArgs e)
        {
            if (popup == null) return;
            popup.Cursor = Cursors.Hand;
            e.Handled = true;
        }

        private void SetPopupRegularMouse(object sender, MouseEventArgs e)
        {
            if (popup == null) return;
            popup.Cursor = Cursors.Arrow;
            e.Handled = true;
        }

        private string SelectFolder()
        {
            VistaFolderBrowserDialog dialog = new()
            {
                Description = "Select Fortnite Install Location",
                UseDescriptionForTitle = true,
            };
            dialog.ShowDialog();
            return dialog.SelectedPath ?? string.Empty;
        }

        private void VersionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _version = (string?)e.AddedItems[0] ?? string.Empty;
            e.Handled = true;
        }

        private void InstallLocationTextChanged(object sender, TextChangedEventArgs e)
        {
            _install_path = Install_Location_Entry?.Text ?? string.Empty;
            e.Handled = true;
        }

        private void SetPathToFolderSelection(object sender, MouseButtonEventArgs e)
        {
            string selectedPath = SelectFolder();
            if (selectedPath == string.Empty) return;

            _install_path = selectedPath;
            if (Install_Location_Entry != null)
                Install_Location_Entry.Text = selectedPath;

            e.Handled = true;
        }

        private void SaveVersion(object sender, RoutedEventArgs e)
        {
            if (_version == string.Empty || _install_path == string.Empty)
            {
                MessageBox.Show("One or more required fields are empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(_install_path + "\\"))
            {
                MessageBox.Show("Invalid installation path. Make sure the \"FortniteGame\" and \"Engine\" folders exist");
                return;
            }

            Settings.AddFortniteEntry(new FortniteEntrySchema(_version, _install_path));
            popup?.Close();
        }

        private void AddVersion(object sender, MouseButtonEventArgs e)
        {
            // Build out the popup window UI
            StackPanel popupUI = new()
            {
                Margin = new Thickness()
                {
                    Left = 15,
                    Right = 15,
                    Top = 10,
                    Bottom = 10,
                }
            };
            TextBlock Title_Text = new()
            {
                FontSize = 28,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "Add A Version",
                TextDecorations = TextDecorations.Underline,
            };
            TextBlock Install_Path_Text = new()
            {
                FontSize = 18,
                Margin = new Thickness()
                {
                    Top = 15,
                },
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Install Location:"
            };
            StackPanel Location_Entry_Panel = new()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness()
                {
                    Top = 10
                },
            };

            Install_Location_Entry = new()
            {
                Width = 370,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                AcceptsTab = true,
                AllowDrop = false,
                AcceptsReturn = false
            };

            Install_Location_Entry.TextChanged += InstallLocationTextChanged;

            ImageAwesome Folder_Icon = new()
            {
                Icon = FontAwesomeIcon.Folder,
                Foreground = new BrushConverter().ConvertFrom("#ffd664") as Brush,
                Height = 25,
                Margin = new Thickness()
                {
                    Left = 15,
                }
            };

            Folder_Icon.MouseEnter += SetPopupHoverMouse;
            Folder_Icon.MouseLeave += SetPopupRegularMouse;
            Folder_Icon.MouseEnter += (object sender, MouseEventArgs e) => Folder_Icon.Foreground = new BrushConverter().ConvertFrom("#ffc21a") as Brush;
            Folder_Icon.MouseLeave += (object sender, MouseEventArgs e) => Folder_Icon.Foreground = new BrushConverter().ConvertFrom("#ffd664") as Brush;
            Folder_Icon.MouseLeftButtonDown += SetPathToFolderSelection;

            Location_Entry_Panel.Children.Add(Install_Location_Entry);
            Location_Entry_Panel.Children.Add(Folder_Icon);

            TextBlock Version_Text = new()
            {
                FontSize = 18,
                Margin = new Thickness()
                {
                    Top = 10
                },
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Fortnite Version:"
            };
            ComboBox Version = new()
            {
                Height = 30,
                Margin = new Thickness()
                {
                    Top = 10,
                }
            };
            for (int i = 0; i < _fn_versions.Length; i++)
                Version.Items.Add(_fn_versions[i]);

            Version.SelectionChanged += VersionSelectionChanged;

            Add_Version_Button = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness()
                {
                    Top = 25
                },
                Content = "Add Version",
                FontSize = 18,
            };

            Add_Version_Button.MouseEnter += SetPopupHoverMouse;
            Add_Version_Button.MouseLeave += SetPopupRegularMouse;
            Add_Version_Button.Click += SaveVersion;

            popupUI.Children.Add(Title_Text);
            popupUI.Children.Add(Install_Path_Text);
            popupUI.Children.Add(Location_Entry_Panel);
            popupUI.Children.Add(Version_Text);
            popupUI.Children.Add(Version);
            popupUI.Children.Add(Add_Version_Button);

            popup = new()
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Height = 375,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Width = 450,
                Background = new BrushConverter().ConvertFrom("#1f1f1f") as Brush,
                Content = popupUI
            };
            popup.Show();
            // Debug.WriteLine(SelectFolder());
            // MessageBox.Show("Please select a version first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

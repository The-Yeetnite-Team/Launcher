using FontAwesome.WPF;
using Lunar;
using Ookii.Dialogs.Wpf;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yeetnite_Launcher
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private Regex _legacyVersionFormat = new (@"^[0-9]{1}\.[0-9]{1}\.[0-9]{1}$");
        private Regex _modernVersionFormat = new (@"^[0-9]{1,2}\.[0-9]{1,2}$");
        
        private string _version = "", _install_path = "";

        private Window? popup;
        private TextBox? Install_Location_Entry;
        private TextBox? Version_Entry;
        private Button? Add_Version_Button;

        public HomePage()
        {
            InitializeComponent();

            FortniteBackgroundImage.Source = new BitmapImage(new System.Uri(string.Format("/Assets/Home-Background/{0}.jpg", new System.Random().Next(1, 10)), System.UriKind.Relative));
            for (int i = 0; i < Settings.FortniteEntries().Count; i++)
            {
                ModernWpf.Controls.SimpleStackPanel version = new()
                {
                    Orientation = Orientation.Vertical,
                    Height = 190
                };
                Image background = new()
                {
                    Width = 166,
                    Height = 250,
                    Opacity = 0.5,
                    Source = new BitmapImage(new System.Uri("/Assets/version_image.jpg", System.UriKind.Relative)),
                    Margin = new Thickness
                    {
                        Left = i == 0 ? 20 : 10,
                        Right = 10
                    }
                };

                version.Children.Add(background);

                VersionsList.Children.Add(version);
            }
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
            DiscordLink.Foreground = SystemParameters.WindowGlassBrush;
            e.Handled = true;
        }

        private void SetRegularDiscordLink(object sender, MouseEventArgs e)
        {
            SetRegularMouse(sender, e);
            DiscordLink.Foreground = new BrushConverter().ConvertFrom("#ffffff") as Brush;
            e.Handled = true;
        }

        private void SetHoveredPlusButton(object sender, MouseEventArgs e)
        {
            SetHoverMouse(sender, e);
            PlusIcon.Foreground = SystemParameters.WindowGlassBrush;
            e.Handled = true;
        }

        private void SetRegularPlusButton(object sender, MouseEventArgs e)
        {
            SetRegularMouse(sender, e);
            PlusIcon.Foreground = new BrushConverter().ConvertFrom("#ffffff") as Brush;
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

        private static string SelectFolder()
        {
            VistaFolderBrowserDialog dialog = new()
            {
                Description = "Select Fortnite Install Location",
                UseDescriptionForTitle = true,
            };
            dialog.ShowDialog();
            return dialog.SelectedPath ?? string.Empty;
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
        
        private void VersionTextChanged(object sender, TextChangedEventArgs e)
        {
            _version = Version_Entry?.Text ?? string.Empty;
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
                MessageBox.Show("Invalid installation path. Folder does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(_install_path + "\\Engine") || !Directory.Exists(_install_path + "\\FortniteGame"))
            {
                MessageBox.Show("Invalid installation path. Make sure the \"FortniteGame\" and \"Engine\" folders exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!_legacyVersionFormat.IsMatch(_version) && !_modernVersionFormat.IsMatch(_version))
            {
                MessageBox.Show($"'{_version}' is not a valid version.\nValid version example: 6.21", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Settings.FortniteVersionsStored().Contains(_version))
            {
                MessageBox.Show($"Fortnite {_version} is already stored as a version", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Settings.AddFortniteEntry(new FortniteEntrySchema(_version, _install_path));
            popup?.Close();
        }

        private void Launch(object sender, RoutedEventArgs e)
        {
            Process fnProcess = new();
            fnProcess.StartInfo.FileName = "F:\\Builds\\Season 6\\Fortnite 6.30\\FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe";
            fnProcess.StartInfo.Arguments = "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -EpicPortal -HTTP=WinInet -skippatchcheck -NOSSLPINNING -AUTH_TYPE=password -AUTH_LOGIN=Revvz -AUTH_PASSWORD=ed79a237cda964ee -noeac -fromfl=be -fltoken=f7b9gah4h5380d10f721dd6a";
            fnProcess.StartInfo.WorkingDirectory = "F:\\Builds\\Season 6\\Fortnite 6.30\\FortniteGame\\Binaries\\Win64";
            fnProcess.Start();

/*            LibraryMapper mapper = new LibraryMapper(fnProcess, "C:\\Users\\antonios\\Desktop\\repos\\Yeetnite\\x64\\Debug\\YeetniteClientDLL.dll");
            mapper.MapLibrary();*/

            fnProcess.Dispose();
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
            Folder_Icon.MouseEnter += (object Sender, MouseEventArgs args) => Folder_Icon.Foreground = new BrushConverter().ConvertFrom("#ffc21a") as Brush;
            Folder_Icon.MouseLeave += (object Sender, MouseEventArgs args) => Folder_Icon.Foreground = new BrushConverter().ConvertFrom("#ffd664") as Brush;
            Folder_Icon.MouseLeftButtonDown += SetPathToFolderSelection;

            Location_Entry_Panel.Children.Add(Install_Location_Entry);
            Location_Entry_Panel.Children.Add(Folder_Icon);

            TextBlock Version_Text = new()
            {
                FontSize = 18,
                Margin = new Thickness()
                {
                    Top = 10,
                    Bottom = 10
                },
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Fortnite Version:"
            };
            
            Version_Entry = new()
            {
                Width = 75,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                AcceptsTab = false,
                AllowDrop = false,
                AcceptsReturn = false
            };

            Version_Entry.TextChanged += VersionTextChanged;

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
            popupUI.Children.Add(Version_Entry);
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
        }
    }
}

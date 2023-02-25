using System;
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
using ModernWpf.Controls;
using WinRT;

namespace Yeetnite_Launcher
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage
    {
        private readonly Regex _legacyVersionFormat = new (@"^[0-9]{1}\.[0-9]{1}\.[0-9]{1}$");
        private readonly Regex _modernVersionFormat = new (@"^[0-9]{1,2}\.[0-9]{1,2}$");
        
        private string _version = "", _installPath = "";

        private Window? _popup;
        private TextBox? _installLocationEntry;
        private TextBox? _versionEntry;
        private Button? _addVersionButton;

        public HomePage()
        {
            InitializeComponent();

            FortniteBackgroundImage.Source = new BitmapImage(new Uri($"/Assets/Home-Background/{new Random().Next(1, 10)}.jpg", UriKind.Relative));
            if (Settings.FortniteSelectedIndex() != -1)
            {
                SelectedVersionText.Text = Settings.FortniteVersionsStored()[Settings.FortniteSelectedIndex()];
            }
            
            for (int i = 0; i < Settings.FortniteEntries().Count; i++)
            {
                AddFortniteVersionToList(Settings.FortniteEntries()[i], i == 0 ? 20 : 10);
            }
        }

        private void AddFortniteVersionToList(FortniteEntrySchema fortniteEntry, int leftPadding=10)
        {
            SimpleStackPanel version = new()
            {
                Orientation = Orientation.Vertical,
                Height = 220
            };

            SimpleStackPanel buttonContent = new()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness
                {
                    Top = 180,
                    Left = 45
                },
                Height = 75,
                Width = 146
            };

            buttonContent.Children.Add(
                new TextBlock
                {
                    Text = fortniteEntry.Version,
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255))
                }
            );
            
            buttonContent.Children.Add(
                new Rectangle
                {
                    Width = 200,
                    Height = 200,
                    Fill = new LinearGradientBrush(Color.FromArgb(255, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), new Point(0,1),new Point(0,0))
                }
            );

            Button selectFortnite = new()
            {
                Width = 128,
                Height = 220,
                Background = new ImageBrush(new BitmapImage(new Uri("Assets/version_image.jpg", UriKind.Relative))),
                Margin = new Thickness
                {
                    Left = leftPadding,
                    Right = 10
                },
                Content = buttonContent
            };

            selectFortnite.MouseEnter += SetHoverMouse;
            selectFortnite.MouseLeave += SetRegularMouse;
            selectFortnite.Click += (sender, _) =>
            {
                string selectedVersion = ((Button)sender).Content.As<SimpleStackPanel>().Children[0].As<TextBlock>().Text;
                Settings.FortniteSelectedIndex(Settings.FortniteVersionsStored().IndexOf(selectedVersion));
                SelectedVersionText.Text = selectedVersion;
            };

            version.Children.Add(selectFortnite);
            VersionsList.Children.Add(version);
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
            e.Handled = true;
            if (Settings.FortniteSelectedIndex() == -1)
            {
                Toast.ShowError("Please select a version first");
                return;
            }
        }

        private void SetPopupHoverMouse(object sender, MouseEventArgs e)
        {
            if (_popup == null) return;
            _popup.Cursor = Cursors.Hand;
            e.Handled = true;
        }

        private void SetPopupRegularMouse(object sender, MouseEventArgs e)
        {
            if (_popup == null) return;
            _popup.Cursor = Cursors.Arrow;
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
            _installPath = _installLocationEntry?.Text ?? string.Empty;
            e.Handled = true;
        }

        private void SetPathToFolderSelection(object sender, MouseButtonEventArgs e)
        {
            string selectedPath = SelectFolder();
            if (selectedPath == string.Empty) return;

            _installPath = selectedPath;
            if (_installLocationEntry != null)
                _installLocationEntry.Text = selectedPath;

            e.Handled = true;
        }
        
        private void VersionTextChanged(object sender, TextChangedEventArgs e)
        {
            _version = _versionEntry?.Text ?? string.Empty;
            e.Handled = true;
        }

        private void SaveVersion(object sender, RoutedEventArgs e)
        {
            if (_version == string.Empty || _installPath == string.Empty)
            {
                MessageBox.Show("One or more required fields are empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(_installPath + "\\"))
            {
                MessageBox.Show("Invalid installation path. Folder does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(_installPath + "\\Engine") || !Directory.Exists(_installPath + "\\FortniteGame"))
            {
                MessageBox.Show("Invalid installation path. Make sure the \"FortniteGame\" and \"Engine\" folders exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!File.Exists(_installPath + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe"))
            {
                MessageBox.Show("Fortnite was not found within the install folder.\n\nYour version is likely corrupt. Re-download Fortnite and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            FortniteEntrySchema versionEntry = new FortniteEntrySchema(_version, _installPath);
            Settings.AddFortniteEntry(versionEntry);
            AddFortniteVersionToList(versionEntry);
            _popup?.Close();
        }

        private async void Launch(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (Settings.FortniteSelectedIndex() == -1)
            {
                Toast.ShowError("Please select a Fortnite version to launch");
                return;
            }

            string installPath = Settings.FortniteEntries()[Settings.FortniteSelectedIndex()].InstallPath;

            if (!File.Exists($"{installPath}\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe"))
            {
                Toast.ShowError("Your Fortnite install seems to be corrupt\nPlease reinstall Fortnite and try again.");
                return;
            }
            
            Process fnProcess = new();
            fnProcess.StartInfo.FileName = $"{installPath}\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe";
            fnProcess.StartInfo.Arguments = "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -EpicPortal -AUTH_TYPE=password -AUTH_LOGIN=Revvz -AUTH_PASSWORD=ed79a237cda964ee -noeac -fromfl=be -fltoken=f7b9gah4h5380d10f721dd6a";
            fnProcess.StartInfo.WorkingDirectory = $"{installPath}\\FortniteGame\\Binaries\\Win64";
            fnProcess.Start();

/*            LibraryMapper mapper = new LibraryMapper(fnProcess, "C:\\Users\\antonios\\Desktop\\repos\\Yeetnite\\x64\\Debug\\YeetniteClientDLL.dll");
            mapper.MapLibrary();*/

            LaunchButton.IsEnabled = false;
            LaunchButton.Content = "FORTNITE RUNNING";
            SettingsButton.IsEnabled = false;

            await fnProcess.WaitForExitAsync();
            fnProcess.Dispose();
            
            LaunchButton.IsEnabled = true;
            SettingsButton.IsEnabled = true;
            LaunchButton.Content = "Launch";
        }

        private void AddVersion(object sender, MouseButtonEventArgs e)
        {
            // Build out the popup window UI
            StackPanel popupUi = new()
            {
                Margin = new Thickness()
                {
                    Left = 15,
                    Right = 15,
                    Top = 10,
                    Bottom = 10,
                }
            };
            TextBlock titleText = new()
            {
                FontSize = 28,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "Add A Version",
                TextDecorations = TextDecorations.Underline,
            };
            TextBlock installPathText = new()
            {
                FontSize = 18,
                Margin = new Thickness()
                {
                    Top = 15,
                },
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Install Location:"
            };
            StackPanel locationEntryPanel = new()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness()
                {
                    Top = 10
                },
            };

            _installLocationEntry = new()
            {
                Width = 370,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                AcceptsTab = true,
                AllowDrop = false,
                AcceptsReturn = false
            };

            _installLocationEntry.TextChanged += InstallLocationTextChanged;

            ImageAwesome folderIcon = new()
            {
                Icon = FontAwesomeIcon.Folder,
                Foreground = new BrushConverter().ConvertFrom("#ffd664") as Brush,
                Height = 25,
                Margin = new Thickness()
                {
                    Left = 15,
                }
            };

            folderIcon.MouseEnter += SetPopupHoverMouse;
            folderIcon.MouseLeave += SetPopupRegularMouse;
            folderIcon.MouseEnter += (Sender, args) => folderIcon.Foreground = new BrushConverter().ConvertFrom("#ffc21a") as Brush;
            folderIcon.MouseLeave += (Sender, args) => folderIcon.Foreground = new BrushConverter().ConvertFrom("#ffd664") as Brush;
            folderIcon.MouseLeftButtonDown += SetPathToFolderSelection;

            locationEntryPanel.Children.Add(_installLocationEntry);
            locationEntryPanel.Children.Add(folderIcon);

            TextBlock versionText = new()
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
            
            _versionEntry = new TextBox
            {
                Width = 75,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                AcceptsTab = false,
                AllowDrop = false,
                AcceptsReturn = false
            };

            _versionEntry.TextChanged += VersionTextChanged;

            _addVersionButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness
                {
                    Top = 25
                },
                Content = "Add Version",
                FontSize = 18,
            };

            _addVersionButton.MouseEnter += SetPopupHoverMouse;
            _addVersionButton.MouseLeave += SetPopupRegularMouse;
            _addVersionButton.Click += SaveVersion;

            popupUi.Children.Add(titleText);
            popupUi.Children.Add(installPathText);
            popupUi.Children.Add(locationEntryPanel);
            popupUi.Children.Add(versionText);
            popupUi.Children.Add(_versionEntry);
            popupUi.Children.Add(_addVersionButton);

            _popup = new()
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Height = 375,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Width = 450,
                Background = new BrushConverter().ConvertFrom("#1f1f1f") as Brush,
                Content = popupUi
            };
            _popup.Show();
        }
    }
}

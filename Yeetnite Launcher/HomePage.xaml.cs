﻿using System.Diagnostics;
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
        public HomePage()
        {
            InitializeComponent();
            Fortnite_Background_Image.Source = new BitmapImage(new System.Uri(string.Format("/Assets/Home-Background/{0}.jpg", new System.Random().Next(1, 10)), System.UriKind.Relative));
        }

        private void SetHoverMouse(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ContentGrid.Cursor = Cursors.Hand;
        }

        private void SetRegularMouse(object sender, MouseEventArgs e)
        {
            ContentGrid.Cursor = Cursors.Arrow;
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
        }

        private void SetRegularDiscordLink(object sender, MouseEventArgs e)
        {
            SetRegularMouse(sender, e);
            Discord_Link.Foreground = new BrushConverter().ConvertFrom("#ffffff") as Brush;
        }

        private void SetHoveredPlusButton(object sender, MouseEventArgs e)
        {
            SetHoverMouse(sender, e);
            Plus_Icon.Foreground = SystemParameters.WindowGlassBrush;
        }

        private void SetRegularPlusButton(object sender, MouseEventArgs e)
        {
            SetRegularMouse(sender, e);
            Plus_Icon.Foreground = new BrushConverter().ConvertFrom("#ffffff") as Brush;
        }
    }
}

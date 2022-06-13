#include "pch.h"
#include "HomeWindow.xaml.h"
#if __has_include("HomeWindow.g.cpp")
#include "HomeWindow.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::YeetniteLauncher::implementation
{
    HomeWindow::HomeWindow()
    {
        InitializeComponent();
    }
}

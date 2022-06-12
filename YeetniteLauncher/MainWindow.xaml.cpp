#include "pch.h"
#include "MainWindow.xaml.h"
#if __has_include("MainWindow.g.cpp")
#include "MainWindow.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;
using namespace Windows::Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::YeetniteLauncher::implementation
{
	MainWindow::MainWindow()
	{
		InitializeComponent();
		try {
			hstring::const_pointer username = winrt::unbox_value<winrt::hstring>(ApplicationData::Current().LocalSettings().Values().Lookup(L"Username")).c_str();
			if (username != nullptr)
				rootFrame().Navigate(xaml_typename<HomePage>());
			else
				rootFrame().Navigate(xaml_typename<LoginPage>());
		}
		catch (hresult_no_interface e) {
			OutputDebugString(L"We failed again:\n\n");
			OutputDebugString(e.message().c_str());
			rootFrame().Navigate(xaml_typename<LoginPage>());
		}
	}
}

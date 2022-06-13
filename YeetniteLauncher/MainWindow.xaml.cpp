#include "pch.h"
#include "MainWindow.xaml.h"
#include "nlohmann/json.hpp"
#include "winrt/Windows.Web.Http.h"
#include "winrt/Windows.ApplicationModel.Core.h"
#include <format>
#if __has_include("MainWindow.g.cpp")
#include "MainWindow.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;
using namespace Windows::Storage;
using namespace Windows::ApplicationModel::Core;
using namespace nlohmann;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::YeetniteLauncher::implementation
{
	MainWindow::MainWindow()
	{
		InitializeComponent();
	}

	Windows::Foundation::IAsyncAction MainWindow::Login(Windows::Foundation::IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& e)
	{
		progressRing().IsActive(true);
		loginButton().IsEnabled(false);

		Windows::Foundation::Uri uri{ std::format(L"https://www.yeetnite.ml/api/user?username={}&password={}", Username().Text().c_str(), Password().Password().c_str()).c_str() };
		Windows::Web::Http::HttpClient httpClient{};

		Controls::ContentDialog errorDialog{};
		errorDialog.Title(box_value(L"Error"));
		Controls::TextBlock text{};
		errorDialog.PrimaryButtonText(L"OK");
		errorDialog.XamlRoot(this->Content().XamlRoot());

		try
		{
			hstring response{ co_await httpClient.GetStringAsync(uri) };
			json responseJson = json::parse(response);
			if (!responseJson.at("success").get<bool>()) {
				text.Text(L"Invalid username or password.");
				errorDialog.Content(text);
				errorDialog.ShowAsync();
				progressRing().IsActive(false);
				loginButton().IsEnabled(true);
				httpClient.Close();
				co_return;
			}
			ApplicationDataContainer localSettings{ ApplicationData::Current().LocalSettings() };
			Windows::Foundation::Collections::IPropertySet values{ localSettings.Values() };
			std::string username = responseJson.at("user").at("username").get<std::string>();
			values.Insert(L"Username", Windows::Foundation::PropertyValue::CreateString(std::wstring(username.begin(), username.end())));
		}
		catch (hresult_error const&)
		{
			text.Text(L"Failed to make network request. Check your internet and try again.");
			errorDialog.Content(text);
			errorDialog.ShowAsync();
		}

		progressRing().IsActive(false);
		loginButton().IsEnabled(true);
		httpClient.Close();
	}
}

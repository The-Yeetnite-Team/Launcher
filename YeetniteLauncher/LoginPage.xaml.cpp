#include "pch.h"
#include "LoginPage.xaml.h"
#if __has_include("LoginPage.g.cpp")
#include "LoginPage.g.cpp"
#endif
#include <format>
#include "winrt/Windows.Web.Http.h"
#include "nlohmann/json.hpp"
using namespace nlohmann;

using namespace winrt;
using namespace Microsoft::UI::Xaml;
using namespace Windows::Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::YeetniteLauncher::implementation
{
	LoginPage::LoginPage()
	{
		InitializeComponent();
	}

	Windows::Foundation::IAsyncAction LoginPage::Login(IInspectable const& sender, RoutedEventArgs const& e)
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
			winrt::hstring response{ co_await httpClient.GetStringAsync(uri) };
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
			ApplicationDataContainer roamingSettings{ ApplicationData::Current().RoamingSettings() };
			auto values{ roamingSettings.Values() };
			std::string username = responseJson.at("user").at("username").get<std::string>();
			values.Insert(L"Username", Windows::Foundation::PropertyValue::CreateString(std::wstring(username.begin(), username.end())));
			winrt::hstring usernameFromSettings{ winrt::unbox_value<winrt::hstring>(values.Lookup(L"Username")) };
			OutputDebugString(usernameFromSettings.c_str());
			loginFrame().Navigate(xaml_typename<HomePage>());
		}
		catch (winrt::hresult_error const&)
		{
			OutputDebugString(L"Failed to make network request.\n");
			text.Text(L"Failed to make network request. Check your internet and try again.");
			errorDialog.Content(text);
			errorDialog.ShowAsync();
		}

		progressRing().IsActive(false);
		loginButton().IsEnabled(true);
		httpClient.Close();
	}
}

#pragma once

#include "LoginPage.g.h"

namespace winrt::YeetniteLauncher::implementation
{
    struct LoginPage : LoginPageT<LoginPage>
    {
        LoginPage();
        Windows::Foundation::IAsyncAction Login(winrt::Windows::Foundation::IInspectable const& sender, winrt::Microsoft::UI::Xaml::RoutedEventArgs const& e);
    };
}

namespace winrt::YeetniteLauncher::factory_implementation
{
    struct LoginPage : LoginPageT<LoginPage, implementation::LoginPage>
    {
    };
}

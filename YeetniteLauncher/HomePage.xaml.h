#pragma once

#include "HomePage.g.h"

namespace winrt::YeetniteLauncher::implementation
{
    struct HomePage : HomePageT<HomePage>
    {
        HomePage();

        void myButton_Click(Windows::Foundation::IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& args);
    };
}

namespace winrt::YeetniteLauncher::factory_implementation
{
    struct HomePage : HomePageT<HomePage, implementation::HomePage>
    {
    };
}

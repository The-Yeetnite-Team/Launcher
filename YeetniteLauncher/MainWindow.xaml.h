#pragma once

#include "MainWindow.g.h"

namespace winrt::YeetniteLauncher::implementation
{
    struct MainWindow : MainWindowT<MainWindow>
    {
        MainWindow();
        Windows::Foundation::IAsyncAction Login(Windows::Foundation::IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& e);
    };
}

namespace winrt::YeetniteLauncher::factory_implementation
{
    struct MainWindow : MainWindowT<MainWindow, implementation::MainWindow>
    {
    };
}

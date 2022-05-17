#pragma once

#include "MainWindow.g.h"

namespace winrt::YeetniteLauncher::implementation
{
    struct MainWindow : MainWindowT<MainWindow>
    {
        MainWindow();
    };
}

namespace winrt::YeetniteLauncher::factory_implementation
{
    struct MainWindow : MainWindowT<MainWindow, implementation::MainWindow>
    {
    };
}

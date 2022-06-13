#pragma once

#include "HomeWindow.g.h"

namespace winrt::YeetniteLauncher::implementation
{
    struct HomeWindow : HomeWindowT<HomeWindow>
    {
        HomeWindow();
    };
}

namespace winrt::YeetniteLauncher::factory_implementation
{
    struct HomeWindow : HomeWindowT<HomeWindow, implementation::HomeWindow>
    {
    };
}

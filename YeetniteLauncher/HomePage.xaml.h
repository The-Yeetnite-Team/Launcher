#pragma once

#include "HomePage.g.h"

namespace winrt::YeetniteLauncher::implementation
{
    struct HomePage : HomePageT<HomePage>
    {
        HomePage();
    };
}

namespace winrt::YeetniteLauncher::factory_implementation
{
    struct HomePage : HomePageT<HomePage, implementation::HomePage>
    {
    };
}

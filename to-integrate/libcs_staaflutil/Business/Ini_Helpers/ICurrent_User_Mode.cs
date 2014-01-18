using System;
using System.Collections.Generic;
using Common;
using Fairweather.Service;

namespace DTA
{
    public interface ICurrent_User_Mode
    {
        Common.User_Level Current_User_Mode { get; }
    }
}

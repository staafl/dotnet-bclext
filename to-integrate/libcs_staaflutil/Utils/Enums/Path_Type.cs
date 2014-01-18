using System;

namespace Fairweather.Service
{
    [Flags]
    public enum Path_Type
    {
        None,
        Directory,
        File,
        Network_Share,
    }
}

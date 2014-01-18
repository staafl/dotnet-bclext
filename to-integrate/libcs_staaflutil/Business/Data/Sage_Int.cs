using System;
using System.Collections.Generic;
using System.IO;
using Fairweather.Service;

namespace Common
{
    public static partial class Data
    {
        public const int
        ALL_OK = 0,
        FILE_FAILED_TO_VALIDATE = 1,
        LOGIC_ERROR = 2,
        LICENSE_ERROR = 3,
        DATAPATH_ERROR = 4,
        WRONG_PARAMETERS = 5,
        FILE_FAILED_TO_INTERFACE = 6,
        CONNECTION_ERROR = 7,
        OTHER_ERROR = 8;
    }
}
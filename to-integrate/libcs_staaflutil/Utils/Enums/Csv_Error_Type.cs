using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fairweather.Service
{
    public enum Csv_Error_Type
    {
        Unknown,
        Runaway_Quote,
        Trailing_Chars,
        Unexpected_Quote,
        Trailing_Quote,
        Invalid_Field_Count,
    }
}

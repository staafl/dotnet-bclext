using System;
using System.Collections.Generic;
using Fairweather.Service;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace Common
{
    static partial class Data
    {
        public static string Date_Format(bool sortable) {
            return sortable ? Data.SORT_DATE_FORMAT : Data.DEF_DATE_FORMAT;
        }

        public const string DEF_DATE_FORMAT = "dd/MM/yyyy";
        public const string SORT_DATE_FORMAT = "yyyy/MM/dd";
        public const int DEF_DISPLAY_PRECISION = 2;
    }
}

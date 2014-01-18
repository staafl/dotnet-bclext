using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Common;
using Versioning;

using Fairweather.Service;
using D = System.Collections.Generic.Dictionary<string, string>;
using Standardization;
using Line = Fairweather.Service.Pair<Common.Posting.Data_Line, Sage_Int.Line_Data>;
using Common.Posting;

namespace Sage_Int
{
    public interface I_SIT_Exe
    {
        void Warn(string str);
        void Help(Ini_File ini, Sit_General_Settings sett_global);
        bool RetryIgnoreFail(Func<bool> f, Func<string> msg, bool can_ignore, out bool ignore);
        void Tell(bool scan, Record_Type mode, object ix, Line_Data line);
        bool YesNo(Func<string> msg);
        void Clear();
        void Prepare(SIT_Engine engine);

    }
}

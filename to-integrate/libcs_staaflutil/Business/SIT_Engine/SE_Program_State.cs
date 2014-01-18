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

namespace Sage_Int
{
    partial class SIT_Engine
    {

        // Mutex mut;

        SDO_Engine sdo;
        Work_Space ws;

        Triple_Dict<Record_Type, string, string, string>
        mappings = new Triple_Dict<Record_Type, string, string, string>();




    }
}

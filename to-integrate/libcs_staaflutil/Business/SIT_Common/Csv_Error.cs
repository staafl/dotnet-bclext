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
using Common.Posting;
using Line = Fairweather.Service.Pair<Common.Posting.Data_Line, Sage_Int.Line_Data>;

namespace Sage_Int
{
    public class Csv_Error
    {
        public int Column { get; private set; }
        public bool Fatal { get; private set; }
        public string Description { get; private set; }
        public bool Parsed { get; private set; }
        public Csv_Error_Type Error_Type { get; private set; }

        public Csv_Error(Csv_Error_Type error_type, string description, int column, bool fatal, bool parsed) {
            this.Description = description;
            this.Column = column;
            this.Fatal = fatal;
            this.Parsed = parsed;
            this.Error_Type = error_type;
        }

        public Csv_Error(XCsv csv)
            : this(csv.Error_Type, csv.Message, csv.Column, csv.Fatal, false) {
        }
    }
}

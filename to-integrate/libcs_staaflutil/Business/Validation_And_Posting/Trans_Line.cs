using System;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using Common.Sage;

namespace Common.Posting
{
    public class Trans_Line : Data_Line
    {

        public Trans_Line()
            : base() {
        }

        public Trans_Line(int capacity)
            : base(capacity) {
        }

        public Trans_Line(IDictionary<string, object> values)
            : base(values) {
        }

        public Trans_Line(Data_Line data)
            : base(data.Dict.Count) {
            this.Dict.Fill(data.Dict, false);
        }

        readonly SortedList<int, decimal> allocated = new SortedList<int, decimal>();

        public SortedList<int, decimal> Allocations {
            get {
                return allocated;
            }
        }
        public FF_Alloc_Type Alloc_Type {
            get;
            set;
        }
        public int? Trans_Ref {
            get;
            set;
        }

    }
}

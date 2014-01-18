using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;

namespace Common.Posting
{
    public class Always_Valid_Rule : IRule
    {
        public bool
        Inspect_Line(Data_Line line,
                     int ix,
bool is_last,
                out bool to_skip,
                out IEnumerable<Validation_Error> errors
                ) {

            errors = null;

            to_skip = false;

            return true;

        }

        public bool
        Are_Valid(IEnumerable<Data_Line> lines) {
            return true;
        }

        public void Clear() { }
    }
}

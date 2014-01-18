
using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;
namespace Common.Posting
{
    public interface IRule : IState
    {

        bool Inspect_Line(
                    Data_Line line,
                    int ix,
bool is_last,
            // in case of serious errors, such as failed type coercion
            // this allows us to skip some lines in further validation.
                     out bool skip,
                     out IEnumerable<Validation_Error> errors
                     );

        bool Are_Valid(IEnumerable<Data_Line> lines);

    }

}
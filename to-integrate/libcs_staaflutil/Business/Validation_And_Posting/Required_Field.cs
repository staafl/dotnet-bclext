using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;

namespace Common.Posting
{
    /// <summary>
    /// Allows you to specify that a field is required or not allowed.
    /// </summary>
    public class Required_Field_Rule : Rule_Base
    {

        /// <summary>
        /// Creates an instance which represents a required field 'field'.
        /// </summary>
        public Required_Field_Rule(string field)
            : this(field, true) {
        }
        /// <summary>
        /// If 'allowed', creates an instance which represents a required field 'field'.
        /// Else, creates an instance which represents a prohibited field 'field'.
        /// </summary>
        public Required_Field_Rule(string field, bool required)
            : this(field, required ? STR_RequiredFieldLeftBlank : STR_YouAreNotAllowedToEnterAValueInField, required) {
        }
        /// <summary>
        /// If 'allowed', creates an instance which represents a required field 'field'.
        /// Else, creates an instance which represents a prohibited field 'field'.
        /// 'msg' is the message for the error, {0} will be replaced with the field name.
        /// </summary>
        Required_Field_Rule(string field, string msg, bool required) {
            this.field = field;
            this.required = required;
            this.msg = msg.spf(field);
        }

        const string STR_RequiredFieldLeftBlank = "Required field '{0}' left blank.";
        const string STR_YouAreNotAllowedToEnterAValueInField = "You are not allowed to enter a value in field '{0}'.";
        readonly string field;
        readonly string msg;
        readonly bool required;


        public override bool
        Inspect_Line(Data_Line line,
                     int ix,
                     bool is_last,
                out bool to_skip,
                out IEnumerable<Validation_Error> errors
                ) {

            H.assign(out to_skip, out errors);
            string error_msg = null;

            object val;

            if (!line.Dict.TryGetValue(field, out val))
                return true;

            var as_str = val.strdef("");

            bool present = as_str.Trim() != "";

            if (present != required)
                error_msg = msg;

            if (error_msg == null)
                return true;

            var error = new Validation_Error(ix, error_msg);
            errors = new[] { error };
            return false;
        }

    }
}

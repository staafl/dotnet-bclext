using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Fairweather.Service;
using System.Diagnostics;

namespace Common.Posting
{
    [DebuggerStepThrough]
    [Serializable]
    public struct Validation_Error
    {

        public Validation_Error(int main_row, string description)
            : this(main_row, description, new int[0]) {
        }
        public Validation_Error(int main_row,
                    string description,
                    params int[] affected_rows)
            : this(main_row, description, (IEnumerable<int>)affected_rows) {


        }

        public Validation_Error(int main_row,
                    string description,
                    IEnumerable<int> affected_rows)
            : this() {

            affected_rows = affected_rows ?? new int[0];
            this.Main_Row = main_row;
            this.Description = description;
            this.Affected_Rows = affected_rows;
            if (!affected_rows.Contains(main_row))
                this.Affected_Rows = this.Affected_Rows.Pend(main_row, true);
        }


        /// <summary>
        /// The row with which this error is most strongly
        /// associated.
        /// </summary>
        public int Main_Row {
            get;
            private set;
        }

        /// <summary>
        /// User-friendly description of the error.
        /// </summary>
        public string Description {
            get;
            private set;
        }

        /// <summary>
        /// All rows related to this error.
        /// </summary>
        public IEnumerable<int> Affected_Rows {
            get;
            private set;
        }



        /* Boilerplate */

        public override string ToString() {

            string ret = "";
            const string _null = "[null]";

#pragma warning disable 472


            ret += "Main_Row = " + this.Main_Row == null ? _null : this.Main_Row.ToString();
            ret += ", ";
            ret += "Description = " + this.Description == null ? _null : this.Description.ToString();
            ret += ", ";
            ret += "Affected_Rows = " + this.Affected_Rows == null ? _null : this.Affected_Rows.ToString();


#pragma warning restore

            ret = "{Validation_Error: " + ret + "}";
            return ret;

        }


        public bool Equals(Validation_Error obj2) {

#pragma warning disable 472


            if (this.Main_Row == null) {
                if (obj2.Main_Row != null)
                    return false;
            }
            else if (!this.Main_Row.Equals(obj2.Main_Row)) {
                return false;
            }

            if (this.Description == null) {
                if (obj2.Description != null)
                    return false;
            }
            else if (!this.Description.Equals(obj2.Description)) {
                return false;
            }

            if (this.Affected_Rows == null) {
                if (obj2.Affected_Rows != null)
                    return false;
            }
            else if (!this.Affected_Rows.Equals(obj2.Affected_Rows)) {
                return false;
            }

#pragma warning restore
            return true;
        }


        public override bool Equals(object obj2) {

            if (obj2 == null)
                return false;

            if (!(obj2 is Validation_Error))
                return false;

            var ret = this.Equals((Validation_Error)obj2);

            return ret;

        }


        public static bool operator ==(Validation_Error left, Validation_Error right) {

            var ret = left.Equals(right);
            return ret;

        }


        public static bool operator !=(Validation_Error left, Validation_Error right) {

            var ret = !left.Equals(right);
            return ret;

        }


        public override int GetHashCode() {

#pragma warning disable 472
            unchecked {
                int ret = 23;
                int temp;


                if (this.Main_Row != null) {
                    ret *= 31;
                    temp = this.Main_Row.GetHashCode();
                    ret += temp;

                }

                if (this.Description != null) {
                    ret *= 31;
                    temp = this.Description.GetHashCode();
                    ret += temp;

                }

                if (this.Affected_Rows != null) {
                    ret *= 31;
                    temp = this.Affected_Rows.GetHashCode();
                    ret += temp;

                }

                return ret;

            } // unchecked block
#pragma warning restore
        } // method


    }
}

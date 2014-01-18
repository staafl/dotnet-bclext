using System;
using System.Diagnostics;
namespace Fairweather.Service
{

    // autogenerated: D:\Scripts\struct_creator2.pl
    [Serializable]
    [DebuggerStepThrough]
    public struct Command_Line_Argument
    {

        public Command_Line_Argument(string as_string)
            : this() {

            this.As_String = as_string;

        }


        public string As_String {
            get;
            private set;
        }

        public int? As_Int {
            get {
                int ret;

                if (int.TryParse(As_String, out ret))
                    return ret;

                return null;
            }
        }

        public uint? As_Uuint {
            get {
                uint ret;

                if (uint.TryParse(As_String, out ret))
                    return ret;

                return null;
            }
        }

        public decimal? As_Decimal {
            get {
                decimal ret;

                if (decimal.TryParse(As_String, out ret))
                    return ret;

                return null;
            }
        }

        public DateTime? As_DateTime {
            get {
                DateTime ret;

                if (DateTime.TryParse(As_String, out ret))
                    return ret;

                return null;
            }
        }

        //public string As_File {
        //    get;
        //    private set;
        //}

        //public string As_Directory {
        //    get;
        //    private set;
        //}

        //public string As_Path {
        //    get;
        //    private set;
        //}

        //public string As_Url {
        //    get;
        //    private set;
        //}



        /* Boilerplate */

        public override string ToString() {

            return As_String;

        }



    }
}

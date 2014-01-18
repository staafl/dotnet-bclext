using System;
using System.Diagnostics;
using Fairweather.Service;
using Standardization;
namespace Common
{
    /// <summary>
    /// The name and description of a memorized invoice
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    public struct Memorize_Header
    {

        public Memorize_Header(string name,
                               string description,
                               DateTime invoice_day)
            : this() {

            Invoice_Date = invoice_day.Date;
            Name = name;
            Description = description;

        }

        public string Description {
            get;
            set;
        }


        public string Name {
            get;
            set;
        }

        public DateTime Invoice_Date {
            get;
            set;
        }

        public static Simple_Serializer<Memorize_Header> Formatter {
            get {
                return new Simple_Serializer<Memorize_Header>(
                    5 /* leave some room for growth */,
                    "# ", /* in case of csv */
                    _hdr => new[] { _hdr.Name, _hdr.Description, _hdr.Invoice_Date.ToString(true) }
                    ,
                    _seq =>
                    {
                        var arr = _seq.arr();
                        return new Memorize_Header(arr[0], arr[1], arr[2].ToDateTime());
                    })
                    ;
            }
        }

    }
}

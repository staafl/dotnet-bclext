using System;
using System.IO;

namespace Fairweather.Service
{
    public class Prefix_Writer : Writer_Decorator
    {
        const string def_prefix_str = ">>> ";

        public Prefix_Writer(TextWriter writer)
            : this(writer, def_prefix_str) {

        }


        public Prefix_Writer(TextWriter writer, string prefix)
            : this(writer, () => prefix) {
        }
        public Prefix_Writer(TextWriter writer, Func<string> prefix)
            : base(writer) {
            this.Prefix = prefix;
        }


        // ****************************



        public Func<string> Prefix {
            get;
            set;
        }


        // ****************************




        protected override void Line_Action() {
            writer.Write(Prefix());
        }


    }
}

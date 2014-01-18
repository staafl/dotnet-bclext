using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public struct Nominal_Record
    {
        public Nominal_Record(string description,
            string reference)
            : this() {

            this.Description = description;
            this.Reference = reference;

        }


        public string Description {
            get;
            set;
        }

        public string Reference {
            get;
            set;
        }



    }
}

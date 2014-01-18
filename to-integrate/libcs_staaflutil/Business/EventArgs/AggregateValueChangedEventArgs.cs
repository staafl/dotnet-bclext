using System;
using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public class AggregateValueChangedEventArgs : EventArgs
    {
        readonly string name;
        public string Name {
            get { return name; }
        }

        public AggregateValueChangedEventArgs(string name) {

            this.name = name;
        }
    }
}

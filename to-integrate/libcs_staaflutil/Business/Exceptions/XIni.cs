using System;
using System.Diagnostics;
using Common;

namespace DTA
{
    [DebuggerStepThrough]
    public class XIni : XOur
    {
        public enum Fault_Type
        {
            File_Missing = 0,
            File_Corrupted = 1,
        };
        public readonly Fault_Type Type;

        public XIni() { }

        public XIni(string message)
            : base(message) {
        }

        public XIni(Fault_Type type) {
            Type = type;
        }

        public XIni(string message, Fault_Type type)
            : base(message) {
            Type = type;
        }

        public XIni(string message, Exception inner)
            : base(message, inner) {
        }

        public XIni(string msg, Exception inner, Fault_Type type)
            : base(msg, inner) {
            Type = type;
        }

    }
}

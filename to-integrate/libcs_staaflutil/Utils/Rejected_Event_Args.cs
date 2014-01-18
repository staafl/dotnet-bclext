using System;

namespace Fairweather.Service
{
    public static class Rejected_Event_Args
    {
        public static Rejected_Event_Args<TValue> Make<TValue>(TValue proposed) {

            var ret = new Rejected_Event_Args<TValue>(proposed);

            return ret;

        }
        public static Rejected_Event_Args<TValue> Make<TValue>(TValue proposed, string message) {

            var ret = new Rejected_Event_Args<TValue>(proposed, message);

            return ret;

        }
    }

    public class Rejected_Event_Args<TValue> : EventArgs
    {
        const string def_message = "Value failed to validate.";

        readonly TValue m_proposed;
        readonly string m_message;

        public TValue Proposed {
            get { return m_proposed; }
        }

        public string Message {
            get { return m_message; }
        }

        public Rejected_Event_Args(TValue proposed) : this(proposed, def_message) {


        }

        public Rejected_Event_Args(TValue proposed, string message) {

            m_proposed = proposed;
            m_message = message;

        }


    }
}

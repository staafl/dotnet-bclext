using System;
using System.Diagnostics;


namespace Fairweather.Service
{
    public static class IO_Pair
    {
#if NEVER
        public static 
        IO_Pair<TIn, TOut> Make<TIn, TOut>(Action<TIn> input,
                                           Func<TOut> output) {

            var ret = new IO_Pair<TIn, TOut>(input, output);
            return ret;
        }
#endif

        public static IO_Pair<T> Make<T>(Action<T> input,
                                         Func<T> output) {

            var ret = new IO_Pair<T>(input, output);
            return ret;
        }

        public static IO_Pair<T> Make<T>(Func<T> output, Action<T> input) {

            var ret = new IO_Pair<T>(input, output);
            return ret;
        }
    }

    [DebuggerStepThrough]
    public struct IO_Pair<T>
    {

        readonly Action<T> m_input;

        readonly Func<T> m_output;

        public Action<T> Input {
            get {
                return this.m_input;
            }
        }

        public Func<T> Output {
            get {
                return this.m_output;
            }
        }


        public IO_Pair(Action<T> input,
                       Func<T> output) {

            input.tifn();
            output.tifn();

            this.m_input = input;
            this.m_output = output;
        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "input = " + this.m_input;
            ret += ", ";
            ret += "output = " + this.m_output;

            ret = "{IO_Pair<T>: " + ret + "}";
            return ret;

        }

        #region IO_Pair<T>


        public bool Equals(IO_Pair<T> obj2) {

            if (!this.m_input.Equals(obj2.m_input))
                return false;

            if (!this.m_output.Equals(obj2.m_output))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is IO_Pair<T>);

            if (ret)
                ret = this.Equals((IO_Pair<T>)obj2);


            return ret;

        }

        public static bool operator ==(IO_Pair<T> left, IO_Pair<T> right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(IO_Pair<T> left, IO_Pair<T> right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_input.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_output.GetHashCode();
                ret += temp;

                return ret;
            }
        }

        #endregion
    }

    #region IO_Pair<TIn, TOut>

#if NEVER

    [DebuggerStepThrough]
    public struct IO_Pair<TIn, TOut>
    {

        readonly Action<TIn> m_input;

        readonly Func<TOut> m_output;


        public Action<TIn> Input {
            get {
                return this.m_input;
            }
        }

        public Func<TOut> Output {
            get {
                return this.m_output;
            }
        }


        public IO_Pair(Action<TIn> input,
                    Func<TOut> output) {
            this.m_input = input;
            this.m_output = output;
        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "input = " + this.m_input;
            ret += ", ";
            ret += "output = " + this.m_output;

            ret = "{IO_Pair<TIn, TOut>: " + ret + "}";
            return ret;

        }

        public bool Equals(IO_Pair<TIn, TOut> obj2) {

            if (!this.m_input.Equals(obj2.m_input))
                return false;

            if (!this.m_output.Equals(obj2.m_output))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is IO_Pair<TIn, TOut>);

            if (ret)
                ret = this.Equals((IO_Pair<TIn, TOut>)obj2);


            return ret;

        }

        public static bool operator ==(IO_Pair<TIn, TOut> left, IO_Pair<TIn, TOut> right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(IO_Pair<TIn, TOut> left, IO_Pair<TIn, TOut> right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_input.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_output.GetHashCode();
                ret += temp;

                return ret;
            }
        }

    }

#endif
    #endregion
}
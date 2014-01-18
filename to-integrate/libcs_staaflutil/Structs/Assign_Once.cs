using System;
using System.Diagnostics;

namespace Fairweather.Service
{
    [DebuggerStepThrough]

    /*       Mutable        */
    public struct Assign_Once<T>
    {

        Assign_Once(T value) {
            this.m_value = value;
            this.m_assigned = true;
        }

        /// <summary>
        /// Do not use!
        /// </summary>
        /// <param name="assign"></param>
        /// <returns></returns>
        public static explicit operator T(Assign_Once<T> assign) {
            return assign.Value;
        }

        public static explicit operator Assign_Once<T>(T value) {
            return new Assign_Once<T>(value);
        }


        readonly T m_value;

        readonly bool m_assigned;

        public T Value_Or_Default() {

            return this.m_value;

        }

        public T Value {
            get {
                Assigned.tiff();

                return this.m_value;
            }
        }

        public bool Assigned {
            get {
                return this.m_assigned;
            }
        }

        public void Assign(Func<T> f) {

            if (this.m_assigned)
                return;

            this = new Assign_Once<T>(f());

        }

        public void Assign(T value) {

            if (this.m_assigned)
                return;

            this = new Assign_Once<T>(value);

        }




        /* Boilerplate */
        #region Assign_Once<T>


        public override string ToString() {

            string ret = "";

            ret += "value = " + this.m_value;
            ret += ", ";
            ret += "assigned = " + this.m_assigned;

            ret = "{Assign_Once<T>: " + ret + "}";
            return ret;

        }

        public bool Equals(Assign_Once<T> obj2) {

            if (this.m_value == null) {
                if (obj2.m_value != null)
                    return false;
            }
            else if (!this.m_value.Equals(obj2.m_value))
                return false;

            if (!this.m_assigned.Equals(obj2.m_assigned))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Assign_Once<T>);

            if (ret)
                ret = this.Equals((Assign_Once<T>)obj2);


            return ret;

        }

        public static bool operator ==(Assign_Once<T> left, Assign_Once<T> right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Assign_Once<T> left, Assign_Once<T> right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            if (!m_assigned)
                return 0;

            return this.m_value.GetHashCode();

        }

        #endregion
    }
}

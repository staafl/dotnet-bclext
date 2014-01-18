using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Fairweather.Service
{

    [Serializable]
    
    [DebuggerStepThrough]
    public struct Pair<T1, T2> : IPair<T1, T2>
    {

        public Pair(T1 first, T2 second) {
            this.m_first = first;
            this.m_second = second;
        }

        public T1 First {
            get { return m_first; }
        }
        public T2 Second {
            get { return m_second; }
        }

        readonly T1 m_first;
        readonly T2 m_second;

        public static implicit operator KeyValuePair<T1, T2>(Pair<T1, T2> pair) {
            return new KeyValuePair<T1, T2>(pair.First, pair.Second);
        }

        public static implicit operator Pair<T1, T2>(KeyValuePair<T1, T2> kvp) {
            return new Pair<T1, T2>(kvp.Key, kvp.Value);
        }


        public object[] ToArray() {
            return new object[] { First, Second };
        }


        public override string ToString() {

            string ret = "";

            ret += "first = " + this.m_first;
            ret += ", ";
            ret += "second = " + this.m_second;

            ret = "{Pair<,>: " + ret + "}";

            return ret;

        }

        public bool Equals(Pair<T1, T2> obj2) {

            if (!this.m_first.Safe_Equals(obj2.m_first))
                return false;

            if (!this.m_second.Safe_Equals(obj2.m_second))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Pair<T1, T2>);

            if (ret)
                ret = this.Equals((Pair<T1, T2>)obj2);


            return ret;

        }

        public static bool operator ==(Pair<T1, T2> left, Pair<T1, T2> right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Pair<T1, T2> left, Pair<T1, T2> right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            int ret = 0;
            int temp;

            if (m_first != null) {
                ret *= 31;
                temp = this.m_first.GetHashCode();
                ret += temp;
            }
            if (m_second != null) {
                ret *= 31;
                temp = this.m_second.GetHashCode();
                ret += temp;
            }
            return ret;
        }




    }

    [Serializable]
    
    [DebuggerStepThrough]
    public struct Triple<T1, T2, T3> : ITriple<T1, T2, T3>
    {

        readonly T1 m_first;

        readonly T2 m_second;

        readonly T3 m_third;


        public T1 First {
            get {
                return this.m_first;
            }
        }

        public T2 Second {
            get {
                return this.m_second;
            }
        }

        public T3 Third {
            get {
                return this.m_third;
            }
        }


        public Triple(T1 first,
                    T2 second,
                    T3 third) {
            this.m_first = first;
            this.m_second = second;
            this.m_third = third;
        }


        public object[] ToArray() {
            return new object[] { First, Second, Third };
        }

        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "first = " + this.m_first;
            ret += ", ";
            ret += "second = " + this.m_second;
            ret += ", ";
            ret += "third = " + this.m_third;

            ret = "{Triple<,,>: " + ret + "}";
            return ret;

        }


        #region Triple<TFirDst, T2, T3>

        public bool Equals(Triple<T1, T2, T3> obj2) {

            if (!this.m_first.Safe_Equals(obj2.m_first))
                return false;

            if (!this.m_second.Safe_Equals(obj2.m_second))
                return false;

            if (!this.m_third.Safe_Equals(obj2.m_third))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Triple<T1, T2, T3>);

            if (ret)
                ret = this.Equals((Triple<T1, T2, T3>)obj2);


            return ret;

        }

        public static bool operator ==(Triple<T1, T2, T3> left, Triple<T1, T2, T3> right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Triple<T1, T2, T3> left, Triple<T1, T2, T3> right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

#pragma warning disable
            unchecked {
                int ret = 23;
                int temp;

                if (this.m_first != null) {
                    ret *= 31;
                    temp = this.m_first.GetHashCode();
                    ret += temp;

                }

                if (this.m_second != null) {
                    ret *= 31;
                    temp = this.m_second.GetHashCode();
                    ret += temp;

                }

                if (this.m_third != null) {
                    ret *= 31;
                    temp = this.m_third.GetHashCode();
                    ret += temp;

                }

                return ret;
            }
#pragma warning restore
        }

        #endregion
    }



    [Serializable]
    
    [DebuggerStepThrough]
    public struct Quad<T1, T2, T3, T4> : IQuad<T1, T2, T3, T4>
    {
        readonly T1 m_first;

        readonly T2 m_second;

        readonly T3 m_third;

        readonly T4 m_fourth;


        public T1 First {
            get {
                return this.m_first;
            }
        }

        public T2 Second {
            get {
                return this.m_second;
            }
        }

        public T3 Third {
            get {
                return this.m_third;
            }
        }

        public T4 Fourth {
            get {
                return this.m_fourth;
            }
        }


        public Quad(T1 first,
                    T2 second,
                    T3 third,
                    T4 fourth) {
            this.m_first = first;
            this.m_second = second;
            this.m_third = third;
            this.m_fourth = fourth;
        }


        public object[] ToArray() {
            return new object[] { First, Second, Third, Fourth };
        }

        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "first = " + this.m_first;
            ret += ", ";
            ret += "second = " + this.m_second;
            ret += ", ";
            ret += "third = " + this.m_third;
            ret += ", ";
            ret += "fourth = " + this.m_fourth;

            ret = "{Quad<,,,>: " + ret + "}";

            return ret;

        }

        public bool Equals(Quad<T1, T2, T3, T4> obj2) {

            if (!this.m_first.Safe_Equals(obj2.m_first))
                return false;

            if (!this.m_second.Safe_Equals(obj2.m_second))
                return false;

            if (!this.m_third.Safe_Equals(obj2.m_third))
                return false;

            if (!this.m_fourth.Safe_Equals(obj2.m_fourth))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Quad<T1, T2, T3, T4>);

            if (ret)
                ret = this.Equals((Quad<T1, T2, T3, T4>)obj2);


            return ret;

        }

        public static bool operator ==(Quad<T1, T2, T3, T4> left,
                                       Quad<T1, T2, T3, T4> right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Quad<T1, T2, T3, T4> left,
                                       Quad<T1, T2, T3, T4> right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {

                int ret = 23;
                int temp;

                if (m_first != null) {
                    ret *= 31;
                    temp = this.m_first.GetHashCode();
                    ret += temp;
                }

                if (m_second != null) {
                    ret *= 31;
                    temp = this.m_second.GetHashCode();
                    ret += temp;
                }

                if (m_third != null) {
                    ret *= 31;
                    temp = this.m_third.GetHashCode();
                    ret += temp;
                }

                if (m_fourth != null) {
                    ret *= 31;
                    temp = this.m_fourth.GetHashCode();
                    ret += temp;
                }

                return ret;
            }
        }

    }




}

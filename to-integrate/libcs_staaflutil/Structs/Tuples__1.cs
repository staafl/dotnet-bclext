using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fairweather.Service
{
    [Serializable]
    
    [DebuggerStepThrough]
    public struct Pair<T> : IPair<T, T>
    {

        public Pair(T first,
                          T second) {
            this.m_first = first;
            this.m_second = second;
        }

        readonly T m_first;

        readonly T m_second;


        public T First {
            get {
                return this.m_first;
            }
        }

        public T Second {
            get {
                return this.m_second;
            }
        }


        public object[] ToArray() {
            return new object[] { First, Second};
        }



        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "first = " + this.m_first;
            ret += ", ";
            ret += "second = " + this.m_second;

            ret = "{Pair<T>: " + ret + "}";
            return ret;

        }

        public bool Equals(Pair<T> obj2) {

            if (!this.m_first.Safe_Equals(obj2.m_first))
                return false;

            if (!this.m_second.Safe_Equals(obj2.m_second))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Pair<T>);

            if (ret)
                ret = this.Equals((Pair<T>)obj2);


            return ret;

        }

        public static bool operator ==(Pair<T> left, Pair<T> right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Pair<T> left, Pair<T> right) {

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

                return ret;
            }
#pragma warning restore
        }


        public static implicit operator KeyValuePair<T, T>(Pair<T> pair) {
            return new KeyValuePair<T, T>(pair.First, pair.Second);
        }

        public static implicit operator Pair<T>(KeyValuePair<T, T> kvp) {
            return new Pair<T>(kvp.Key, kvp.Value);
        }


        public static explicit operator Pair<T, T>(Pair<T> pair) {

            var ret = new Pair<T, T>(pair.First, pair.Second);

            return ret;
        }

        public static implicit operator Pair<T>(Pair<T, T> pair) {

            var ret = new Pair<T>(pair.First, pair.Second);

            return ret;
        }



    }

    [Serializable]
    
    [DebuggerStepThrough]
    public struct Triple<T> : ITriple<T, T, T>
    {

        public Triple(T first,
            T second,
            T third) {

            this.m_first = first;
            this.m_second = second;
            this.m_third = third;

        }

        readonly T m_first;

        readonly T m_second;

        readonly T m_third;

        public T First {
            get {
                return this.m_first;
            }
        }

        public T Second {
            get {
                return this.m_second;
            }
        }

        public T Third {
            get {
                return this.m_third;
            }
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

            ret = "{Triple<T>: " + ret + "}";
            return ret;

        }

        public bool Equals(Triple<T> obj2) {

            if (!this.m_first.Safe_Equals(obj2.m_first))
                return false;

            if (!this.m_second.Safe_Equals(obj2.m_second))
                return false;

            if (!this.m_third.Safe_Equals(obj2.m_second))
                return false;

            return true;

        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Triple<T>);

            if (ret)
                ret = this.Equals((Triple<T>)obj2);


            return ret;

        }

        public static bool operator ==(Triple<T> left, Triple<T> right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Triple<T> left, Triple<T> right) {

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



        public static explicit operator Triple<T, T, T>(Triple<T> triple) {

            var ret = new Triple<T, T, T>(triple.m_first, triple.m_second, triple.m_third);

            return ret;
        }

        public static implicit operator Triple<T>(Triple<T, T, T> triple) {

            var ret = new Triple<T>(triple.First, triple.Second, triple.Third);

            return ret;

        }

    }

    [Serializable]
    
    [DebuggerStepThrough]
    public struct Quad<T> : IQuad<T, T, T, T>
    {
        public Quad(T first,
                    T second,
                    T third,
                    T fourth) {
            this.m_first = first;
            this.m_second = second;
            this.m_third = third;
            this.m_fourth = fourth;
        }


        readonly T m_first;

        readonly T m_second;

        readonly T m_third;

        readonly T m_fourth;


        public T First {
            get {
                return this.m_first;
            }
        }

        public T Second {
            get {
                return this.m_second;
            }
        }

        public T Third {
            get {
                return this.m_third;
            }
        }

        public T Fourth {
            get {
                return this.m_fourth;
            }
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

            ret = "{Quad<T>: " + ret + "}";
            return ret;

        }

        public bool Equals(Quad<T> obj2) {

            if (!this.m_first.Safe_Equals(obj2.m_first))
                return false;

            if (!this.m_second.Safe_Equals(obj2.m_second))
                return false;

            if (!this.m_third.Safe_Equals(obj2.m_second))
                return false;

            if (!this.m_fourth.Safe_Equals(obj2.m_fourth))
                return false;

            return true;

        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Quad<T>);

            if (ret)
                ret = this.Equals((Quad<T>)obj2);


            return ret;

        }

        public static bool operator ==(Quad<T> left, Quad<T> right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Quad<T> left, Quad<T> right) {

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

                if (this.m_fourth != null) {
                    ret *= 31;
                    temp = this.m_fourth.GetHashCode();
                    ret += temp;

                }

                return ret;
            }
#pragma warning restore
        }





        public static implicit operator Quad<T, T, T, T>(Quad<T> quad) {

            var ret = new Quad<T, T, T, T>(quad.m_first, quad.m_second, quad.m_third, quad.m_fourth);
            return ret;

        }

        public static implicit operator Quad<T>(Quad<T, T, T, T> quad) {

            var ret = new Quad<T>(quad.First, quad.Second, quad.Third, quad.Fourth);
            return ret;

        }

    }


}

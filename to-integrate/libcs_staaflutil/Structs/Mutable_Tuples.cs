using System;
using System.Diagnostics;

namespace Fairweather.Service
{
    // autogenerated: D:\Scripts\struct_creator2.pl
    [Serializable]
    [DebuggerStepThrough]
    public class Mutable_Pair<T1, T2> : IPair<T1, T2>
    {

        public Mutable_Pair(T1 first,
                    T2 second) {

            this.First = first;
            this.Second = second;
        }


        public T1 First {
            get;
            set;
        }

        public T2 Second {
            get;
            set;
        }

        public object[] ToArray() {
            return new object[] { First, Second };
        }

        /* Boilerplate */

        //        public override string ToString() {

        //            string ret = "";
        //            const string _null = "[null]";

        //#pragma warning disable 472


        //            ret += "First = " + this.First == null ? _null : this.First.ToString();
        //            ret += ", ";
        //            ret += "Second = " + this.Second == null ? _null : this.Second.ToString();


        //#pragma warning restore

        //            ret = "{Mutable_Pair<,>: " + ret + "}";
        //            return ret;

        //        }


        //        public bool Equals(Mutable_Pair<T1, T2> obj2) {

        //#pragma warning disable 472


        //            if (this.First == null) {
        //                if (obj2.First != null)
        //                    return false;
        //            }
        //            else if (!this.First.Equals(obj2.First)) {
        //                return false;
        //            }

        //            if (this.Second == null) {
        //                if (obj2.Second != null)
        //                    return false;
        //            }
        //            else if (!this.Second.Equals(obj2.Second)) {
        //                return false;
        //            }

        //#pragma warning restore
        //            return true;
        //        }


        //        public override bool Equals(object obj2) {

        //            if (obj2 == null)
        //                return false;

        //            if (!(obj2 is Mutable_Pair<T1, T2>))
        //                return false;

        //            var ret = this.Equals((Mutable_Pair<T1, T2>)obj2);

        //            return ret;

        //        }


        //        public static bool operator ==(Mutable_Pair<T1, T2> left, Mutable_Pair<T1, T2> right) {

        //            var ret = left.Equals(right);
        //            return ret;

        //        }


        //        public static bool operator !=(Mutable_Pair<T1, T2> left, Mutable_Pair<T1, T2> right) {

        //            var ret = !left.Equals(right);
        //            return ret;

        //        }


        //        public override int GetHashCode() {

        //#pragma warning disable 472
        //            unchecked {
        //                int ret = 23;
        //                int temp;


        //                if (this.First != null) {
        //                    ret *= 31;
        //                    temp = this.First.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.Second != null) {
        //                    ret *= 31;
        //                    temp = this.Second.GetHashCode();
        //                    ret += temp;

        //                }

        //                return ret;

        //            } // unchecked block
        //#pragma warning restore
        //        } // method


    }


    // autogenerated: D:\Scripts\struct_creator2.pl
    [Serializable]
    [DebuggerStepThrough]
    public class Mutable_Triple<T1, T2, T3> : ITriple<T1, T2, T3>
    {

        public Mutable_Triple(T1 first,
                    T2 second,
                    T3 third) {

            this.First = first;
            this.Second = second;
            this.Third = third;
        }


        public T1 First {
            get;
            set;
        }

        public T2 Second {
            get;
            set;
        }

        public T3 Third {
            get;
            set;
        }

        public object[] ToArray() {
            return new object[] { First, Second, Third };
        }

        /* Boilerplate */

        //        public override string ToString() {

        //            string ret = "";
        //            const string _null = "[null]";

        //#pragma warning disable 472


        //            ret += "First = " + this.First == null ? _null : this.First.ToString();
        //            ret += ", ";
        //            ret += "Second = " + this.Second == null ? _null : this.Second.ToString();
        //            ret += ", ";
        //            ret += "Third = " + this.Third == null ? _null : this.Third.ToString();


        //#pragma warning restore

        //            ret = "{Mutable_Triple<T1, T2, T3>: " + ret + "}";
        //            return ret;

        //        }


        //        public bool Equals(Mutable_Triple<T1, T2, T3> obj2) {

        //#pragma warning disable 472


        //            if (this.First == null) {
        //                if (obj2.First != null)
        //                    return false;
        //            }
        //            else if (!this.First.Equals(obj2.First)) {
        //                return false;
        //            }

        //            if (this.Second == null) {
        //                if (obj2.Second != null)
        //                    return false;
        //            }
        //            else if (!this.Second.Equals(obj2.Second)) {
        //                return false;
        //            }

        //            if (this.Third == null) {
        //                if (obj2.Third != null)
        //                    return false;
        //            }
        //            else if (!this.Third.Equals(obj2.Third)) {
        //                return false;
        //            }

        //#pragma warning restore
        //            return true;
        //        }


        //        public override bool Equals(object obj2) {

        //            if (obj2 == null)
        //                return false;

        //            if (!(obj2 is Mutable_Triple<T1, T2, T3>))
        //                return false;

        //            var ret = this.Equals((Mutable_Triple<T1, T2, T3>)obj2);

        //            return ret;

        //        }


        //        public static bool operator ==(Mutable_Triple<T1, T2, T3> left, Mutable_Triple<T1, T2, T3> right) {

        //            var ret = left.Equals(right);
        //            return ret;

        //        }


        //        public static bool operator !=(Mutable_Triple<T1, T2, T3> left, Mutable_Triple<T1, T2, T3> right) {

        //            var ret = !left.Equals(right);
        //            return ret;

        //        }


        //        public override int GetHashCode() {

        //#pragma warning disable 472
        //            unchecked {
        //                int ret = 23;
        //                int temp;


        //                if (this.First != null) {
        //                    ret *= 31;
        //                    temp = this.First.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.Second != null) {
        //                    ret *= 31;
        //                    temp = this.Second.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.Third != null) {
        //                    ret *= 31;
        //                    temp = this.Third.GetHashCode();
        //                    ret += temp;

        //                }

        //                return ret;

        //            } // unchecked block
        //#pragma warning restore
        //        } // method


    }

    // autogenerated: D:\Scripts\struct_creator2.pl
    [Serializable]
    [DebuggerStepThrough]
    public class Mutable_Quad<T1, T2, T3, T4> : IQuad<T1, T2, T3, T4>
    {

        public Mutable_Quad(T1 first,
                    T2 second,
                    T3 third,
                    T4 fourth) {

            this.First = first;
            this.Second = second;
            this.Third = third;
            this.Fourth = fourth;
        }


        public T1 First {
            get;
            set;
        }

        public T2 Second {
            get;
            set;
        }

        public T3 Third {
            get;
            set;
        }

        public T4 Fourth {
            get;
            set;
        }

        public object[] ToArray() {
            return new object[] { First, Second, Third, Fourth };
        }


        /* Boilerplate */

        //        public override string ToString() {

        //            string ret = "";
        //            const string _null = "[null]";

        //#pragma warning disable 472


        //            ret += "First = " + this.First == null ? _null : this.First.ToString();
        //            ret += ", ";
        //            ret += "Second = " + this.Second == null ? _null : this.Second.ToString();
        //            ret += ", ";
        //            ret += "Third = " + this.Third == null ? _null : this.Third.ToString();
        //            ret += ", ";
        //            ret += "Fourth = " + this.Fourth == null ? _null : this.Fourth.ToString();


        //#pragma warning restore

        //            ret = "{Mutable_Quad<T1, T2, T3, T4>: " + ret + "}";
        //            return ret;

        //        }


        //        public bool Equals(Mutable_Quad<T1, T2, T3, T4> obj2) {

        //#pragma warning disable 472


        //            if (this.First == null) {
        //                if (obj2.First != null)
        //                    return false;
        //            }
        //            else if (!this.First.Equals(obj2.First)) {
        //                return false;
        //            }

        //            if (this.Second == null) {
        //                if (obj2.Second != null)
        //                    return false;
        //            }
        //            else if (!this.Second.Equals(obj2.Second)) {
        //                return false;
        //            }

        //            if (this.Third == null) {
        //                if (obj2.Third != null)
        //                    return false;
        //            }
        //            else if (!this.Third.Equals(obj2.Third)) {
        //                return false;
        //            }

        //            if (this.Fourth == null) {
        //                if (obj2.Fourth != null)
        //                    return false;
        //            }
        //            else if (!this.Fourth.Equals(obj2.Fourth)) {
        //                return false;
        //            }

        //#pragma warning restore
        //            return true;
        //        }


        //        public override bool Equals(object obj2) {

        //            if (obj2 == null)
        //                return false;

        //            if (!(obj2 is Mutable_Quad<T1, T2, T3, T4>))
        //                return false;

        //            var ret = this.Equals((Mutable_Quad<T1, T2, T3, T4>)obj2);

        //            return ret;

        //        }


        //        public static bool operator ==(Mutable_Quad<T1, T2, T3, T4> left, Mutable_Quad<T1, T2, T3, T4> right) {

        //            var ret = left.Equals(right);
        //            return ret;

        //        }


        //        public static bool operator !=(Mutable_Quad<T1, T2, T3, T4> left, Mutable_Quad<T1, T2, T3, T4> right) {

        //            var ret = !left.Equals(right);
        //            return ret;

        //        }


        //        public override int GetHashCode() {

        //#pragma warning disable 472
        //            unchecked {
        //                int ret = 23;
        //                int temp;


        //                if (this.First != null) {
        //                    ret *= 31;
        //                    temp = this.First.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.Second != null) {
        //                    ret *= 31;
        //                    temp = this.Second.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.Third != null) {
        //                    ret *= 31;
        //                    temp = this.Third.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.Fourth != null) {
        //                    ret *= 31;
        //                    temp = this.Fourth.GetHashCode();
        //                    ret += temp;

        //                }

        //                return ret;

        //            } // unchecked block
        //#pragma warning restore
        //        } // method


    }

}

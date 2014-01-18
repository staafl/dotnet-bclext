

#if false
#pragma warning disable
namespace Screens
{
    public interface ILogicWrapper<T>
    {
        object GetProperty(string Name);
        T InnerObject {
            get;
        }
    }
    public class LogicWrapper : ILogicWrapper<string>
    {
        Dictionary<string, string> _properties;
        public object GetProperty(string Name) {
            return _properties[Name];
        }

        string _inner;
        public string InnerObject {
            get {
                return _inner;
            }
        }
        public LogicWrapper(string str) {
            _inner = str;
            _properties = new Dictionary<string, string>();
        }
    }
    static class Logic
    {
        public static bool Test() {

            bool alloc = true; //  NativeMethods.AllocConsole();
            int balance = 100;
            int age = 30;
            string title = "mrs";

            //Console.WriteLine((balance == 100).And((age <= 30).And(title == "mrs")));

            //IEnumerable<LogicWrapper> lst = new List<LogicWrapper> { new LogicWrapper("hello"), new LogicWrapper("bye") };
            Console.WriteLine(Parse("bbb < aa;"));
            Console.WriteLine(Parse("bbb <= aa;"));
            Console.WriteLine(Parse("bbb > aa;"));
            Console.WriteLine(Parse("bbb >= aa;"));
            Console.WriteLine(Parse("bbb == aa;"));

            Console.WriteLine(Parse("acucount == acucount && age == age;"));
            Console.WriteLine(Parse("aaa != aab && age == age && uuu == uuu && tt == t;"));
            //Console.WriteLine(Parse("acucount == acucount && age == age;"));
            //Console.WriteLine(Parse("acucount == acucount && age == age;"));
            //Console.WriteLine(Parse("acucount == acucount && age == age;"));

            Console.ReadKey();

            return true;// b_1;
        }

        public static Func<T, U, bool> ToOperator<T, U>(string op)
            where T : IComparable
            where U : IComparable {
            if (typeof(T) != typeof(U))
                return ((a, b) => false);

            switch (op) {
            case "==":
                return ((a, b) => a.Equals(b));
            case "!=":
                return ((a, b) => !a.Equals(b));
            case ">":
                return ((a, b) => a.CompareTo(b) == 1);
            case "<":
                return ((a, b) => a.CompareTo(b) == -1);
            case ">=":
                return ((a, b) => a.CompareTo(b) != -1);
            case "<=":
                return ((a, b) => a.CompareTo(b) != 1);
            default:
                return ((a, b) => false);
            }
        }

        public static Predicate<bool> ToLogic(this bool target, string op) {
            switch (op) {
            case "&&":
                return ((a) => target.And(a));
            case "||":
                return ((a) => target.Or(a));
            case "~|":
                return ((a) => target.Xor(a));
            case "=>":
                return ((a) => target.Follows(a));
            default:
                return ((a) => false);
            }
        }

        public static string ch_op = "<=>!";
        public static string ch_white = " \t\r\n";
        public static string ch_logic = "&~|";
        public static string ch_id = Enumerable.Range(((char)'a'), 26)
                                               .Aggregate("", (s, i) => s + (char)i + Char.ToUpper((char)i));
        public static string ch_num = "0123456789";

        public static bool Parse(String condition){
            Stack<Predicate<bool>> stack = new Stack<Predicate<bool>>();

            Predicate<bool> temp1 = ((x) => x);
            Predicate<bool> temp2 = ((x) => x);

            object obj = new object();
            Type t = typeof(int);

            string leftOp = "";
            string rightOp = "";
            bool right = false;

            Func<string, string, bool> op = ((x, y) => false);
            StringBuilder buffer = new StringBuilder();

            int ii = 0;
            int depth = 0;

            string cond = condition;
            int cnt = cond.Length;

            bool result = false;

            while (ii < cnt) {
                while (" \r\n\t".Contains(cond[ii])) {
                    ++ii;
                }

                if (cond[ii] == '(') {
                    stack.Push(temp1);
                    buffer.Length = 0;
                }
                else if (cond[ii] == ')') {
                    temp1 = stack.Pop();
                    temp2 = stack.Pop();

                }
                else if ("<>=!".Contains(cond[ii])) {
                    leftOp = buffer.ToString();
                    buffer.Length = 0;

                    while ("<>=!".Contains(cond[ii])) {
                        buffer.Append(cond[ii]);
                        ++ii;
                    }

                    op = ToOperator<string, string>(buffer.ToString());
                }
                else if ("&~|".Contains(cond[ii])) {
                    buffer.Length = 0;
                    while ("&~|".Contains(cond[ii])) {
                        buffer.Append(cond[ii]);
                        ++ii;
                    }
                    temp1 = temp1(op(leftOp, rightOp)).ToLogic(buffer.ToString());
                }
                else if (ch_id.Contains(cond[ii])) {
                    buffer.Length = 0;
                    while (ch_id.Contains(cond[ii])) {
                        buffer.Append(cond[ii]);
                        ++ii;
                    }
                    if (right) {
                        right = false;
                        rightOp = buffer.ToString();
                    }
                    else {
                        right = true;
                        leftOp = buffer.ToString();
                    }
                }
                else if (cond[ii] == ';') {
                    result = temp1(op(leftOp, rightOp)).And()(true);
                    break;
                }
                else {
                    //throw
                    ++ii;
                }
            }

            return result;// collection.Select(v => v.InnerObject);
        }
    
        #region EVALUATION

        public static bool True(this Predicate<bool> pred) {
            return pred(true);
        }

        public static bool False(this Predicate<bool> pred) {
            return pred(false);
        }

        #endregion

        #region PARTIAL EVALUATION
        //Bool -> (Bool -> Bool)

        public static Predicate<bool> And(this bool a) {
            return a ? ((x) => a && x) : ((Predicate<bool>)((x) => false));
        }

        public static Predicate<bool> Or(this bool a) {
            return a ? ((Predicate<bool>)((x) => true)) :
                       ((x) => a || x);
        }

        public static Predicate<bool> Xor(this bool a) {
            return a ? ((Predicate<bool>)((x) => !x)) :
                       ((x) => x);
        }

        public static Predicate<bool> Follows(this bool a) {
            return a ? ((x) => x) :
                       ((Predicate<bool>)((x) => true));
        }

        #endregion

        #region PREDICATE CHAINING
        ////(T -> Bool) -> (T -> Bool) -> (T -> Bool)
        //public static Predicate<T> And<T>(this Predicate<T> pred1, Predicate<T> pred2) {
        //    return ((T a) => pred1(a) && pred2(a));
        //}

        //public static Predicate<T> Or<T>(this Predicate<T> pred1, Predicate<T> pred2) {
        //    return ((T a) => pred1(a) || pred2(a));
        //}

        public static Predicate<T> Not<T>(this Predicate<T> pred) {
            return ((T x) => !pred(x));
        }

        #endregion

        #region BOOLEAN ALGEBRA
        //Bool -> Bool
        public static bool And(this bool a, bool b) {
            return a ? b : false;
        }

        public static bool Or(this bool a, bool b) {
            return a ? true : b;
        }

        public static bool Xor(this bool a, bool b) {
            return a ? !b : b;
        }

        public static bool Not(bool a) {
            return !a;
        }

        public static bool Follows(this bool a, bool b) {
            return a ? b : true;
        }

        #endregion
    }
}
#endif


namespace Fairweather.Service
{
    using System;

    /// <summary> REFLECTION
    /// </summary>
    internal static class Exception_Factory<TException>
        where TException : Exception, new()
    {

        public static TException Create_New() {

            var ret = new TException();

            return ret;

        }

        public static TException Create_New(string message) {

            var ctor = Get_Str_Ctor();
            var ret = ctor(message);

            return ret;

        }

        public static TException Create_New(string message, Exception inner) {

            var ctor = Get_Str_Exc_Ctor();
            var ret = ctor(message, inner);

            return ret;

        }

        static readonly object locker = new object();

        static Func<string, TException> m_str_ctor;
        static Func<string, Exception, TException> m_str_exc_ctor;

        static Func<string, TException> 
        Get_Str_Ctor() {

            lock (locker) {

                if (m_str_ctor == null) {

                    var ctor = typeof(TException).GetConstructor(new [] { typeof(string) } );

                    if (ctor == null) {

                        string message = "Type constructor not found for " + typeof(TException).ToString();
                        throw new InvalidOperationException(message);

                    }

                    var temp = ctor.CreateDelegate(typeof(Func<string, TException>));
                    m_str_ctor = (Func<string, TException>)temp;

                }
            }

            return m_str_ctor;
        }

        static Func<string, Exception, TException> 
        Get_Str_Exc_Ctor() {

            lock (locker) {

                if (m_str_ctor == null) {

                    var ctor = typeof(TException).GetConstructor(new [] { typeof(string), typeof(Exception) });

                    if (ctor == null) {

                        string message = "Type constructor not found for " + typeof(TException).ToString();
                        throw new InvalidOperationException(message);

                    }

                    var temp = ctor.CreateDelegate(typeof(Func<string, Exception, TException>));
                    m_str_exc_ctor = (Func<string, Exception, TException>)temp;

                }
            }

            return m_str_exc_ctor;
        }


    }
}

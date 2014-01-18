using System;

namespace Fairweather.Service
{
    static partial class Extensions
    {




        static public void tifn<TException>(this object arg)
                            where TException : Exception, new() {

            if (arg == null)
                Throw_Helper.Throw<TException>();

        }

        static public void tifn<TException>(this object arg, string message)
                            where TException : Exception, new() {

            if (arg == null)
                Throw_Helper.Throw<TException>(message);

        }



        static public void tiff<TException>(this bool b)
                            where TException : Exception, new() {

            if (!b) Throw_Helper.Throw<TException>();
        }

        static public void tiff<TException>(this bool b, string message)
                            where TException : Exception, new() {

            if (!b) Throw_Helper.Throw<TException>(message);

        }




        static public void tift<TException>(this bool b)
                            where TException : Exception, new() {

            if (b) Throw_Helper.Throw<TException>();
        }

        static public void tift<TException>(this bool b, string message)
                            where TException : Exception, new() {

            if (b) Throw_Helper.Throw<TException>(message);

        }



    }
}
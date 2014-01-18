

namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;

    using System.Diagnostics;
    using System.Reflection;

    public static partial class D
    {
        static public void Ensure_Caller(Delegate del) {

            // Method's caller   2  {--
            // Method            1
            // Ensure_Caller     0
            // Get_Caller_Name   x

            string caller = Get_Caller_Name(2);

            (del.Method.Name == caller).tiff();

        }

        static public void Ensure_Caller(string head, params string[] tail) {

            string caller = Get_Caller_Name(3);

            (caller == head
            ||
            tail.Contains(caller)).tiff();

        }

        static public string Hex_Dump(Byte[] data) {

            //et.Add(0
            throw new NotImplementedException();

        }

        /// <summary>
        /// Method's caller   1  {--
        /// Method            0
        /// Get_Caller        x
        /// </summary>
        public static MethodBase Get_Caller(int depth) {

            (depth >= 0).tiff();

            var frame = new StackFrame(depth + 1, true);
            MethodBase ret = null;

            if (frame != null)
                ret = frame.GetMethod();


            return ret;
        }

        /// <summary>
        /// Method's caller   1  {--
        /// Method            0
        /// Get_Caller_Name   x
        /// </summary>
        public static string Get_Caller_Name(int depth) {

            (depth >= 0).tiff();

            string ret = null;

            var frame = new StackFrame(depth + 1, true);

            if (frame != null) {

                var method = frame.GetMethod();

                ret = method.Name;

            }



            return ret;

        }

        public static MethodBase Get_Caller() {

            return Get_Caller(1);

        }

        public static string Get_Caller_Name() {

            return Get_Caller_Name(1);


        }

        public static Stack<StackFrame> Get_Frames() {

            var ret = new Stack<StackFrame>();

            // an alternative is StackTrace.GetFrames()
            for (int depth = 1; ; ++depth) {

                var frame = new StackFrame(depth, true);
                if (frame == null)
                    break;

                ret.Push(frame);

            }

            return ret;

        }

        static string Dump_Variables() {

            throw new NotImplementedException();
            /*       Todo: get caller's signature with reflection and walk the stack with pointer arithmetic        */

            //new StackFrame();
            //
            //int k = 0;
            //++k;
            //k++;

            //return "";
        }

        //// http://www.codeproject.com/KB/architecture/exceptionbestpractices.aspx
        ///
        ///// <summary> Plug into Application.ThreadException
        ///// </summary>
        //static public ThreadExceptionEventHandler Create_ThreadExceptionHandler() {

        //      ThreadExceptionEventHandler ret =
        //          (sender, e) =>
        //          {
        //                var original = e.Exception;

        //                Notify(original);
        //          };

        //      return ret;
        //}
        ///
        ///// <summary> Plug into AppDomain.CurrentDomain.UnhandledException
        ///// </summary>
        //static public UnhandledExceptionEventHandler Create_UnhandledExceptionHandler() {

        //      UnhandledExceptionEventHandler ret =
        //          (sender, e) =>
        //          {
        //                var original = (e.ExceptionObject as Exception);

        //                Notify(original);

        //          };

        //      return ret;
        //}
    }


}
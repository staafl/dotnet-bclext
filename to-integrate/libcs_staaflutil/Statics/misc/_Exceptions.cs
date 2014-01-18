namespace Fairweather.Service
{
    using System;
    internal static class Throw
    {
        const string stack_trace = "STACK_TRACE";
        const string stack_frame = "STACK_FRAME";


        /* The following methods:
         * * create an exception of the desired type or use the one provided as an argument
         * * fill its Data[stack_trace] with the current stack trace
         * * call Loggins.Notify with the exception, instructing it to print the stack trace
         * * throw
         * 
         * This is done in order to make sure that handled exceptions can also be logged.*/


        static public void _<TException>() where TException : Exception, new() {

            var ex = Exception_Factory<TException>.Create_New();

            /*
            ex.Data[stack_trace] = new StackTrace(2, true);
            ex.Data[stack_frame] = new StackFrame(2, true);

            Logging.Notify(ex, stack_trace, stack_frame);

            /*/
            Logging.Notify(ex);
            
            //*/




            throw ex;

        }

        static public void _<TException>(this bool condition) where TException : Exception, new() {

            if (condition) {

                var ex = Exception_Factory<TException>.Create_New();

                /*
                ex.Data[stack_trace] = new StackTrace(2, true);
                ex.Data[stack_frame] = new StackFrame(2, true);

                Logging.Notify(ex, stack_trace, stack_frame);

                /*/
                Logging.Notify(ex);

                //*/

                throw ex;

            }

        }

        static public void _<TException>(this bool condition, string message) where TException : Exception, new() {

            if (condition) {

                var ex = Exception_Factory<TException>.Create_New(message);

                /*
                ex.Data[stack_trace] = new StackTrace(2, true);
                ex.Data[stack_frame] = new StackFrame(2, true);

                Logging.Notify(ex, stack_trace, stack_frame);

                /*/
                Logging.Notify(ex);

                //*/

                throw ex;

            }

        }

        static public void _(this bool condition, Exception ex) {

            if (!condition)
                return;
            /*
            ex.Data[stack_trace] = new StackTrace(2, true);
            ex.Data[stack_frame] = new StackFrame(2, true);

            Logging.Notify(ex, stack_trace, stack_frame);

            /*/
            Logging.Notify(ex);

            //*/

            throw ex;

        }



    }

}

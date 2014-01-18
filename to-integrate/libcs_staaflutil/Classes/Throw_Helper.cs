using System;
using System.Diagnostics;

namespace Fairweather.Service
{
    // Minimalism!

    public abstract class Throw_Helper
    {
        // ****************************

        const string stack_trace = Logging.stack_trace;
        const string stack_frame = Logging.stack_frame;

        static internal void Fill_Stack_Trace(Exception ex) {

            // Fill_Stack_Trace()
            // _()
            // tift()
            // Calling Method

            ex.Data[stack_trace] = new StackTrace(3, true);
            ex.Data[stack_frame] = new StackFrame(3, true);

        }

        static internal void Throw(Exception ex) {

            Fill_Stack_Trace(ex);

            Logging.Notify(ex);

            throw ex;

        }

        static internal void Throw<TException>()
            where TException : Exception, new() {

            var ex = Exception_Factory<TException>.Create_New();

            Fill_Stack_Trace(ex);

            Logging.Notify(ex);

            throw ex;

        }

        static internal void Throw<TException>(string message)
            where TException : Exception, new() {


            var ex = Exception_Factory<TException>.Create_New(message);

            Fill_Stack_Trace(ex);

            Logging.Notify(ex);

            throw ex;


        }


        // ****************************
    }
}
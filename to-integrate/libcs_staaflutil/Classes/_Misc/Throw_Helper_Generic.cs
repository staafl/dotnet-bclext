using System;

namespace Fairweather.Service
{

    // This class provides metadata for a client that 
    // wants to throw only one kind of exception
    public class Throw_Helper<TException> : Throw_Helper
        where TException : Exception, new()
    {


        public Throw_Helper() {
        }


        public void Throw_If_Null(object arg) {

            if (arg == null) Throw<TException>();

        }

        public void Throw_If_Null(object arg, string message) {

            if (arg == null) Throw<TException>(message);

        }



        public void Throw_If_True(bool cond) {
            if (cond) Throw<TException>();
        }

        public void Throw_If_True(bool cond, string message) {
            if (cond) Throw<TException>(message);
        }

        public void Throw_If_True(bool cond, TException ex) {
            if (cond) Throw(ex);
        }


        public void Throw_If_False(bool cond) {
            if (!cond) Throw<TException>();
        }

        public void Throw_If_False(bool cond, string message) {
            if (!cond) Throw<TException>(message);
        }

        public void Throw_If_False(bool cond, TException ex) {
            if (!cond) Throw(ex);
        }


    }
}

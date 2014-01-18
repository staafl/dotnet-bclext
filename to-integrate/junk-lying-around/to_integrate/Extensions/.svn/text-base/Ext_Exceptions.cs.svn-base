using System;


namespace Fairweather.Service
{
    static partial class Extensions
    {
        static public string Msg_And_Type(this Exception ex) {
            string ret;

            if (ex.Message.IsNullOrEmpty())
                ret = ex.GetType().Name;
            else
                ret = ex.GetType().Name + ": " + ex.Message;

            return ret;
        }

        static public string Msg_Or_Type(this Exception ex) {

            if (ex.Message.IsNullOrEmpty())
                return ex.GetType().Name;

            return ex.Message;

        }



        // All of these methods throw ApplicationException

        /// <summary> ApplicationException
        /// </summary>
        static public void tifn(this object arg, string message) {
            if (arg == null)
                Throw_Helper.Throw<ApplicationException>(message);
        }

        /// <summary> ApplicationException
        /// </summary>
        static public void tifn(this object arg) {
            if (arg == null)
                Throw_Helper.Throw<ApplicationException>();

        }





        /// <summary> ApplicationException
        /// </summary>
        static public void tift(this bool b) {
            if(b) 
                Throw_Helper.Throw<ApplicationException>();

        }

        /// <summary> ApplicationException
        /// </summary>
        static public void tift(this bool b, string message) {
            if(b) 
                Throw_Helper.Throw<ApplicationException>(message);

        }

        static public void tift(this bool b, Exception ex) {
            if(b) 
                Throw_Helper.Throw(ex);

        }




        /// <summary> ApplicationException
        /// </summary>
        static public void tiff(this bool b) {
            if (!b) Throw_Helper.Throw<ApplicationException>();

        }

        /// <summary> ApplicationException
        /// </summary>
        static public void tiff(this bool b, string message) {
            if (!b) Throw_Helper.Throw<ApplicationException>(message);
            
        }

        static public void tiff(this bool b, Exception ex) {
            if (!b) Throw_Helper.Throw(ex);

        }




    }
}
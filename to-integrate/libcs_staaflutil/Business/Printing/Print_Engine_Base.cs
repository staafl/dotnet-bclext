using System;


namespace Fairweather.Service
{
    public abstract class Print_Engine_Base
    {

        const string ENGINE = "ENGINE";

        protected Print_Engine_Base() {

            Error_Mode = Error_Mode.Throw;

        }

        public bool Muddle_On_Through { get; set; }
        public Error_Mode Error_Mode { get; set; }

        public event Handler<Exception> Printing_Exception;



        protected void Notify(Exception ex) {
            ex.Data[ENGINE] = this.GetType().ToString();
            Logging.Notify(ex, ENGINE);
        }


        protected static void 
        On_Printing_X_Static(string msg, Exception ex) {

            if (ex == null)
                return;

            Logging.Notify(ex);

            if (msg.Emptyish())
                msg = ex.Message;

            throw new XPrinting(msg, ex);

        
        }

        protected void On_Printing_X(Exception ex) {
            On_Printing_X(null, ex);
        }

        protected void On_Printing_X(string msg, Exception ex) {

            if (ex == null)
                return;

            Notify(ex);

            if (Error_Mode.Contains(Error_Mode.Raise_Event)) {
                Printing_Exception.Raise(this, Args.Make(ex));

            }

            if (Error_Mode.Contains(Error_Mode.Throw))
                On_Printing_X_Static(msg, ex);

        }



    }


}













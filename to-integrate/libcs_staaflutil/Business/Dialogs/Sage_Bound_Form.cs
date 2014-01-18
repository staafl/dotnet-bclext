using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using Common.Dialogs;
using Fairweather.Service;

namespace Common.Dialogs
{
    public class Sage_Bound_Form : Hook_Enabled_Form
    {
        protected Sage_Bound_Form() { 
        }
        public Sage_Bound_Form(Form_Kind kind)
            : base(kind) {

            field_to_ctrl = Get_Bindings();
            binding = new Sage_Binding(Get_Sgr(), field_to_ctrl);

        }

        readonly Dictionary<string, Control>
        field_to_ctrl;

        readonly protected Sage_Binding binding;

        protected Sage_Binding
        Get_Binding() {
            var dict = Get_IOs();

            if (dict != null)
                return new Sage_Binding(Get_Sgr(), dict);

            var dict2 = Get_Bindings();

            if (dict2 != null)
                return new Sage_Binding(Get_Sgr(), dict2);

            throw new InvalidOperationException();
        }


        protected virtual Dictionary<string, IO_Pair<object>>
        Get_IOs() {
            return null;
        }


        protected virtual Dictionary<string, Control>
        Get_Bindings() {
            return null;
        }

        protected virtual Sage_Logic
        Get_Sgr() {
            return Data.SDR;
        }


    }
}

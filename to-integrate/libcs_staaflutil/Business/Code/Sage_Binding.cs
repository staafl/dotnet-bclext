using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Common.Controls;
using Fairweather.Service;
namespace Common
{


    // A helper class to facilitate binding of controls to Sage fields
    public class Sage_Binding
    {
        public void Clear(object obj) {

            old_values.Clear();
            foreach (var pair in IOs)
                pair.Input(obj);

        }


        public Sage_Binding(Sage_Logic sdr, Dictionary<string, Control> bindings)
            : this(sdr,
                   bindings.Transform_Values(tb => Sage_Binding.Make_IO(tb))) {

        }

        public Sage_Binding(
            Sage_Logic sdr,
            Dictionary<string,
            IO_Pair<object>> bindings) {

            this.m_sdr = sdr;
            field_to_io = new Twoway<string, IO_Pair<object>>(bindings.Count);
            field_to_io.Load(bindings);

        }

        readonly Sage_Logic m_sdr;

        readonly Dictionary<string, object>
        old_values = new Dictionary<string, object>();
        public Dictionary<string, object> Old_Values {
            get {
                return old_values;
            }
        }

        readonly Twoway<string, IO_Pair<object>>
        field_to_io;

        public IEnumerable<IO_Pair<object>>
        IOs {
            get {
                return field_to_io.Rights;
            }
        }

        public IEnumerable<string>
Fields {
            get {
                return field_to_io.Lefts;
            }
        }


        public event EventHandler<EventArgs> Loaded;
        public event EventHandler<EventArgs> Stored;

        public bool Load(string index, Record_Type type) {

            old_values.Clear();

            var array = field_to_io.Lefts.ToArray();

            using (var disp = m_sdr.Attempt_Transaction()) {
                if (disp == null)
                    return false;

                var dict = m_sdr.Get_Record_Fields(index, type, array);

                // ****************************

                disp.Try_Dispose();

                // ****************************

                foreach (var kvp in dict) {

                    string key = kvp.Key;
                    object val = kvp.Value;

                    field_to_io[key].Input(val);
                    old_values[key] = val;

                }

                Loaded.Raise(this);
            }

            return true;
        }

        public Account_Record?
        Store(bool is_new, string index, Record_Type type) {

            var changes = new Dictionary<string, object>();

            foreach (var kvp in this.field_to_io) {

                var field = kvp.First;

                var text = kvp.Second.Output();

                if (is_new || text != old_values[field])
                    changes.Add(field, text);

            }

            if (!is_new && changes.Is_Empty()) 
                return null;

            var record = Sage_Access.Get_Value(() => m_sdr.Update_Account_Record(is_new, index, type, changes));

            Stored.Raise(this);

            return record;
        }


        public static IO_Pair<object>
        Make_IO(Control ctrl) {
            if (ctrl is TextBox)
                return Make_IO((TextBox)ctrl);
            if (ctrl is CheckBox)
                return Make_IO((CheckBox)ctrl);
            if (ctrl is Numeric_Box)
                return Make_IO((Numeric_Box)ctrl);
            if (ctrl is DateTimePicker)
                return Make_IO((DateTimePicker)ctrl);
            if (ctrl is ComboBox)
                return Make_IO((ComboBox)ctrl);
            if (ctrl is IAmountControl)
                return Make_IO((IAmountControl)ctrl);

            //if (ctrl is Our_DateTime)
            //    return Make_IO((Our_DateTime)ctrl);

            Func<object> output = () => ctrl.Text;
            Action<object> input = (str) => ctrl.Text = str.StringOrDefault();

            var ret = IO_Pair.Make(input, output);
            return ret;
        }


        public static IO_Pair<object>
        Make_IO(ComboBox ctrl, Dictionary<object, int> values) {

            Func<object> output = () => ctrl.SelectedItem.StringOrDefault();
            Action<object> input = (pbj) => ctrl.SelectedIndex = values[pbj];

            var ret = IO_Pair.Make(input, output);
            return ret;

        }

        public static IO_Pair<object>
        Make_IO(ComboBox ctrl, object[] values) {

            Func<object> output = () => ctrl.SelectedItem.StringOrDefault();
            Action<object> input = (pbj) => ctrl.SelectedIndex = values.IndexOf(pbj);

            var ret = IO_Pair.Make(input, output);
            return ret;

        }

        public static IO_Pair<object>
        Make_IO(ComboBox ctrl) {

            Func<object> output = () => ctrl.SelectedItem.StringOrDefault();
            Action<object> input = (pbj) => ctrl.SelectedIndex = ctrl.Items.Add(pbj.StringOrDefault());

            var ret = IO_Pair.Make(input, output);
            return ret;

        }



        public static IO_Pair<object>
        Make_IO(IAmountControl ctrl) {

            return IO_Pair.Make(
                obj =>
                {
                    decimal value = (decimal)Convert.ChangeType(obj ?? default(decimal), TypeCode.Decimal);
                    ctrl.Value = value;
                },
                () =>
                {
                    return (object)ctrl.Value;
                    // (object)ctrl.Value.ToString("F" + ctrl.DecimalPlaces));
                });
        }
        //public static IO_Pair<object>
        //Make_IO(NumericBox ctrl) {

        //    return IO_Pair.Make(
        //        obj =>
        //        {
        //            decimal value = (decimal)Convert.ChangeType(obj ?? default(decimal), TypeCode.Decimal);
        //            ctrl.Value = value;
        //        },
        //        () =>
        //        {
        //            return (object)ctrl.Value;
        //            // (object)ctrl.Value.ToString("F" + ctrl.DecimalPlaces));
        //        });

        //}
        public static IO_Pair<object>
        Make_IO(DateTimePicker ctrl) {

            return IO_Pair.Make(
                obj => ctrl.Value = ((DateTime)Convert.ChangeType(obj ?? default(DateTime), TypeCode.DateTime)),
                () => (object)ctrl.Value);

        }
        //public static IO_Pair<object>
        //Make_IO(Our_DateTime ctrl) {

        //    throw new NotImplementedException();

        //}


        public static IO_Pair<object>
        Make_IO(TextBox tb) {

            Func<object> output = () => tb.Text;
            Action<object> input = (str) => tb.Text = str.StringOrDefault();

            var ret = IO_Pair.Make(input, output);

            return ret;

        }

        public static IO_Pair<object>
        Make_IO(CheckBox chb) {

            Func<object> output = () => chb.Checked ? (short)1 : 0;

            Action<object> input = (sh) =>
            {
                (sh.GetType() == typeof(short)).tiff();

                if ((short)sh == 1) {

                    chb.Checked = true;

                }
                else {

                    ((short)sh == 0).tiff();
                    chb.Checked = false;

                }

            };

            return IO_Pair.Make(input, output);

        }

    }
}

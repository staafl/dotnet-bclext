using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Common.Controls;
using Common.Sage;
using Fairweather.Service;

namespace Common.Controls
{
    partial class Search_DGV : Advanced_Grid_View
    {

        public Search_DGV() {

        }

        public Record_Type Record_Type {
            get;
            set;
        }



        // ****************************

        void Prepare_Main_Combo_Box(int col, bool first) {

            (col == COL_JOIN).tiff();

            var cb = Make_Combo_Box();

            if (first) {
                cb.Items.Add(WHERE);
                cb.Items.Add(NONE);
                cb_where = cb;
            }
            else {
                var items = new string[] { AND, OR, NONE };
                cb.Items.AddRange(items);
                cb_andor = cb;
            }

        }

        void Prepare_Precedence_Combobox(int col) {

            (col == COL_PRECEDENCE).tiff();

            var cb = Make_Combo_Box();

            cb_precedence = cb;

        }

        void Prepare_Value_Controls(int col) {

            (col == COL_VALUE).tiff();

            nb_decimal = Prepare_Numeric_Box(col);
            tb_string = Prepare_Text_Box(col);

            cb_yesno = Make_Combo_Box();

            cb_yesno.Items.AddRange(new string[] { YES, NO });

            nb_integer = Make_Integer_Box();

            dt_date = Prepare_Date_Time(col);


        }

        void Prepare_Field_Combobox(int col) {

            (col == COL_FIELD).tiff();

            var cb = Make_Combo_Box();

            var list = Sage_Fields.Fields(Record_Type).Select(kvp => kvp.Value).ToList();
            list.Sort((f1, f2) => String.Compare(f1.Description,
                                                 f2.Description,
                                                 true));

            int cnt = list.Count;

            m_sage_fields = new Triple<int, string, string>[cnt];

            int ii = 0;
            foreach (var field in list) {

                m_sage_fields[ii] = Triple.Make(ii, field.Name, field.Description);
                ++ii;

            }

            var field_strings = m_sage_fields.Select(triple => triple.Third).ToArray();

            cb.Items.AddRange(field_strings);

            cb_field = cb;
        }


        void Prepare_Condition_Comboboxes(int col) {

            (col == COL_CONDITION).tiff();

            m_cond_cbxs = new ComboBox[CBX_BOOL + 1];

            foreach (var kvp in conditions) {

                var cb = Make_Combo_Box();

                cb.Items.AddRange(kvp.Value);

                m_cond_cbxs[kvp.Key] = cb;
            }

        }


        // ****************************


        protected override bool Kill_Scroll {
            get {
                return true;
            }
        }
        protected override bool Clean_On_Leave {
            get {
                return true;
            }
        }

        protected override bool Setup_Column(int col) {

            Action<int> main_cb = column_index =>
            {
                Prepare_Main_Combo_Box(column_index, true);
                Prepare_Main_Combo_Box(column_index, false);
            };

            var dict = new Dictionary<int, Action<int>>
            {

                { COL_JOIN, main_cb},

                { COL_VALUE,Prepare_Value_Controls},

                { COL_FIELD,Prepare_Field_Combobox},

                { COL_PRECEDENCE,Prepare_Precedence_Combobox},

                { COL_CONDITION,Prepare_Condition_Comboboxes}

            };

            dict[col](col);
            return true;

        }

        void InitializeComponent() {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // Search_DGV
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.RowTemplate.Height = 21;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

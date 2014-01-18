using System;
using System.Windows.Forms;
using Standardization;

using Fairweather.Service;

namespace Common.Dialogs
{
    public partial class Product_Tab2 : UserControl
    {
        public Label[] DGV_Labels {
            get {
                return new[] {
                lab_no_matching,
                lab_please_select,
                lab_please_use_one,
                lab_please_select_customer,
            };
            }
        }

        public Product_Tab2() {

            InitializeComponent();

            // Standard.Make_Flat_DGV(dgv_history);

            dgv_history.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            col_date.Set_Value_Type(typeof(DateTime), true);

            col_number.Set_Value_Type(typeof(int));
            col_posted.Boolean_Column();

            col_qty.Set_Value_Type(typeof(decimal), true);
            col_unit_vat.Set_Value_Type(typeof(decimal), true);
            col_unit_nvat.Set_Value_Type(typeof(decimal), true);
            col_disc_perc.Set_Value_Type(typeof(decimal), true);
            col_full_price.Set_Value_Type(typeof(decimal), true);

            col_qty.Numeric_Column();
            col_unit_vat.Numeric_Column();
            col_unit_nvat.Numeric_Column();
            col_unit_nvat.Numeric_Column();
            col_disc_perc.Numeric_Column();
            col_full_price.Numeric_Column();

       

        }




        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            foreach (var lab in DGV_Labels) {
                lab.BackColor = dgv_history.BackgroundColor;
                lab.BringToFront();
                lab.Visible = false;
            }
            lab_please_use_one.Visible = true;

        }


    }
}

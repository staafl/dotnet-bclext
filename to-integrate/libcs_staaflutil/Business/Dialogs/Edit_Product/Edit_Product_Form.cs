using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Common;
using Common.Controls;
using Fairweather.Service;
using Standardization;
using System.Linq;
using System.Threading;

namespace Common.Dialogs
{
    public partial class Edit_Product_Form : Sage_Bound_Form
    {
        const string STR_LAST_PURCHASE_PRICE = "LAST_PURCHASE_PRICE";
        const string STR_LAST_COST_DISCOUNT = "LAST_COST_DISCOUNT";

        const string please_select_product = "Please select a product from the 'Details' tab in order to see its Sales History";
        const string please_select_customer = "Please select a customer from the main screen in order to see his/her Sales History";
        const string please_use_a_button = "Please use one of the buttons below to retrieve data.";


        public Edit_Product_Form(
                Product product,                       // currently selected
                Func<string, Product> products,        // all products
                IEnumerable<string> other_stock_codes, // from the invoice
                int? start_position,
                string null_customer,
                bool skip_to_history,
                Auto_Sales_History_Mode ashm,
                bool auto_calculate_sb,
                Average_Cost_Type avg_cost_type)

            : base(Form_Kind.Modal_Dialog) {

            Use_OSK = true;
            this.skip_to_history = skip_to_history;

            this.auto_sales_history_mode = ashm;
            this.auto_calculate_sb = auto_calculate_sb;

            this.null_customer = null_customer;
            this.products = products;
            this.Product = product;

            this.other_stock_codes = other_stock_codes.lst();
            this.current_position = start_position;
            this.avg_cost_type = avg_cost_type;

            // ASSERTION
            if (start_position != null)
                (products(this.other_stock_codes[start_position.Value]).Code == product.Code).tiff();


            InitializeComponent();

            Set_Update_Buttons_Enabled(false);

            base.binding.Loaded += binding_Loaded;

            Set_Text();

            Prepare_Tabs(null_customer);


            tab_control.Setup(tab1, tab2);
            tab_control.Tab_Activated += tab_control_Tab_Activated;

            Prepare_Combo_box();

            but_next.Click += (_1, _2) => Cycle(true);
            but_prev.Click += (_1, _2) => Cycle(false);

            Refresh_Cycle_Buttons();

            tab1.but_update_sb.Lock_To_Label(tab1.amount_Label1);
            tab1.but_update_avg_cost.Lock_To_Label(tab1.amount_Label3);


            but_keyboard.Click += but_keyboard_Click;


        }


        readonly Average_Cost_Type avg_cost_type;

        const Auto_Sales_History_Mode
            all = Auto_Sales_History_Mode.All,
            by_date = Auto_Sales_History_Mode.By_Date,
            none = Auto_Sales_History_Mode.None;

        protected override void OnLoad(EventArgs e) {


            base.OnLoad(e);

            this.Force_Handle();

            if (skip_to_history && !null_customer.IsNullOrEmpty()) {

                // bastard case...
                // update the labels manually
                BeginInvoke((MethodInvoker)(() =>
                {
                    var sdr = Data.SDR;
                    using (var disp = sdr.Attempt_Transaction()) {

                        if (disp == null) {
                            Discard();
                            return;
                        }

                        var cost_price = sdr.Get_Record_Field(
                            Product.Code,
                            Record_Type.Stock,
                            STR_LAST_PURCHASE_PRICE).ToDecimal();

                        Set_Tab2_Labels(cost_price);

                    };

                    // then open the second tab as though the user did
                    tab_control.Active_Tab = 1;

                }));

            }
            else {

                BeginInvoke((MethodInvoker)(() =>
                {
                    /* isn't it a capital idea to not have a default active tab ? */
                    tab_control.Active_Tab = 0;
                }));

            }

        }

        void Prepare_Combo_box() {
            tab1.acb_product.Setup(Quick_Search_Form_Mode.Products_view);
            tab1.acb_product.Enabled = true;
            tab1.acb_product.Allow_Blank = true;
            tab1.acb_product.Value = Product.Code;
            tab1.acb_product.Accept_Changes += acb_product_AcceptChanges;
            tab1.acb_product.MaxLength = 30;
            tab1.acb_product.Set_Alignment(Rectangle_Vertex.LD);


        }

        void Prepare_Tabs(string null_customer) {

            tab1.Name = "Details";
            tab2.Name = "History";

            tab1.but_update_avg_cost.Click += (_1, _2) => Calculate_Average_Cost();
            tab1.but_update_sb.Click += (_1, _2) => Calculate_Stock_Balance();

            Set_Enabled_State();

            if (null_customer.IsNullOrEmpty()) {
                tab2.Enabled = false;
                tab2.lab_customer.Text = "NONE";
                tab2.lab_please_select.Text = please_select_customer;
            }
            else {
                tab2.dgv_history.Flat_Style();

                tab2.lab_please_select.Text = please_use_a_button;


                // only ever set here!
                tab2.lab_customer.Text = null_customer;

                tab2.but_get_all.Click += (_1, _2) => Load_History(false, false);
                tab2.but_get_by_number.Click += (_1, _2) => Load_History(true, false);
                tab2.but_get_by_date.Click += (_1, _2) => Load_History(true, true);
                tab2.but_clear.Click += (_1, _2) => Reset_History_State();
            }
        }


        void Traverse(Action<bool, Control> act) {

            var unwanted = new Set<Type>(
                typeof(Advanced_Combo_Box),
                typeof(GroupBox),
                typeof(Label),
                typeof(Product_Tab1),
                typeof(Product_Tab2),
                typeof(Amount_Label),
                typeof(Text_Label),
                typeof(Moving_Border),
                typeof(Button));

            foreach (var ctrl in tab1.All_Children()) {

                bool interesting = !unwanted[ctrl.GetType()];

                Console.WriteLine("{0},{1}", interesting, ctrl);
                act(interesting, ctrl);

            }


        }

        /// <summary>
        /// What this method does is make all controls corresponding
        /// to field values disabled, and make sure they are consistently
        /// coloured.
        /// </summary>
        void Set_Enabled_State() {


            var tbxs = new List<TextBox>(100);

            Action<bool, Control> act = (interesting, ctrl) =>
            {

                if (!interesting) {
                    ctrl.Enabled = true;
                    return;

                }

                if (ctrl.Parent is Advanced_Combo_Box)
                    return;

                if (ctrl is TextBox) {
                    if (!(ctrl is Validating_Box) &&
                        !(ctrl is Numeric_Box))
                        tbxs.Add((TextBox)ctrl);
                }


                //if (ctrl is ComboBox) {
                //    ((ComboBox)ctrl).Flat_Style();
                //}

                ctrl.Enabled = false;

                ctrl.ForeColor = Color.Black;
                ctrl.BackColor = Colors.TextBoxes.ReadOnlyBackGround;

            };

            Traverse(act);

            foreach (var tb in tbxs) {

                // i've shied from changing the textboxes
                // to actual labels since one day we might
                // want to allow the user to edit the product's
                // fields from the dialog

                var lab = tb.Readonly_Label();

                lab.ForeColor = Color.Black;
                lab.BackColor = Colors.TextBoxes.ReadOnlyBackGround;

                // lab.BorderStyle = 0 /*none*/;
            }
        }


        // ****************************


        void Set_Update_Buttons_Enabled(bool enabled) {

            foreach (var but in this.All_Children().OfType<Update_Button>()) {

                but.Enabled = enabled;
                if (!enabled)
                    but.Fresh = false;

            }

        }


        // ****************************



        void Cycle(bool fwd) {

            int pos;

            if (current_position == null)
                pos = fwd ? -1 : 0;
            else
                pos = current_position.Value;

            pos += fwd ? 1 : -1;

            var len = other_stock_codes.Count;

            if (pos < 0)
                pos = len - 1;
            else
                pos %= len;

            current_position = pos;

            var new_code = other_stock_codes[pos];

            Set_Product(products(new_code));

        }


        void Refresh_Cycle_Buttons() {

            if (other_stock_codes.Count + (current_position == null ? 1 : 0) <= 1)
                // at least two invoice items OR an invoice item and a user-selected stock record

                but_next.Enabled =
                but_prev.Enabled = false;

        }



        // ****************************

        void Calculate_Average_Cost() {

            Func<Sage_Logic, decimal?> calc = sdr =>
            {
                return sdr.Calculate_Average_Cost(Product.Code, avg_cost_type);
            };

            Calculate(tab1.but_update_avg_cost,
                      calc,
                      "Average Product Cost");

        }

        void Calculate_Stock_Balance() {

            Func<Sage_Logic, decimal?> calc = sdr =>
            {
                var unposted = sdr.Get_Unposted_Sales_For_Stock_Records(new[] { Product.Code })
                                  .Single()
                                  .Value;

                var posted = tab1.numericBox1.Value;

                return posted + unposted;

            };

            Calculate(tab1.but_update_sb,
                      calc,
                      "Stock Balance");


        }

        void Calculate(Update_Button but,
                       Func<Sage_Logic, decimal?> calculator,
                       string title) {

            but.Fresh = false;

            var sdr = Data.SDR;
            bool ok;

            Action show, close;
            Func<Form> _;
            using (Progress_Reporter.Separate_Thread_Unknown_Total(
                            "Calculating " + title,
                            100,
                            this.Bounds,
                            out show,
                            out close,
                            out _))
            using (/* var disp = */
                   sdr.Attempt_Transaction(out ok)) {

                if (ok) {

                    show();

                    var null_amount = calculator(sdr);
                    but.Fresh = true;

                    if (null_amount == null)
                        but.Label.Text = "N/A";
                    else
                        ((Amount_Label)but.Label).Value = null_amount.Value;

                    close();
                }
                else {
                    Standard.Warn("Unable to refresh {0}.", title);
                    but.Fresh = false;

                }


            }

        }

        // ****************************



        void Maybe_Reset_Sales_History_State() {

            if (null_customer.IsNullOrEmpty() ||
                Product == null) {
                Reset_History_State();
            }

        }

        void Reset_History_State() {



            tab2.dgv_history.Rows.Clear();




            loaded_stock_history = null;



            // ****************************
            // 
            /*       Finish modifying the state        */
            /*       before updating GUI        */


            Refresh_History_Controls(true);


        }

        void Set_Visible(Label lab) {
            foreach (var label in tab2.DGV_Labels) {
                label.Visible = label == lab;
            }
        }

        void Refresh_History_Controls(bool update_buttons) {

            Action set_unchecked = () =>
            {
                if (!update_buttons)
                    return;

                tab2.but_get_all.Checked =
                tab2.but_get_by_number.Checked =
                tab2.but_get_by_date.Checked = false;
            };

            Action<bool> set_enabled = b =>
            {
                set_unchecked();

                tab2.but_get_all.Enabled =
                tab2.but_get_by_number.Enabled =
                tab2.but_get_by_date.Enabled =
                tab2.but_clear.Enabled =
                    b;

            };


            if (null_customer.IsNullOrEmpty()) {
                Set_Visible(tab2.lab_please_select_customer);
                set_enabled(false);
            }
            else if (Product == null) {
                Set_Visible(tab2.lab_please_select);
                set_enabled(false);
            }
            else if (tab2.dgv_history.RowCount == 0) {
                Set_Visible(tab2.lab_please_use_one);
                set_unchecked();
                set_enabled(true);

            }
            else {
                Set_Visible((tab2.lab_no_matching.Visible && loaded_stock_history == Product)
                            ? tab2.lab_no_matching : null);
                set_enabled(true);
                set_unchecked();

            }

        }




        // ****************************




        void acb_product_AcceptChanges(object sender, EventArgs e) {

            current_position = null;
            var product = products(tab1.acb_product.Value);
            Set_Product(product);

            Refresh_Cycle_Buttons();

        }


        void tab_control_Tab_Activated(object sender, Args_RO<int> eargs) {

            var ix = eargs.Im;

            tab_control.Refresh();
            this.Refresh();

            if (ix == 0) {

                if (loaded_product != Product)
                    Set_Product(Product);

                return;
            }
            else if (ix == 1) {
                if (loaded_stock_history != Product && /* do we need to and can we */
                    auto_sales_history_mode != none)

                    Load_History();
            }

        }

        void but_discard_Click(object sender, EventArgs e) {

            Discard();
        }


        // ****************************
        // 
        /*       Movers and Shakers        */


        void Load_History() {

            Load_History(auto_sales_history_mode != all,
                         auto_sales_history_mode == by_date);


        }

        void Load_History(bool just_one, bool by_date) {

            var tmp_product = this.Product;

            Reset_History_State();

            var but = just_one ? (by_date ? tab2.but_get_by_date
                                             : tab2.but_get_by_number)
                                  : tab2.but_get_all;
            but.Checked = true;

            var dgv = tab2.dgv_history;

            var sdr = Data.SDR;

            dgv.SuspendLayout();

            try {
                Action show, close;
                Func<Form> getter;


                using (var disp = sdr.Attempt_Transaction()) {

                    if (disp == null) {
                        but.Checked = false;
                        return;
                    }

                    using (var pb = Progress_Reporter.Separate_Thread_Unknown_Total(
                        "Reading Sales History",
                        100,
                        this.Bounds,
                        out show,
                        out close,
                        out getter)) {

                        show();

                        Refresh();

                        foreach (var she in sdr.Get_Sales_History(tmp_product,
                                                                    null_customer,
                                                                    just_one,
                                                                    by_date)) {

                            int ix = dgv.Rows.Add();

                            var row_obj = dgv.Rows[ix];

                            if (she.Is_Credit_Note)
                                row_obj.DefaultCellStyle.ForeColor = Colors.GridView.Red;

                            var cells = row_obj.Cells;

                            cells[0].Value = she.Invoice_Number;
                            cells[1].Value = she.Is_Credit_Note ? "Crd" : "Inv";
                            cells[2].Value = she.Date;
                            cells[3].Value = she.Discounted_Price_Vat;
                            cells[4].Value = she.Discounted_Price_Nvat;
                            cells[5].Value = she.Full_Price_Nvat;
                            cells[6].Value = she.Discount_Perc;
                            cells[7].Value = she.Qty;
                            cells[8].Value = she.Posted;


                        }

                        close();
                    }
                }

                var form = getter();

                try {
                    if (!form.Disposed_Ing())
                        form.Invoke((MethodInvoker)(() => form.Try_Dispose()));
                }
                catch (ObjectDisposedException) { }

                if (dgv.RowCount == 0) {

                    this.Force_Handle();
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        // Reset_History_State();

                        // give the stupid form time to dispose
                        // as it would otherwise become the parent of
                        // the mbox and we'd come to grief
                        Set_Visible(tab2.lab_no_matching);
                    }));

                }
                else {
                    dgv.Sort(dgv.Columns[0], ListSortDirection.Descending);
                    dgv.CurrentCell = dgv[0, 0];

                }

                loaded_stock_history = tmp_product;

            }
            finally {
                dgv.ResumeLayout();
                Refresh_History_Controls(false);

            }
        }

        void binding_Loaded(object sender, EventArgs e) {

            tab1.acb_product.Value = Product.Code;
            // ! Not the barcode

            var str = tab1.comboBox6.Text;
            if (!str.IsNullOrEmpty()) {
                /*       Dept Number        */

                str = str.PadLeft(3, '0');
                tab1.comboBox6.Text = str;
            }

            str = tab1.comboBox8.Text;
            if (!str.IsNullOrEmpty()) {
                /*       Tax Code        */

                str = str.Trim();
                str = Tax_Code.Get_Combobox_String(new Tax_Code(short.Parse(str)));
                tab1.comboBox8.Text = str;


            }

            str = tab1.comboBox1.Text;

            if (!str.IsNullOrEmpty()) {
                /*       Item Type        */

                str = str.Trim();

                int tmp;
                var arr = new[] { "Standard", "Ignore Stock Levels", "Ignore Stock Levels + Memo" };
                // SDO 16:
                // StockItemType 
                // Constant        Value Description 
                // sdoStockItem    0  Standard Stock Item  
                // sdoNonStockItem 1  Stock Item with 'Ignore Stock Levels' Set  
                // sdoServiceItem  2  Stock Item with 'Ignore Stock Levels' Set and the facility to use memos  


                if (int.TryParse(str, out tmp) && tmp < arr.Length) {

                    tab1.comboBox1.Text = arr[tmp];

                }

            }

            Set_Tab2_Labels(tab1.numericBox6.Value);

            /* free stock */
            tab1.amount_Label2.Value = tab1.numericBox1.Value /* stock balance */
                                     - tab1.textBox10.Value   /* allocated */;

            tab1.but_update_sb.Fresh = false;
            tab1.but_update_avg_cost.Fresh = false;

            if (auto_calculate_sb) {

                tab_control.Refresh();
                this.Refresh();

                Calculate_Stock_Balance();

            }

            loaded_product = Product;
            Reset_History_State();
            Set_Update_Buttons_Enabled(true);

        }

        void Set_Text() {

            if (Product == null) {
                this.Text = "Product Record";
            }
            else {

                var description = Product.Description;

                if (description.IsNullOrEmpty())
                    description = Product.Code;

                this.Text = "Product Record - " + description;

            }

        }

        void Set_Product(Product product) {

            this.Product = product;

            if (product == null)
                return;


            // Reset_History_State();

            if (binding.Load(product.Code, Record_Type.Stock)) {

                tab2.lab_product.Text = product.Code;

            }


        }

        void Discard() {

            loaded_product = null;

            foreach (var ctrl in this.All_Children()) {

                if (ctrl == tab2.lab_customer)
                    continue;

                if (ctrl is TextBox ||
                   ctrl is Amount_Label ||
                   ctrl is Numeric_Box ||
                   ctrl is Text_Label ||
                   ctrl is ComboBox ||
                   ctrl is Our_Date_Time) {

                    ctrl.ResetText();

                }


            }

            Set_Update_Buttons_Enabled(false);

            tab1.but_update_sb.Fresh = false;
            tab1.but_update_avg_cost.Fresh = false;

            tab1.acb_product.Value = null;
            Product = null;

            tab_control.Active_Tab = 0;
            tab1.acb_product.Select_Focus();

            Reset_History_State();

        }

        // ****************************


        void Set_Tab2_Labels() {

            tab2.lab_product.Text = Product == null ? "" : Product.Code;

            if (Product == null)
                tab2.alab_sales_price.Text = "";
            else
                tab2.alab_sales_price.Value = Product.Price;

        }

        void Set_Tab2_Labels(decimal? cost_price) {

            Set_Tab2_Labels();

            if (cost_price == null)
                tab2.alab_last_cost.Text = "";
            else
                tab2.alab_last_cost.Value = cost_price.Value;


        }








        readonly Product_Tab1 tab1 = new Product_Tab1();
        readonly Product_Tab2 tab2 = new Product_Tab2();

        readonly string null_customer;
        readonly Func<string, Product> products;

        readonly bool auto_calculate_sb;

        readonly Auto_Sales_History_Mode auto_sales_history_mode;
        readonly bool skip_to_history;

        /// <summary>
        /// "do not touch", meaning - only modify using the relevant property
        /// </summary>
        Product dnt_roduct;


        Product Product {
            set {
                dnt_roduct = value;
                Set_Text();
                Set_Tab2_Labels();
            }
            get {
                return dnt_roduct;
            }
        }

        Product loaded_stock_history;
        Product loaded_product;

        int? current_position;
        readonly List<string> other_stock_codes;

        protected override Dictionary<string, Control> Get_Bindings() {

            return new Dictionary<string, Control>
            {   
                //{"STOCK_CODE",tab1.textBox1},
                {"DESCRIPTION",tab1.textBox2},
                {"STOCK_CAT",tab1.comboBox2},
                {"INTRASTAT_COMMODITY_CODE",tab1.textBox4},
                {"COMMODITY_CODE",tab1.textBox5},
                {"ITEM_TYPE",tab1.comboBox1},
                {"LOCATION",tab1.textBox3},
                {"UNIT_WEIGHT",tab1.numericBox8},

                {STR_LAST_PURCHASE_PRICE,tab1.numericBox6},
                {STR_LAST_COST_DISCOUNT,tab1.numericBox7},
                {"QTY_LAST_ORDER",tab1.textBox6},
                {"LAST_PURCHASE_DATE",tab1.dateTime2},


                {"NOMINAL_CODE",tab1.comboBox3},
                {"PURCHASE_NOMINAL_CODE",tab1.comboBox4},
                {"PURCHASE_REF",tab1.comboBox5},
                {"TAX_CODE",tab1.comboBox8},
                {"SUPPLIER_PART_NUMBER",tab1.textBox8},
                {"DEPT_NUMBER",tab1.comboBox6},

                {"SALES_PRICE",tab1.numericBox9},
                {"UNIT_OF_SALE",tab1.textBox9},

                {"QTY_IN_STOCK",tab1.numericBox1},
                {"QTY_ALLOCATED",tab1.textBox10},
                //{"QTY_FREE???",tab1.numericBox2},
                {"QTY_ON_ORDER",tab1.textBox11},
                {"QTY_REORDER",tab1.numericBox3},
                {"QTY_REORDER_LEVEL",tab1.numericBox4},

                {"STOCK_TAKE_DATE",tab1.dateTime1},
                {"QTY_LAST_STOCK_TAKE",tab1.numericBox5},
            };

        }






    }
}

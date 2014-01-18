using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Common;
using Common.Controls;
using Common.Dialogs;
using Fairweather.Service;
using Colors = Standardization.Colors;
using Standardization;
namespace Common.Dialogs
{
    public partial class New_Customer : Hook_Enabled_Form
    {
        readonly bool customer;
        public New_Customer(string preselected, Sage_Logic sdr, bool customer)
            : base(true, true, Form_Kind.Modal_Dialog) {


            base.Allow_Normal_Close = true;
            base.Use_OSK = true;

            InitializeComponent();

            this.CancelButton = but_close;

            Prepare_Text_Boxes();

            this.customer = customer;

            RecordsCursor = customer ? Records_Access.CustomerAccounts : Records_Access.SupplierAccounts;

            bool prepare;
            using (RecordsCursor.Prepare(out prepare)) {

                if (!prepare) {
                    this.Force_Handle();
                    BeginInvoke((MethodInvoker)(Try_Close));
                    return;
                }

                m_sdr = sdr;
                binding = new Sage_Binding(sdr, Get_IOs());

                bool ok;
                Dictionary<string, Country_Record> dict;

                bool prepared = false;
                while (true) {

                    using (m_sdr.Attempt_Transaction(out ok)) {

                        if (!ok)
                            continue;

                        dict = m_sdr.Get_Countries();

                        s_countries = new SortedList<string, Country_Record>(dict);

                        var def_country_code = m_sdr.Get_Default_Country();

                        default_country = s_countries.IndexOfKey(def_country_code);

                        if (default_country == -1)
                            default_country = 0;

                        Prepare_Cbxs();


                        if (preselected.IsNullOrEmpty()) {
                            Clear_Controls(true);
                        }
                        else {
                            prepared = true;
                            Prepare_Country_Cbx();

                            cb_account.Value = preselected;

                            if (cb_account.Value == null)
                                Handle_New_Account(false);
                            else
                                Accept_Selected_User();

                        }



                        break;
                    }
                }

                if (!prepared)
                    Prepare_Country_Cbx();


                Prepare_Tab_Control();


                this.chb_email.Visible = false;

                /*       TODO: Find out what field this goes into        */

                // fields.Add(Make_Chb_Func(this.chb_email));

                Set_Event_Handlers();
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e) {

            RecordsCursor.End();
            base.OnFormClosing(e);

        }

        void Set_Event_Handlers() {

            cb_account.Accept_Changes += (_1, _2) => Accept_Selected_User();
            cb_account.New_Account_Event += (_1, _2) => Handle_New_Account(true);

        }



        void Prepare_Calculator() {

            calculator = new Calculator(this);

            this.panel1.Controls.Add(calculator);
            var vert = panel1.ClientRectangle.Vertex(2);

            var rect = calculator.ClientRectangle;
            var rect2 = rect.Move_Vertex(2, vert);

            calculator.Location = rect2.Location.Translate(-6, -2);

        }

        void Prepare_Tab_Control() {

            var tabc = new Advanced_Tab_Control();
            var user = new UserControl();
            user.Name = "Details";
            tabc.Setup(user);

            this.Controls.Add(tabc);
            tabc.Location = new Point(8, 9);

            tabc.Highlight_Color = Color.White;

            this.Force_Handle();
            BeginInvoke((MethodInvoker)(() =>
            {
                tabc.Active_Tab = 0;

            }));
        }




        void Prepare_Text_Boxes() {

            address_Box1.Top_Offset = 7;
            address_Box1.Interim = 5;
            address_Box1.Refresh_Layout();

            m_adress_bxs = address_Box1.TextBoxes.Pend(null, true).arr();
            address_Box1.TextBoxes[3].move(0, 2);
            address_Box1.TextBoxes[4].move(0, 2);

        }

        void Prepare_Cbxs() {

            //Our_Moving_Border.Create(cb_country);

            cb_account.Control_Host = this;
            cb_account.Allow_New_Account = true;
            // cb_account.Setup(QuickSearchFormMode.Customers);

            cb_account.QSF_Orientation = new Rectangle_Vertex(3);

            cb_account.Setup(customer ? Quick_Search_Form_Mode.Customers_new : Quick_Search_Form_Mode.Suppliers_new);


        }

        void Prepare_Country_Cbx() {

            Moving_Border.Create(cb_country);
            cb_country.Font = Standard.Cb_Monospace_Font;

            var format = "{0}\t{1}";
            cb_country.Items.Clear();

            foreach (var country in s_countries) {

                string str = format.spf(country.Key, country.Value.Country_Name);
                str = str.Transform_Tabs(5);
                cb_country.Items.Add(str);

            }

            Set_Default_Country();

        }

        Dictionary<string, IO_Pair<object>>
        Get_IOs() {

            /*       Textbox binding        */

            var temp1 = new Dictionary<string, TextBox> 
                            { { "NAME", this.tb_name },
                              {"DEL_FAX", this.tb_contact_fax},
                              {"DEL_CONTACT_NAME", this.tb_contact_name},
                              {"E_MAIL", this.tb_email},

                              {"TELEPHONE", this.tb_contact_telephone},
                              {"TELEPHONE_2", this.tb_contact_telephone2},
                              {"TRADE_CONTACT", this.tb_contact_trade},
                              {"VAT_REG_NUMBER", this.tb_vat},
                              {"WWW", this.tb_contact_website},};


            string address_format = "ADDRESS_{0}";
            for (int ii = 1; ii < m_adress_bxs.Length; ++ii) {

                var item = m_adress_bxs[ii];

                temp1.Add(address_format.spf(ii),
                          item);

            }

            var temp2 = temp1.Transform_Values((tb) => Sage_Binding.Make_IO(tb));



            // /*       RTB binding 
            //removed in revision 716       */

            /*       Balance NB binding        */
            {
                Func<object> first = () =>
                {
                    object ret;

                    binding.Old_Values.TryGetValue(BALANCE, out ret);

                    return ret;
                };

                Action<object> second = (dbl) =>
                {
                    if (dbl.IsNullOrEmpty())
                        dbl = 0.00m;

                    lab_balance.Value = (dbl).ToDecimal();

                };


                temp2.Add(BALANCE, IO_Pair.Make(first, second));
            }

            /*       Country code binding        */
            {
                Func<object> first = () =>
                {
                    var item = cb_country.SelectedItem;

                    if (item == null)
                        return null;

                    string str = item.ToString();
                    string code = str.Split(' ')[0];

                    return code;
                };

                Action<object> second = (str) =>
                {
                    string code = str.ToString();
                    cb_country.SelectedIndex = s_countries.IndexOfKey(code);
                };

                temp2.Add(COUNTRY_CODE, IO_Pair.Make(first, second));
            }

            return temp2;
        }



        /*       Called on construction        */




        #region controls - 50 lines


        Button but_delivery;
        Button but_keyboard;
        CheckBox chb_email;
        ComboBox cb_country;
        GroupBox groupBox1;
        GroupBox groupBox2;
        GroupBox groupBox3;
        GroupBox groupBox4;
        Amount_Label lab_balance;
        Label lab_new_acc;
        Label label1;
        Label label10;
        Label label11;
        Label label12;
        Label label13;
        Label label14;
        Label label15;
        Label label16;
        Label label17;
        Label label18;
        Label label2;
        Label label3;
        Label label4;
        Label label5;
        Label label6;
        Label label7;
        Label label8;
        Label label9;
        Advanced_Combo_Box cb_account;
        Our_Text_Box tb_contact_fax;
        Our_Text_Box tb_contact_name;
        Our_Text_Box tb_contact_telephone;
        Our_Text_Box tb_contact_telephone2;
        Our_Text_Box tb_contact_trade;
        Our_Text_Box tb_contact_website;
        Our_Text_Box tb_email;
        Our_Text_Box tb_name;
        Our_Text_Box tb_vat;
        Panel panel1;


        Button but_save;
        Button but_discard;
        Button but_delete;
        Button but_previous;
        Button but_next;
        Button but_close;
        #endregion
        Address_Box address_Box1;

        #region designer - 600 lines

        System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        void InitializeComponent() {
            this.but_delete = new System.Windows.Forms.Button();
            this.but_discard = new System.Windows.Forms.Button();
            this.but_next = new System.Windows.Forms.Button();
            this.but_previous = new System.Windows.Forms.Button();
            this.but_save = new System.Windows.Forms.Button();
            this.but_close = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chb_email = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tb_email = new Common.Controls.Our_Text_Box();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lab_balance = new Amount_Label();
            this.lab_new_acc = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_account = new Common.Controls.Advanced_Combo_Box();
            this.tb_name = new Common.Controls.Our_Text_Box();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb_contact_website = new Common.Controls.Our_Text_Box();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tb_contact_fax = new Common.Controls.Our_Text_Box();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_contact_telephone2 = new Common.Controls.Our_Text_Box();
            this.tb_contact_telephone = new Common.Controls.Our_Text_Box();
            this.tb_contact_trade = new Common.Controls.Our_Text_Box();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_contact_name = new Common.Controls.Our_Text_Box();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.address_Box1 = new Common.Controls.Address_Box(5);
            this.cb_country = new System.Windows.Forms.ComboBox();
            this.tb_vat = new Common.Controls.Our_Text_Box();
            this.but_delivery = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.but_keyboard = new Unselectable_Button();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // but_delete
            // 
            this.but_delete.Location = new System.Drawing.Point(171, 493);
            this.but_delete.Name = "but_delete";
            this.but_delete.Size = new System.Drawing.Size(75, 23);
            this.but_delete.TabIndex = 18;
            this.but_delete.Text = "Dele&te";
            this.but_delete.UseVisualStyleBackColor = true;
            this.but_delete.Click += new System.EventHandler(this.but_Click);
            // 
            // but_discard
            // 
            this.but_discard.Location = new System.Drawing.Point(90, 493);
            this.but_discard.Name = "but_discard";
            this.but_discard.Size = new System.Drawing.Size(75, 23);
            this.but_discard.TabIndex = 16;
            this.but_discard.Text = "&Discard";
            this.but_discard.UseVisualStyleBackColor = true;
            this.but_discard.Click += new System.EventHandler(this.but_discard_Click);
            // 
            // but_next
            // 
            this.but_next.Location = new System.Drawing.Point(331, 493);
            this.but_next.Name = "but_next";
            this.but_next.Size = new System.Drawing.Size(75, 23);
            this.but_next.TabIndex = 22;
            this.but_next.Text = "&Next";
            this.but_next.UseVisualStyleBackColor = true;
            this.but_next.Click += new System.EventHandler(this.but_next_Click);
            // 
            // but_previous
            // 
            this.but_previous.Location = new System.Drawing.Point(252, 493);
            this.but_previous.Name = "but_previous";
            this.but_previous.Size = new System.Drawing.Size(75, 23);
            this.but_previous.TabIndex = 20;
            this.but_previous.Text = "&Previous";
            this.but_previous.UseVisualStyleBackColor = true;
            this.but_previous.Click += new System.EventHandler(this.but_previous_Click);
            // 
            // but_save
            // 
            this.but_save.Location = new System.Drawing.Point(9, 493);
            this.but_save.Name = "but_save";
            this.but_save.Size = new System.Drawing.Size(75, 23);
            this.but_save.TabIndex = 14;
            this.but_save.Text = "&Save";
            this.but_save.UseVisualStyleBackColor = true;
            this.but_save.Click += new System.EventHandler(this.but_Click);
            // 
            // but_close
            // 
            this.but_close.Location = new System.Drawing.Point(712, 493);
            this.but_close.Name = "but_close";
            this.but_close.Size = new System.Drawing.Size(75, 23);
            this.but_close.TabIndex = 30;
            this.but_close.Text = "Close";
            this.but_close.UseVisualStyleBackColor = true;
            this.but_close.Click += new System.EventHandler(this.but_close_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Location = new System.Drawing.Point(10, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(777, 447);
            this.panel1.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chb_email);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.tb_email);
            this.groupBox4.Location = new System.Drawing.Point(324, 184);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(310, 72);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "e-Mail Settings";
            // 
            // chb_email
            // 
            this.chb_email.AutoSize = true;
            this.chb_email.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_email.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_email.Location = new System.Drawing.Point(8, 43);
            this.chb_email.Name = "chb_email";
            this.chb_email.Size = new System.Drawing.Size(289, 17);
            this.chb_email.TabIndex = 4;
            this.chb_email.Text = "I send invoices to this customer electronically                  ";
            this.chb_email.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chb_email.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(11, 23);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 13);
            this.label17.TabIndex = 11;
            this.label17.Text = "e-Mail";
            // 
            // tb_email
            // 
            this.tb_email.Auto_Highlight = false;
            this.tb_email.BackColor = System.Drawing.Color.White;
            this.tb_email.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_email.Is_Readonly = false;
            this.tb_email.Location = new System.Drawing.Point(115, 16);
            this.tb_email.Name = "tb_email";
            this.tb_email.Right_Padding = 0;
            this.tb_email.Size = new System.Drawing.Size(183, 20);
            this.tb_email.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lab_balance);
            this.groupBox1.Controls.Add(this.lab_new_acc);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_account);
            this.groupBox1.Controls.Add(this.tb_name);
            this.groupBox1.Location = new System.Drawing.Point(17, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 102);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Account Details";
            // 
            // lab_balance
            // 
            this.lab_balance.Decimal_Places = 2;
            this.lab_balance.BackColor = Color.White;
            this.lab_balance.Location = new System.Drawing.Point(100, 71);
            this.lab_balance.Name = "lab_balance";
            this.lab_balance.Size = new System.Drawing.Size(101, 20);
            this.lab_balance.TabIndex = 12;
            this.lab_balance.Text = "0.00";
            // 
            // lab_new_acc
            // 
            this.lab_new_acc.Location = new System.Drawing.Point(208, 16);
            this.lab_new_acc.Name = "lab_new_acc";
            this.lab_new_acc.Size = new System.Drawing.Size(80, 22);
            this.lab_new_acc.TabIndex = 10;
            this.lab_new_acc.Text = "New Account";
            this.lab_new_acc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lab_new_acc.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Balance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "A/C";
            // 
            // cb_account
            // 
            this.cb_account.Allow_New_Account = false;
            this.cb_account.Allow_Blank = false;
            this.cb_account.Auto_Tab = false;
            this.cb_account.BackColor = System.Drawing.Color.Silver;
            this.cb_account.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.cb_account.Control_Host = null;
            this.cb_account.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_account.Location = new System.Drawing.Point(100, 16);
            this.cb_account.Max_Shortlist_Size = new System.Drawing.Size(0, 0);
            this.cb_account.MaxLength = 0;
            this.cb_account.MinimumSize = new System.Drawing.Size(80, 21);
            this.cb_account.Name = "cb_account";
            this.cb_account.QSF_Visible = false;
            this.cb_account.SelectedIndex = -1;
            this.cb_account.SelectedText = "";
            this.cb_account.SelectionLength = 0;
            this.cb_account.SelectionStart = 0;
            this.cb_account.Size = new System.Drawing.Size(100, 21);
            this.cb_account.TabIndex = 0;
            this.cb_account.Value = null;
            // 
            // tb_name
            // 
            this.tb_name.Auto_Highlight = false;
            this.tb_name.BackColor = System.Drawing.Color.White;
            this.tb_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_name.Is_Readonly = false;
            this.tb_name.Location = new System.Drawing.Point(100, 45);
            this.tb_name.Name = "tb_name";
            this.tb_name.Right_Padding = 0;
            this.tb_name.Size = new System.Drawing.Size(191, 20);
            this.tb_name.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb_contact_website);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.tb_contact_fax);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.tb_contact_telephone2);
            this.groupBox3.Controls.Add(this.tb_contact_telephone);
            this.groupBox3.Controls.Add(this.tb_contact_trade);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.tb_contact_name);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(324, 14);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(310, 168);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Contact Information";
            // 
            // tb_contact_website
            // 
            this.tb_contact_website.Auto_Highlight = false;
            this.tb_contact_website.BackColor = System.Drawing.Color.White;
            this.tb_contact_website.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_contact_website.Is_Readonly = false;
            this.tb_contact_website.Location = new System.Drawing.Point(115, 138);
            this.tb_contact_website.Name = "tb_contact_website";
            this.tb_contact_website.Right_Padding = 0;
            this.tb_contact_website.Size = new System.Drawing.Size(183, 20);
            this.tb_contact_website.TabIndex = 12;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 141);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 13);
            this.label14.TabIndex = 11;
            this.label14.Text = "Website";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 93);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 10;
            this.label13.Text = "Telephone 2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 69);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "Telephone";
            // 
            // tb_contact_fax
            // 
            this.tb_contact_fax.Auto_Highlight = false;
            this.tb_contact_fax.BackColor = System.Drawing.Color.White;
            this.tb_contact_fax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_contact_fax.Is_Readonly = false;
            this.tb_contact_fax.Location = new System.Drawing.Point(115, 113);
            this.tb_contact_fax.Name = "tb_contact_fax";
            this.tb_contact_fax.Right_Padding = 0;
            this.tb_contact_fax.Size = new System.Drawing.Size(183, 20);
            this.tb_contact_fax.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 117);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Fax";
            // 
            // tb_contact_telephone2
            // 
            this.tb_contact_telephone2.Auto_Highlight = false;
            this.tb_contact_telephone2.BackColor = System.Drawing.Color.White;
            this.tb_contact_telephone2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_contact_telephone2.Is_Readonly = false;
            this.tb_contact_telephone2.Location = new System.Drawing.Point(115, 89);
            this.tb_contact_telephone2.Name = "tb_contact_telephone2";
            this.tb_contact_telephone2.Right_Padding = 0;
            this.tb_contact_telephone2.Size = new System.Drawing.Size(183, 20);
            this.tb_contact_telephone2.TabIndex = 9;
            // 
            // tb_contact_telephone
            // 
            this.tb_contact_telephone.Auto_Highlight = false;
            this.tb_contact_telephone.BackColor = System.Drawing.Color.White;
            this.tb_contact_telephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_contact_telephone.Is_Readonly = false;
            this.tb_contact_telephone.Location = new System.Drawing.Point(115, 65);
            this.tb_contact_telephone.Name = "tb_contact_telephone";
            this.tb_contact_telephone.Right_Padding = 0;
            this.tb_contact_telephone.Size = new System.Drawing.Size(183, 20);
            this.tb_contact_telephone.TabIndex = 6;
            // 
            // tb_contact_trade
            // 
            this.tb_contact_trade.Auto_Highlight = false;
            this.tb_contact_trade.BackColor = System.Drawing.Color.White;
            this.tb_contact_trade.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_contact_trade.Is_Readonly = false;
            this.tb_contact_trade.Location = new System.Drawing.Point(115, 41);
            this.tb_contact_trade.Name = "tb_contact_trade";
            this.tb_contact_trade.Right_Padding = 0;
            this.tb_contact_trade.Size = new System.Drawing.Size(183, 20);
            this.tb_contact_trade.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Trade Contact";
            // 
            // tb_contact_name
            // 
            this.tb_contact_name.Auto_Highlight = false;
            this.tb_contact_name.BackColor = System.Drawing.Color.White;
            this.tb_contact_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_contact_name.Is_Readonly = false;
            this.tb_contact_name.Location = new System.Drawing.Point(115, 17);
            this.tb_contact_name.Name = "tb_contact_name";
            this.tb_contact_name.Right_Padding = 0;
            this.tb_contact_name.Size = new System.Drawing.Size(183, 20);
            this.tb_contact_name.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Contact Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.address_Box1);
            this.groupBox2.Controls.Add(this.cb_country);
            this.groupBox2.Controls.Add(this.tb_vat);
            this.groupBox2.Controls.Add(this.but_delivery);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(17, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 227);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Address";
            // 
            // address_Box1
            // 
            this.address_Box1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.address_Box1.Interim = 4;
            this.address_Box1.Left_Offset = 3;
            this.address_Box1.Location = new System.Drawing.Point(100, 16);
            this.address_Box1.Max_Chars_On_Row = 32767;
            this.address_Box1.Name = "address_Box1";
            this.address_Box1.Right_Offset = 2;
            this.address_Box1.Size = new System.Drawing.Size(191, 110);
            this.address_Box1.TabIndex = 10;
            this.address_Box1.Top_Offset = 1;
            // 
            // cb_country
            // 
            this.cb_country.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_country.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_country.FormattingEnabled = true;
            this.cb_country.Location = new System.Drawing.Point(101, 133);
            this.cb_country.Name = "cb_country";
            this.cb_country.Size = new System.Drawing.Size(189, 21);
            this.cb_country.TabIndex = 20;
            // 
            // tb_vat
            // 
            this.tb_vat.Auto_Highlight = false;
            this.tb_vat.BackColor = System.Drawing.Color.White;
            this.tb_vat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_vat.Is_Readonly = false;
            this.tb_vat.Location = new System.Drawing.Point(100, 160);
            this.tb_vat.Name = "tb_vat";
            this.tb_vat.Right_Padding = 0;
            this.tb_vat.Size = new System.Drawing.Size(191, 20);
            this.tb_vat.TabIndex = 30;
            // 
            // but_delivery
            // 
            this.but_delivery.Enabled = false;
            this.but_delivery.Location = new System.Drawing.Point(146, 187);
            this.but_delivery.Name = "but_delivery";
            this.but_delivery.Size = new System.Drawing.Size(145, 23);
            this.but_delivery.TabIndex = 40;
            this.but_delivery.Text = "Delivery";
            this.but_delivery.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "VAT Number";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 99);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 13);
            this.label15.TabIndex = 6;
            this.label15.Text = "Post Code";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 80);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(40, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "County";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Country";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Town";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Street 1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Street 2";
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(11, 39);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 10);
            this.label18.TabIndex = 26;
            // 
            // but_keyboard
            // 
            this.but_keyboard.Location = new System.Drawing.Point(412, 493);
            this.but_keyboard.Name = "but_keyboard";
            this.but_keyboard.Size = new System.Drawing.Size(75, 23);
            this.but_keyboard.TabIndex = 27;
            this.but_keyboard.Text = "Keyboard";
            this.but_keyboard.UseVisualStyleBackColor = true;
            this.but_keyboard.Click += new System.EventHandler(this.but_keyboard_Click_1);
            // 
            // New_Customer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ClientSize = new System.Drawing.Size(800, 523);
            this.Controls.Add(this.but_keyboard);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.but_close);
            this.Controls.Add(this.but_next);
            this.Controls.Add(this.but_previous);
            this.Controls.Add(this.but_delete);
            this.Controls.Add(this.but_discard);
            this.Controls.Add(this.but_save);
            this.Name = "New_Customer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Customer Account";
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion



        protected override void OnShown(EventArgs e) {

            base.OnShown(e);

            cb_account.Select_Focus();

        }

    }
}

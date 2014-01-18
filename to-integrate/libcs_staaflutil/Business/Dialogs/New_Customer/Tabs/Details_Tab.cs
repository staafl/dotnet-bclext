using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Colors = Standardization.Colors;

using Fairweather.Service;


using Common;
using Common.Controls;

namespace Screens
{
    class Details_Tab : UserControl, IQuickSearchFormHost
    {
        // Not set
        void cb_account_AcceptChanges(object sender, EventArgs e) {

            Accept_Selected_User();

        }

        // Not set
        void cb_account_NewAccount(object sender, EventArgs e) {

            Handle_New_Account(true);

        }

        public void Handle_QuickSearchFormEvent(QuickSearchFormEvent event_type) {
            (event_type == QuickSearchFormEvent.Search).Throw_If_False();
        }
        public void CollectQuickSearchResult(string result, QuickSearchFormMode mode) {

            cb_account.Value = result;
            Accept_Selected_User();

        }


        static SortedList<string, Country_Record> s_countries;
        const string ACCOUNT_REF = "ACCOUNT_REF";
        const string BALANCE = "BALANCE";
        const string COUNTRY_CODE = "COUNTRY_CODE";

        const int cst_address_line_cnt = 5;

        readonly
        Dictionary<string, object> m_old_values = new Dictionary<string, object>();

        readonly
        TextBox[] m_adress_bxs = new TextBox[cst_address_line_cnt + 1];

        readonly
        Twoway<string, IO_Pair<object>> m_fields = new Twoway<string, IO_Pair<object>>(30);

        public Details_Tab() {

            if (s_countries == null)
                s_countries = new SortedList<string, Country_Record>(m_sgr.Get_Countries());

            InitializeComponent();

            m_adress_bxs = new TextBox[]{null,
                                        tb_address_1,
                                        tb_address_2,
                                        tb_address_3,
                                        tb_address_4,
                                        tb_address_5,};

            foreach (TextBox item in m_adress_bxs) {
                if (item == null)
                    continue;

                item.BorderStyle = BorderStyle.None;

            }

            Prepare_Fields();

            Prepare_Cbxs();

            Prepare_Country_Cbx();

            Prepare_Label();

            this.chb_email.Visible = false;

            if (!preselected.IsNullOrEmpty()) {

                cb_account.Value = preselected;

                if (cb_account.Value == null)
                    Handle_New_Account(false);
                else
                    Accept_Selected_User();
            }



            //// http://www.xtremevbtalk.com/showthread.php?t=238220
            //var rtf2 = rtb_address.Rtf.Replace("\\par", "\\par\\sl1");      
            // rtb_address.Rtf = rtf2;

            /*       TODO: Find out what field this goes into        */

            // fields.Add(Make_Chb_Func(this.chb_email));
            // 
        }

        void Handle_New_Account(bool tb_name_focus) {

            /*       Hack        */

            this.Clear_Controls(false);

            lab_new_acc.Visible = true;

            new_account = cb_account.Text;

            if (tb_name_focus) {

                tb_name.Focus();

            }
        }


        void Prepare_Cbxs() {

            Our_Moving_Border.Create(cb_country);

            cb_account.Control_Host = this;
            cb_account.Allow_New_Account = true;

            cb_account.NewAccount += new EventHandler<EventArgs>(cb_account_NewAccount);
            cb_account.QSF_Orientation = Pair.Make(DirectionLR.Left, DirectionUD.Down);

            cb_account.Setup_QuickSearchForm(this, QuickSearchFormMode.CustomerAccounts_new);


        }

        void Prepare_Country_Cbx() {

            cb_country.Font = new System.Drawing.Font("Lucida Console", 8f, FontStyle.Regular);

            var format = "{0}\t{1}";
            cb_country.Items.Clear();

            foreach (var country in s_countries) {

                string str = format.Printf(country.Key, country.Value.Country_Name);
                str = str.Transform_Tabs(5);
                cb_country.Items.Add(str);

            }
        }

        protected override void OnVisibleChanged(EventArgs e) {

            base.OnVisibleChanged(e);

            cb_account.Focus();

        }

        void Prepare_Label() {

            this.lab_balance.BackColor = Colors.TextBoxes.ReadOnlyBackGround;

        }

        void Prepare_Fields() {

            {   /*       Textbox binding        */

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

                    temp1.Add(address_format.Printf(ii),
                              item);

                }

                var temp2 = temp1.Transform_Values((tb) => Make_Textbox_Func(tb));

                m_fields.Load(temp2);

            }

            // /*       RTB binding 
            //removed in revision 716       */

            {   /*       Balance NB binding        */
                Func<object> first = () =>
                {
                    object ret;

                    m_old_values.TryGetValue(BALANCE, out ret);

                    return ret;
                };

                Action<object> second = (dbl) =>
                {
                    if (dbl.IsNullOrEmpty())
                        dbl = 0.00m;

                    lab_balance.Text = Convert.ToDecimal(dbl).ToString("F2");

                };


                m_fields.Add(BALANCE, IO_Pair.Make(first, second));
            }

            {   /*       Country code binding        */

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

                m_fields.Add(COUNTRY_CODE, IO_Pair.Make(first, second));
            }


        }

        static
        IO_Pair<object> Make_Textbox_Func(TextBox tb) {

            Func<object> output = () => tb.Text;
            Action<object> input = (str) => tb.Text = str.ToString();

            var ret = IO_Pair.Make(input, output);

            return ret;

        }

        static
        IO_Pair<object> Make_Rtb_Func(int row, Our_Advanced_RTB rtb) {

            Func<object> output = () => rtb.Lines[row];

            Action<object> input = (str) =>
            {
                var lines = rtb.Lines;

                if (lines.Length <= row) {

                    Array.Resize(ref lines, row + 1);

                }

                lines[row] = str.ToString();

                rtb.Lines = lines;
            };


            var ret = IO_Pair.Make(input, output);

            return ret;
        }

        static
        IO_Pair<object> Make_Chb_Func(CheckBox chb) {

            Func<object> output = () => chb.Checked ? (short)1 : 0;

            Action<object> input = (sh) =>
            {
                (sh.GetType() == typeof(short)).Throw_If_False();

                if ((short)sh == 1) {

                    chb.Checked = true;

                }
                else {

                    ((short)sh == 0).Throw_If_False();
                    chb.Checked = false;

                }

            };

            return IO_Pair.Make(input, output);

        }



        private Panel panel1;
        private GroupBox groupBox4;
        private CheckBox chb_email;
        private Label label17;
        private Our_Text_Box tb_email;
        private GroupBox groupBox1;
        private Label lab_balance;
        private Label lab_new_acc;
        private Label label3;
        private Label label2;
        private Label label1;
        private Advanced_Combo_Box cb_account;
        private Our_Text_Box tb_name;
        private GroupBox groupBox3;
        private Our_Text_Box tb_contact_website;
        private Label label14;
        private Label label13;
        private Label label12;
        private Our_Text_Box tb_contact_fax;
        private Label label11;
        private Our_Text_Box tb_contact_telephone2;
        private Our_Text_Box tb_contact_telephone;
        private Our_Text_Box tb_contact_trade;
        private Label label10;
        private Our_Text_Box tb_contact_name;
        private Label label9;
        private GroupBox groupBox2;
        private Our_Text_Box tb_address_5;
        private Our_Text_Box tb_address_4;
        private ComboBox cb_country;
        private Our_Text_Box tb_address_3;
        private Our_Text_Box tb_vat;
        private Button but_delivery;
        private Our_Text_Box tb_address_1;
        private Label label8;
        private Our_Text_Box tb_address_2;
        private Label label15;
        private Label label16;
        private Label label7;
        private Label label4;
        private Label label6;
        private Label label5;
        private Label rtb_address;

        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chb_email = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tb_email = new Screens.Our_TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lab_balance = new System.Windows.Forms.Label();
            this.lab_new_acc = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_account = new Screens.Our_Advanced_Combo_Box();
            this.tb_name = new Screens.Our_TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb_contact_website = new Screens.Our_TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tb_contact_fax = new Screens.Our_TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_contact_telephone2 = new Screens.Our_TextBox();
            this.tb_contact_telephone = new Screens.Our_TextBox();
            this.tb_contact_trade = new Screens.Our_TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_contact_name = new Screens.Our_TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_address_5 = new Screens.Our_TextBox();
            this.tb_address_4 = new Screens.Our_TextBox();
            this.cb_country = new System.Windows.Forms.ComboBox();
            this.tb_address_3 = new Screens.Our_TextBox();
            this.tb_vat = new Screens.Our_TextBox();
            this.but_delivery = new System.Windows.Forms.Button();
            this.tb_address_1 = new Screens.Our_TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_address_2 = new Screens.Our_TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rtb_address = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(777, 447);
            this.panel1.TabIndex = 26;
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
            this.lab_balance.BackColor = System.Drawing.Color.White;
            this.lab_balance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_balance.Location = new System.Drawing.Point(100, 71);
            this.lab_balance.Name = "lab_balance";
            this.lab_balance.Size = new System.Drawing.Size(101, 20);
            this.lab_balance.TabIndex = 12;
            this.lab_balance.Text = "0.00";
            this.lab_balance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.label3.Location = new System.Drawing.Point(11, 76);
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
            this.cb_account.AllowBlank = false;
            this.cb_account.AutoCompleteCustomSource = null;
            this.cb_account.AutoTab = false;
            this.cb_account.BackColor = System.Drawing.Color.Silver;
            this.cb_account.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.cb_account.Control_Host = null;
            this.cb_account.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_account.Location = new System.Drawing.Point(100, 16);
            this.cb_account.MaximumShortListSize = new System.Drawing.Size(0, 0);
            this.cb_account.MaxLength = 0;
            this.cb_account.MinimumSize = new System.Drawing.Size(80, 21);
            this.cb_account.Name = "cb_account";
            this.cb_account.SelectedIndex = 0;
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
            this.groupBox2.Controls.Add(this.tb_address_5);
            this.groupBox2.Controls.Add(this.tb_address_4);
            this.groupBox2.Controls.Add(this.cb_country);
            this.groupBox2.Controls.Add(this.tb_address_3);
            this.groupBox2.Controls.Add(this.tb_vat);
            this.groupBox2.Controls.Add(this.but_delivery);
            this.groupBox2.Controls.Add(this.tb_address_1);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tb_address_2);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.rtb_address);
            this.groupBox2.Location = new System.Drawing.Point(17, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 227);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Address";
            // 
            // tb_address_5
            // 
            this.tb_address_5.Auto_Highlight = false;
            this.tb_address_5.BackColor = System.Drawing.Color.White;
            this.tb_address_5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_address_5.Is_Readonly = false;
            this.tb_address_5.Location = new System.Drawing.Point(104, 97);
            this.tb_address_5.Name = "tb_address_5";
            this.tb_address_5.Size = new System.Drawing.Size(184, 20);
            this.tb_address_5.TabIndex = 32;
            this.tb_address_5.TabStop = false;
            // 
            // tb_address_4
            // 
            this.tb_address_4.Auto_Highlight = false;
            this.tb_address_4.BackColor = System.Drawing.Color.White;
            this.tb_address_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_address_4.Is_Readonly = false;
            this.tb_address_4.Location = new System.Drawing.Point(104, 78);
            this.tb_address_4.Name = "tb_address_4";
            this.tb_address_4.Size = new System.Drawing.Size(184, 20);
            this.tb_address_4.TabIndex = 31;
            this.tb_address_4.TabStop = false;
            // 
            // cb_country
            // 
            this.cb_country.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_country.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_country.FormattingEnabled = true;
            this.cb_country.Location = new System.Drawing.Point(101, 133);
            this.cb_country.Name = "cb_country";
            this.cb_country.Size = new System.Drawing.Size(189, 21);
            this.cb_country.TabIndex = 4;
            // 
            // tb_address_3
            // 
            this.tb_address_3.Auto_Highlight = false;
            this.tb_address_3.BackColor = System.Drawing.Color.White;
            this.tb_address_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_address_3.Is_Readonly = false;
            this.tb_address_3.Location = new System.Drawing.Point(104, 59);
            this.tb_address_3.Name = "tb_address_3";
            this.tb_address_3.Size = new System.Drawing.Size(184, 20);
            this.tb_address_3.TabIndex = 30;
            this.tb_address_3.TabStop = false;
            // 
            // tb_vat
            // 
            this.tb_vat.Auto_Highlight = false;
            this.tb_vat.BackColor = System.Drawing.Color.White;
            this.tb_vat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_vat.Is_Readonly = false;
            this.tb_vat.Location = new System.Drawing.Point(100, 160);
            this.tb_vat.Name = "tb_vat";
            this.tb_vat.Size = new System.Drawing.Size(191, 20);
            this.tb_vat.TabIndex = 8;
            // 
            // but_delivery
            // 
            this.but_delivery.Enabled = false;
            this.but_delivery.Location = new System.Drawing.Point(146, 187);
            this.but_delivery.Name = "but_delivery";
            this.but_delivery.Size = new System.Drawing.Size(145, 23);
            this.but_delivery.TabIndex = 10;
            this.but_delivery.Text = "Delivery";
            this.but_delivery.UseVisualStyleBackColor = true;
            // 
            // tb_address_1
            // 
            this.tb_address_1.Auto_Highlight = false;
            this.tb_address_1.BackColor = System.Drawing.Color.White;
            this.tb_address_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_address_1.Is_Readonly = false;
            this.tb_address_1.Location = new System.Drawing.Point(104, 21);
            this.tb_address_1.Name = "tb_address_1";
            this.tb_address_1.Size = new System.Drawing.Size(184, 20);
            this.tb_address_1.TabIndex = 0;
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
            // tb_address_2
            // 
            this.tb_address_2.Auto_Highlight = false;
            this.tb_address_2.BackColor = System.Drawing.Color.White;
            this.tb_address_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_address_2.Is_Readonly = false;
            this.tb_address_2.Location = new System.Drawing.Point(104, 40);
            this.tb_address_2.Name = "tb_address_2";
            this.tb_address_2.Size = new System.Drawing.Size(184, 20);
            this.tb_address_2.TabIndex = 30;
            this.tb_address_2.TabStop = false;
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
            // rtb_address
            // 
            this.rtb_address.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtb_address.Location = new System.Drawing.Point(100, 16);
            this.rtb_address.Name = "rtb_address";
            this.rtb_address.Size = new System.Drawing.Size(191, 110);
            this.rtb_address.TabIndex = 0;
            // 
            // Details_Tab
            // 
            this.Controls.Add(this.panel1);
            this.Name = "Details_Tab";
            this.Size = new System.Drawing.Size(777, 447);
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
    }
}
using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using Common.Controls;


namespace Common.Dialogs
{
    public partial class New_Customer
    {
        public Records_Cursor RecordsCursor {
            get;
            set;
        }

        string new_account;
        int current_index = -1;

        Calculator calculator;


        const string ACCOUNT_REF = "ACCOUNT_REF";
        const string BALANCE = "BALANCE";
        const string COUNTRY_CODE = "COUNTRY_CODE";

        const int cst_address_line_cnt = 5;
        
        readonly Sage_Binding binding;


        TextBox[] m_adress_bxs = new TextBox[cst_address_line_cnt + 1];

        readonly int default_country;
        readonly SortedList<string, Country_Record> s_countries;

        readonly Sage_Logic m_sdr;
    }

}

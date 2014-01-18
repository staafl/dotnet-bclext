using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using Common.Controls;
using Fairweather.Service;

namespace Common.Dialogs
{
    public partial class Search_Form : Hook_Enabled_Form
    {
        readonly Sage_Logic m_sgr;
        readonly Record_Type record_type;

        static Multi_Dict<Record_Type, string> titles = new Multi_Dict<Record_Type, string>
        {
            {Record_Type.Stock, "Record Search - Products", "Products", 
                                "Stock Code", 
                                "Description",
                                "Sales Price",
                                "Stock Balance",
                                "Balance incl. unposted"},

            {Record_Type.Sales, "Record Search - Customers", "Customers",
                                "A/C Ref", 
                                "Name", 
                                "Telephone", 
                                "Balance",
                                ""},
            {Record_Type.Purchase, "Record Search - Suppliers", "Suppliers",
                                "A/C Ref", 
                                "Name", 
                                "Telephone", 
                                "Balance",
                                ""},
        };

        readonly bool m_caseless_contains;

        const string ADDRESS = "ADDRESS";
        const string TELEPHONE = "TELEPHONE";
        const string COUNTRY = "COUNTRY";

        const int RESULT_AC = 0;
        const int RESULT_NAME = 1;
        const int RESULT_PHONE = 2;
        const int RESULT_BALANCE = 3;
        const int RESULT_TOTAL_BALANCE = 4;


        Pair<string>? m_result = null;

        readonly Dictionary<Our_Text_Box, Pair<string, Label>> rd_predefined_tbxs;

    }

}
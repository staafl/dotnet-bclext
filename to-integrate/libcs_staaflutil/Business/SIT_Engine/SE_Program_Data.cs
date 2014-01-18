using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Common;
using Versioning;

using Fairweather.Service;
using D = System.Collections.Generic.Dictionary<string, string>;

namespace Sage_Int
{
    partial class SIT_Engine
    {
        static readonly Set<Record_Type>
       accounts = new Set<Record_Type> { 
                Record_Type.Sales ,
                Record_Type.Purchase,
                Record_Type.Expense,
                Record_Type.Bank ,
            };



        // ****************************

        public static readonly Dictionary<int, string>
        mapping_files = new Dictionary<int, string>{
        {1, "cus"}, {2, "pur"}, {3, "nom"},
        {4, "bank"}, {5, "stock"}, {21, "aud"}, {22, "sttran"}};

        public  enum Return_Code
        {
            All_OK,
            Unknown_Error = 2,
            Not_Registered = 3,
            Datapath_Error = 4,
        }


        static public  readonly Set<Record_Type>
        sit_tbi_modes = new Set<Record_Type> { 
            Record_Type.Document, 
            Record_Type.Sales_Order, 
            Record_Type.Purchase_Order, 
            Record_Type.Stock_Tran };

        static public readonly Twoway<int, Record_Type>
        sit_modes = new Twoway<int, Record_Type>
        {
            {1, Record_Type.Sales},
            {2, Record_Type.Purchase},
            {3, Record_Type.Expense},
            {4, Record_Type.Bank},
            {5, Record_Type.Stock},
            {10,Record_Type.Document},
            {11,Record_Type.Invoice_Or_Credit},
            {12,Record_Type.Sales_Order},
            {13,Record_Type.Purchase_Order},
            {21, Record_Type.Audit_Trail},
            {22, Record_Type.Stock_Tran},
        };

        static public readonly Triple_Dict<Record_Type, int, string, Func<Sit_General_Settings, bool>>
        sitgui_modules = new Triple_Dict<Record_Type, int, string, Func<Sit_General_Settings, bool>> 
        {
            {Record_Type.Sales, 1,"Customers Records Interface", _sett => _sett.Records_Module == true}, 
            {Record_Type.Purchase,2,"Suppliers Records Interface", _sett => _sett.Records_Module == true},
            {Record_Type.Expense,3,"Nominal Records Interface", _sett => _sett.Records_Module == true},
            {Record_Type.Bank,4,"Bank Records Interface", _sett => _sett.Records_Module == true},
            {Record_Type.Stock,5,"Products Records Interface", _sett => _sett.Records_Module == true},

            //{Record_Type.Document,10,"Documents Interface", _sett => _sett.Docs_Module == true},
            // d3c40ddf-e24f-4cf8-a4d8-520d1301fe99
            {Record_Type.Invoice_Or_Credit,11,"Invoice/Credit Notes Interface", _sett => _sett.Docs_Module == true},
            // {Record_Type.Sales_Order,12,"Sales Orders Interface", _sett => _sett.Docs_Module == true},
            // {Record_Type.Purchase_Order,13,"Purchase Orders Interface", _sett => _sett.Docs_Module == true},

            {Record_Type.Audit_Trail, 21,"Audit Trail Transactions Interface", _sett => _sett.Trans_Module == true},
            // {Record_Type.Stock_Tran, 22,"Stock Transactions Interface", _sett => _sett.Trans_Module == true},

        };

        static readonly Dictionary<Record_Type, string>
        mode_descriptions = new Dictionary<Record_Type, string>{ 
          {Record_Type.Sales, "Customers Records"}, 
          {Record_Type.Purchase, "Suppliers Records"}, 
          {Record_Type.Expense, "Nominal Records"}, 
          {Record_Type.Bank, "Bank Records"},
          {Record_Type.Stock, "Stock Records"}, 
          {Record_Type.Document,"Document Entries"},
          {Record_Type.Invoice_Or_Credit,"Invoice or Credit Note Entries"},
          {Record_Type.Sales_Order,"Sales Orders"},
          {Record_Type.Purchase_Order,"Purchase Orders"},
          //{6, "Project Records"}, 
          {Record_Type.Audit_Trail, "Audit Trail Transactions"}, 
          {Record_Type.Stock_Tran, "Stock Transactions" }
        };

        // 6a7000a2-e4b4-4aaa-8537-50a1af396c7a

        Set<Record_Type>
        docs_modes = new Set<Record_Type>
        {
            //Record_Type.Document,
            Record_Type.Invoice_Or_Credit,
            //Record_Type.Sales_Order,
            //Record_Type.Purchase_Order
        };

        //# 05596fee-da61-49cf-8730-1345ec7ab158

        Set<Record_Type>
        trans_modes = new Set<Record_Type>
        {
           Record_Type.Audit_Trail, 
           Record_Type.Stock_Tran
        };

        Set<Record_Type>
        record_modes = new Set<Record_Type>{
                          Record_Type.Sales, 
                          Record_Type.Purchase, 
                          Record_Type.Expense, 
                          Record_Type.Bank, 
                          Record_Type.Stock 
        };

        static readonly Record_Type[]
        mode_ixes = { 
                          Record_Type.Sales, 
                          Record_Type.Purchase, 
                          Record_Type.Expense, 
                          Record_Type.Bank, 
                          Record_Type.Stock, 
                          Record_Type.Audit_Trail, 
                          Record_Type.Stock_Tran,
                          Record_Type.Document,
                          Record_Type.Invoice_Or_Credit,
                          Record_Type.Sales_Order,
                          Record_Type.Purchase_Order,
                    };


 




    }
}

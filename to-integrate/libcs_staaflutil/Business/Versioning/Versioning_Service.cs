using System;
using System.Collections.Generic;
using Common;
using Fairweather.Service;

using TT = Versioning.TransType;
using STT = Versioning.StockTransType;


namespace Versioning
{

    public static class Service
    {
        public static First_Next First_Next(this IMove imove) {
            return imove.First_Next(false);
        }
        public static First_Next First_Next(this IMove imove, bool reverse) {
            if (reverse)
                return new First_Next(() => imove.MoveLast(),
                                     () => imove.MovePrev());
            else
                return new First_Next(() => imove.MoveFirst(),
                                     () => imove.MoveNext());
        }



        public static void Set_Index_Value(this IFields fields, object obj) {

            fields[fields.Index_String] = obj;

        }

        public static object Get_Index_Value(this IFields fields) {

            return fields[fields.Index_String];

        }

        static Dictionary<InvoiceType, LedgerType> type_dict = new Dictionary<InvoiceType, LedgerType> 
        {
            {InvoiceType.sdoProductInvoice, LedgerType.sdoLedgerInvoice},
            {InvoiceType.sdoProductCredit, LedgerType.sdoLedgerCredit},

            {InvoiceType.sdoProductProforma, LedgerType.sdoLedgerInvoice},
            //{InvoiceType.sdoProductProforma, LedgerType.sdoLedgerNone},

            {InvoiceType.sdoProductQuotation, LedgerType.sdoLedgerInvoice},
            //{InvoiceType.sdoProductQuotation, LedgerType.sdoLedgerNone},

            {InvoiceType.sdoServiceInvoice, LedgerType.sdoLedgerService },
            {InvoiceType.sdoServiceCredit, LedgerType.sdoLedgerServiceCredit },

            {InvoiceType.sdoServiceProforma, LedgerType.sdoLedgerService }, 
            //{InvoiceType.sdoServiceProforma, LedgerType.sdoLedgerNone }, 

            {InvoiceType.sdoServiceQuotation, LedgerType.sdoLedgerService },
            //{InvoiceType.sdoServiceQuotation, LedgerType.sdoLedgerNone },

        };

        static Dictionary<TransType, bool>
        tt_receipts = new Dictionary<TransType, bool>
        {
            {TransType.sdoSA, false},
            {TransType.sdoPA, false},

            {TransType.sdoPR, true},
            {TransType.sdoSR, true},
            {TransType.sdoBR, true},
            {TransType.sdoCR, true},
            {TransType.sdoVR, true},
            //{TransType.ourXR, true},

            {TransType.sdoSP, false},
            {TransType.sdoBP, false},
            {TransType.sdoCP, false},
            {TransType.sdoVP, false},
            //{TransType.ourXP, false},
        };

        public static void Try_Disconnect(this Work_Space ws) {
            ws.Disconnect();
        }
        public static bool Is_Deleted(this IFields obj) {

            var cast = obj as Sage_Container;

            if (cast != null)
                return cast.Deleted;

            var flag = (short)obj["DELETED_FLAG"];

            return flag != 0;
        }

        public static LedgerType?
        Ledger_Type(this InvoiceType inv_type) {

            LedgerType lt;

            bool ok = type_dict.TryGetValue(inv_type, out lt);

            if (ok)
                return lt;
            else
                return null;

        }

        public static bool Is_Payment(this TT tt) {
            return !tt_receipts.Get_Or_Default_(tt, true);
        }
        public static bool Is_Receipt(this TT tt) {
            return tt_receipts.Get_Or_Default_(tt, false);
        }
        public static TT
        Correct_Trans_Type(this TT base_tt, BankType acc_type) {

            bool is_receipt;
            if (!tt_receipts.TryGetValue(base_tt, out is_receipt))
                return base_tt;

            return vcb_types[acc_type, is_receipt];

        }

        public static TT?
        From_Short_String(this string str) {
            TT ret;
            if (tt_to_short_str.TryGetValue(str, out ret))
                return ret;
            return null;
        }

        public static TT?
        From_String(this string str) {
            TT ret;
            if (tt_to_str.TryGetValue(str, out ret))
                return ret;
            return null;
        }

        public static string
        String(this InvoiceType it) {
            var ret = "";
            if (invoice_type_to_str.TryGetValue(it, out ret))
                return ret;
            return it.Get_String();
        }

        public static string
        Short_String(this TT tt) {
            var ret = "";
            if (tt_to_short_str.TryGetValue(tt, out ret))
                return ret;
            return tt.Get_String();
        }

        public static string
        String(this TT tt) {
            var ret = "";
            if (tt_to_str.TryGetValue(tt, out ret))
                return ret;
            return tt.Get_String();
        }

        static public bool
        Invented(this TT tt) {
            switch (tt) {
                //case TransType.ourXR:
                //case TransType.ourXP:
                //    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Whether or not a transaction's amount logically represents
        /// income.
        /// </summary>
        public static bool
        Logically_Positive(this TT type) {
            return !negative[type];
        }
        public static Record_Type
        Get_TT_Mode(this TT type) {
            return sales_TT[type] ? Record_Type.Sales :
                   purchase_TT[type] ? Record_Type.Purchase :
                   nominal_TT[type] ? Record_Type.Expense :
                   bank_TT[type] ? Record_Type.Bank :
                   0 /* PAI, PAO, CC, CD, CO etc*/;
            //((Func<Record_Type>)(() => { throw new InvalidOperationException(); }))();
        }

        public static bool
        Should_Be_Bank(this TT type) {
            return bank_bank[type];
        }

        public static bool
        Should_Not_Be_Bank_Major(this TT type) {
            return bank_not_bank_or_major[type] || bank_TT[type];
        }

        public static bool
        CC_Required(this TT type, int? version) {
            return cc_required[type] && type.Projects_Supported(version);
        }

        public static bool
        CC_Allowed(this TT type, int? version) {
            return cc_allowed[type] && type.Projects_Supported(version);
        }

        static readonly Set<TT> negative = new Set<TT> { SC, SP, SR, SD, SA, PI };

        const TransType
        SI = TransType.sdoSI,
        SC = TransType.sdoSC,
        SR = TransType.sdoSR,
        SA = TransType.sdoSA,
        SD = TransType.sdoSD,
        PI = TransType.sdoPI,
        PC = TransType.sdoPC,
        PP = TransType.sdoPP,
        PA = TransType.sdoPA,
        PD = TransType.sdoPD,
        BP = TransType.sdoBP,
        BR = TransType.sdoBR,
        CP = TransType.sdoCP,
        CR = TransType.sdoCR,
        JD = TransType.sdoJD,
        JC = TransType.sdoJC,
        VP = TransType.sdoVP,
        VR = TransType.sdoVR,
        CC = TransType.sdoCC,
        CD = TransType.sdoCD,
        PAI = TransType.sdoPAI,
        PAO = TransType.sdoPAO,
        CO = TransType.sdoCO,
        SP = TransType.sdoSP,
        PR = TransType.sdoPR//,
            //XR = TransType.ourXR,
            //XP = TransType.ourXP
        ;

        public static Set<TT>
            //Transaction types broken down by category
        sales_TT = new Set<TransType> { SI, SC, SA, SR, SD, SP },
        purchase_TT = new Set<TransType> { PI, PC, PA, PD, PR, PP },
        nominal_TT = new Set<TransType> { JD, JC },
        bank_TT = new Set<TransType> { BP, BR, CP, CR, VP, VR
            //, XP, XR
        },
        bank_not_bank_or_major = new Set<TransType> { SC, SI, SD, PC, PI, PD, }, // NOMINAL_CODE is NOT a BANK or MAJOR CONTROL account 
        bank_bank = new Set<TransType> { SA, SR, SP, 
                                         PA, PR, PP, }, // NOMINAL_CODE must be a BANK account, can be MAJOR CONTROL

        //Version-specific values for the TYPE field of a transaction record
        audit_TT_11 = new Set<TransType> { SI, SC, SR, SA, SD, PI, PC, PP, PA, PD, BP, BR, CP, CR, JD, JC, VP, VR },
        audit_TT_14 = new Set<TransType> { SI, SC, SR, SA, SD, PI, PC, PP, PA, PD, BP, BR, CP, CR, JD, JC, VP, VR, CC, CD, PAI, PAO, CO, SP, PR },
        latest_TTs = audit_TT_14,

        project_TT_V12 = new Set<TransType> { BP, VP, CP, PI, SI, PC, SC//, XR
        },
        project_TT_V13 = new Set<TransType> { BP, VP, CP, PI, SI, PC, SC, BR, VR, CR//, 
            //XP, XR 
            },
        cc_allowed = new Set<TransType> { BP, VP, CP, PI, PC
            //, XP
        },
        cc_required = cc_allowed;

        static readonly Dictionary<int, Set<TransType>>
        audit_types =
        new Dictionary<int, Set<TransType>> { 
            { 11, audit_TT_11 }, 
            { 12, audit_TT_11 }, 
            { 13, audit_TT_11 }, 
            { 14, audit_TT_14 }, 
            { 15, audit_TT_14 },
            { 16, audit_TT_14 },
        };

        static public bool Supported(this TT type, int? version) {
            version = version ?? 16;
            return audit_types[version.Value][type];
        }

        static public bool Projects_Supported(this TT type, int? version) {
            version = version ?? 16;
            if (version <= 11)
                return false;
            if (version >= 13)
                return project_TT_V13[type];
            return project_TT_V12[type];
        }

        static public Set<TT>
        Can_Allocate_To(this TT tt) {
            switch (tt) {
                case TransType.sdoSC:
                case TransType.sdoSR:
                    // case TransType.sdoSA:
                    // case TransType.sdoSD:
                    return new Set<TT> { TT.sdoSI };

                case TransType.sdoPC:
                case TransType.sdoPP:
                    // case TransType.sdoPA:
                    // case TransType.sdoPD:
                    return new Set<TT> { TT.sdoPI };

                default:
                    return new Set<TT> { };

            }
        }

        static readonly Pair_Key_Dict<BankType, bool, TT>
        vcb_types = new Pair_Key_Dict<BankType, bool, TT> 
        {
            {BankType.sdoTypeCash, true, TransType.sdoCR},
            {BankType.sdoTypeCash, false, TransType.sdoCP},

            {BankType.sdoTypeCredit, true, TransType.sdoVR},
            {BankType.sdoTypeCredit, false, TransType.sdoVP},

            {BankType.sdoTypeCheque, true, TransType.sdoBR},
            {BankType.sdoTypeCheque, false, TransType.sdoBP},
        };

        public static readonly Twoway<InvoiceType, string>
        invoice_type_to_str = new Twoway<InvoiceType, string>
        {
            {InvoiceType.sdoProductInvoice, "Product Invoice"},
            {InvoiceType.sdoProductCredit, "Product Credit"}
        };


        public static readonly Twoway<TT, string>
        tt_to_short_str = new Twoway<TT, string>
        #region data
		{
            {TT.sdoSI, "SI"},
            {TT.sdoSC, "SC"},
            {TT.sdoSR, "SR"},
            {TT.sdoSA, "SA"},
            {TT.sdoSD, "SD"},

            {TT.sdoPI, "PI"},
            {TT.sdoPC, "PC"},
            {TT.sdoPP, "PP"},
            {TT.sdoPA, "PA"},
            {TT.sdoPD, "PD"},

            {TT.sdoJD, "JD"},
            {TT.sdoJC, "JC"},

            {TT.sdoCC, "CC"},
            {TT.sdoCD, "CD"},

            {TT.sdoPAI, "PAI"},
            {TT.sdoPAO, "PAO"},

            {TT.sdoCO, "CO"},

            {TT.sdoSP, "SP"},
            {TT.sdoPR, "PR"},

            {TT.sdoBP, "BP"},
            {TT.sdoBR, "BR"},

            {TT.sdoCP, "CP"},
            {TT.sdoCR, "CR"},

            {TT.sdoVP, "VP"},
            {TT.sdoVR, "VR"},

            //{TT.ourXP, "XP"},
            //{TT.ourXR, "XR"},
        };
        #endregion


        static public readonly Twoway<TT, string>
        tt_to_str = new Twoway<TT, string>
        #region data
		{
            {TT.sdoSI, "Sales Invoice"},
            {TT.sdoSC, "Sales Credit"},
            {TT.sdoSR, "Sales Receipt"},
            {TT.sdoSA, "Sales Receipt on Account"},
            {TT.sdoSD, "Sales Discount"},

            {TT.sdoPI, "Purchase Invoice"},
            {TT.sdoPC, "Purchase Credit"},
            {TT.sdoPP, "Purchase Payment"},
            {TT.sdoPA, "Purchase Payment on Account"},
            {TT.sdoPD, "Purchase Discount"},

            {TT.sdoJD, "Journal Debit"},
            {TT.sdoJC, "Journal Credit"},

            {TT.sdoCC, "Project Cost Credit"},
            {TT.sdoCD, "Project Cost Debit - Charge"},

            {TT.sdoPAI, "Project Adjustment In"},
            {TT.sdoPAO, "Project Adjustment Out"},

            {TT.sdoCO, "Project Committed Cost"},

            {TT.sdoSP, "Sales Payment"},
            {TT.sdoPR, "Purchase Receipt"},

            {TT.sdoBP, "Bank Payment"},
            {TT.sdoBR, "Bank Receipt"},

            {TT.sdoCP, "Cash Payment"},
            {TT.sdoCR, "Cash Receipt"},

            {TT.sdoVP, "Visa Payment"},
            {TT.sdoVR, "Visa Receipt"},

            //{TT.ourXP, "Bank / Cash / Visa Payment"},
            //{TT.ourXR, "Bank / Cash / Visa Receipt"},                   
        };
        #endregion

        static public readonly Twoway<StockTransType, string>
        stt_to_short_str = new Twoway<StockTransType, string>
        #region data
		{
            {STT.sdoAI, "AI"},
            {STT.sdoAO, "AO"},
            {STT.sdoGI, "GI"},
            {STT.sdoGO, "GO"},
            {STT.sdoMI, "MI"},
            {STT.sdoMO, "MO"},
            {STT.sdoGR, "GR"},
            {STT.sdoDI, "DI"},
            {STT.sdoDO, "DO"},
            {STT.sdoWO, "WO"},
        };
        #endregion
        //public static string Get_Index_Value(this IFields my_object) {
        //      return my_object[my_object.Index_String].ToString();
        //}

        //public static void Set_Index_Value(this IFields my_object, object value) {
        //      my_object[my_object.Index_String] = value;
        //}
    }

}

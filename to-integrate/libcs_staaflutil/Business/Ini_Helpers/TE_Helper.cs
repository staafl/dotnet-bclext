using System;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using TT = Versioning.TransType;

namespace DTA
{
    public class TE_Helper : Ini_Helper
    {
        public TE_Helper(Ini_File ini, Company_Number company) :
            base(ini, company) {

        }

        public IEnumerable<TT> Check_Ref() {

            if (!False(DTA_Fields.TE_sales_ref_check)) {
                yield return TT.sdoSI;
                yield return TT.sdoSC;

            }

            if (!False(DTA_Fields.TE_purchase_ref_check)) {
                yield return TT.sdoPI;
                yield return TT.sdoPC;

            }

        }

        public int Max_Details_History_Length {
            get {
                return Math.Max(Int(DTA_Fields.TE_max_details_history_length), 0);
            }
        }

        public string Default_Nominal_Code(bool is_receipt) {
            return String(is_receipt ?
                        DTA_Fields.TE_default_bank_receipts :
                        DTA_Fields.TE_default_bank_payments);
        }

        public bool Jx_Against_Debtors_Creditors_Ctrl {
            get {
                return True(DTA_Fields.TE_jx_against_debtors_creditors_ctrl);
            }
        }
        public bool Posted_Report {
            get {
                return True(DTA_Fields.TE_posted_report);
            }
        }

    }
}

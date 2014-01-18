
using Common;

using Fairweather.Service;

namespace DTA
{
    public class SDR_DTA_Helper : Ini_Helper
    {



        public SDR_DTA_Helper(Company_Number company,
                              Ini_File ini)
            : base(ini, company) {

            this.Company = company;

        }

        public Company_Number Company {
            get;
            private set;
        }

        public bool Calculate_Unposted_Balance_Scanning_Account {
            get {
                if (String(DTA_Fields.POS_calculate_unposted_balance_scanning) == ACCOUNT)
                    return true;
                Calculate_Unposted_Balance_Scanning_Unposted.tiff();
                return false;
            }
        }
        public bool Calculate_Unposted_Balance_Scanning_Unposted {
            get {
                if (String(DTA_Fields.POS_calculate_unposted_balance_scanning) == UNPOSTED)
                    return true;
                Calculate_Unposted_Balance_Scanning_Account.tiff();
                return false;
            }
        }

        public bool Calculate_Days_Documents_Scanning_Date {
            get {
                if (String(DTA_Fields.POS_calculate_days_documents_scanning) == DATE)
                    return true;
                Calculate_Days_Documents_Scanning_Posted_By.tiff();
                return false;
            }
        }
        public bool Calculate_Days_Documents_Scanning_Posted_By {
            get {
                if (String(DTA_Fields.POS_calculate_days_documents_scanning) == POSTED_BY)
                    return true;
                Calculate_Days_Documents_Scanning_Date.tiff();
                return false;
            }
        }

        public bool Remittance_By_Number {
            get {
                return True(DTA_Fields.ESF_post_remittance_by_number);
            }
        }

    }
}

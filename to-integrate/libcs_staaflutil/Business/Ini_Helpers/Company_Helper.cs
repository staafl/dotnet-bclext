using System;
using System.Collections.Generic;
using Common;

using Fairweather.Service;

namespace DTA
{
    public class Company_Helper : Ini_Helper
    {


        public Company_Helper(Ini_File ini, Company_Number company, int version)
            : base(ini, company) {
            m_version = version;
            m_company = company;
        }

        public Company_Helper(Ini_File ini, Company_Number company)
            : this(ini, company, new General_Helper(ini).Version) {



        }

        public Credentials Credentials {
            get {
                return new Credentials(Company_Number, Username, Password);
            }
        }




        public string Username {
            get {
                return String(DTA_Fields.USERNAME);
            }
        }

        public string Password {
            get {
                return String(DTA_Fields.PASSWORD);
            }
        }

        public string Company_Path {
            get {
                return String(DTA_Fields.COMPANY_PATH);
            }
        }

        public Company_Number Company_Number {
            get {
                return m_company;
            }
        }

        /// <summary> Includes the filename and extension </summary>
        public string Company_Sage_Usr {
            get {
                return String(DTA_Fields.USR_FILE_PATH);
            }
        }

        public string Registration_Name {
            get {
                return String(DTA_Fields.COMPANY_NAME);
            }
        }

        public string Company_Name {
            get {
                string ret;
                if (!m_names.TryGetValue(m_company, out ret)) {
                    Sage_Logic sdr;
                    bool ok;

                    Sage_Logic.Get(ini, m_company, out sdr).tiff();

                    ret = sdr.Get_Company_Name(false, out ok);

                    if (ok)
                        m_names[m_company] = ret;

                }

                return ret;

            }
        }
#if !NO_PERIOD

        public string Company_Period {
            get {
                // Hopefully the period will not change while the application
                // is running

                Func<string>
                      getter = () =>
                      {
                          string _ret;
                          XSage_Conn ex;
                          bool ok = Activation.Activation_Helpers.Get_Period(Company_Path, Username,
                                                                 Password, m_version, false,

                                                                 out _ret, out ex);
                          ok.tiff(ex);
                          return _ret;
                      };

                var ret = m_periods.Get_Or_Default(m_company, getter);

                return ret;

            }
        }
#endif

        static Dictionary<Company_Number, string>
        m_periods = new Dictionary<Company_Number, string>();

        static Dictionary<Company_Number, string>
        m_names = new Dictionary<Company_Number, string>();

        readonly Company_Number m_company;
        readonly int m_version;

    }
}
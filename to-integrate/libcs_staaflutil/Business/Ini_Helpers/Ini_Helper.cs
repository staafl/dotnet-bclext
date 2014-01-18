using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Fairweather.Service;
using Common;

namespace DTA
{
    public abstract class Ini_Helper : IDisposable
    {

        protected Ini_Helper(
            Ini_File ini,
            bool write_on_dispose) {

            this.ini = ini;
            this.m_proxy = ini as IReadWrite<Ini_Field, string>;

            if (write_on_dispose) {
                m_on_dispose = () => ini.Write_Data();
                do_dispose = true;
            }

        }

        protected Ini_Helper(Ini_File ini) :
            this(ini, true) {
        }

        protected Ini_Helper(Ini_File ini, Company_Number company)
            : this(ini) {

            this.m_proxy = ini.Company_Proxy(company);

        }


        void IDisposable.Dispose() {

            if (do_dispose && m_on_dispose != null)
                m_on_dispose();

        }



        #region Constants
        protected const string CASH = Ini_Main.CASH;
        protected const string SUPER = Ini_Main.SUPER;
        protected const string NONE = Ini_Main.NONE;
        protected const string QTY = Ini_Main.QTY;
        protected const string AMOUNT = Ini_Main.AMOUNT;

        protected const string NO = Ini_Main.NO;
        protected const string YES = Ini_Main.YES;

        protected const string NVAT = Ini_Main.NVAT;
        protected const string VAT = Ini_Main.VAT;
        protected const string BOTH = Ini_Main.BOTH;

        protected const string PROMPT = Ini_Main.PROMPT;
        protected const string FROM_DTA = Ini_Main.FROM_DTA;


        protected const string DATE = Ini_Main.DATE;
        protected const string UNPOSTED = Ini_Main.UNPOSTED;
        protected const string ACCOUNT = Ini_Main.ACCOUNT;
        protected const string POSTED_BY = Ini_Main.POSTED_BY;

        protected const string DATE_FORMAT = Ini_Main.DATE_FORMAT;
        #endregion


        readonly protected Ini_File ini;

        readonly protected IReadWrite<Ini_Field, string> m_proxy;

        readonly Action m_on_dispose;
        readonly bool do_dispose;


        protected bool False(Ini_Field field) {
            return m_proxy[field] == NO;
        }

        public static Set<string> Set(string str) {
            if (str.IsNullOrEmpty())
                return new Set<string>();


            var split = str.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var ret = new Set<string>(split.Select(_s => _s.strdef().Trim()));
            return ret;
        }
        protected Set<string> Set(Ini_Field field) {
            var str = String(field).strdef().Trim();
            return Set(str);
        }

        protected bool True(Ini_Field field) {
            return m_proxy[field] == YES;
        }

        protected DateTime Date(Ini_Field field) {

            var str = m_proxy[field];
            var ret = DateTime.ParseExact(str, DATE_FORMAT, Thread.CurrentThread.CurrentCulture.DateTimeFormat);

            return ret;
        }

        protected virtual int Safe_Int(Ini_Field field) {

            var str = m_proxy[field];

            if (str == "")
                str = "0";

            return int.Parse(str);

        }

        protected int Int(Ini_Field field) {

            var str = m_proxy[field];
            var ret = int.Parse(str);

            return ret;

        }

        protected string String(Ini_Field field) {

            var str = m_proxy[field];
            var ret = str.strdef();

            return ret;

        }

        protected string Font(Ini_Field field) {

            var str = m_proxy[field];

            var dict = new Dictionary<string, string> {
                    {Ini_Main.COURIER, "Courier New"},
                    {Ini_Main.LUCIDA, "Lucida Console"},
                };



            var ret = dict[str];

            return ret;

        }

    }

}
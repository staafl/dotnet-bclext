using System;
using Common;
using Fairweather.Service;

namespace DTA
{
    public class Write_Able_Helper : Ini_Helper
    {


        public Write_Able_Helper(Ini_File ini, Company_Number number)
            : base(ini) {

            writer = ini.Company_Proxy(number);
            act_commit = () => ini.Write_Data();

        }

        public Write_Able_Helper(Ini_File ini)
            : base(ini) {

            writer = ini as IReadWrite<Ini_Field, string>;
            act_commit = () => ini.Write_Data();

        }


        bool Supports_Commit {
            get {
                return act_commit != null;
            }
        }

        Action act_commit;

        void Commit() {

            Supports_Commit.tiff();

            act_commit();

        }

        readonly IReadWrite<Ini_Field, string> writer;

        protected IReadWrite<Ini_Field, string> Writer {
            get { return writer; }
        }


        protected void Set(Ini_Field field, bool value) {

            Set(field, From_Bool(value));


        }
        protected void Set(Ini_Field field, object value) {

            Set(field, value.ToString());

        }

        protected void Set(Ini_Field field, string value) {

            writer[field] = value;

        }


        string From_Bool(bool value) {

            return value ? YES : NO;

        }


    }
}
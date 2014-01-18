using System;
using System.Diagnostics;
using Common;
using Common.Queries;
using Common.Sage;
using Fairweather.Service;


namespace Versioning
{

    public abstract class Sage_Object
    {
        protected readonly int m_version;

        public int Version {
            get { return m_version; }
        }

        //static readonly Set<int>
        //good_versions = new Set<int> { 11, 12, 13, 14, 15, 16 };

        public Sage_Object(int version) {

            // good_versions[version].tiff("bad version {0}".spf(version));
            // validation?
            
            m_version = version;

        }

    }

    [DebuggerStepThrough]
    public abstract class Sage_Container : Sage_Object, IReadWrite<string, object>
, IEntity
    {
        public Sage_Container(int version) : base(version) { }

        [Obsolete("")]
        public Argument Instantiate(Argument arg) {

            if (arg.Type != Argument_Type.Entity)
                return arg;

            string key = arg.Data.ToString();
            object value = this[key];

            Argument_Type arg_type;
            Type type;

            if (value != null) {
                type = value.GetType();
            }
            else {
                arg_type = Argument_Type.Null;
                type = null;
                return new Argument(arg_type, value);

            }


            /*       A two-way dictionary might come in handy here        */

            if (type == typeof(int)) {

                value = Convert.ToInt32(value);
                arg_type = Argument_Type.Integer;

            }
            else if (type == typeof(double) ||
                     type == typeof(float) ||
                     type == typeof(decimal)) {

                value = Convert.ToDecimal(value);
                arg_type = Argument_Type.Decimal;

            }
            else if (type == typeof(string)) {

                arg_type = Argument_Type.String;


            }
            else if (type == typeof(DateTime)) {

                value = Convert.ToDateTime(value).Date;
                arg_type = Argument_Type.Date;

            }
            else if (type == typeof(sbyte) ||
                       type == typeof(short)) {

                Type get_type = this.GetType();
                if (get_type == typeof(Versioning.SalesRecord)) {
                    arg_type = Sage_Fields.Get_Argument_Type(
                          key, Sage_Object_Type.Sales_Record);
                }
                else if (get_type == typeof(Versioning.StockRecord)) {
                    arg_type = Sage_Fields.Get_Argument_Type(
                          key, Sage_Object_Type.Stock_Record);
                }
                else {
                    true.tiff("Searching by fields of records other than SalesRecord is no yet supported.");
                    throw new ApplicationException();

                }
            }
            else
                throw new ApplicationException();


            var ret = new Argument(arg_type, value);

            return ret;

        }

        public virtual bool Deleted {
            get {
                var flag = (short)this[DELETED_FLAG];

                return flag != 0;
            }
        }

        public string Address {
            get {
                var nl = Environment.NewLine;
                return this["ADDRESS_1"].ToString() + nl +
                        this["ADDRESS_2"].ToString() + nl +
                        this["ADDRESS_3"].ToString() + nl +
                        this["ADDRESS_4"].ToString() + nl +
                        this["ADDRESS_5"].ToString();
            }
        }

        public Fields Fields { get { return m_fields; } }

        public object this[string index] {
            get {
                return m_fields[index];
            }
            set {
                m_fields[index] = value;
            }
        }

        public virtual string
        Index_String { get { return ACCOUNT_REF; } }

        public virtual string
        Key {
            get {
                return this[Index_String].ToString();
            }
            set {
                this[Index_String] = value;
            }
        }

        bool IContains<string>.Contains(string key) {

            return (m_fields as IContains<string>).Contains(key);

        }


        // ****************************


        protected const string INVOICE_NUMBER = "INVOICE_NUMBER";
        protected const string RECORD_NUMBER = "RECORD_NUMBER";
        const string DELETED_FLAG = "DELETED_FLAG";
        const string ACCOUNT_REF = "ACCOUNT_REF";
        const string cst_invalid_index = "INVALID INDEX";
        protected const string STR_INVOICE_TYPE_CODE = "INVOICE_TYPE_CODE";

        protected Fields m_fields;




    }
}

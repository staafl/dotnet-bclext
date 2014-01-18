using System.Collections.Generic;
using Fairweather.Service;


namespace Common
{
    static public class Records_Access
    {


        static readonly Dictionary<Record_Type, Records_Cursor>
        m_cursors = new Dictionary<Record_Type, Records_Cursor>();

        static public bool
        Cursor_Assigned(Record_Type type) {
            return m_cursors.ContainsKey(type);
        }


        public static Records_Cursor
        Prepare_Departments_Cursor() {

            string[] depts = Data.SDR.Get_Departments();

            return Prepare_Departments_Cursor(depts);
        }

        public static Records_Cursor
        Prepare_Departments_Cursor(string[] depts) {

            var slist = new SortedList<string, string>();

            foreach (var pair in depts.Ixed())
                slist.Add(pair.First.ToString("000"), pair.Second);

            return Records_Access.Set_Or_Load(Record_Type.Department, slist);

        }

        static public void
        Set_Cursor(Record_Type type) {
            var sdr = Data.SDR;
            var cursor = sdr.Get_Accounts_Cursor(type);

            Set_Cursor(type, cursor);
        }

        static public bool
        Set_Cursor(Record_Type type, Records_Cursor cursor) {

            if (m_cursors.ContainsKey(type))
                return false;

            m_cursors[type] = cursor;
            return true;
        }

        public static Records_Cursor
        Set_Or_Load(Record_Type rt, IDictionary<string, string> idict) {

            Records_Cursor cursor;

            if (Records_Access.Cursor_Assigned(rt)) {

                cursor = Records_Access.Get_Cursor(rt);
                ((Sorted_List_Cursor)cursor).Load_Data(idict);

            }
            else {
                cursor = new Sorted_List_Cursor(idict);
                Records_Access.Set_Cursor(rt, cursor);
            }

            return cursor;
        }


        static public Records_Cursor
        Get_Cursor(Record_Type type) {

            return m_cursors[type];

        }


        public static void
        Set_Cursors(Sage_Logic sdr, params Record_Type[] types) {


            foreach (var type in types) {

                var cursor = sdr.Get_Accounts_Cursor(type);

                Set_Cursor(type, cursor);

            }

        }

        // Sugar
        public static Records_Cursor CustomerAccounts {
            get {
                return Get_Cursor(Record_Type.Sales);
            }
            set {
                Set_Cursor(Record_Type.Sales, value);
            }
        }

        public static Records_Cursor SupplierAccounts {
            get {
                return Get_Cursor(Record_Type.Purchase);
            }
            set {
                Set_Cursor(Record_Type.Purchase, value);
            }
        }

        public static Records_Cursor BankAccounts {
            get {
                return Get_Cursor(Record_Type.Bank);
            }
            set {
                Set_Cursor(Record_Type.Bank, value);
            }
        }

        public static Records_Cursor ProductRecords {
            get {
                return Get_Cursor(Record_Type.Stock);
            }
            set {
                Set_Cursor(Record_Type.Stock, value);
            }
        }

        public static Records_Cursor ExpenseAccounts {
            get {
                return Get_Cursor(Record_Type.Expense);
            }
            set {
                Set_Cursor(Record_Type.Expense, value);
            }
        }

        //public static RecordsCursor ### {
        //    get {
        //        return GetCursor(RecordsType.###);
        //    }
        //    set {
        //        SetCursor(RecordsType.###, value);
        //    }
        //}
    }
}

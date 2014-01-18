using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Common;
using Common.Queries;
using Common.Sage;
using Fairweather.Service;
using Fairweather.Service.Syntax;

namespace Common.Controls
{

    partial class Search_DGV 
    {
        string Get_Field(int row) {

            var value = this[COL_FIELD, row].Value.StringOrDefault();

            var ret = m_sage_fields.FirstOrDefault(triple => triple.Third == value)
                                   .Second;

            return ret;

        }

        Argument_Type Get_Argument_Type(int row) {

            var field = Get_Field(row);

            //if (field == "ACCOUNT_ON_HOLD" ||
            //    field == "TERMS_AGREED_FLAG")
            //    return Argument_Type.Bool;

            var ret = Sage_Fields.Get_Argument_Type(field, Record_Type);

            return ret;
        }

        public Operation Get_Operation(int row) {

            string condition = this[COL_CONDITION, row].Value.StringOrDefault();


            foreach (var kvp in rd_Operations) {

                if (kvp.Key == condition)
                    return kvp.Value;

            }

            true.tift();
            throw new ApplicationException();
        }

        public int Get_Precedence(int row) {

            return Convert.ToInt32(this[COL_PRECEDENCE, row].Value);

        }

        public Clause Get_Clause(int row) {

            var arg1 = new Argument(Argument_Type.Entity, Get_Field(row));
            var arg2 = new Argument(Get_Argument_Type(row), Get_Value(row));
            var type = Get_Operation(row);

            var ret = Query.Make_Clause(type, arg1, arg2);

            return ret;

        }

        public object Get_Value(int row) {

            var obj = this[COL_VALUE, row].Value;

            if (obj == null)
                return null;

            var arg = Get_Argument_Type(row);

            if (arg == Argument_Type.Date)
                return Convert.ToDateTime(obj);

            if (arg == Argument_Type.Decimal)
                return Convert.ToDecimal(obj);

            if (arg == Argument_Type.Integer)
                return Convert.ToInt32(obj);

            if (arg == Argument_Type.String)
                return obj.ToString();

            if (arg == Argument_Type.Bool) {

                var str = obj.ToString();

                bool is_no = (str == NO);
                bool is_yes = (str == YES);

                (is_no || is_yes).tiff("Bad boolean value - row" + row.ToString());
                var ret = is_yes;
                return ret;

            }

            true.tift();
            throw new ApplicationException();
        }

        public Operation Get_Join(int row) {

            var value = this[COL_JOIN, row].Value;

            if (value.ToString() == AND)
                return Operation.And;
            if (value.ToString() == OR)
                return Operation.Or;

            true.tift();
            throw new ApplicationException();

        }

        public bool Get_Tokens(out IToken[] tokens) {

            tokens = null;
            int cnt = this.RowCount;
            var ret = new List<IToken>(cnt);

            for (int ii = 0; ii < cnt; ++ii) {

                int with_value = 0;
                bool last = (ii == cnt - 1);

                foreach (DataGridViewCell cell in this.Rows[ii].Cells) {

                    if (!cell.Value.IsNullOrEmpty()) {
                        ++with_value;
                    }

                }

                if (with_value != this.ColumnCount) {

                    if (last && with_value == 0)
                        break;

                    return false;

                }

                var join = this[COL_JOIN, ii].Value;

                if (ii != 0) {
                    var join_token = new Binary_Token(Get_Precedence(ii), Get_Join(ii));
                    ret.Add(join_token);
                }

                var clause_token = new Argument_Token(Get_Clause(ii));
                ret.Add(clause_token);

            }
            tokens = ret.ToArray();
            return true;

        }

        static readonly Dictionary<Argument_Type, int> rd_arg_types = new Dictionary<Argument_Type, int>()
        {   
            {Argument_Type.Bool, CBX_BOOL},
            {Argument_Type.Date, CBX_DATE},
            {Argument_Type.Decimal, CBX_DECIMAL},
            {Argument_Type.Integer, CBX_INT32},
            {Argument_Type.String, CBX_STRING},
        };

        static readonly Dictionary<int, string[]> conditions = new Dictionary<int, string[]>()
        {
            {CBX_STRING, new string[]{EQUAL, NOT_EQUAL, 
                                      GREATER_THAN, LESS_THAN, 
                                      GREATER_OR_EQ, LESS_OR_EQ,
                                      /*BETWEEN,*/ CONTAINS}},

            {CBX_DATE,  new string[]{EQUAL, NOT_EQUAL, 
                                     GREATER_THAN, LESS_THAN, 
                                     GREATER_OR_EQ, LESS_OR_EQ,
                                     /*BETWEEN*/}},

            {CBX_DECIMAL, new string[]{EQUAL, NOT_EQUAL, 
                                     GREATER_THAN, LESS_THAN, 
                                     GREATER_OR_EQ, LESS_OR_EQ,
                                     /*BETWEEN*/}},

            {CBX_INT32, new string[]{EQUAL, NOT_EQUAL, 
                                     GREATER_THAN, LESS_THAN, 
                                     GREATER_OR_EQ, LESS_OR_EQ,
                                     /*BETWEEN*/}},

            {CBX_BOOL, new string[]{EQUAL, NOT_EQUAL}},
        };

        public bool Case_Insensitive_Contains {
            get {
                var operation = rd_Operations[CONTAINS];
                return operation == Operation.Contains_Caseless;
            }
            set {
                var operation = value ? Operation.Contains_Caseless : Operation.Contains;
                rd_Operations[CONTAINS] = operation;
            }
        }

        static readonly Dictionary<string, Operation> rd_Operations
        = new Dictionary<string, Operation>()
        {
            {EQUAL, Operation.Equal_To},
            {NOT_EQUAL, Operation.Not_Equal_To},
            {GREATER_OR_EQ, Operation.Greater_Or_Equal},
            {GREATER_THAN, Operation.Greater_Than},
            {LESS_OR_EQ, Operation.Less_Or_Equal},
            {LESS_THAN, Operation.Less_Than},
            {CONTAINS, Operation.Contains},
            {BETWEEN, Operation.Between},
        };

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Common.Queries;
using Fairweather.Service;

namespace Common.Sage
{

    public static partial class Sage_Fields
    {
        /*       Interface        */

        public static Dict_ro<string, Sage_Field>
        Fields(Record_Type type) {
            if (type == Record_Type.Audit_Trail) {
                var d1 = HeaderData.Fields;
                var d2 = SplitData.Fields;

                var ret = new Dictionary<string, Sage_Field>(d1.Count + d2.Count);

                ret.Fill(d1, true);
                ret.Fill(d2, true);

                return new Dict_ro<string, Sage_Field>(ret);
            }

            // c4bbfbd3-08af-4700-a24b-e06c1c709033

            if (type == Record_Type.Document || 
                type == Record_Type.Invoice_Or_Credit) {

                var d1 = InvoiceRecord.Fields;
                var d2 = InvoiceItem.Fields;

                var ret = new Dictionary<string, Sage_Field>(d1.Count + d2.Count);

                ret.Fill(d1, true);
                ret.Fill(d2, true);

                return new Dict_ro<string, Sage_Field>(ret);
            }

            return type_to_fields[(Sage_Object_Type)type].Value;
        }

        public static IEnumerable<KeyValuePair<string, Sage_Field>>
        Fields(params Record_Type[] types) {

            foreach (var type in types) {
                foreach (var kvp in Fields((Sage_Object_Type)type))
                    yield return kvp;
            }


        }

        static public IEnumerable<KeyValuePair<string, Sage_Field>>
        Fields(params Sage_Object_Type[] types) {
            foreach (var type in types) {
                foreach (var kvp in type_to_fields[type].Value)
                    yield return kvp;
            }
        }

        static public Sage_Field
        Get(Sage_Object_Type type, string name) {
            var dict = type_to_fields[type].Value;
            var ret = dict[name];
            return ret;
        }

        public static Argument_Type
        Get_Argument_Type(string field, Sage_Object_Type type) {

            var sage_field = type_to_fields[type].Value[field];

            var field_type = sage_field.Type;

            var ret = dict_arg_types[field_type];

            return ret;

        }

        public static Argument_Type
        Get_Argument_Type(string field, Record_Type type) {
            return Get_Argument_Type(field, (Sage_Object_Type)type);
        }

        // ****************************


        // Argument types

        static readonly Twoway<Sage_Object_Type, Lazy<Dict_ro<string, Sage_Field>>>
        type_to_fields = new Twoway<Sage_Object_Type, Lazy<Dict_ro<string, Sage_Field>>>
        {
            {Sage_Object_Type.Invoice_Record, Lazy.Make(() => InvoiceRecord.Fields)},
            {Sage_Object_Type.Invoice_Item, Lazy.Make(() => InvoiceItem.Fields)},
            {Sage_Object_Type.Sales_Record, Lazy.Make(() => SalesRecord.Fields)},
            {Sage_Object_Type.Supplier_Record, Lazy.Make(() => SalesRecord.Fields)}, /* temp */
            {Sage_Object_Type.Stock_Record, Lazy.Make(() => StockRecord.Fields)},
            {Sage_Object_Type.Header_Data, Lazy.Make(() => HeaderData.Fields)},
            {Sage_Object_Type.Split_Data, Lazy.Make(() => SplitData.Fields)},
            {Sage_Object_Type.Stock_Tran, Lazy.Make(() => StockTran.Fields)},
            {Sage_Object_Type.Expense_Record, Lazy.Make(() => NominalRecord.Fields)},
            {Sage_Object_Type.Bank_Record, Lazy.Make(() => BankRecord.Fields)},

        };

        static readonly Dictionary<TypeCode, Argument_Type>
        dict_arg_types = new Dictionary<TypeCode, Argument_Type>
        {
            #region data - 7 lines
		
            {TypeCode.Boolean, Argument_Type.Bool},

            {TypeCode.Double, Argument_Type.Decimal},
            {TypeCode.DateTime, Argument_Type.Date},

            {TypeCode.SByte, Argument_Type.Integer},
            {TypeCode.Byte, Argument_Type.Integer},
            {TypeCode.Int16, Argument_Type.Integer},
            {TypeCode.Int32, Argument_Type.Integer},

            {TypeCode.String, Argument_Type.String}, 

	        #endregion
        };




        // Helper methods for the contained classes

        static Dict_ro<string, Sage_Field>
            Make(params Sage_Field[] fields) {

            var pairs = fields.Select(f => KVP.Make(f.Name, f));

            return Dict_ro.Make(pairs);

        }

        static Sage_Field Int(string name, string description, int length) {
            return new Sage_Field(name, description, length, TypeCode.Int32);
        }
        static Sage_Field String(string name, string description, int length) {
            return new Sage_Field(name, description, length, TypeCode.String);
        }
        static Sage_Field SByte(string name, string description, int length) {
            return new Sage_Field(name, description, length, TypeCode.SByte);
        }
        static Sage_Field Short(string name, string description, int length) {
            return new Sage_Field(name, description, length, TypeCode.Int16);
        }
        static Sage_Field Double(string name, string description, int length) {
            return new Sage_Field(name, description, length, TypeCode.Double);
        }
        static Sage_Field Date(string name, string description, int length) {
            return new Sage_Field(name, description, length, TypeCode.DateTime);
        }
        static Sage_Field Bool(string name, string description, int length) {
            return new Sage_Field(name, description, length, TypeCode.Boolean);
        }

    }

}
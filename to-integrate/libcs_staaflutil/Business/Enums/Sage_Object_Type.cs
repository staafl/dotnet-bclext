namespace Common
{
    using System;
    public enum Sage_Object_Type
    {
        // {[^,]+},
        // \1 = Record_Type.\1,

        Undefined = 0,

        Sales_Record,
        Supplier_Record,
        Bank_Record,
        Expense_Record,
        Stock_Record,
        Department_Record, /* there is no such thing as 'Department Record', not really */
        Invoice_Record,

        Invoice_Item,
        Header_Data,
        Split_Data,
        Stock_Tran,
    }
}

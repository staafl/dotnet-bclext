namespace Common
{
    using System;
    public enum Record_Type // also used for SIT modules
    {
        Undefined = 0x0,

        Sales = Sage_Object_Type.Sales_Record,
        Purchase = Sage_Object_Type.Supplier_Record,
        Bank = Sage_Object_Type.Bank_Record,
        Expense = Sage_Object_Type.Expense_Record,
        Stock = Sage_Object_Type.Stock_Record,
        Department = Sage_Object_Type.Department_Record,
        Invoice = Sage_Object_Type.Invoice_Record,

        Header_Data = Sage_Object_Type.Header_Data,
        Split_Data = Sage_Object_Type.Split_Data,

        /* fictitious */
        TT,
        Tax_Code,
        Project,
        Cost_Code,
        Audit_Trail,
        Document, // what the blazes is the difference b/w document and invoice here??
        Sales_Order,
        Purchase_Order,
        Invoice_Or_Credit,
        Stock_Tran = Sage_Object_Type.Stock_Tran,
        Invoice_Item = Sage_Object_Type.Invoice_Item,
    }
}

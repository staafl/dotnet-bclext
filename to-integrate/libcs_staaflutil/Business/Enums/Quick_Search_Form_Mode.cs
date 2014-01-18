namespace Common
{
    public enum Quick_Search_Form_Mode
    {

        cst_new_edit_view_mode = 0x010000,
        cst_dynamic_mode = 0x001000,

        Banks = 1,

        Customers = 2 | cst_dynamic_mode,
        Suppliers = 3 | cst_dynamic_mode,

        Products = 4,
        Expenses = 5,

        Customers_new = Customers | cst_new_edit_view_mode,
        Suppliers_new = Suppliers | cst_new_edit_view_mode,
        Products_view = Products | cst_new_edit_view_mode,

        Departments,
        TTs,
        Tax_Codes,
        Projects,
        Cost_Codes,
    }
}

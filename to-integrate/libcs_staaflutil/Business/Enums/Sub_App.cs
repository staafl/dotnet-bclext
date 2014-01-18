namespace Common
{
    using System;
    /// <summary>
    /// Represents a particular mode in which an executable may
    /// be ran (i.e. SupplierPayments, CustomerReceipts, Products)
    /// Some values correspond to program modules.
    /// </summary>
    public enum Sub_App : int
    {
        Unknown = 0x0,

        Ses_Cfg = 0x1 | App.Ses_Cfg,

        Excel_Helper = 0x1 | App.Excel_Helper,

        Sage_To_Excel = 0x1 | App.Sage_To_Excel,

        Dashboard = 0x1 | App.Entry_Screens,

        Entry_Customers = 0x2 | App.Entry_Screens,

        Entry_Suppliers = 0x3 | App.Entry_Screens,

        /// <summary> Point of sales screen </summary>
        Products = 0x4 | App.Entry_Screens,

        Transactions_Entry = 0x5 | App.Entry_Screens,

        Documents_Transfer = 0x6 | App.Entry_Screens,

        Startup = 0x7 | App.Entry_Screens,

        Sit_Cfg = 0x1 | App.Sit_Cfg,

        Sit_Exe = 0x1 | App.Sit_Exe,

        Sit_Gui = 0x1 | App.Sit_Gui,



    }
}

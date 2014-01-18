namespace Common
{
    using System;
    /// <summary>
    /// Represents a particular executable (.exe)
    /// </summary>
    public enum App : int
    {
        Unknown = 0x0,

        Entry_Screens = 0x1000 | Enum_Service.SES_suite,

        Ses_Cfg = 0x2000 | Enum_Service.SES_suite,

        Doc_Trans = 0x4000 | Enum_Service.SES_suite,


        // ****************************

        Sit_Exe = 0x4000 | Enum_Service.SIT_suite,

        Sit_Cfg = 0x10000 | Enum_Service.SIT_suite,

        Sit_Gui = 0x20000 | Enum_Service.SIT_suite,


        Excel_Helper = 0x8000 | Enum_Service.SIT_suite,

        Sage_To_Excel = 0x40000 | Enum_Service.SIT_suite,

    }
}

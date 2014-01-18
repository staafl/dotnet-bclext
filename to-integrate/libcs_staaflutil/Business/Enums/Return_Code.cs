namespace Common
{
    using System;
    public enum Return_Code
    {
        All_OK = 0x0,
        Bad_Command_Line = 0x1,
        Multiple_Instances = 0x2,
        Not_Registered = 0x3,
        Files_Missing = 0x4,
        Registration_Failed = 0x5,
        User_Cancelled = 0x6,
        Missing_Nominal_Codes = 0x7,
        Sage_Connection_Error = 0x8,
        Invalid_Company = 0x9,

        Other_Error = 0x10000,
    }
}

namespace Common
{
    using System;
    public enum Command_Line_Status
    {
        Unknown = 0,
        Command_Line_Is_Empty = 0x1 & ~Enum_Service.Command_Line_Status_Valid,
        Module_Not_Licensed = 0x2 & ~Enum_Service.Command_Line_Status_Valid,
        Invalid_Module = 0x4 & ~Enum_Service.Command_Line_Status_Valid,
        Missing_Switch_Parameter = 0x8 & ~Enum_Service.Command_Line_Status_Valid,
        Invalid_Switch = 0x10 & ~Enum_Service.Command_Line_Status_Valid,
        Wrong_Number_Of_Arguments = 0x20 & ~Enum_Service.Command_Line_Status_Valid,
        Invalid_Company_String = 0x40 & ~Enum_Service.Command_Line_Status_Valid,
        Invalid_Company_Number = 0x100 & ~Enum_Service.Command_Line_Status_Valid,
        //Company_Needed = 0x80 & ~Enum_Service.Command_Line_Status_Valid,

        OK_No_Company_Username_Password = Enum_Service.Command_Line_Status_Valid,
        OK_Has_Company_No_Username_Password = Enum_Service.Command_Line_Status_Has_Company,
        OK_Has_Company_Username_Password = Enum_Service.Command_Line_Status_Has_Company_Username_Password,


    }
}

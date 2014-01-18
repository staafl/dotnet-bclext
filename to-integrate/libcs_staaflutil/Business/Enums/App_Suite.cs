namespace Common
{
    using System;
    /// <summary>
    /// Represents a group of application with shared properties,
    /// styles and behavior
    /// </summary>
    public enum App_Suite
    {
        Unknown = 0x0,

        Entry_Screens = Enum_Service.SES_suite,

        Interface_Tools = Enum_Service.SIT_suite,
    }
}

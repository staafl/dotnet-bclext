

namespace Activation
{
    public enum SDO_Activation_Status
    {
        OK = 0,
        Not_Activated = 1,
        Sage_Not_Registered_User_Refuses = 2,
        Unknown_Error = 3,
        COM_Exception = 4,
    }

    public enum User_Credentials_Status
    {
        Valid_User = 0,
        Incorrect_Credentials = 1,
        Unknown_Error = 2
    }

    public enum SDO_Installation_Status
    {
        All_OK,
        Try_Again,
        User_Clicked_NO,

        Regsvr32_Error,
        Wrong_Version,
        Firewall,
        Access_Error,

        Unknown_Error,

    }
}

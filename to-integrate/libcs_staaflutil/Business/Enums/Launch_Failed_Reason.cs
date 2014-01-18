namespace Common
{
    public enum Launch_Failed_Reason
    {
        Unknown,
        User_Cancelled = 0x1,
#warning
        Missing_Codes,
        Sage_Connection_Error,
        Invalid_Company,
        Forbidden_Company,
        Unknown_Module,
    }
}

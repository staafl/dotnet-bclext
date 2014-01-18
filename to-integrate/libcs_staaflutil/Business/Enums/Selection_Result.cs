namespace Common
{

    public enum Selection_Result
    {
        Account_Does_Not_Exist = 0x1 | cst_not_ok,
        Empty_Account = 0x2 | cst_not_ok,
        Invalid_Account = 0x4 | cst_not_ok,
        Unable_To_Verify = Invalid_Account,

        Already_Selected = 0x1 | cst_ok,
        Existing_Account = 0x2 | cst_ok,
        New_Account = 0x4 | cst_ok,
        Empty_Account_OK = 0x8 | cst_ok,
        cst_ok = 0x10000,
        cst_not_ok = 0x1000,
    }
}

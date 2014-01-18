namespace Common
{
    using System;
    public enum Message_Type
    {
        Error = 0x1,
        System_Error,
        Warning,
        Warning_Yes_No,
        Attention_OK_Cancel,
        Success,
        Question,
        Info,
        Request,
        Warning_Retry_Cancel,
        Abort_Retry_Ignore,
    }
}

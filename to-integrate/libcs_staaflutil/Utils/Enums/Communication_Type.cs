using System;

namespace Fairweather.Service
{
    public enum Communication_Type
    {
        Normal,
        Question,
        Error,
        ErrorQuestion,
        Warning,
        WarningQuestion,
        AbortRetryIgnore,

        Info,
        Success,
        Failure,
        Confirm,
        YesNo,
    }
}

using System;

namespace Common
{
    public interface IBreakdown_Data
    {
        string Account_Ref { get; }

        DateTime Date { get; }

        int Number { get; }

        Decimal Payment_Amount(Payment_Type type);
    }
}

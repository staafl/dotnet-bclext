using System;
using System.Windows.Forms;
using System.Diagnostics;
using Standardization;
using Fairweather.Service;

namespace Common.Controls
{
    public interface IAmountControl // <TCtrl> where TCtrl : IAmountControl<TCtrl>
    {
        int Decimal_Places { get; set; }
        decimal Value { get; set; }
        string Default_Text { get; }
        string Default_Format { get; }
        Control Control { get; }
    }
}

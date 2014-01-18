namespace Common
{
    using System;
    using System.Windows.Forms;
    public interface IActiveControlEnabledForm
    {
        Control ActiveControl { get; }

        bool Focus();
        IntPtr Handle { get; }
        event EventHandler<EventArgs> ActiveControlChanged;
    }
}

namespace Common
{
    using System;
    public interface IControlHost
    {
        event EventHandler<EventArgs> MouseClickedOnScreen;
    }
}

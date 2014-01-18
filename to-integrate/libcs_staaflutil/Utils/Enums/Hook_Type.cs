using System;

namespace Fairweather.Service
{
    public enum Hook_Type
    {
        MouseDown,
        KillFocus,
        SetFocus,

        // http://stackoverflow.com/questions/746188/c-how-to-capture-global-mouse-hid-events
        MouseWheel,
        All
    }
}

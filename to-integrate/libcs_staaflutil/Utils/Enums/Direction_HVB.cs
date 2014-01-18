using System;

namespace Fairweather.Service
{
    [Flags]
    /// Horizontal, Vertical, Both
    public enum Direction_HVB
    {
        Vertical = 0x1,
        Horizontal = 0x2,
        Both = 0x3,
    }
}

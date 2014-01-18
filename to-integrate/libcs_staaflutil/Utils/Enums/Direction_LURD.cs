using System;

namespace Fairweather.Service
{
    [Flags]
    /// Left, Up, Right, Down
    public enum Direction_LURD
    {
        Left = 0x1,
        Up = 0x2,
        Right = 0x4,
        Down = 0x8,
    }
}

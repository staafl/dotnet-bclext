using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Fairweather.Service
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MENUITEMINFO
    {
        public uint cbSize;
        public uint fMask;
        public uint fType;
        public uint fState;
        public uint wID;
        public IntPtr hSubMenu;
        public IntPtr hbmpChecked;
        public IntPtr hbmpUnchecked;
        public string dwTypeData;
        public IntPtr dwItemData;
        public uint cch;
        public IntPtr hbmpItem;
    }
}

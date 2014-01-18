namespace Fairweather.Service
{
    using System;
    public static class Native_Const
    {
        public const int UNIVERSAL_NAME_INFO_LEVEL = 0x00000001;
        public const int REMOTE_NAME_INFO_LEVEL = 0x00000002;
        public const int ERROR_MORE_DATA = 234;
        public const int ERROR_NOT_CONNECTED = 2250;
        public const int ERROR_NO_NETWORK = 1222;
        public const int NOERROR = 0;

        public const Int32 GWL_STYLE = -16;

        public const Int32 WS_VSCROLL = 0x00200000;
        public const Int32 WS_VISIBLE = 0x10000000;

        public const Int32 WM_USER = 0x400;

        public const Int32 WM_COPYDATA = 0x004A;

        public enum FileProtection : uint      // constants from winnt.h
        {
            ReadOnly = 2,
            ReadWrite = 4
        }

        public enum FileRights : uint          // constants from WinBASE.h
        {
            Read = 4,
            Write = 2,
            ReadWrite = Read | Write
        }

        public const Int32 WH_MOUSE_LL = 0xE;
        public const Int32 WH_MOUSE = 0x7;
        public const Int32 WH_CALLWNDPROC = 4;

        public const int ATTACH_PARENT_PROCESS = -1;




        // \#define  -- > public const Int32 
        //   +{:z}  -->  = \1
        //   $  --> ;
        public const Int32 SIZE_RESTORED = 0;
        public const Int32 SIZE_MINIMIZED = 1;
        public const Int32 SIZE_MAXIMIZED = 2;
        public const Int32 SIZE_MAXSHOW = 3;
        public const Int32 SIZE_MAXHIDE = 4;

        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_RESTORE = 0xF120;
        public const int SC_CLOSE = 0xF060;

        public const uint MF_BYCOMMAND = 0x00000000;
        public const uint MF_BYPOSITION = 0x00000400;
        public const uint MF_ENABLED = 0x00000000;
        public const uint MF_GRAYED = 0x00000001;
        public const uint MF_DISABLED = 0x00000002;

        public const uint MIIM_STATE = 0x00000001;
        public const uint MIIM_ID = 0x00000002;
        public const uint MIIM_SUBMENU = 0x00000004;
        public const uint MIIM_CHECKMARKS = 0x00000008;
        public const uint MIIM_TYPE = 0x00000010;
        public const uint MIIM_DATA = 0x00000020;
        public const uint MIIM_STRING = 0x00000040;

        public const Int32 WM_SIZE = 0x0005;

        public const Int32 WM_SYSCOMMAND = 0x0112;
        public const Int32 WM_CREATE = 0x0001;
        public const Int32 WM_DESTROY = 0x0002;

        public const Int32 WM_KEYDOWN = 0x100;
        public const Int32 WM_KEYUP = 0x0101;
        public const Int32 WM_CHAR = 0x0102;

        public const Int32 WM_LBUTTONDOWN = 0x0201;
        public const Int32 WM_LBUTTONUP = 0x0202;
        public const Int32 WM_LBUTTONDBLCLK = 0x0203;

        public const Int32 WM_RBUTTONDOWN = 0x0204;
        public const Int32 WM_RBUTTONUP = 0x0205;
        public const Int32 WM_RBUTTONDBLCLK = 0x0206;

        public const Int32 WM_MBUTTONDOWN = 0x0207;
        public const Int32 WM_MBUTTONUP = 0x0208;
        public const Int32 WM_MBUTTONDBLCLK = 0x0209;

        public const Int32 WM_MOUSELAST = 0x0209;

        public const Int32 WM_MOUSEMOVE = 0x200;
        public const Int32 WM_MOUSEWHEEL = 0x020A;

        public const Int32 WM_NCHITTEST = 0x84;
        public const Int32 WM_NOTIFY = 0x004e;
        public const Int32 WM_PAINT = 0xF;
        public const Int32 WM_ERASEBKGND = 0x0014;
        public const Int32 WM_PARENTNOTIFY = 0x210;

        public const Int32 WM_SETCURSOR = 0x0020;
        public const Int32 WM_SETREDRAW = 0xB;

        public const Int32 WM_SETFOCUS = 0x0007;
        public const Int32 WM_KILLFOCUS = 0x0008;


        public const Int32 WM_SYSKEYDOWN = 0x0104;
        public const Int32 WM_SYSKEYUP = 0x0105;

        public const Int32 WM_PASTE = 0x0302;


        // http://msdn.microsoft.com/en-us/library/bb787577(VS.85).aspx
        public const Int32 WM_VSCROLL = 0x115;
        public const int WM_HSCROLL = 0x114;

        public static class WM_VSCROLL_low
        {
            public const short SB_BOTTOM = 7,
            SB_ENDSCROLL = 8,
            SB_LINEDOWN = 1,
            SB_LINEUP = 0,
            SB_PAGEDOWN = 3,
            SB_PAGEUP = 2,
            SB_THUMBPOSITION = 4,
            SB_THUMBTRACK = 5,
            SB_TOP = 6;
        }

        public const Int32 VK_CONTROL = 0x11;

        // http://www.vbforums.com/showthread.php?t=459890
        // Part of the OSK control
        public const Int32 WS_EX_NOACTIVATE = 0x08000000;
        public const Int32 WS_EX_TOOLWINDOW = 0x00000080;

        public const Int32 WM_NCACTIVATE = 0x086;
        public const Int32 WM_ACTIVATEAPP = 0x01C;
        public const Int32 WM_ACTIVATE = 0x06;
        public const Int32 WM_MOUSEACTIVATE = 0x21;

        public const Int32 MA_ACTIVATE = 0x1;
        public const Int32 MA_ACTIVATEANDEAT = 0x2;

        public const Int32 MA_NOACTIVATE = 0x3;
        public const Int32 MA_NOACTIVATEANDEAT = 0x4;

        public const Int32 SW_SHOWNOACTIVATE = 0x4;

        public struct Other_Constants
        {
            public const Int32 KEYEVENTF_EXTENDEDKEY = 0x0001;
            public const Int32 KEYEVENTF_KEYUP = 0x0002;
        }

        public static class ShowWindow
        {
            #region
            public const int SW_HIDE = 0;

            public const int SW_SHOWNORMAL = 1;

            public const int SW_NORMAL = 1;

            public const int SW_SHOWMINIMIZED = 2;

            public const int SW_SHOWMAXIMIZED = 3;

            public const int SW_MAXIMIZE = 3;

            public const int SW_SHOWNOACTIVATE = 4;

            public const int SW_SHOW = 5;

            public const int SW_MINIMIZE = 6;

            public const int SW_SHOWMINNOACTIVE = 7;

            public const int SW_SHOWNA = 8;

            public const int SW_RESTORE = 9;

            public const int SW_SHOWDEFAULT = 10;

            public const int SW_FORCEMINIMIZE = 11;

            public const int SW_MAX = 11;

            public const int SW_RESTORE_13 = 13;

            #endregion
        }
    }
}

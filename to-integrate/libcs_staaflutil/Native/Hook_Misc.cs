using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Fairweather.Service
{

    public static partial class Hook
    {
        class HookInfo
        {
            public HookInfo(HookProc proc, Hook_Type type, int handle) {

                procedure = proc;
                this.type = type;
                this.handle = handle;
            }

            readonly HookProc procedure;
            readonly int handle;
            readonly Hook_Type type;

            public Hook_Type Type {
                [DebuggerStepThrough]
                get { return type; }
            }

            public int Handle {
                [DebuggerStepThrough]
                get { return handle; }
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        ///Hooks
        const Int32 WH_MOUSE_LL = Native_Const.WH_MOUSE_LL;
        const Int32 WH_MOUSE = Native_Const.WH_MOUSE;
        const Int32 WH_CALLWNDPROC = Native_Const.WH_CALLWNDPROC;
        //

        ///Messages
        const Int32 WM_LBUTTONDOWN = Native_Const.WM_LBUTTONDOWN;
        const Int32 WM_SETFOCUS = Native_Const.WM_SETFOCUS;
        const Int32 WM_KILLFOCUS = Native_Const.WM_KILLFOCUS;
        //
        const Int32 WM_MOUSEWHEEL = Native_Const.WM_MOUSEWHEEL;
        //


        //http://msdn.microsoft.com/en-us/library/ms644968(VS.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        //http://msdn.microsoft.com/en-us/library/ms644964(VS.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        public class CWPStruct
        {
            public IntPtr lParam;
            public IntPtr wParam;
            public uint msg;
            public IntPtr hwnd;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(int dHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

    }
}
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Fairweather.Service
{

    public static class Native_Methods
    {



        // http://msdn.microsoft.com/en-us/library/aa288469(VS.71).aspx
        //[SuppressUnmanagedCodeSecurityAttribute]
        [DllImport("mpr.dll")]
        [return: MarshalAs(UnmanagedType.U4)]
        static public extern int WNetGetUniversalName(
            string lpLocalPath,
            [MarshalAs(UnmanagedType.U4)] int dwInfoLevel,
            IntPtr lpBuffer,
            [MarshalAs(UnmanagedType.U4)] ref int lpBufferSize);


        [DllImport("user32.dll")]
        static public extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        static public extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        static public extern IntPtr RemoveMenu(IntPtr hMenu, uint nPosition, uint wFlags);

        [DllImport("user32.dll")]
        static public extern IntPtr DrawMenuBar(IntPtr hWnd);

        [DllImport("user32.dll")]
        static public extern uint GetMenuItemCount(IntPtr hMenu);

        //[DllImport("user32.dll")]
        //static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //[DllImport("user32.dll")]
        //public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("KERNEL32.dll")] //[DllImport("toolhelp.dll")]      
        public static extern int CreateToolhelp32Snapshot(uint flags, uint processid);

        [DllImport("KERNEL32.DLL")] //[DllImport("toolhelp.dll")]      
        public static extern int CloseHandle(int handle);

        [DllImport("KERNEL32.DLL")] //[DllImport("toolhelp.dll")      
        public static extern int Process32Next(int handle, ref ProcessEntry32 pe);


        public static Process FindParentProcess() {

            int SnapShot = CreateToolhelp32Snapshot(0x00000002, 0); //2 = SNAPSHOT of all procs      
            try {
                var pe32 = new ProcessEntry32();
                pe32.dwSize = 296;
                int procid = Process.GetCurrentProcess().Id;

                while (Process32Next(SnapShot, ref pe32) != 0) {
                    string xname = pe32.szExeFile.ToString();
                    if (procid == pe32.th32ProcessID) {
                        int pid = Convert.ToInt32(pe32.th32ParentProcessID);
                        return Process.GetProcessById(pid);

                    }
                }

            }
            //catch (Exception ex) {
            //   throw;
            //}
            finally {
                CloseHandle(SnapShot);
            }
            return null;
        }

        // ****************************

        // http://vb.mvps.org/articles/ap199902.pdf
        public static bool ForceForegroundWindow(IntPtr hWnd) {

            IntPtr ThreadID1;
            IntPtr ThreadID2;
            int nRet;

            if (hWnd == GetForegroundWindow())
                return true;

            //  First need to get the thread responsible for
            //  the foreground window, then the thread running
            //  the passed window.

            int _;
            ThreadID1 = GetWindowThreadProcessId(GetForegroundWindow(), out _);
            ThreadID2 = GetWindowThreadProcessId(hWnd, out _);

            //  By sharing input state, threads share their
            //  concept of the active window.

            if (ThreadID1 == ThreadID2) {
                nRet = SetForegroundWindow(hWnd);
            }
            else {
                try {
                    AttachThreadInput(ThreadID1, ThreadID2, true);
                    nRet = SetForegroundWindow(hWnd);
                }
                finally {
                    AttachThreadInput(ThreadID1, ThreadID2, false);
                }
            }


            //  Restore and repaint
            if (IsIconic(hWnd)) {
                ShowWindow(hWnd, Native_Const.ShowWindow.SW_RESTORE);
            }
            else {
                ShowWindow(hWnd, Native_Const.ShowWindow.SW_SHOW);
            }

            //  SetForegroundWindow return accurately reflects
            //  success.
            bool ret = nRet == 0;
            return ret;
        }

        // Here we're using enums because they're safer than constants

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern IntPtr
        CreateFileMapping(IntPtr hFile,
                                                int lpAttributes,
                                                Native_Const.FileProtection flProtect,
                                                uint dwMaximumSizeHigh,
                                                uint dwMaximumSizeLow,
                                                string lpName);

        // http://msdn.microsoft.com/en-us/library/ms633584(VS.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hwnd, int nIndex);


        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern IntPtr OpenFileMapping(Native_Const.FileRights dwDesiredAccess,
                                              bool bInheritHandle,
                                              string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern IntPtr MapViewOfFile(IntPtr hFileMappingObject,
                                            Native_Const.FileRights dwDesiredAccess,
                                            uint dwFileOffsetHigh,
                                            uint dwFileOffsetLow,
                                            uint dwNumberOfBytesToMap);
        [DllImport("Kernel32.dll")]
        static public extern bool UnmapViewOfFile(IntPtr map);

        [DllImport("kernel32.dll")]
        static public extern int CloseHandle(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, bool fAttach);
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ShowWindow(IntPtr hWnd, IntPtr nCmdShow);



        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static public extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);



        [DllImport("User32.dll")]
        public static extern IntPtr
        FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr
        GetWindowThreadProcessId(IntPtr hWnd, out int pid);

        [DllImport("User32.dll")]
        public static extern int
        SetClipboardViewer(int hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool
        ChangeClipboardChain(IntPtr hWndRemove,
                        IntPtr hWndNewNext);




        // http://www.csharp411.com/console-output-from-winforms-application/

        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr handle);



        /// http://blogs.msdn.com/adam_nathan/archive/2003/04/25/56643.aspx
        /// "GetLastError and managed code"
        public static string GetLastWin32Error() {

            var ex = new Win32Exception();
            string ret = ex.Message;

            return ret;

        }

        public static int GetLastWin32ErrorCode() {

            var ex = new Win32Exception();
            int ret = ex.ErrorCode;

            return ret;
        }

        [DllImport("user32.dll")]
        public static extern int FindWindow(string szClass, string szTitle);

        [DllImport("user32.dll")]
        public static extern int ShowWindow(int Handle, int showState);

        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 uMsg,
                                                UInt32 wParam, UInt32 lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, UInt32 uMsg,
                                                UInt32 wParam, UInt32 lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool DestroyCaret();

        [DllImport("user32.dll")]
        public static extern bool HideCaret(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("User32.dll")]
        public static extern short GetKeyState(int vKey);


        [DllImport("user32.dll")]
        public static extern bool keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);

        //[DllImport("coredll.dll", EntryPoint = "PostKeybdMessage", SetLastError = true)]
        //internal static extern bool PostKeybdMessage(IntPtr hwnd, uint vKey,
        //                                             KeyStateFlags flags, uint cCharacters,
        //                                             KeyStateFlags[] pShiftStateBuffer,
        //                                             uint[] pCharacterBuffer);

        // http://msdn.microsoft.com/en-us/library/ms633539(VS.85).aspx
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);


        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // http://www.bobpowell.net/singleinstance.htm
        // http://msdn.microsoft.com/en-us/library/ms633549(VS.85).aspx
        [DllImport("User32.dll")]
        public static extern int ShowWindowAsync(IntPtr hWnd, int swCommand);


        [DllImport("user32.dll")]
        public static extern byte VkKeyScan(char ch);
    }
}

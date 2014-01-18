using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Fairweather.Service
{
    //[DebuggerStepThrough]
    public static partial class Hook
    {
        static SortedList<int, HookInfo> _hooks = new SortedList<int, HookInfo>();

        public static int SetHook(Hook_Type type, HookProc action) {

            if (type == Hook_Type.All)
                throw new ArgumentException("HookType.All is not allowed as parameter for SetHook");

            lock (_hooks) {

                int key = _hooks.Count;

                if (key == int.MaxValue)
                    throw new InvalidOperationException("Maximum number of hooks exceeded.");

                var msg = GetMessage(type);

                procs.List_Modify(msg, action);

                if (!hooks[msg]) {

                    hooks[msg] = true;

                    var proc = CreateProcedure(type, key);

                    int param = GetParameter(type);

#pragma warning disable

                    // This is OK for this purpose
                    int handle = SetWindowsHookEx(param, proc, (IntPtr)0, AppDomain.GetCurrentThreadId());

#pragma warning restore

                    if (handle == 0)
                        throw new Win32Exception(Marshal.GetLastWin32Error());

                    var h_info = new HookInfo(proc, type, handle);

                    // TODO: Concurrency
                    _hooks[key] = h_info;
                }

                return key;
            }

        }


        static HookProc
        CreateProcedure(Hook_Type type, int key) {

            HookProc ret;
            int message = GetMessage(type);

            bool tmp = false;

            if (type == Hook_Type.MouseDown) {

                ret = (
                (_nCode, _wParam, _lParam) =>
                {
                    // var MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

                    if (_nCode >= 0 && (int)_wParam == message) {

                        foreach (var act in procs[message])
                            act(_nCode, _wParam, _lParam);

                    }

                    int handle = _hooks[key].Handle;
                    return CallNextHookEx(handle, _nCode, _wParam, _lParam);

                });
            }       // Global hooks
            else if (type == Hook_Type.KillFocus ||
                     type == Hook_Type.SetFocus ||
                     type == Hook_Type.MouseWheel) {

                // KillFocus/SetFocus:
                // wParam_tmp  will contain a handle to the control which
                // has lost or gained focus

                ret = (_nCode, _wParam, _lParam) =>
                {
                    if (tmp)
                        return 0;

                    var _cwp = (CWPStruct)Marshal.PtrToStructure(_lParam, typeof(CWPStruct));
                    int _msg = (int)_cwp.msg;

                    if (_nCode >= 0 && _msg == message) {

                        IntPtr wParam_tmp = _cwp.hwnd;

                        foreach (var act in procs[message])
                            act(_nCode, wParam_tmp, _lParam);

                    }

                    int handle = _hooks[key].Handle;
                    tmp = true;
                    int ret_1 = CallNextHookEx(handle, _nCode, _wParam, _lParam);
                    tmp = false;
                    return ret_1;
                };
            }
            else {
                throw new ArgumentException("Unknown hook type: " + type);
            }

            return ret;

        }

        #region HELPERS

        static Set<int> hooks = new Set<int>();

        static Dictionary<int, List<HookProc>>
        procs = new Dictionary<int, List<HookProc>>();


        static Dictionary<Hook_Type, int>
        dict1 = new Dictionary<Hook_Type, int>
            {
                {Hook_Type.MouseDown, WH_MOUSE},
                {Hook_Type.KillFocus, WH_CALLWNDPROC},
                {Hook_Type.SetFocus, WH_CALLWNDPROC},
                {Hook_Type.MouseWheel, WH_CALLWNDPROC},
            };
        static Dictionary<Hook_Type, int>
        dict2 = new Dictionary<Hook_Type, int>
            {
                {Hook_Type.MouseDown, WM_LBUTTONDOWN},
                {Hook_Type.KillFocus, WM_KILLFOCUS},
                {Hook_Type.SetFocus, WM_SETFOCUS},
                {Hook_Type.MouseWheel, WM_MOUSEWHEEL},
            };

        static int GetParameter(Hook_Type type) {


            int ret;
            dict1.TryGetValue(type, out ret)
                 .tiff<ArgumentException>("Unknown hook type");

            return ret;

        }

        static int GetMessage(Hook_Type type) {

            int ret;
            dict2.TryGetValue(type, out ret)
                 .tiff<ArgumentException>("Unknown hook type");

            return ret;

        }
        #endregion
    }
}

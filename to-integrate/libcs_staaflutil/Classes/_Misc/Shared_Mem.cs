
namespace Fairweather.Service.Classes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct Running_Apps_Id
    {
        public fixed int open_instances[100];
        public fixed int pids[100];
    }

    // Todo: write this in C++
    // Source: C# In a Nutshell 3rd ed.
    public unsafe class SharedMem<T> : IDisposable where T : struct // serializable
    {
        static readonly IntPtr NoFileHandle = new IntPtr(-1);

        IntPtr fileHandle, fileMap;

        public IntPtr Root { get { return fileMap; } }

        public unsafe void* Pointer {
            get {
                return (void*)Root.ToPointer();
            }
        }

        public SharedMem(string name) {

            uint size_in_bytes = (uint)Marshal.SizeOf(typeof(T));

            const Native_Const.FileProtection rwp = Native_Const.FileProtection.ReadWrite;
            const Native_Const.FileRights rwr = Native_Const.FileRights.ReadWrite;

            fileHandle = Native_Methods.CreateFileMapping(
                                NoFileHandle,
                                0,
                                rwp,
                                0,
                                size_in_bytes,
                                name);


            int ALREADY_OPEN = 0;
            if (fileHandle == IntPtr.Zero && (Marshal.GetLastWin32Error() == ALREADY_OPEN))
                fileHandle = Native_Methods.OpenFileMapping(rwr, false, name);


            (fileHandle == IntPtr.Zero).Throw_If_True<Exception>("Open/create error: " + Marshal.GetLastWin32Error());

            // Obtain a read/write map for the entire file
            fileMap = Native_Methods.MapViewOfFile(fileHandle, rwr, 0, 0, 0);

            (fileMap == IntPtr.Zero).Throw_If_True<Exception>("MapViewOfFile error: " + Marshal.GetLastWin32Error());
        }

        ~SharedMem() { Dispose(false); }

        public void Dispose() {

            Dispose(true);

        }

        private void Dispose(bool disposing) {

            if (fileMap != IntPtr.Zero)
                Native_Methods.UnmapViewOfFile(fileMap);

            if (fileHandle != IntPtr.Zero)
                Native_Methods.CloseHandle(fileHandle);

            fileMap = fileHandle = IntPtr.Zero;
        }
    }



}

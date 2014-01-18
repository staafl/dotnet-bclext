using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Fairweather.Service.Native
{
    // http://msdn.microsoft.com/en-us/library/aa288468(VS.71).aspx
    // http://www.devnewsgroups.net/group/microsoft.public.dotnet.framework.windowsforms/topic45164.aspx
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class LOGFONT
    {
        public const int LF_FACESIZE = 32;
        public int lfHeight;
        public int lfWidth;
        public int lfEscapement;
        public int lfOrientation;
        public int lfWeight;
        [MarshalAs(UnmanagedType.U1)]
        public byte lfItalic;
        [MarshalAs(UnmanagedType.U1)]
        public byte lfUnderline;
        [MarshalAs(UnmanagedType.U1)]
        public byte lfStrikeOut;
        public byte lfCharSet;
        public byte lfOutPrecision;
        public byte lfClipPrecision;
        public byte lfQuality;
        public byte lfPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LF_FACESIZE)]
        public string lfFaceName;
    }

    public static class Fonts
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
         static extern IntPtr  CreateFontIndirect   (
            [In, MarshalAs(UnmanagedType.LPStruct)]
            LOGFONT lplf   // characteristics
              );

        [DllImport("gdi32.dll")]
         static extern bool DeleteObject(IntPtr handle);

        public static Font Create_Logical_Font(string name, int height) {

            var lf = new LOGFONT();
            lf.lfHeight = height;
            lf.lfFaceName = name;

            Font ret;
            var handle = CreateFontIndirect(lf);

            try {

                 ret = Font.FromLogFont(lf);

            }
            finally {
                DeleteObject(handle);
            }

            return ret;
        }
    }
}

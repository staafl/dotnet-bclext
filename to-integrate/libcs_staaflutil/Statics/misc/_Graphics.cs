using System;
using System.Drawing;
using System.Drawing.Imaging;

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Fairweather.Service
{
    public static class GR
    {
        // http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/94dc2598-bc40-4170-8dcb-7215223d0fd3/
        public static Bitmap BitmapTo1Bpp(Bitmap img) {
            int w = img.Width;
            int h = img.Height;
            var bmp = new Bitmap(w, h, PixelFormat.Format1bppIndexed);

            var data =
                  bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            for (int yy = 0; yy < h; yy++) {

                var scan = new byte[(w + 7) / 8];

                for (int xx = 0; xx < w; xx++) {

                    Color c = img.GetPixel(xx, yy);
                    if (c.GetBrightness() >= 0.5)
                        scan[xx / 8] |= (byte)(0x80 >> (xx % 8));

                }

                Marshal.Copy(scan, 0, (IntPtr)((int)data.Scan0 + data.Stride * yy), scan.Length);
            }
            return bmp;
        }

        // Converting BMP to monochrome:
        // http://www.wischik.com/lu/programmer/1bpp.html
        // http://www.bobpowell.net/onebit.htm
        // http://www.experts-exchange.com/Programming/Languages/C_Sharp/Q_21843884.html

        // Not tested
        /// <summary>
        /// Draws a bitmap onto the screen.
        /// </summary>
        /// <param name="b">the bitmap to draw on the screen</param>
        /// <param name="x">x screen coordinate</param>
        /// <param name="y">y screen coordinate</param>
        public static void Splash_Image(Bitmap b, int x, int y) {

            IntPtr hbm, sdc, hdc;

            hbm = sdc = hdc = IntPtr.Zero;

            try {
                // (1) Copy the Bitmap into a GDI hbitmap
                hbm = b.GetHbitmap();

                // (2) obtain the GDI equivalent of a "Graphics" for the screen
                sdc = GetDC(IntPtr.Zero);

                // (3) obtain the GDI equivalent of a "Graphics" for the hbitmap
                hdc = CreateCompatibleDC(sdc);

                SelectObject(hdc, hbm);

                // (4) Draw from the hbitmap's "Graphics" onto the screen's "Graphics"
                BitBlt(sdc, x, y, b.Width, b.Height, hdc, 0, 0, SRCCOPY);
            }
            finally {
                // and do boring GDI cleanup:
                DeleteDC(hdc);
                ReleaseDC(IntPtr.Zero, sdc);
                DeleteObject(hbm);
            }

        }

        [DllImport("gdi32.dll")]
        public static extern
        bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern
        IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        public static extern
        IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("user32.dll")]
        public static extern
        int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern
        int DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern
        IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern
        int BitBlt(IntPtr hdcDst, int xDst, int yDst, int w, int h, IntPtr hdcSrc, int xSrc, int ySrc, int rop);

        static int SRCCOPY = 0x00CC0020;

        [DllImport("gdi32.dll")]
        static extern
IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO bmi, uint Usage, out IntPtr bits, IntPtr hSection, uint dwOffset);

        static uint BI_RGB = 0;
        static uint DIB_RGB_COLORS = 0;

        [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public uint biSize;
            public int biWidth, biHeight;
            public short biPlanes, biBitCount;
            public uint biCompression, biSizeImage;
            public int biXPelsPerMeter, biYPelsPerMeter;
            public uint biClrUsed, biClrImportant;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 256)]
            public uint[] cols;
        }

        static uint MAKERGB(int r, int g, int b) {

            return ((uint)(b & 255)) | ((uint)((g & 255) << 8)) | ((uint)((r & 255) << 16));

        }
    }
}

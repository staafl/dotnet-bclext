using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Fairweather.Service;
namespace Fairweather.Service
{



    public class Pos_Notepad_Printer : Pos_Printer_Engine
    {
        static readonly string nl = Environment.NewLine;
        readonly StringBuilder sb;

        public StringBuilder Buffer {
            get { return sb; }
        }

        readonly string m_filename;



#if WINFORMS

        Func<IDisposable> Make_Show_Hide_Form(System.Windows.Forms.Form form) {

            var tmp_form = form;

            if (tmp_form == null)
                return () => On_Dispose.Nothing;

            Action show = () =>
            {
                if (!tmp_form.IsDisposed && !tmp_form.Visible)
                    tmp_form.Show();
            };

            return () =>
            {
                if (tmp_form.IsDisposed)
                    return On_Dispose.Nothing;

                if (Open_File)
                    tmp_form.Hide();

                return new On_Dispose(show);
            };

        }

        public Pos_Notepad_Printer(
System.Windows.Forms.Form form,
 int capacity, string filename) {
            Show_Hide_Form = Make_Show_Hide_Form(form);
#else 
             public Pos_Notepad_Printer(
 int capacity, string filename) {
#endif //       WINFORMS

            sb = new StringBuilder(capacity);
            m_filename = filename;

            Print_Tags = Tag_Levels.All;
            Make_Backup = true;

        }

        readonly Func<IDisposable> Show_Hide_Form = () => On_Dispose.Nothing;


        static string Tag(string name) {
            return Tag(name, "");
        }

        public enum Tag_Levels
        {
            None,
            Images = 0x1,
            Fonts = 0x2,
            Misc = 0x4,
            Barcodes = 0x8,
            All = -1,

        }

        public Tag_Levels Print_Tags {
            get;
            set;
        }


        public bool Make_Backup { get; set; }

        public bool Open_File { get; set; }

        public bool Open_Dir { get; set; }

        static string Tag(string name, string content) {

            if (content.IsNullOrEmpty())
                return "[{0}]".spf(name);
            else
                return "[{0}: {1}]".spf(name, content);
        }

        protected override void Print_Image_Inner(string filename, HAlignment alignment) {

            if (!Print_Tags.Contains(Tag_Levels.Images))
                return;
            Print_Text(Tag("image", filename));

        }

        public override void Print_Barcode(string barcode) {

            if (!Print_Tags.Contains(Tag_Levels.Barcodes))
                return;
            Print_Text(Tag("barcode", barcode));
        }

        public override void New_Page() {

            if (!Print_Tags.Contains(Tag_Levels.Misc))
                return;

            Print_Text(Tag("new page"));

        }

        public override void Open_Cash_Drawer() {

            if (!Print_Tags.Contains(Tag_Levels.Misc))
                return;

            Print_Text(Tag("cash drawer opened"));

        }

        public override bool Initialized { get { return true; } }

        internal override bool Initialize_Inner() {
            return true;
        }

        internal override bool Leave_Inner() {

            using (var stream = File.CreateText(m_filename)) {

                if (stream == null)
                    return false;

                stream.Write(sb.ToString());
                sb.Length = 0;
            }

            if (Open_File) {
#if WINFORMS
                using (Show_Hide_Form())
#endif
                    H.Run_Notepad(m_filename, true, true);
            }
            if (Open_Dir) {
                var dir = Directory.GetParent(m_filename).FullName;
                Process.Start(dir);
                //                H.Open_In_Explorer(dir, false, false, m_filename);
            }

            return true;
        }

        public override bool Supports_Font(Pos_Print_Font _) {
            return false;
        }



        public override void Set_Font(Pos_Print_Font font) {

            if (!Print_Tags.Contains(Tag_Levels.Fonts))
                return;

            Print_Text(Tag("font", font.ToString()));
        }

        public override bool Is_Async { get { return false; } }

        void Next_Line() {
            sb.Append(nl);
        }

        public override void Print_Text(string text) {

            int len = text.Safe_Length();

            if (len == 0)
                return;

            int ii, jj = 0;

            for (ii = 0; ii < len; ) {

                for (jj = 0; jj < Characters_Per_Line && ii < len; ++ii, ++jj) {
                    sb.Append(text[ii]);

                }
                if (ii < len)
                    Next_Line();
            }

            var rem = Characters_Per_Line - jj;

            if (rem > 0 && len != 0)
                sb.Append(space.Repeat(rem));

            Next_Line();


        }

        public override object Printer_Object { get { throw new InvalidOperationException(); } }

        /*
        public override byte Characters_Per_Line { get { return 48; } }

        /*/
        protected override byte Characters_Per_Line { get { return cst_def_chars_per_line; } }

        //*/




    }
}

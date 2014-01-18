using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Fairweather.Service
{
    public abstract class Writer_Decorator : TextWriter
    {



        public Writer_Decorator(TextWriter writer)
            : base(CultureInfo.InvariantCulture) {


            if (writer == null)
                throw new ArgumentNullException("writer", "writer is null.");

            this.writer = writer;
            this.cast = writer as Writer_Decorator;

        }

        // ****************************

        public override void Close() {
            this.writer.Close();
        }

        public override void Flush() {
            this.writer.Flush();
        }





        public override void Write(bool value) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(value);
        }

        public override void Write(char value) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(value);
        }

        public override void Write(char[] buffer) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(buffer);
        }

        public override void Write(double value) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(value);
        }

        public override void Write(int value) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(value);
        }

        public override void Write(long value) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(value);
        }

        public override void Write(float value) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(value);
        }




        public override void Write(object value) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(value);
        }

        public override void Write(string s) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(s);
        }

        public override void Write(string format, object arg0) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(format, arg0);
        }

        public override void Write(string format, params object[] arg) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(format, arg);
        }

        public override void Write(string format, object arg0, object arg1) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(format, arg0, arg1);
        }

        public override void Write(char[] buffer, int index, int count) {
            this.Maybe_Perform_Line_Action();
            this.writer.Write(buffer, index, count);
        }





        public override void WriteLine() {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine();
            this.func_pending = YES;
        }

        public override void WriteLine(bool value) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(value);
            this.func_pending = YES;
        }

        public override void WriteLine(char value) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(value);
            this.func_pending = YES;
        }

        public override void WriteLine(double value) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(value);
            this.func_pending = YES;
        }

        public override void WriteLine(char[] buffer) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(buffer);
            this.func_pending = YES;
        }

        public override void WriteLine(int value) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(value);
            this.func_pending = YES;
        }

        public override void WriteLine(long value) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(value);
            this.func_pending = YES;
        }

        public override void WriteLine(float value) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(value);
            this.func_pending = YES;
        }

        // [CLSCompliant(false)]
        public override void WriteLine(uint value) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(value);
            this.func_pending = YES;
        }


        public override void WriteLine(object value) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(value);
            this.func_pending = YES;
        }

        public override void WriteLine(string s) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(s);
            this.func_pending = YES;
        }

        public override void WriteLine(string format, object arg0) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(format, arg0);
            this.func_pending = YES;
        }

        public override void WriteLine(string format, params object[] arg) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(format, arg);
            this.func_pending = YES;
        }

        public override void WriteLine(string format, object arg0, object arg1) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(format, arg0, arg1);
            this.func_pending = YES;
        }

        public override void WriteLine(char[] buffer, int index, int count) {
            this.Maybe_Perform_Line_Action();
            this.writer.WriteLine(buffer, index, count);
            this.func_pending = YES;
        }


        // ****************************


        public override Encoding Encoding {
            get {
                return this.writer.Encoding;
            }
        }

        public override string NewLine {
            get {
                return this.writer.NewLine;
            }
            set {
                this.writer.NewLine = value;
            }
        }



        public TextWriter Inner {
            get {
                return this.writer;
            }
        }


        // ****************************


        void Maybe_Perform_Line_Action() {

            if (H.Swap(ref func_pending, NO) != YES)
                return;

            Line_Action();

            //if (cast == null)
            //    return;

            //cast.Perform_Inner();
        }

        protected abstract void Line_Action();


        const int YES = 1;
        const int NO = 0;

        int func_pending = YES;

        readonly protected TextWriter writer;
        readonly Writer_Decorator cast;


    }
}

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fairweather.Service
{
    public class Multi_Writer : TextWriter
    {
        public Multi_Writer(IEnumerable<TextWriter> writers) {
            this.Writers = writers.lst();
        }
        public Multi_Writer(bool parallel, IEnumerable<TextWriter> writers) {
            this.Writers = writers.lst();
            this.Parallel = parallel;
        }
        public Multi_Writer(params TextWriter[] writers) {
            this.Writers = writers.lst();
        }
        public Multi_Writer(bool serial, params TextWriter[] writers) {
            this.Writers = writers.lst();
            this.Parallel = serial;

        }

        public bool Parallel {
            get;
            set;
        }

        public List<TextWriter> Writers {
            get;
            set;
        }


        public override void Flush() {

            foreach (var writer in Writers)
                writer.Flush();

        }

        public override void Close() {
            foreach (var writer in Writers)
                writer.Close();
        }

        public override string NewLine {
            get {
                return base.NewLine;
            }
            set {
                base.NewLine = value;
                foreach (var writer in Writers)
                    writer.NewLine = value;
            }
        }
        protected override void Dispose(bool disposing) {

            if (disposing)
                foreach (var writer in Writers)
                    writer.Dispose();

        }

        public override Encoding Encoding {
            get { return null; }
        }

        public override void Write(char value) {

            foreach (var writer in Writers)
                writer.Write(value);

        }


        // ****************************



        public override void Write(bool value) {

            if (Parallel) { base.Write(value); return; } foreach (var writer in this.Writers) writer.Write(value);
        }





        public override void Write(char[] buffer) {

            if (Parallel) { base.Write(buffer); return; } foreach (var writer in this.Writers) writer.Write(buffer);
        }

        public override void Write(double value) {

            if (Parallel) { base.Write(value); return; } foreach (var writer in this.Writers) writer.Write(value);
        }

        public override void Write(int value) {

            if (Parallel) { base.Write(value); return; } foreach (var writer in this.Writers) writer.Write(value);
        }

        public override void Write(long value) {

            if (Parallel) { base.Write(value); return; } foreach (var writer in this.Writers) writer.Write(value);
        }

        public override void Write(float value) {

            if (Parallel) { base.Write(value); return; } foreach (var writer in this.Writers) writer.Write(value);
        }




        public override void Write(object value) {

            if (Parallel) { base.Write(value); return; } foreach (var writer in this.Writers) writer.Write(value);
        }

        public override void Write(string s) {

            if (Parallel) { base.Write(s); return; } foreach (var writer in this.Writers) writer.Write(s);
        }

        public override void Write(string format, object arg0) {

            if (Parallel) { base.Write(format, arg0); return; } foreach (var writer in this.Writers) writer.Write(format, arg0);
        }

        public override void Write(string format, params object[] arg) {

            if (Parallel) { base.Write(format, arg); return; } foreach (var writer in this.Writers) writer.Write(format, arg);
        }

        public override void Write(string format, object arg0, object arg1) {

            if (Parallel) { base.Write(format, arg0, arg1); return; } foreach (var writer in this.Writers) writer.Write(format, arg0, arg1);
        }

        public override void Write(char[] buffer, int index, int count) {

            if (Parallel) { base.Write(buffer, index, count); return; } foreach (var writer in this.Writers) writer.Write(buffer, index, count);
        }





        public override void WriteLine() {

            if (Parallel) { base.WriteLine(); return; } foreach (var writer in this.Writers) writer.WriteLine();

        }

        public override void WriteLine(bool value) {

            if (Parallel) { base.WriteLine(value); return; } foreach (var writer in this.Writers) writer.WriteLine(value);

        }

        public override void WriteLine(char value) {

            if (Parallel) { base.WriteLine(value); return; } foreach (var writer in this.Writers) writer.WriteLine(value);

        }

        public override void WriteLine(double value) {

            if (Parallel) { base.WriteLine(value); return; } foreach (var writer in this.Writers) writer.WriteLine(value);

        }

        public override void WriteLine(char[] buffer) {

            if (Parallel) { base.WriteLine(buffer); return; } foreach (var writer in this.Writers) writer.WriteLine(buffer);

        }

        public override void WriteLine(int value) {

            if (Parallel) { base.WriteLine(value); return; } foreach (var writer in this.Writers) writer.WriteLine(value);

        }

        public override void WriteLine(long value) {

            if (Parallel) { base.WriteLine(value); return; } foreach (var writer in this.Writers) writer.WriteLine(value);

        }

        public override void WriteLine(float value) {

            if (Parallel) { base.WriteLine(value); return; } foreach (var writer in this.Writers) writer.WriteLine(value);

        }

        // [CLSCompliant(false)]
        public override void WriteLine(uint value) {

            if (Parallel) { base.WriteLine(value); return; } foreach (var writer in this.Writers) writer.WriteLine(value);

        }


        public override void WriteLine(object value) {

            if (Parallel) { base.WriteLine(value); return; } foreach (var writer in this.Writers) writer.WriteLine(value);

        }

        public override void WriteLine(string s) {

            if (Parallel) { base.WriteLine(s); return; } foreach (var writer in this.Writers) writer.WriteLine(s);

        }

        public override void WriteLine(string format, object arg0) {

            if (Parallel) { base.WriteLine(format, arg0); return; } foreach (var writer in this.Writers) writer.WriteLine(format, arg0);

        }

        public override void WriteLine(string format, params object[] arg) {

            if (Parallel) { base.WriteLine(format, arg); return; } foreach (var writer in this.Writers) writer.WriteLine(format, arg);

        }

        public override void WriteLine(string format, object arg0, object arg1) {

            if (Parallel) { base.WriteLine(format, arg0, arg1); return; } foreach (var writer in this.Writers) writer.WriteLine(format, arg0, arg1);

        }

        public override void WriteLine(char[] buffer, int index, int count) {

            if (Parallel) { base.WriteLine(buffer, index, count); return; } foreach (var writer in this.Writers) writer.WriteLine(buffer, index, count);

        }
    }
}

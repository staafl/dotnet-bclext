using System.Diagnostics;
using System.IO;
using System.Text;

namespace Fairweather.Service
{
    public class Line_Counter : TextReader
    {
        readonly TextReader inner;

        public Line_Counter(TextReader inner) {
            this.inner = inner;
        }

        /// <summary>
        /// Also includes the \r character before the \n.
        /// </summary>
        public int Col {
            get;
            private set;
        }

        public int
        Line {
            get;
            private set;
        }

        bool seen_r;

        public override int Peek() {
            return inner.Peek();
        }
        public override void
        Close() {
            inner.Close();
        }

        public override int
        Read() {
            var ret = inner.Read();

            if (seen_r) {
                if (ret == '\n') {
                    ++Line;
                    Col = 0;
                }
                seen_r = false;
            }
            else {
                ++Col;
                seen_r = (ret == '\r');

            }

            return ret;
        }

        //public virtual int Read(char[] buffer, int index, int count);

        //public virtual int ReadBlock(char[] buffer, int index, int count) 

        //public virtual string ReadToEnd();

        public override string
        ReadLine() {

            var ret = inner.ReadLine();
            seen_r = false;
            Col = 0;

            if (ret != null) {
                // if the stream is at a '\n', it will read to the next
                // "\r\n" pair (inclusive)
                if (ret[0] == '\n' && seen_r)
                    ++Line;

                var len = ret.Length;

                // did we read another pair?
                if (len >= 2 &&
                   ret[len - 1] == '\n' &&
                   ret[len - 2] == '\r')
                    ++Line;


            }

            return ret;

        }

    }


}

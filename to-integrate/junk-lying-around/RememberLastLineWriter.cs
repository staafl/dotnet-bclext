using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {


        var writer = new RememberLastLineWriter(Console.Out);

        Console.SetOut(writer);

        Console.WriteLine("foo");
        Console.WriteLine(writer.LastWrittenLine); // foo
        Console.WriteLine("bar");
        Console.WriteLine(writer.LastWrittenLine); // bar

    }

    public class RememberLastLineWriter : System.IO.TextWriter
    {
        readonly System.IO.TextWriter inner;
        readonly System.Text.StringBuilder sb = new System.Text.StringBuilder();

        public string LastWrittenLine
        {
            get;
            private set;
        }

        public RememberLastLineWriter(TextWriter inner)
        {
            this.inner = inner;
        }

        public override System.Text.Encoding Encoding
        {
            get
            {
                return this.inner.Encoding;
            }
        }
        public override void Write(char value)
        {
            this.inner.Write(value);

            if (value == '\r')
                return;
            if (value == '\n')
            {
                // remove everything but the last line in the buffer
                if(this.LastWrittenLine != null)
                    sb.Remove(0, this.LastWrittenLine.Length);
                this.LastWrittenLine = sb.ToString();
                return;
            }

            sb.Append(value);
        }
    }
}
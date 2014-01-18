using System;
using System.Linq;

using Fairweather.Service;


namespace Fairweather.Service
{
    public abstract class Pos_Printer_Engine : Print_Engine_Base
    {
        protected const int cst_def_chars_per_line = 46;


        protected Pos_Printer_Engine() {
        }

        protected const char space = ' ';

        Pos_Print_Font font = Pos_Print_Font.Default;

        public virtual Pos_Print_Font Font {
            get { return font; }
            protected set { font = value; }
        }

        public Pos_Printer_Type Type {
            get;
            protected set;
        }

        public abstract object Printer_Object {
            get;
            //protected set;
        }

        public abstract bool Initialized { get; }
        public void Print_Image(string file, HAlignment align) {

            file = file.Trim();

            if (!System.IO.File.Exists(file)) {
                var ex = new XPrinting("Image file {0} doesn't exist.".spf(file));

                Notify(ex);

                if (Muddle_On_Through)
                    return;

                throw ex;

            }

            Print_Image_Inner(file, align);

        }

        protected abstract void Print_Image_Inner(string file, HAlignment align);
        public abstract void Print_Barcode(string barcode_string);
        public abstract void New_Page();
        public abstract void Open_Cash_Drawer();

        /// <summary>
        /// Whether or not the printing operations are being
        /// performed asynchronously
        /// </summary>
        public abstract bool Is_Async { get; }

        /// <summary>
        /// Represents the number of characters which fit on a single
        /// line using the current Pos_Print_Font
        /// </summary>
        protected abstract byte Characters_Per_Line {
            get;// { return char_lengths[font]; }
            //set { (value <= 10).tiff(); char_lengths[font] = value; }
        }


        /// <summary>
        /// Prints a blank line.
        /// </summary>
        public void Print_Empty_Line() {
            Print_Empty_Line(1);
        }


        /// <summary>
        /// Prints a line break using the underscore '_' character
        /// </summary>
        public virtual void Print_Line_Break() {
            Print_Line_Break('-');
        }

        /// <summary>
        /// Prints a line break using the selected character.
        /// </summary>
        public virtual void Print_Line_Break(char ch) {

            Print_Line_Break(ch, Characters_Per_Line);

        }

        /// <summary>
        /// Prints a line break using the selected character.
        /// </summary>
        public virtual void Print_Line_Break(char ch, int width) {

            var line = ch.Repeat(width);
            Print_Text(line);

        }


        /// <summary>
        /// Prints the selected number of blank lines.
        /// NOTE: should only be called after a newline
        /// </summary>
        public virtual void Print_Empty_Line(int count) {

            for (int ii = 0; ii < count; ++ii)
                Print_Line_Break(space);
        }


        public virtual void Print_Columns_Weighted(Func<int, Pos_Columns_Info> producer, params string[] text) {

            var weighted = producer(this.Characters_Per_Line);
            Print_Columns(weighted, text);

        }

        /// <summary>
        /// Prints the specified columns using the information in "col_info" 
        /// Throws ArgumentException if the column count indicated by "col_info" does not 
        /// equal the number of strings in "text", i.e. the number of entries
        /// </summary>
        public virtual void Print_Columns(Pos_Columns_Info col_info, params string[] text) {

            (col_info.Count == text.Length).tiff<ArgumentException>();


            string to_print = "";
            int caret = 0;
            var padding_char = col_info.Padding_Char;

            bool last_to_the_right = col_info.Last_Column_Right_Aligned;

            var pairs = col_info.Widths_And_Intervals.Mark_Last().ToArray();

            for (int ii = 0; ii < text.Length; ++ii) {


                var truncatable = col_info.Truncatable[ii];
                var column_text = text[ii];
                var pair = pairs[ii];

                var last = pair.Second;
                var column = pair.First;

                int width = column.First;
                int interval = column.Second;
                int total_width = width + interval;

                bool to_right = last && last_to_the_right;

                var remaining = Characters_Per_Line - caret;

                Action bail = () =>
                {
                    Print_Text(to_print);
                    to_print = "";
                    remaining = Characters_Per_Line;
                    caret = 0;
                    --ii;
                };

                int column_textLength = column_text.Length;

                if (to_right) {

                    if (remaining < column_textLength) {
                        bail();
                        to_print += (padding_char).Repeat(Characters_Per_Line - column_textLength);
                        continue;

                    }

                }
                else {

                    if (remaining < width) {
                        bail();
                        continue;
                    }

                    if (truncatable || total_width > column_textLength)
                        column_text = column_text.Trim_Pad(padding_char, width, false);

                }


                if (remaining < total_width)
                    total_width = remaining;


                if (truncatable || total_width > column_textLength)
                    column_text = column_text.Trim_Pad(padding_char, total_width, to_right);

                to_print += column_text;
                caret += column_text.Length;

            }

            Print_Text(to_print);

        }

        /// <summary>
        /// Prints the specified text with the specified alignment
        /// </summary>
        public virtual void Print_Single_Line(string text, HAlignment alignment) {


            text = text.Clip_Right(Characters_Per_Line);

            int remaining = Characters_Per_Line - text.Length;

            if (alignment == HAlignment.Right) {
                text = space.Repeat(remaining) + text;
            }
            else if (alignment == HAlignment.Center) {
                text = space.Repeat(remaining / 2) + text;
            }

            Print_Text(text);

        }

        public void Print_Several_Lines(string text, HAlignment alignment) {

            if (text.IsNullOrEmpty())
                return;

            var lines = text.Lines(true, false, false);

            var max = lines.Max(str => str.Length);

            lines = lines.Select(str => str.PadRight(max, ' ')).lst();

            foreach (var line in lines)
                Print_Single_Line(line, alignment);


        }


        /// <summary>
        /// Prints a block of text line by line, splitting on whitespace whenever possible.
        /// </summary>
        public virtual void Print_Block(string text, bool trim_start_of_newline) {


            int in_text = 0;

            int in_line = 0;

            int text_length = text.Length;

            var left = "";
            var right = "";

            while (in_text <= text_length) {

                if (in_text == text_length) {
                    if (!right.IsNullOrEmpty())
                        Print_Block(right, trim_start_of_newline);
                    break;
                }

                while (in_text < text_length && in_line < Characters_Per_Line) {

                    char ch = text[in_text];

                    right += ch;
                    if (Char.IsWhiteSpace(ch)) {
                        left += right;
                        right = "";
                    }

                    ++in_text;
                    ++in_line;
                }

                if (in_text == text_length) {

                    var len = right.Length;

                    if (in_line + len <= Characters_Per_Line) {
                        left += right;
                        right = "";
                    }

                }

                if (left.IsNullOrEmpty()) {
                    left += right.Clip_Right(Characters_Per_Line);
                    right = right.Trim_Left(Characters_Per_Line);
                }

                if (trim_start_of_newline && (in_text != in_line))
                    left = left.TrimStart(space);

                Print_Text(left);

                left = "";
                in_line = 0;
            }
        }


        public abstract bool Supports_Font(Pos_Print_Font font);

        public abstract void Set_Font(Pos_Print_Font font);

        /// <summary>
        /// Sets the printer font to the first compatible font in the list.
        /// Useful for compatibility between different printing providers.
        /// Returns whether any of the given fonts was compatible (and therefore
        /// set).
        /// </summary>
        public virtual bool Set_Any_Of(params Pos_Print_Font[] fonts) {
            foreach (var font in fonts) {
                if (Supports_Font(font)) {
                    Set_Font(font);
                    return true;
                }
            }
            return true;
        }

        public bool Initialize() {

            return Initialize_Inner();

        }

        public bool Leave() {
            return Leave_Inner();

        }

        internal abstract bool Initialize_Inner();

        internal abstract bool Leave_Inner();


        /// <summary>
        /// Prints a block of text line by line
        /// </summary>
        public abstract void Print_Text(string text);

        public virtual void Raw_Command(string command) {
            throw new NotImplementedException();
        }

        public IDisposable Use(bool throw_on_failed_init) {

            if (!Initialize()) {

                throw_on_failed_init.tift();
                return null;

            }
            return new On_Dispose(() => this.Leave().tiff());
        }

        public IDisposable Use() {
            return Use(true);
        }

        protected void Ensure_Init(string message) {
            if (!Initialized)
                throw new XPrinting(message);

        }
        protected void Ensure_Init() {
            Ensure_Init("not initialized");
        }


    }


}

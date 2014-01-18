using System;
using System.Collections.Generic;

using Fairweather.Service;

namespace Fairweather.Service
{
    public class Pos_Printing_Recorder : Pos_Printer_Engine
    {
        readonly List<Action<int, int, Pos_Printer_Engine>> actions;
        readonly List<Pos_Printer_Engine> printers;

        public int Printout_Copies {
            get;
            set;
        }

        public Pos_Printing_Recorder(Pos_Printer_Engine printer, int printout_copies)
            : this(new List<Pos_Printer_Engine> { printer }, printout_copies) { }

        public Pos_Printing_Recorder(List<Pos_Printer_Engine> printers, int printout_copies) {

            this.printers = printers;
            Printout_Copies = printout_copies;
            actions = new List<Action<int, int, Pos_Printer_Engine>>(48);

        }


        public bool Economize_Actions {
            get;
            set;
        }

        void Add(Action<int, int, Pos_Printer_Engine> act) {
            actions.Add(act);
        }

        void Execute(int times) {

            foreach (var printer in printers)
                for (int ii = 0; ii < times; ++ii)
                    foreach (var action in actions)
                        action(ii, times, printer);

        }

        //public void Cut_Paper(bool linefeed) {
        //    return printer.Cut_Paper(linefeed);
        //Add(() => printer.Cut_Paper(linefeed));
        //}



        public override bool Initialized {
            get {
                return false;
            }
        }


        Action<int, int, Pos_Printer_Engine>
        Start(Action<Pos_Printer_Engine> act) {
            return (_ii, _, _printer) =>
            {
                if (_ii != 0 && Economize_Actions)
                    return;
                act(_printer);
            };
        }

        Action<int, int, Pos_Printer_Engine>
        End(Action<Pos_Printer_Engine> act) {
            return (_ii, _cnt, _printer) =>
            {
                if (_ii != _cnt - 1 && Economize_Actions)
                    return;
                act(_printer);
            };
        }

        Action<int, int, Pos_Printer_Engine>
        Always(Action<Pos_Printer_Engine> act) {
            return (_1, _2, _printer) =>
            {
                act(_printer);
            };
        }


        protected override void Print_Image_Inner(string filename, HAlignment alignment) {
            Add(Always(_p => _p.Print_Image(filename, alignment)));
        }
        public override void Print_Single_Line(string line, HAlignment alignment) {
            Add(Always(_p => _p.Print_Single_Line(line, alignment)));

        }
        public override void Print_Barcode(string barcode_string) {
            Add(Always(_p => _p.Print_Barcode(barcode_string)));

        }


        internal override bool Initialize_Inner() {
            Add(Start(_p => _p.Initialize_Inner().tiff()));
            return true;
        }
        public override void Open_Cash_Drawer() {
            Add(End(_p => _p.Open_Cash_Drawer()));

        }
        public override void New_Page() {
            Add(Always(_p => _p.New_Page()));

        }

        internal override bool Leave_Inner() {

            Add(End(_p => _p.Leave_Inner().tiff()));

            Execute(Printout_Copies);

            actions.Clear();

            return true;

        }

        public override bool Set_Any_Of(params Pos_Print_Font[] fonts) {

            //bool ret = false;
            //foreach (var printer in printers) {
            //    if (printer.Supports_Font(font)) {

            //        ret = true;
            //        break;

            //    }
            //}

            Add(Always(_p => _p.Set_Any_Of(fonts)));


            return true;
        }

        public override bool Supports_Font(Pos_Print_Font font) {
            return true;
        }
        public override void Set_Font(Pos_Print_Font font) {

            Add(Always(_p => _p.Set_Font(font)));
        }
        public override bool Is_Async {
            get {
                return false;
            }
        }
        public override void Print_Empty_Line(int count) {

            Add(Always(_p => _p.Print_Empty_Line(count)));
        }
        public override void Print_Text(string text) {

            Add(Always(_p => _p.Print_Text(text)));
        }
        public override object Printer_Object {
            get {
                return printers;
            }
        }
        protected override byte Characters_Per_Line {
            get {
                throw new NotImplementedException();
            }
        }
        public override void Raw_Command(string command) {

            Add(Always(_p => _p.Raw_Command(command)));
        }

        public override void Print_Line_Break(char ch, int width) {
            Add(Always(_p => _p.Print_Line_Break(ch, width)));
        }
        public override void Print_Columns_Weighted(Func<int, Pos_Columns_Info> producer, params string[] text) {
            Add(Always(_p => _p.Print_Columns_Weighted(producer, text)));

        }

        public override void Print_Columns(Pos_Columns_Info cols, params string[] text) {
            Add(Always(_p => _p.Print_Columns(cols, text)));

        }
        public override void Print_Block(string text, bool trim_start_of_newline) {
            Add(Always(_p => _p.Print_Block(text, trim_start_of_newline)));

        }

        public override void Print_Line_Break(char ch) {

            Add(Always(_p => _p.Print_Line_Break(ch)));


        }
    }
}




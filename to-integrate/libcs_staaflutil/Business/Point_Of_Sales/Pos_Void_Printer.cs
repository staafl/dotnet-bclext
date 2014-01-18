
namespace Fairweather.Service
{

    public class Pos_Void_Printer : Pos_Printer_Engine
    {

        public Pos_Void_Printer() {


        }

        public override bool Initialized {
            get {
                return true;
            }
        }

        protected override void Print_Image_Inner(string filename, HAlignment alignment) {


        }
        public override void Print_Single_Line(string line, HAlignment alignment) {


        }
        public override void Print_Barcode(string barcode_string) {


        }
        internal override bool Initialize_Inner() {


            return true;
        }
        public override void Open_Cash_Drawer() {


        }
        public override void New_Page() {


        }
        internal override bool Leave_Inner() {



            return true;

        }
        public override bool Supports_Font(Pos_Print_Font font) {
            return false;
        }
        public override void Set_Font(Pos_Print_Font font) {


        }
        public override bool Is_Async {
            get {
                return false;
            }
        }
        public override void Print_Empty_Line(int count) {


        }
        public override void Print_Text(string text) {


        }
        public override object Printer_Object {
            get {
                return null;
            }
        }
        protected override byte Characters_Per_Line {
            get {
                return 100;
            }
        }
        public override void Raw_Command(string command) {


        }
    }
}
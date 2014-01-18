using System;
using System.Diagnostics;

namespace Common
{
    // autogenerated: C:\Users\Fairweather\Desktop\struct_creator2.pl
    /*
    string description
    string barcode
    string image_src
    */
    [Serializable]
    [DebuggerStepThrough]
    public struct Quick_Item_Data
    {

        public Quick_Item_Data(
                    string name,
                    string barcode,
                    string image_src)
            : this() {

            this.Name = name;
            this.Barcode = barcode.ToUpper();
            this.Image_Src = image_src;
        }


        public string Name {
            get;
            private set;
        }

        public string Barcode {
            get;
            private set;
        }

        public string Image_Src {
            get;
            private set;
        }



    }
}

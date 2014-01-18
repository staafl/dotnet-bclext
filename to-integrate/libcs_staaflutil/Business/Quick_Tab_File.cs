using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Fairweather.Service;

namespace Common
{
    public class Quick_Tab_File
    {
        const string STR_NAMEBARCODEIMAGE = "NAME,BARCODE,IMAGE";

        public Quick_Tab_File(int ix,
                              string name,
                              string dir) {
            this.Ix = ix;
            this.Name = name;
            this.Dir = dir;
        }

        public void Delete() {
            File.Delete(this.Path);

        }

        public bool
        Read(out List<Quick_Item_Data> list) {
            return Read(out list, cst_max_items_per_tab);
        }

        bool
        Read(out List<Quick_Item_Data> list, int? count) {

            list = null;
            if (!File.Exists(Path)) {
                Write_Empty();
                return false;
            }

            list = new List<Quick_Item_Data>();

            // mind the buffering

            using (var sr = new StreamReader(Path)) {
                var str = sr.ReadLine();
                if (str == null)
                    return true;

                if (str.ToUpper() != STR_NAMEBARCODEIMAGE)
                    return false;

                var ser = new Csv_Formatter<Quick_Item_Data>(new QLD_Formatter());

                list.AddRange(ser.Deserialize(sr, count).Take(count ?? int.MaxValue));

                return true;

            }

        }

        public bool
        Write_Empty() {
            return Write(new Quick_Item_Data[0]);
        }
        public bool
        Write(IEnumerable<Quick_Item_Data> links) {
            return Write(Path, links);
        }

        public static bool
        Write(string path, IEnumerable<Quick_Item_Data> links) {
            return Write(path, links, cst_max_items_per_tab);
        }

        static bool
        Write(string path, IEnumerable<Quick_Item_Data> links, int count) {
            using (var sw = new StreamWriter(path, false)) {

                sw.WriteLine("Name,Barcode,Image");
                sw.Flush();

                var ser = new Csv_Formatter<Quick_Item_Data>(new QLD_Formatter());
                foreach (var link in links.Take(count))
                    ser.Serialize(sw.BaseStream, link);

            }

            return true;
        }


        public static string Get_Tab_Path(string dir, int ix) {
            return dir.Cpath(Get_Tab_Filename(ix));

        }

        public static string Get_Tab_Filename(int ix) {
            return cst_file_prefix + (ix + 1) + "." + cst_ext;

        }



        public string Path {
            get {
                return Get_Tab_Path(Dir, Ix);
            }
        }

        public string Name {
            get;
            set;
        }

        public int Ix {
            get;
            set;
        }

        public string Dir {
            get;
            private set;
        }


        class QLD_Formatter : ICsvFormatter<Quick_Item_Data>
        {
            public Quick_Item_Data
            Deserialize(IEnumerable<string> fields) {

                var arr = fields.arr();
                return new Quick_Item_Data(arr[0], arr[1], arr.Length >= 3 ? arr[2] : "");

            }

            public IEnumerable<object> Serialize(Quick_Item_Data obj) {
                return new[] { obj.Name, obj.Barcode, obj.Image_Src };
            }
        }

        public const int cst_max_items_per_tab = 24;

        const string cst_ext = "csv";
        const string cst_rx_file_general = "^" + cst_rx_name + "." + cst_ext + "$";
        const string cst_rx_name = cst_file_prefix + " [1-9]+[0-9]*";
        const string cst_file_prefix = "Tab ";

    }
}

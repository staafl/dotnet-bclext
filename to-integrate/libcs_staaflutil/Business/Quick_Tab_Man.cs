using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Fairweather.Service;

namespace Common
{
    public class Quick_Tab_Man
    {
        public Quick_Tab_Man(string dir) {
            this.Dir = dir;
            Refresh();
        }

        public void Refresh() {
            tabs.Clear();
            tabs.AddRange(Get_Tabs(Dir));
        }

        // ****************************

        public string Dir {
            get;
            private set;
        }

        public IEnumerable<Quick_Tab_File> Tabs {
            get {
                return tabs;
            }
        }

        public int Tab_Count {
            get {
                return tabs.Count;
            }
        }

        static readonly List<Quick_Tab_File> tabs = new List<Quick_Tab_File>();

        // ****************************

        public Quick_Tab_File New_Tab_File(string name) {
            int ix = Tab_Count;
            var path = Quick_Tab_File.Get_Tab_Path(Dir, ix);

            var ret = new Quick_Tab_File(ix, name, Dir);
            tabs.Add(ret);  

            ret.Write_Empty();
            Write_Index();

            return ret;
        }

        public void Rename(Quick_Tab_File from, string to) {
            from.Name = to;
            Write_Index();
        }

        public bool Delete(Quick_Tab_File file) {
            var ix = file.Ix;
            if (file.Dir != Dir)
                return false;

            if (tabs[ix] != file)
                return false;

            tabs.RemoveAt(ix);
            System.IO.File.Delete(file.Path);

            for (int ii = ix; ii < Tab_Count; ++ii) {
                var file2 = tabs[ii];
                file2.Ix = ii;
                H.Swap_Files(file2.Path, Quick_Tab_File.Get_Tab_Path(Dir, ii));
            }

            Write_Index();
            return true;
        }

        public void Reorder(Quick_Tab_File file, int how_much) {

            int old_ix = file.Ix;
            int new_ix = file.Ix + how_much;
            var there = tabs[new_ix];

            tabs[file.Ix = new_ix] = file;
            tabs[there.Ix = old_ix] = there;

            Write_Index();

            H.Swap_Files(there.Path, file.Path);

        }

        // ****************************

        public static List<Quick_Tab_File>
        Get_Tabs(string dir) {

            var ret = new List<Quick_Tab_File>();

            var files = Read_Index(dir).lst();
            var cnt = files.Count();

            foreach (var pair in files.Ixed()) {

                var qtf = Get_Tab_File(pair.First, pair.Second, dir);

                ret.Add(qtf);

            }

            return ret;

        }

        public static Quick_Tab_File
        Get_Tab_File(int index, string name, string dir) {

            var path = Quick_Tab_File.Get_Tab_Path(dir, index);

            var ret = new Quick_Tab_File(index, name, dir);

            if (!File.Exists(path))
                ret.Write_Empty();

            return ret;
        }

        /// <summary>
        /// reads 'tabs.txt' and yields its contents one line at a time
        /// </summary>
        public static IEnumerable<string>
        Read_Index(string dir) {
            var path = dir.Cpath("tabs.txt");
            if (!File.Exists(path)) {
                if (!Directory.Exists(path))
                    File.WriteAllText(path, "");
                yield break;
            }

            using (var sr = new StreamReader(path)) {
                string str;
                while ((str = sr.ReadLine()) != null)
                    if (!str.Trim().IsNullOrEmpty())
                        yield return str;
            }
        }

        public void Write_Index() {
            Write_Index(Dir, tabs.Select(_file => _file.Name));
        }
        public static void
        Write_Index(string dir, IEnumerable<string> files) {
            File.WriteAllText(dir.Cpath("tabs.txt"), files.Pretty_Print(_file => _file + Environment.NewLine));

        }

    }
}

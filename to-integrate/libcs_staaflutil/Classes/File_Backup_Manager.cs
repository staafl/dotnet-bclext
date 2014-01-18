using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Common;
using Common.Dialogs;
using Fairweather.Service;
using Standardization;
using Excel = Microsoft.Office.Interop.Excel;
using Versioning;

namespace Fairweather.Service
{
    // todo: error handling and validation
    public class File_Backup_Manager : IDisposable
    {
        public File_Backup_Manager(string dir, string ext) {

            this.Dir = dir;
            this.Extension = ext.TrimStart('.');

        }


        // Dangerous!
        public bool Delete_Old_Backup {
            get;
            set;
        }

        public string Extension {
            get;
            private set;
        }

        public string Dir {
            get;
            private set;
        }

        public bool Success {
            get;
            set;
        }

        public void Dispose() {
            if (!Success)
                Restore_All(true, false);
        }

        readonly List<Pair<string>> backed_up = new List<Pair<string>>();

        public IEnumerable<Pair<string>> Backed_Up {
            get {
                foreach (var item in backed_up) {
                    yield return item;
                }
            }
        }

        public void Backup(string path) {

            if (!File.Exists(path))
                return;

            var filename = Path.GetFileNameWithoutExtension(path);
            var source_dir = Path.GetDirectoryName(path);

            var backup_path = "";
            var suffix = "";
            int ii = 1;

            do {
                backup_path = Dir.Cpath(filename + suffix + "." + Extension);

                suffix = "." + ii;
                ++ii;

            } while (File.Exists(backup_path) && !Delete_Old_Backup);



            Directory.CreateDirectory(Dir);

            if (Delete_Old_Backup)
                File.Delete(backup_path);

            File.Copy(path, backup_path);

            backed_up.Add(new Pair<string>(path, backup_path));


        }

        public void Restore_All(bool overwrite, bool delete) {
            foreach (var pair in backed_up) {
                File.Copy(pair.Second, pair.First, overwrite);
                if (delete)
                    File.Delete(pair.Second);
            }
            backed_up.Clear();
        }

    }
}

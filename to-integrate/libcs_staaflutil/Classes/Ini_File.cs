using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Text.RegularExpressions;

namespace Fairweather.Service
{
    public class Ini_File : IReadWrite<Ini_Field, string>
    {
        /*       This system is not flexible, but at least it works        */

        static Ini_File() {

            instances = new Dictionary<string, Ini_File>();

            crypt = new Lazy_Dict<string, ICrypt>(_ => new Crypt_Clear());

        }

        protected Ini_File(string file, out bool ok) {

            ok = false;

            this.file = file;

            groups = new Dictionary<string, Set<string>>();

            data = new Dictionary<Pair<string>, string>();

            comments = new Dictionary<Pair<string>, List<string>>();

            // Represents the ordering of keys in a particular group
            lines = new Dictionary<string, List<string>>();

            ok = Read_Data(file);


        }

        public static bool
        Get(string file, out Ini_File ini) {

            ini = null;

            file = Normalize(file);

            if (file == "")
                return false;

            if (!File.Exists(file))
                return false;

            if (instances.TryGetValue(file, out ini))
                return true;

            bool ok;


            ini = new Ini_File(file, out ok);

            if (!ok)
                return false;

            instances[file] = ini;
            return true;

        }


        // ****************************

        public void
        Read_Data() {
            Read_Data(file);
        }

        bool
        Read_Data(string file) {

            ICrypt crypt = Ini_File.crypt[file];

            string cipher = File.ReadAllText(file);
            string clear = crypt.Decrypt(cipher);

            string temp;
            string group = "";

            var group_keys = new Set<string>();
            var group_lines = new List<String>();
            var group_comments = new List<string>();


            Action make_group = () =>
            {
                lines[group] = group_lines;
                groups[group] = group_keys;

                group_keys = new Set<string>();
                group_lines = new List<string>();
            };



            // To add whitespace, modify the comments infrastructure
            using (var str_r = new StringReader(clear))

                while ((temp = str_r.ReadLine()) != null) {

                    temp = temp.Trim();

                    if (temp.IsNullOrEmpty())
                        continue;

                    if (temp.StartsWith("#")) {

                        var comment = temp.Substring(1);

                        group_comments.Add(comment);

                        continue;
                    }

                    var match_1 = rx_group.Match(temp);

                    if (match_1.Success) {

                        make_group();

                        // Groups[0] is always the entire match

                        group = match_1.Groups[1].Value;

                        if (group.IsNullOrEmpty())
                            return false;

                        continue;
                    }


                    var match_2 = temp.Match(rx_input);

                    if (!match_2.Success)
                        return false;

                    var key = match_2.Groups[1].Value;
                    var value = match_2.Groups[2].Value;

                    if (key.IsNullOrEmpty())
                        return false;

                    group_keys.Add(key);
                    group_lines.Add(key);

                    var pair = new Pair<string>(group, key);

                    data[pair] = value;

                    comments[pair] = group_comments;
                    group_comments = new List<string>();


                }

            make_group();

            Is_Fresh = true;

            return true;
        }


        // ****************************


        public void
        Write_Data() {
            Write_Data(false);
        }

        public void
        Write_Data(bool force) {
            Write_Data(file, force);
        }

        void
        Write_Data(string file, bool force) {

            file = Normalize(file);

            if (Is_Fresh && !force)
                return;

            var crypt = Ini_File.crypt[file];

            var sb = new StringBuilder(30000);
            string output = "";

            foreach (var group_pair in groups.Sort_By_Key()) {

                var group = group_pair.Key;
                // var keys = new Set<string>(group_pair.Value); /*[sic]*/
                var keys = group_pair.Value;

                var lines_list = lines[group];


                if (!group.IsNullOrEmpty())
                    sb.AppendLine("[{0}]".spf(group));

                foreach (var field in lines_list) {

                    // keys.Remove(key).tiff();

                    var pair = Pair.Make(group, field);


                    /*       Comments        */

                    List<string> list;

                    if (comments.TryGetValue(pair, out list)) {

                        foreach (var comment in list)
                            sb.AppendLine("#" + comment);


                    }

                    /*       Value        */

                    var new_str = "{0}={1}".spf(field, data[pair]);
                    sb.AppendLine(new_str);

                }

                // keys.Is_Empty().tiff();

            }

            output = crypt.Encrypt(sb.ToString());

            File.WriteAllText(file, output);

            Is_Fresh = true;

        }


        // ****************************

        string Filename {
            get { return file; }
        }

        // ****************************


        bool IContains<Ini_Field>.Contains(Ini_Field key) {

            string _;
            bool ret = this.Try_Get_Data(key, out _);
            return ret;

        }

        public string this[string key] {
            get { return this.Get_Data(new Ini_Field(key)); }
            set { this.Set_Data(new Ini_Field(key), value); }
        }

        public string this[Ini_Field key] {
            get { return this.Get_Data(key); }
            set { this.Set_Data(key, value); }
        }

        public string this[string group, Ini_Field key] {
            get { return this.Get_Data(group, key); }
            set { this.Set_Data(group, key, value); }
        }


        string
        Get_Data(Ini_Field key) {

            return Get_Data("", key);

        }

        string
        Get_Data(string group, Ini_Field field) {

            string result;

            if (!Try_Get_Data(group, field, out result)) {

                true.tift<KeyNotFoundException>("Required field not found: " + field.Field);

            }

            return result;

        }

        public bool
        Try_Get_Data(Ini_Field field, out string result) {
            string group = "";
            return Try_Get_Data(group, field, out result);
        }
        /*       Main overload        */


        public bool
        Try_Get_Data(string group,
                     Ini_Field field,
                     out string result) {

            var pair = new Pair<string>(group, field.Field);

            var ret = data.TryGetValue(pair, out result);

            if (ret && !field.Allow_Blank) {

                if (result.IsNullOrEmpty())
                    ret = false;

            }

            if (!ret && field.Optional) {

                result = field.Default_Value;
                Set_Data(group, field, result);
                ret = true;

            }

            return ret;

        }


        public void
        Set_Data(Ini_Field key, string value) {

            Set_Data("", key, value);

        }

        /*       Main overload        */

        public bool
        Set_Data(string group, Ini_Field field, string value) {

            var pair = new Pair<string>(group, field.Field);

            bool new_key, changed;
            string temp_value;

            if (data.TryGetValue(pair, out temp_value)) {
                new_key = false;
                changed = (temp_value != value);
            }
            else {
                new_key = true;
                changed = true;
            }

            if (!changed)
                return false;

            data[pair] = value;

            var keys = groups.Get_Or_Default(group, () => new Set<string>());
            var lines_2 = lines.Get_Or_Default(group, () => new List<string>());

            keys.Add(field.Field);

            if (new_key)
                lines_2.Add(field.Field);

            Is_Fresh = false;

            return changed;

        }


        // ****************************


        readonly static Dictionary<string, Ini_File>
        instances;

        readonly static Lazy_Dict<string, ICrypt>
        crypt;

        public static void
        Add_Crypto_Proxy(string file, ICrypt proxy) {

            file.tifn();
            proxy.tifn();

            file = Normalize(file);

            crypt[file] = proxy;

        }

        static string Normalize(string file) {
            if (file.IsNullOrEmpty())
                return "";

            file = Path.GetFullPath(file);
            file = file.ToUpper();

            return file;
        }


        // ****************************










        public IRead<Ini_Field, string>
        Group_ro(string category) {

            return new Ini_Group_Class(this, category);

        }

        public Ini_Group_Class
        Group(string category) {

            return new Ini_Group_Class(this, category);

        }



       

        readonly Dictionary<Pair<string>, string> data;
        readonly string file;

        // (group, key) -> list of comments
        readonly Dictionary<Pair<string>, List<string>> comments;

        /// Note - the current writing routine will ignore trailing comments.

        // group -> keys
        readonly Dictionary<string, Set<string>> groups;

        // (group, key) -> line
        readonly
        Dictionary<string, List<string>> lines;

        /// <summary>  Whether the changes in the .ini have been
        /// committed to the hard drive
        /// </summary>
        public bool Is_Fresh {
            get;
            set;
        }

        static readonly string rx_str = @"^({0}+)=({1}*)\s*$".spf(@"[\w\d_]",      // key
                                                                  @".");           // value
        // optional whitespace

        static readonly Regex rx_input = rx_str.To_Regex(RegexOptions.Compiled);

        static readonly string rx_group_str = @"^\[([\w\d_]+)\]";

        static readonly Regex rx_group = rx_group_str.To_Regex(RegexOptions.Compiled);


    }
}

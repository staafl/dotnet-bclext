using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Fairweather.Service;
using Standardization;

using Common;
using Common.Controls;
using Common.Dialogs;

namespace Common.Dialogs
{
    public partial class Version_Form : Form_Base
    {
        readonly List<Text_Label> tbx_list;
        readonly List<Label> lab_list;
        readonly Set<Control> used;
        readonly List<Program_File_Info> files;
        readonly Dictionary<Program_File_Info, string> custom_versions;

        public Version_Form(IEnumerable<Program_File_Info> files,
                           Dictionary<Program_File_Info, string> custom_versions)
            : base(Form_Kind.Modal_Dialog | Form_Kind.Fixed) {

            this.files = files.lst();
            this.custom_versions = custom_versions;

            // ****************************


            InitializeComponent();

            // ****************************


            tbx_list = this.Controls.OfType<Text_Label>().lst();

            lab_list = this.Controls.OfType<Label>()
                                    .Where(l => !(l is Text_Label))
                                    .lst();

            //if (tbx_list[0] is TextBox)
            //    tbx_list.RemoveAt(0);

            used = new Set<Control>();


            // ****************************





        }
        protected override void OnLoad(EventArgs e) {

            base.OnLoad(e);

            but_save.Top = but_close.Top = 288;




            tbx_list.Sort(tb => tb.Location.Y);

            lab_list.Sort(lab => lab.Location.Y);

            Fill_Labels(files, custom_versions);

            Prepare_Visibility();

            this.but_close.Click += (_1, _2) => Cancel_Clicked();



        }
        void Prepare_Visibility() {

            int tab_index = 10;

            foreach (var pair in tbx_list.Zip(lab_list)) {

                var tb = pair.First;
                var lab = pair.Second;

                tb.TabIndex = tab_index;
                tab_index += 10;

                tb.Visible = used[tb];
                lab.Visible = used[lab];

                if (!used[tb]) {
                    this.MinimumSize = this.MinimumSize.Expand(0, -tb.Height);
                    this.Height -= tb.Height;
                }

            }
        }


        void Fill_Labels(IEnumerable<Program_File_Info> files,
                         Dictionary<Program_File_Info, string> custom_versions) {

            foreach (var triple in files.Zip(tbx_list, lab_list)) {

                var file = triple.First;
                var tb = triple.Second;
                var lab = triple.Third;

                string version;
                if (File.Exists(file.Path)) {


                    version = custom_versions
                        .Get_Or_Default(file,
                                        () => FileVersionInfo.GetVersionInfo(file.Path).FileVersion);

                }
                else {
                    version = "Not Found.";
                }

                tb.Text = version;
                lab.Text = file.Name.ToUpper();
                used[tb] = true;
                used[lab] = true;

            }
        }
        // Dumps the contents of the textboxes in versions.txt in the following
        // format:
        // File                     Version
        // SES.EXE         1.0.12132.213132
        void but_save_Click(object sender, EventArgs e) {

            const string filename = "versions.txt";
            string nl = Environment.NewLine;
            string format = "{0,-20} {1,13}";

            var sb = new StringBuilder();

            // sb.AppendFormat("{0} - Component Files Report" + nl + nl, Global.Application_Name);

            var header = M.Get_Report_Header("", "Component Files Versions", filename, true, true);

            sb.AppendLine(header);
            sb.AppendLine();

            sb.AppendFormat(format, "File", "Version");
            sb.AppendLine();
            sb.AppendLine();

            foreach (var pair in lab_list.Zip(tbx_list)) {

                if (used[pair.First]) {
                    string str = format.spf(pair.First.Text, pair.Second.Text);
                    str = str.Replace(" ", ".");
                    sb.AppendLine(str);
                }
            }

            using (var sw = new StreamWriter(filename)) {

                sw.Write(sb.ToString());
                sb.Length = 0;

            }

            this.Allow_Normal_Close = false;
            H.Run_Notepad(filename, true, false);

            this.Force_Handle();
            BeginInvoke((MethodInvoker)(() =>
            {
                this.Allow_Normal_Close = true;
            }));

        }

    }
}

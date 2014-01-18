using System;
using Ionic.Zip;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SageIntConfig
{
    public partial class VCRedForm : Form
    {
        Thread extract_thread;
        bool b_close;
        public VCRedForm(string ver)
        {
            InitializeComponent();

            DirectoryInfo temp_dir = Directory.CreateDirectory(Environment.CurrentDirectory + "\\temp");
            temp_dir.Attributes = FileAttributes.Hidden | FileAttributes.Directory;

            extract_thread = new Thread(ExtractVCRED);
            extract_thread.IsBackground = true;
            extract_thread.Start();

            label1.Text = label1.Text.Replace("XX", ver);
        }
        void ExtractVCRED()
        {
            if (!File.Exists("temp\\vcr.dta"))
                using (ZipFile zip = new ZipFile("sge.dta"))
                {
                    ZipEntry e = zip["vcr.dta"];
                    e.ExtractWithPassword(Environment.CurrentDirectory + "\\temp", true, "Info!0000");
                }
            if (!File.Exists("temp\\vcredist5.exe"))
                using (ZipFile zip = new ZipFile("temp\\vcr.dta"))
                {
                    ZipEntry e = zip.Entries[0];
                    e.Extract(Environment.CurrentDirectory + "\\temp", true);
                }

        }
        bool InstallVCRED()
        {
            try
            {
                Process vcredist = new Process();
                vcredist.StartInfo.FileName = Environment.CurrentDirectory + "\\temp\\vcredist5.exe";
                vcredist.StartInfo.UseShellExecute = false;
                vcredist.StartInfo.Arguments = "/q:a /c:\"msiexec /i vcredist.msi /qn\"";

                if (extract_thread.IsAlive)
                    extract_thread.Join();

                vcredist.Start();
                vcredist.WaitForExit();

                return true;
            }
            catch(Exception ex)
            {
                string trouble = ex.Message.ToUpper();
                if (trouble.Contains("ELEVATION"))
                {
                    MessageBox.Show("The installation of Microsoft Visual C++ Redistributable package has failed.\n\n"
                                   + "Please run the program again, using an account with Administrative\n"
                                   + "priviliges.",
                                    "Sage Interface Tools - Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                } 
                else
                {
                    MessageBox.Show("The installation of Microsoft Visual C++ Redistributable package has failed.\n",
                                    "Sage Interface Tools - Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                }
                return false;
            }

        }
        public bool OK { get; private set; }
        private void but_ok_Click(object sender, EventArgs e)
        {
            label1.Text = "\nInstalling Microsoft Visual C++\nRedistributable package.\n\nPlease wait...";
            label1.Location = new System.Drawing.Point(label1.Location.X - 1, label1.Location.Y);
            Text = "Sage Interface Tools - Installing Components";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            but_no.Visible = but_ok.Visible = false;
            Refresh();
            
            OK = InstallVCRED();
            
            if (OK)
            
            {
                MessageBox.Show("The required files have been installed successfully",
                                "Success",
               MessageBoxButtons.OK,
               MessageBoxIcon.Information);
            }
            b_close = true;
            Close();
        }

        private void but_no_Click(object sender, EventArgs e)
        {
            OK = false;
            b_close = true;
            Close();
        }

        private void VCRedForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !b_close;
            if (b_close)
            {
                if (extract_thread.IsAlive)
                    extract_thread.Join();
                if (Directory.Exists("temp"))
                    Directory.Delete("temp", true);
            }
        }

    }
}

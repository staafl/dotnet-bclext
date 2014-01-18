using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SageIntConfig
{
    public partial class DotNetForm : Form
    {
        bool b_close;
        public static List<string> dotnetpages = new List<string> { @"http://www.microsoft.com/downloads/info.aspx?na=90&p=&SrcDisplayLang=en&SrcCategoryId=&SrcFamilyId=ab99342f-5d1a-413d-8319-81da479ab0d7&u=http%3a%2f%2fdownload.microsoft.com%2fdownload%2f0%2f6%2f1%2f061F001C-8752-4600-A198-53214C69B51F%2fdotnetfx35setup.exe",
                                                                    @"http://www.microsoft.com/downloads/details.aspx?FamilyID=AB99342F-5D1A-413D-8319-81DA479AB0D7&displaylang=en" };
        string dnetpage;
        public DotNetForm()
        {
            InitializeComponent();

            if (File.Exists("net.dta"))
            {
                dotnetpages.Clear();
                StreamReader sr = new StreamReader("net.dta");
                string str;
                while (!String.IsNullOrEmpty(str = sr.ReadLine()))
                {
                   dotnetpages.Add(str);
                }
                sr.Close();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (string page in dotnetpages)
            {
                try
                {
                    WebRequest wr = WebRequest.Create(page);
                    if (((HttpWebResponse)wr.GetResponse()).StatusCode == HttpStatusCode.OK)
                    {
                        dnetpage = page;
                        break;
                    }
                }
                catch
                { }
            }
            if (!String.IsNullOrEmpty(dnetpage))
                System.Diagnostics.Process.Start(dnetpage);
            dnetpage = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            b_close = true;
            Close();
        }

        private void DotNetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !b_close;
        }
    }
}

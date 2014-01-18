using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Common;
using Common.Controls;
using Common.Dialogs;
using DTA;

using Fairweather.Service;

namespace Config_Gui
{

    public partial class Receipt_Designer : Form_Base
    {
        /// <summary>
        /// Used to prevent multiple redraws triggered when reading the
        /// Layout data in Read_Layout
        /// </summary>
        bool b_flow;

        public Receipt_Designer(Receipt_Layout layout, Printing_Helper helper)
            : base(Form_Kind.Modal_Dialog) {

            InitializeComponent();

            this.MaximizeBox = true;
            this.MinimizeBox = true;

            this.helper = helper;
            this.helper.Backup_Layout();

            this.Text = "Receipt Designer - " + layout.Friendly_String();

            b_flow = true;
            try {
                Read_Layout();
                Redraw();
            }
            finally {
                b_flow = false;
            }

            //Standard.Flat_Style(cb_zoom);
            //Standard.Make_Readonly_Label(lab_logo);
            Border.Create(pp_control);

            cb_zoom.Refresh_Border();

            cb_zoom.SelectedIndexChanged += (cb_zoom_SelectedIndexChanged);
            cb_zoom.SelectedIndex = 3;

            tb_name.Font = monospace;
            tb_header.Font = monospace;
            tb_footer.Font = monospace;


        }

        static readonly Font monospace = new Font("Courier New", 9);

        readonly Printing_Helper helper;


        void cb_zoom_SelectedIndexChanged(object sender, EventArgs e) {

            var str = cb_zoom.SelectedItem.ToString();

            var item = str.TrimEnd('%');
            var as_ratio = Convert.ToDouble(item) / 100.0;

            pp_control.Zoom = as_ratio;

        }


        void Redraw() {

            Pos_Printing_Utility.Owning_Form = this;

            helper.Scenario = Printing_Scenario.Preview;

            var pair = Pos_Printing_Utility.Prepare_Sample_Receipt(helper);

            var printing_data = pair.First;
            var printer = (Pos_GDI_Printer)pair.Second;
            var engine = (Print_Engine)printer.Printer_Object;
            pp_control.Document = engine.Printing_Provider;

            printer.Initialize();

            Pos_Printing_Utility.Compose_Sales_Receipt(printer, printing_data);

            pp_control.InvalidatePreview();


        }


        void Read_Layout() {

            var layout = helper.Layout;

            this.tb_header.Text = layout.Header;
            this.tb_footer.Text = layout.Footer;

            this.tb_name.Text = layout.Name;

            this.lab_logo.Text = layout.Image ?? "Not specified";
            this.Image_File = layout.Image;
            chb_use_image.Checked = layout.Use_Image;
            chb_use_category.Checked = layout.Use_Category;
            
        }

        void Save_Layout() {

            var header = this.tb_header.Text.Replace("\r", "");
            var footer = this.tb_footer.Text.Replace("\r", "");

            var name = this.tb_name.Text;
            var img = Image_File;
            var use = chb_use_image.Checked;
            var cat = chb_use_category.Checked;

            var layout = new Receipt_Layout_SV(name, header, footer, img, use, cat);

            helper.Layout = layout;

        }


        string file_inner;

        string Image_File {
            get {
                return file_inner;
            }
            set {

                file_inner = value;
                lab_logo.Text = lab_logo.Ellipsis(value, true);

            }
        }

        void Refresh_Preview() {
            Save_Layout();
            Redraw();
        }

        void
        but_refresh_Click(object sender, EventArgs e) {

            Refresh_Preview();

        }

        void
        but_save_Click(object sender, EventArgs e) {

            Save_Layout();
            helper.Remove_Layout_Backup();
            Close();

        }

        void
        but_file_Click(object sender, EventArgs e) {

            using (var dialog = new OpenFileDialog()) {

                if (File.Exists(Image_File)) {
                    dialog.InitialDirectory = Directory.GetParent(Image_File).ToString();
                }

                dialog.CheckFileExists = true;
                dialog.Filter = "Bitmap File (*.bmp)|*.bmp";
                dialog.Title = "Select Logo Image";
                dialog.DefaultExt = "bmp";

                var result = dialog.ShowDialog(this);

                if (result != DialogResult.OK)
                    return;

                Image_File = dialog.FileName;
                if (chb_use_image.Checked)
                    Refresh_Preview();
            }
        }

        void but_cancel_Click(object sender, EventArgs e) {

            helper.Restore_Layout();
            Close();

        }

        void chb_use_image_CheckedChanged(object sender, EventArgs e) {
            if (b_flow)
                return;
            Refresh_Preview();
        }

        void chb_use_category_CheckedChanged(object sender, EventArgs e) {
            if (b_flow)
                return;
            Refresh_Preview();
        }


    }
}
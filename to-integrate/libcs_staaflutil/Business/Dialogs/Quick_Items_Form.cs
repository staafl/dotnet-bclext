using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using Fairweather.Service;
using Common;
using Common.Controls;
using System.IO;

namespace Common.Dialogs
{
    public partial class Quick_Items_Form : Pos_Tool_Form
    {
        public Quick_Items_Form(Form host)
            : this(host, null) {

        }

        public Quick_Items_Form(Form host, Func<Point> location_producer)
            : base(host, location_producer) {

            InitializeComponent();

            tab_control.Padding = new Padding(2, 31, 4, 4);

            tab_control.Do_Not_Focus_Tabs = true;
            tab_control.Suppress_Mouse_Buttons = true;

            Border.Create(tab_control, Quad.Make(1, -1, -1, -1)).BringToFront();

            var lab = new Label();
            lab.Text = "Use 'Config -> Point Of Sales -> Advanced -> Quick Items' in order to define Quick Item links.";
            lab.Font = fnt_err;
            lab.BringToFront();
            lab.TextAlign = ContentAlignment.MiddleCenter;
            lab.Dock = DockStyle.Fill;
            lab.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(lab);

        }



        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);

        }

        Quick_Tab_Man man;
        public void Setup(string dir, Func<Quick_Item_Data, bool> validate) {
            man = new Quick_Tab_Man(dir);
            var tab_files = man.Tabs.lsta();
            Setup(tab_files, validate);

        }

        public void Setup(List<Quick_Tab_File> tab_files, Func<Quick_Item_Data, bool> validate) {

            this.tab_control.Clear();
            this.panels.Clear();

            int max_len = 16;

            int gb_ww_base = 200;
            int gb_hh_base = 188;

            int pb_ww_base = 170;
            int pb_hh_base = 118;
            int pb_xx = 15;
            int pb_yy = 28;

            int gb_padding_ww = 3;
            int gb_margin_ww = 1;
            int gb_margin_hh = 2;

            int label_loc_yy = 82;
            int flp_padding_ll = 2;
            int flp_padding_tt = 3;

            var but_activity_color = Color.LightGray;
            int but_border_size = 2;
            int lab_height = 20;
            var lab_padding = new Padding(0, 7, 0, 0);


            // george's adjustments

            but_border_size = 1;
            pb_xx = 0;
            pb_yy = 0;
            gb_ww_base = pb_ww_base + 3;
            gb_hh_base = pb_hh_base + 30;
            gb_padding_ww -= 1;
            label_loc_yy = 60;
            flp_padding_ll += 7;
            flp_padding_tt += 7;
            gb_margin_ww = 5;
            gb_margin_hh = 4;

            Func<int, int> size_transform = _sz => (int)(_sz / 1.8);

            var gb_margin_1 = new Padding(gb_margin_ww, gb_margin_hh, gb_margin_ww, gb_margin_hh);
            var gb_padding_1 = new Padding(gb_padding_ww, 5, gb_padding_ww, 3);
            var pb_margin_1 = new Padding(3);
            var flp_padding = new Padding(flp_padding_ll, flp_padding_tt, 0, 0);
            var flp_margin = new Padding(0);


            foreach (var tab_file in tab_files) {

                List<Quick_Item_Data> list;
                if (!tab_file.Read(out list))
                    continue;

                var tab = new UserControl();
                tab.Dock = DockStyle.Fill;
                tab.Name = tab_file.Name;
                tab.Size = new Size(600, 600);

                var flp = new Custom_Flow_Layout_Panel();
                tab.Controls.Add(flp);
                flp.AutoScroll = false;
                flp.Dock = DockStyle.Fill;
                flp.Margin = flp_margin;

                flp.Padding = flp_padding;

                panels[flp] = tab;

                //var tt = new ToolTip();

                foreach (var item in list) {

                    var text = item.Name;// +": " + item.Barcode;

                    var gb = new Panel();

                    flp.Controls.Add(gb);

                    gb.Name = item.Name;
                    gb.Text = "";
                    gb.Size = new Size(size_transform(gb_ww_base), size_transform(gb_hh_base));
                    gb.Padding = gb_padding_1;
                    gb.Margin = gb_margin_1;
                    //gb.BorderStyle = BorderStyle.FixedSingle;
                    var label = new Label();
                    gb.Controls.Add(label);
                    //label.Text = label.Ellipsis(text);
                    label.Text = text.Ellipsis(max_len);
                    label.Font = fnt_title;
                    label.Location = new Point(0, label_loc_yy);
                    label.AutoSize = false;
                    label.Width = gb.Width + 1;
                    label.TextAlign = ContentAlignment.TopCenter;
                    label.BringToFront();
                    label.Height = lab_height;
                    label.AutoEllipsis = true;
                    label.Padding = lab_padding;

                    var but = new Unselectable_Button();
                    gb.Controls.Add(but);
                    but.ForeColor = Color.Black;
                    but.Location = new Point(size_transform(pb_xx), size_transform(pb_yy));
                    but.Margin = pb_margin_1;
                    but.Size = new Size(size_transform(pb_ww_base), size_transform(pb_hh_base));
                    but.FlatStyle = FlatStyle.Flat;
                    but.TextAlign = ContentAlignment.MiddleCenter;
                    but.FlatAppearance.BorderSize = but_border_size;
                    but.FlatAppearance.MouseOverBackColor = but_activity_color;
                    but.FlatAppearance.MouseDownBackColor = but_activity_color;


                    /*
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.move(1, -1);
                    label.resize(-2, -1);
                    //*/


                    var img_src = item.Image_Src ?? "";
                    var img = (Image)null;
                    var err_txt = "";

                    var subscribe_handler = true;

                    if (!validate(item)) {
                        subscribe_handler = false;
                        err_txt = "Product Not Found";

                    }
                    else if (img_src.Trim().IsNullOrEmpty()) {
                        err_txt = text; // "No Image Available";

                    }
                    else if (!File.Exists(img_src)) {
                        err_txt = text;
                        // err_txt = "Image Not Found";

                    }
                    else {
                        img = H.Try_Get_Image(img_src);
                        if (img == null)
                            err_txt = text;
                        //err_txt = "Image Not Valid";

                    }

                    var close = item;
                    EventHandler hdl = (_1, _2) => Clicked.Raise(close);

                    /*
                     tt.SetToolTip(but, text);
                     tt.SetToolTip(gb, text);
                     tt.SetToolTip(label, text);
                    
                     EventHandler hdl2 = (_1, _2) =>
                     {
                         tt.Active = true;
                         tt.Show(text, label);
                     };
                     //label.MouseHover += hdl2;
                     label.MouseEnter += hdl2;
                     */

                    if (subscribe_handler) {
                        but.Click += hdl;
                        gb.Click += hdl;
                        label.Click += hdl;

                    }
                    else {
                        // fake disabled
                        but.FlatAppearance.MouseOverBackColor = Color.Transparent;
                        but.FlatAppearance.MouseDownBackColor = Color.Transparent;

                    }

                    if (img == null) {
                        but.Text = err_txt;
                        but.Font = fnt_err;

                    }
                    else {
                        but.BackgroundImage = img;
                        but.BackgroundImageLayout = ImageLayout.Zoom;

                    }

                    but.BringToFront();
                }


            }

            tab_control.Visible = (panels.Count != 0);

            if (panels.Count == 0)
                return;


            tab_control.Setup(panels.Lefts.First(), panels.Lefts.Skip(1).arr());
            tab_control.Active_Tab = 0;

        }

        public Action<Quick_Item_Data> Clicked;

        const string cst_font_name = "Microsoft Sans Serif";

        readonly Font fnt_err = new Font(cst_font_name, 10, FontStyle.Bold);
        readonly Font fnt_title = new Font(cst_font_name, (float)7.5, FontStyle.Regular/*FontStyle.Bold*/);

        //readonly Twoway<PictureBox, Quick_Item_Data>
        //data = new  Twoway<PictureBox, Quick_Item_Data>();

        readonly Twoway<UserControl, Custom_Flow_Layout_Panel>
        panels = new Twoway<UserControl, Custom_Flow_Layout_Panel>();

        protected override void WndProc(ref Message m) {

            if (m.Msg == Native_Const.WM_MOUSEWHEEL) {

                int scroll = m.Translate_Mouse_Wheel();

                panels[tab_control.Tabs[tab_control.Active_Tab]].Do_Scroll(scroll);
                return;
            }

            base.WndProc(ref m);

        }
    }
}

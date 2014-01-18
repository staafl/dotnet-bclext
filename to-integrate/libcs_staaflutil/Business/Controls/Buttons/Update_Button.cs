using System.Windows.Forms;
using System.Drawing;
using System;
using Fairweather.Service;

namespace Common.Controls
{
    public class Update_Button : Button
    {
        static public Font default_font = new Font(
            "Lucida Console",
            10.0f,
            FontStyle.Bold);

        public Update_Button()
            : base() {

            this.Visible = false;


        }


        // ****************************

        /*       Interface        */


        public void Lock_To_Label(Label lab) {

            this.lab = lab;
            lab.Parent.Controls.Add(this);

            normal_color = lab.BackColor;

            this.FlatStyle = FlatStyle.Flat;
            this.Text = "!";
            this.Font = default_font;
            this.ForeColor = Color.Blue;

            this.Enabled = true;
            this.Visible = true;

            Set_Back_Color();
            Set_Border_Color();

            Align_To_Label(true);

            lab.Move += lab_Move;
            lab.SizeChanged += lab_SizeChanged;
            lab.ParentChanged += lab_ParentChanged;
            lab.VisibleChanged += lab_VisibleChanged;
            lab.BackColorChanged += lab_BackColorChanged;

            Set_Fresh(false);

        }

        public bool Fresh {
            get {
                if (H.Is_In_Designer)
                    return false;
                lab.tifn();
                return fresh;
            }
            set {
                if (H.Is_In_Designer)
                    return;
                if (fresh || value)
                    lab.tifn();
                if (lab == null)
                    return;
                Set_Fresh(value);
            }
        }

        public void Set_Fresh(bool fresh) {

            this.fresh = fresh;

            if (fresh) {
                lab.BackColor = normal_color;
            }
            else {
                lab.BackColor = not_fresh_color;
                lab.Text = "";
            }

        }

        public Color Normal_Color {
            get {
                return normal_color;
            }
            set {
                normal_color = value;
            }
        }

        public Label Label {
            get {
                return lab;
            }
        }


        // ****************************

        void Set_Back_Color() {
            this.FlatAppearance.MouseDownBackColor =
                //this.BackColor;
            this.FlatAppearance.MouseOverBackColor
                //= this.BackColor
                ;
        }

        void Set_Border_Color() {
            this.FlatAppearance.BorderColor = Color.Black;
        }

        protected override void OnBackColorChanged(EventArgs e) {
            base.OnBackColorChanged(e);
            Set_Back_Color();

        }

        protected override void OnForeColorChanged(EventArgs e) {
            base.OnForeColorChanged(e);
            Set_Border_Color();
        }

        void Align_To_Label() {
            bool initial = false;
            Align_To_Label(initial);
        }

        void Align_To_Label(bool initial) {

            var lab_width = lab.Width;
            int lab_height = lab.Height;

            this.Height = lab_height;
            this.Width = lab_height;

            if (initial)
                lab.Width = lab_width - lab_height + 1;

            this.Location = lab.Bounds.Vertex(1).Translate(-1, 0);
            this.BringToFront();

        }


        Label lab;
        Color normal_color;
        bool fresh;

        static readonly Color not_fresh_color = Color.DarkGray.Darken(15);

        void lab_BackColorChanged(object sender, EventArgs e) {
            if (fresh)
                normal_color = lab.BackColor;
        }

        void lab_ParentChanged(object sender, EventArgs e) {
            lab.Parent.Controls.Add(this);
            Align_To_Label();
        }

        void lab_SizeChanged(object sender, EventArgs e) {
            Align_To_Label();

        }

        void lab_Move(object sender, EventArgs e) {
            Align_To_Label();
        }

        void lab_VisibleChanged(object sender, EventArgs e) {
            this.Visible = lab.Visible;
        }

        //protected override void OnEnabledChanged(EventArgs e) {
        //    base.OnEnabledChanged(e);

        //    if (!Enabled)
        //        this.Enabled = true;
        //}
    }
}

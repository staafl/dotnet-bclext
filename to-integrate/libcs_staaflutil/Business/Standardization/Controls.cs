
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Common.Controls;

using Fairweather.Service;


/*       All future control standardization efforts go here        */
namespace Standardization
{
    ////<summary>
    ////USE AND DEVELOP!
    ////</summary>
    public partial class Standard
    {

        public static Font Cb_Monospace_Font {
            get {
                return new Font("Lucida Console", 8f, FontStyle.Regular);
            }
        }

        static public void Numeric_Column(this DataGridViewColumn col) {

            col.Set_Style(style => style.Alignment = DataGridViewContentAlignment.MiddleRight);

        }

        static public Label Readonly_Label(this TextBoxBase tb) {

            var lab = Readonly_Label_Inner(tb);

            tb.Visible = false;
            lab.Visible = true;
            lab.BringToFront();

            return lab;

        }

        static Label Readonly_Label_Inner(this TextBoxBase tb) {

            var lab = new Label();

            Flat_Style(lab);

            lab.Size = tb.Size;

            lab.Location = tb.Location;
            // lab.BackColor = Colors.TextBoxes.ReadOnlyBackGround;
            // lab.BorderStyle = BorderStyle.FixedSingle;

            tb.TextChanged += (_1, _2) => lab.Text = tb.Text;
            lab.Text = tb.Text;

            tb.Parent.Controls.Add(lab);
            return lab;
        }

        static Dictionary<TextBoxBase, Label>
        labs = new Dictionary<TextBoxBase, Label>();

        public static Label
        Toggle_Readonly_Label(this TextBoxBase tb, bool enable) {

            var lab = labs.Get_Or_Default(tb, () => tb.Readonly_Label());

            if (enable) {
                if (lab != null)
                    lab.Visible = false;

                tb.Enabled = true;
                tb.Visible = true;
                tb.BringToFront();

            }
            else {
                tb.Visible = false;
                tb.Enabled = false;
                lab.Visible = true;
                lab.BringToFront();

            }

            return lab;
        }


        static readonly Dictionary<Type, Action<Control>>
        rd_actions = new Dictionary<Type, Action<Control>>
        {
            {typeof(TextBox), ctrl => Flat_Style(ctrl as TextBox)},
            {typeof(DataGridView), ctrl => Flat_Style(ctrl as DataGridView)},

        };


        static public void
        Style_Controls(this Control.ControlCollection ctrls) {

            foreach (Control ctrl in ctrls) {

                Action<Control> action;

                if (!rd_actions.TryGetValue(ctrl.GetType(), out action))
                    continue;

                action(ctrl);

            }

        }

        static public void
        Flat_Style(this RichTextBox rtb) {

            var offsets = new Quad<int>(1, 2, 1, 1);
            var border2 = Border.Create(rtb, offsets);
            border2.BackColor = rtb.BackColor;

        }


        static public void
        Flat_Style(this DataGridView dgv) {

            dgv.tifn();


            dgv.BackgroundColor = Colors.GridView.Background;

            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeRows = false;

            var style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = SystemColors.ButtonHighlight;
            style.Font = new Font("Microsoft Sans Serif", 8.25F,
                                  FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));

            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;

            dgv.ColumnHeadersDefaultCellStyle = style;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        static public void
        Flat_Style(this TextBoxBase tb) {

            tb.tifn();

            if (tb.ReadOnly)
                tb.BackColor = Colors.TextBoxes.ReadOnlyBackGround;
            else
                tb.BackColor = Colors.TextBoxes.NormalBackGround;

            tb.BorderStyle = BorderStyle.FixedSingle;

        }

        static public void
        Flat_Style(this ComboBox box) {

            Quad<int> offsets;

            offsets = Quad.Make(0, 0, 0, 0);

            var border = Border.Create(box, offsets);
            border.Border_Width = 1;

            box.FlatStyle = FlatStyle.Flat;
            box.DropDownStyle = ComboBoxStyle.DropDownList;

            border.BringToFront();
            box.BringToFront();

        }

        //static public IO_Pair<decimal>
        //Readonly_Decimal_Label(this Label lab) {

        //    lab.AutoSize = false;
        //    lab.BackColor = Colors.TextBoxes.ReadOnlyBackGround;
        //    lab.BorderStyle = BorderStyle.FixedSingle;
        //    lab.TextAlign = ContentAlignment.MiddleRight;

        //    var ret = IO_Pair.Make((decimal val) => lab.Text = val.ToString(true),
        //                           () => lab.Text.FromString());
        //    // lab.Tag = ret;

        //    return ret;
        //}


        static public void
        Flat_Style(this Label lab) {
            Flat_Style(lab, true);
        }

        static public void
        Flat_Style(this Label lab, bool left) {

            lab.AutoSize = false;
            lab.AutoEllipsis = true;
            lab.BackColor = Colors.TextBoxes.ReadOnlyBackGround;
            lab.BorderStyle = BorderStyle.FixedSingle;
            lab.UseMnemonic = false;

            lab.Padding = new Padding(left ? 2 : 0, 2, left ? 0 : 1, 0);

            lab.TextAlign = left ? ContentAlignment.TopLeft :
                                   ContentAlignment.TopRight;

            lab.Height = 21;

        }

        static public void
        Normal_Combo_Box(this ComboBox cb) {

            cb.FlatStyle = FlatStyle.Flat;

        }

        static public void
        Style_Validating_Box(this Validating_Box vb, bool left_align) {

            vb.Decimal_Places = 2;
            vb.BorderStyle = BorderStyle.FixedSingle;

            if (left_align) {
                vb.TextAlign = HorizontalAlignment.Left;
            }
            else {
                vb.TextAlign = HorizontalAlignment.Right;
                vb.Right_Padding = 1;
            }


        }

    }
}

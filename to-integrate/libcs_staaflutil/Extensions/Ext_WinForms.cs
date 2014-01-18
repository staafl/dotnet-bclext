#if WINFORMS

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Cell = System.Windows.Forms.DataGridViewCell;
using System.Diagnostics;
using System.ComponentModel;

namespace Fairweather.Service
{

    public static partial class Extensions
    {
        public static void Darken(this Bitmap bmp) {
            using (var g = Graphics.FromImage(bmp))
                Darken(bmp, g);
        }

        public static void Darken(this Bitmap bmp, Graphics g) {

            // http://www.aspfree.com/c/a/C-Sharp/Performing-Color-Transformation-Operations-in-Csharp-GDIplus/

            // define a color transformation matrix
            float[][] color_matrix_elem = {

                new float[]{1.0f, 0.0f, 0.0f, 0.0f, 0.0f},

                new float[]{0.0f, 1.0f, 0.0f, 0.0f, 0.0f},

                new float[]{0.0f, 0.0f, 1.0f, 0.0f, 0.0f},

                new float[]{0.0f, 0.0f, 0.0f, 1.0f, 0.0f},

                new float[]{-0.51f, -0.515f, -0.51f, 0.0f, 1.0f}
                            //R      G       B       A     
            };

            var color_matrix = new ColorMatrix(color_matrix_elem);

            //Transform the image
            var rect = new Rectangle(Point.Empty, bmp.Size);
            //using (
            var img_attr = new ImageAttributes();//) 
            {

                img_attr.SetColorMatrix(color_matrix, 0, ColorAdjustType.Bitmap);

                g.DrawImage(bmp, rect,
                            0, 0, bmp.Width, bmp.Height,
                            GraphicsUnit.Pixel, img_attr);

            }


        }

        public class Panel2 : Panel
        {
            public Panel2() {
                SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint
                         , true);
            }

            public void Done() {
                this.Refresh();
                this.Hide();

            }
        }
        public static IDisposable
        Dim_Form(this Form form) {
            Action do_it;
            var ret = Dim_Form(form, out do_it);
            try {
                do_it();
            }
            catch { using (ret) { } }
            return ret;
        }

        //static Dictionary<Form, Common.Controls.Translucent_Panel> panels = new Dictionary<Form, Common.Controls.Translucent_Panel>();

        static readonly object dimmed = new object();

        public static bool
        Dimmed(this Form form) {
            return form.Controls.OfType<Panel2>().Any(_ctrl => _ctrl.Tag == dimmed);

        }
        public static IDisposable
        Dim_Form(this Form form, out Action do_it) {

            do_it = () => { };

            if (form.Dimmed())
                return null;

            var cl_rect = form.ClientRectangle;
            var cl_size = form.ClientSize;

            //*
            var bmp1 = new Bitmap(cl_size.Width, cl_size.Height, PixelFormat.Format32bppRgb);
            var bmp2 = bmp1;
            var g1 = Graphics.FromImage(bmp1);
            var forms = new Dictionary<Form, double>();
            var panel = new Panel2();
            panel.Tag = dimmed;
            /*/
            var panel = new Common.Controls.Translucent_Panel();
            //*/

            var ret = new On_Dispose(() =>
            {
                //*
                using (bmp1)
                using (bmp2)
                using (g1) {
                    panel.BackgroundImage = bmp2;//*/

                    using (panel) {

                        panel.Done();
                        form.Invalidate(true);
                        form.Controls.Remove(panel);

                    }
                    //* 
                    foreach (var kvp in forms)
                        kvp.Key.Opacity = kvp.Value;
                }//*/

                form.Refresh();
                form.Activate();

            });

            try {

                // http://www.eggheadcafe.com/forumarchives/NETFrameworkdrawing/Feb2006/post25815823.asp

                //*
                var corner = form.PointToScreen(Point.Empty);

                // remove troublesome children
                foreach (Form form2 in form.OwnedForms) {
                    if (form2.Visible && form2 != form) {
                        forms[form2] = form.Opacity;
                        form2.Opacity = 0;

                    }
                }

                form.BringToFront();
                // form.Refresh();

                g1.CopyFromScreen(corner.X, corner.Y, 0, 0, cl_size);

                bmp2 = new Bitmap(bmp1);

                bmp1.Darken(g1);

                panel.BackgroundImage = bmp1;//*/


                do_it = () =>
                {
                    form.Controls.Add(panel);

                    panel.Bounds = cl_rect;
                    panel.Visible = true;
                    panel.BringToFront();
                    panel.Refresh();
                };

                return ret;

            }
            catch {
                using (ret) { }
                throw;
            }
        }

        /// <summary>
        /// Temporarily prevents the form from being closed.
        /// </summary>
        public static IDisposable
        Make_Unclosable(this Form form) {

            FormClosingEventHandler h1 = (_1, _args) => _args.Cancel = true;
            CancelEventHandler h2 = (_1, _args) => _args.Cancel = true;


            try {
                form.FormClosing += h1;
                form.Closing += h2;
                return new On_Dispose(() =>
                {
                    if (!form.Disposed_Ing()) {
                        form.FormClosing -= h1;
                        form.Closing -= h2;
                    }
                });
            }
            catch {
                form.FormClosing -= h1;
                form.Closing -= h2;

                throw;
            }
        }

        /// <summary>
        /// No, Cancel
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static bool
        Negative(this DialogResult dr) {
            switch (dr) {
                case DialogResult.Cancel:
                case DialogResult.No:
                    return true;
                default:
                    return false;
            }

        }
        /// <summary>
        /// OK, Retry, Yes
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static bool
        Positive(this DialogResult dr) {
            switch (dr) {
                case DialogResult.OK:
                case DialogResult.Retry:
                case DialogResult.Yes:
                    return true;
                default:
                    return false;
            }
        }

        public static bool
        Editable(this DataGridViewBand col) {
            return col.Visible && !col.ReadOnly;
        }

        public static bool
        Editable(this DataGridViewCell col) {
            return col.Visible && !col.ReadOnly;
        }
        /// Returns the cell's location
        /// with respect to the grid.
        /// 
        /// Asking for a cell from row -1 is equivalent to asking
        /// for the location of a header cell.
        /// 
        /// If the cell is not visible, the actions taken are:
        ///
        /// (scroll_if_not_visible) -> scroll until the cell is visible 
        ///                            and return coordinates
        /// 
        /// else -> return null;
        [DebuggerStepThrough]
        public static Point?
        Get_Cell_Coords(this DataGridView grid,
                        int col,
                        int row,
                        bool scroll_if_not_visible) {

            return grid.Get_Cell_Coords(col, row, scroll_if_not_visible, false);
        }

        /// <summary>
        /// Returns the indices of the first and of the last rows that are or
        /// can be accommodated by the grid, depending on the scrolling position
        /// and on the size of the rows with respect to the grid. 
        /// 
        /// NB: It assumes that there are no invisible rows.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        public static void
        Displayed_Rows(this DataGridView grid, out int first, out int last) {

            first = grid.FirstDisplayedScrollingRowIndex;

            int count = grid.DisplayedRowCount(false);

            last = first + (count - 1);

        }

        /// <summary>
        /// (Conditionallys) scrolls the grid so that the row specified
        /// by 'ix' is visible and with at least 'margin' rows between it
        /// and the end of the top or bottom of the grid.
        /// 
        /// If 'only_if_not_visible', then the scrolling will be performed only
        /// if the row is outside the currently visible range on the grid, regardless
        /// of whether the margin requirement is satisfied.
        /// 
        /// If 'only_check', no scrolling will be performed - in this case, the function
        /// is referentially transparent and only useful for its return value.
        /// 
        /// Return value: whether scrolling was actually performed.
        /// 
        /// NB: Behavior is not defined if 'margin' > grid.DisplayedRowCount(false).
        /// Garbage in, garbage out...
        /// </summary>
        public static bool
        Ensure_Visible(this DataGridView grid, int ix, int margin, bool only_if_not_visible, bool only_check) {

            int first;
            int last;
            int new_first;

            Displayed_Rows(grid, out first, out last);
            if (last < 0)
                return false;
            if ((only_if_not_visible ? ix : ix - margin) < first) {
                new_first = Math.Max(ix - margin, 0);
            }
            else if ((only_if_not_visible ? ix : ix + margin) > last) {
                new_first = first + ((ix + margin) - last);
            }
            else {
                return false;
            }

            if (first == new_first)
                // this is possible
                return false;

            if (!only_check)
                grid.FirstDisplayedScrollingRowIndex = new_first;
            return true;
        }

        /// <summary>
        /// Slight variations are possible because of the particular grid's
        /// border style, etc.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="scroll_if_not_visible"></param>
        /// <param name="allow_fictive"></param>
        /// <returns></returns>
        public static Point?
        Get_Cell_Coords(this DataGridView grid,
                        int col,
                        int row,
                        bool scroll_if_not_visible,
                        bool allow_fictive) {

            /*       Assumptions        */
            // * All rows are of the same size

            int xx = 2;

            if (grid.RowHeadersVisible)
                xx += grid.RowHeadersWidth;

            for (int ii = 0; ii < col; ++ii) {

                var col_obj = grid.Columns[ii];
                if (col_obj.Visible)
                    xx += col_obj.Width;

            }


            var yy = grid.ColumnHeadersHeight;

            var visible = true;

            if (row != -1) { //Special case

                if (Ensure_Visible(grid, row, 0, true, !scroll_if_not_visible))
                    if (scroll_if_not_visible) {
                        visible = false;
                    }
                    else {
                        // scrolling was performed
                    }


                if (visible || allow_fictive) {
                    if (grid.RowCount > 0) {
                        int first, _;
                        grid.Displayed_Rows(out first, out _);
                        yy += ((row - first) * grid.Rows[0].Height) + 1;
                    }
                }
            }

            if (visible || allow_fictive)
                return new Point(xx, yy);

            return null;
        }

        public static void Delay(this Control ctrl, Action act, int times) {

            if (times <= 0) {
                act();
            }
            else {
                ctrl.Force_Handle();
                ctrl.BeginInvoke((MethodInvoker)(() => Delay(ctrl, act, times - 1)));
            }


        }

        public static int Maximum_Displayed_Rows(this DataGridView dgv, int row_height) {
            int header_height = dgv.ColumnHeadersHeight;
            int grid_height = dgv.ClientRectangle
                                  .Expand(-header_height, false, true, false, false)
                                  .Size
                                  .Height;

            int ret = grid_height / row_height;

            return ret;
        }

        public static IDisposable Suspend_Layout(this Control ctrl) {

            ctrl.SuspendLayout();
            return new On_Dispose(() => ctrl.ResumeLayout());

        }

        public static IDisposable Temp_Hide(this Control ctrl) {

            ctrl.Hide();
            return new On_Dispose(() => ctrl.Show());

        }


        public static int
        Max_Visible_Rows(this DataGridView grid) {
            var hh = grid.Height;

            if (grid.ColumnHeadersVisible)
                hh -= grid.ColumnHeadersHeight;

            var rh = grid.RowCount > 0 ? grid.Rows[0].Height : grid.Font.Height;

            var ratio = hh / (double)rh;

            return (int)ratio;

        }

        /*       Layout        */

        public static void
        Align_Middles(this Control ctrl, params Control[] ctrls) {

            var middle = (ctrl.Top + ctrl.Bottom) / 2;
            foreach (var ctrl2 in ctrls) {
                var top2 = ctrl2.Top;
                var bottom2 = ctrl2.Bottom;
                var middle2 = (top2 + bottom2) / 2;
                ctrl2.Top = top2 + (middle - middle2);

            }

        }

        public static void
        Align_Centers(this Control ctrl, params Control[] ctrls) {

            var center = (ctrl.Top + ctrl.Bottom) / 2;
            foreach (var ctrl2 in ctrls) {
                var top2 = ctrl2.Top;
                var bottom2 = ctrl2.Bottom;
                var center2 = (top2 + bottom2) / 2;
                ctrl2.Top = top2 + (center - center2);

            }

        }


        public static void
        Make_Same_Size(this Control ctrl, params Control[] ctrls) {

            foreach (var ctrl2 in ctrls)
                ctrl2.Size = ctrl.Size;

        }

        public static void
        Make_Same_Width(this Control ctrl, params Control[] ctrls) {

            foreach (var ctrl2 in ctrls)
                ctrl2.Width = ctrl.Width;

        }
        public static void
        Make_Same_Height(this Control ctrl, params Control[] ctrls) {

            foreach (var ctrl2 in ctrls)
                ctrl2.Height = ctrl.Height;

        }

        public static void
        Align_Lefts(this Control ctrl, params Control[] ctrls) {

            foreach (var ctrl2 in ctrls)
                ctrl2.Location = new Point(ctrl.Left, ctrl2.Top);

        }
        public static void
        Align_Rights(this Control ctrl, params Control[] ctrls) {

            foreach (var ctrl2 in ctrls)
                ctrl2.Location = new Point(ctrl.Right - ctrl2.Width, ctrl2.Top);

        }

        public static void
        Align_Tops(this Control ctrl, params Control[] ctrls) {

            foreach (var ctrl2 in ctrls)
                ctrl2.Location = new Point(ctrl2.Left, ctrl.Top);

        }
        public static void
        Align_Bottoms(this Control ctrl, params Control[] ctrls) {

            foreach (var ctrl2 in ctrls)
                ctrl2.Location = new Point(ctrl2.Left, ctrl.Bottom - ctrl2.Height);

        }

        // ****************************


        public static bool
        Ask(this Form form, string message) {
            return Ask(form, message, form.Text);
        }

        public static bool
        Ask(this Form form, string message, string title) {

            var result = MessageBox.Show(form, message, title, MessageBoxButtons.YesNo);

            var ret = (result == DialogResult.Yes);

            return ret;
        }

        public static void
        Tell(this Form form, string message) {
            Tell(form, message, form.Text);
        }

        public static void
        Tell(this Form form, string message, string title) {

            MessageBox.Show(form, message, title);

        }


        // ****************************

        /// <summary>
        /// Shows the form, restores its state if its minimized, activates it,
        /// and optionally brings it to the front and selects the first control.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="to_front"></param>
        /// <param name="select_first"></param>
        public static void Activate(this Form form, bool to_front, bool select_first) {

            form.Restore(false);

            if (!form.Visible)
                form.Visible = true;

            form.Activate();

            if (to_front)
                form.ForReallyReallyRealsGiveFocusToTheForm();

            if (select_first)
                form.SelectNextControl(null, true, true, true, true);

        }

        public static void ForReallyReallyRealsGiveFocusToTheForm(this Form form) {

            MethodInvoker mi = null;

            int ii = 0;

            mi = () =>
            {
                if (++ii == 3)
                    return;
                form.BringToFrontEx();
                form.To_Fore();

                form.Force_Handle();
                form.BeginInvoke(mi);
            };

            mi();

        }

        static void BringToFrontEx(this Form form) {

            var top_most = form.TopMost;

            form.TopMost = true;
            try {
                form.BringToFront();

                Native_Methods.SetForegroundWindow(form.Handle);
            }
            finally {
                form.TopMost = top_most;
            }

        }

        public static void To_Fore(this Form form) {

            // AMEN, BROTHER! - 2010-01-26

            form.Restore(false);

            Native_Methods.ForceForegroundWindow(form.Handle);

            // form.Activate();

            form.Focus();

            Native_Methods.SetForegroundWindow(form.Handle);


        }

        public static void Restore(this Form form, bool force) {

            if (!force && form.WindowState != FormWindowState.Minimized)
                return;

            Native_Methods.SendMessage(form.Handle, Native_Const.WM_SYSCOMMAND, Native_Const.SC_RESTORE, 0);

        }



        // ****************************



        static public Bitmap Shift(this Bitmap bmp, int amount) {

            //var temp = new Bitmap(bmp.Width + amount, bmp.Height + amount);
            //
            int resize = Math.Max(amount, amount);

            var temp = new Bitmap(bmp.Width + resize, bmp.Height + resize);

            using (var g1 = Graphics.FromImage(temp)) {


                g1.TranslateTransform((float)amount, (float)amount);
                g1.DrawImage(bmp, Point.Empty);

                return temp;

            }



        }

        static public void Make_Transparent(this Bitmap bmp) {

            if (bmp != null)
                bmp.MakeTransparent(bmp.GetPixel(1, 1));

        }

        public static Color Add(this Color col, int r, int g, int b) {

            uint int_col = (uint)col.ToArgb();

            uint a1 = int_col >> 24;
            uint r1 = (uint)(int_col << 8) >> 24; // (int_col >> 16) && 0xF
            uint g1 = (uint)(int_col << 16) >> 24;
            uint b1 = (uint)(int_col << 24) >> 24;

            Func<uint, int, int> get = (_c1, _c) => (int)Math.Max(0, Math.Min(255, _c1 + _c));

            Color ret = Color.FromArgb((int)a1, get(r1, r), get(g1, g), get(b1, b));

            return ret;

        }
        public static Color Darken(this Color col, int amount) {

            return col.Add(-amount, -amount, -amount);

        }


        // ****************************

        public static DateTime DateTimeV(this Cell cell) {
            var ret = cell.Value.ToDateTime();
            return ret;
        }
        public static String StringV(this Cell cell) {
            var ret = cell.Value.ToString();
            return ret;
        }
        public static Int32 Int32V(this Cell cell) {
            var ret = cell.Value.ToInt32();
            return ret;
        }
        public static Decimal DecimalV(this Cell cell) {
            var ret = cell.Value.ToDecimal();
            return ret;
        }
        public static Double DoubleV(this Cell cell) {
            var ret = cell.Value.ToDouble();
            return ret;
        }



        public static void Align(this Control ctrl, Direction_LURD edge, params Control[] ctrls) {

            throw new NotImplementedException();

        }

        public static void Make_Same_Size(this Control ctrl, bool ww, bool hh, params Control[] ctrls) {

            throw new NotImplementedException();

        }


        public static void Set_Value_Type(this DataGridViewColumn col, Type type) {
            bool maybe_set_format = false;
            Set_Value_Type(col, type, maybe_set_format);
        }
        static public void
        Set_Value_Type(this DataGridViewColumn col, Type type, bool maybe_set_format) {
            col.ValueType = type;
            col.CellTemplate.ValueType = type;

            if (maybe_set_format) {
                if (type == typeof(Decimal) || type == typeof(Double))
                    col.Set_Format("F2");
                if (type == typeof(DateTime))
                    col.Set_Format("dd/MM/yyyy");
            }

        }

        static public void
        Set_Style(this DataGridViewColumn col, Action<DataGridViewCellStyle> act) {
            act(col.DefaultCellStyle);
            act(col.CellTemplate.Style);
        }

        static public void
        Set_Alignment(this DataGridViewColumn col, DataGridViewContentAlignment align) {

            col.Set_Style(style => style.Alignment = align);
            col.HeaderCell.Style.Alignment = align;

        }

        static public void
        Set_Format(this DataGridViewColumn col, string format) {
            col.DefaultCellStyle.Format = format;
            col.CellTemplate.Style.Format = format;
        }

        static public void
        Clip_Bitmap(this Bitmap bmp, Rectangle clipRect) {

            using (var g = Graphics.FromImage(bmp)) {
                g.DrawImageUnscaled(bmp, clipRect);
            }

        }

        public static void Begin_Invoke(this Control ctrl, Action act) {

            ctrl.BeginInvoke(act);

        }


        /// <summary>
        /// Returned value is positive if the wheel was rolled TOWARD the
        /// user and negative if rolled AWAY from the user.
        /// To get the number of discrete sub-rotations, divide by 40 and take
        /// absolute value.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int Translate_Mouse_Wheel(this Message m) {

            if (m.Msg != Native_Const.WM_MOUSEWHEEL)
                true.tift();

            int delta = B.HiWord(m.WParam);

            int scroll = -40 * (delta / 120);

            return scroll;

        }

        public static bool Disposed_Ing(this Control ctrl) {
            return ctrl.IsDisposed || ctrl.Disposing;
        }

        public static bool ContainsKey(this Keys keyData, Keys keyCode) {
            return (keyData & Keys.KeyCode) == keyCode;
        }

        public static bool Is_Selectable(this Control ctrl) {
            if (ctrl.Disposing || ctrl.IsDisposed)
                return false;
            if (!ctrl.CanFocus || !ctrl.CanSelect)
                return false;
            if (!ctrl.Enabled)
                return false;
            if (!ctrl.Visible)
                return false;
            return true;
        }

        public static Control Active_Child(this ContainerControl ctrl) {

            Control ret = null;

            while (ctrl != null && ctrl.ActiveControl != null) {
                ret = ctrl.ActiveControl;
                ctrl = ret as ContainerControl;
            }

            return ret;

        }

        public static Point move(this Control ctrl, int xx, int yy) {
            bool absolute = false;
            return move(ctrl, xx, yy, absolute);
        }
        public static Point move(this Control ctrl, int? xx, int? yy, bool absolute) {

            xx = xx ?? (absolute ? ctrl.Left : 0);
            yy = yy ?? (absolute ? ctrl.Top : 0);

            var ret = absolute ? new Point(xx.Value, yy.Value)
                               : ctrl.Location.Translate(xx.Value, yy.Value);

            return ctrl.Location = ret;

        }

        public static Size resize(this Control ctrl, int ww, int hh) {
            bool absolute = false;
            return resize(ctrl, ww, hh, absolute);
        }

        public static Size resize(this Control ctrl, int? ww, int? hh, bool absolute) {

            ww = (absolute ? ww : ctrl.Width + ww) ?? ctrl.Width;
            hh = (absolute ? hh : ctrl.Height + hh) ?? ctrl.Height;

            var ret = new Size(ww.Value, hh.Value);

            return ctrl.Size = ret;

        }


        public static Point Center_String(this Control ctrl, string str) {



            using (var g = ctrl.CreateGraphics()) {

                return T.Center_String(g, ctrl.Font, ctrl.ClientRectangle, str);

            }


        }

        public static string Ellipsis(this Control ctrl, string str) {
            return Ellipsis(ctrl, str, false);
        }


        public static string Ellipsis(this Control ctrl, string str, bool left) {

            var font = ctrl.Font;

            int width = ctrl.Width;

            using (var g = ctrl.CreateGraphics()) {
                return T.Ellipsis(g, str, font, width, GraphicsUnit.Pixel, left);
            }
        }



        public static void InvokeOrNot<TControl>(
            this TControl ctrl, bool force_handle, bool begin_invoke, Action action)
        where TControl : Control {

            if (ctrl == null)
                return;

            if (ctrl.IsDisposed)
                return;

            if (ctrl.InvokeRequired) {

                if (force_handle && !ctrl.IsHandleCreated)
                    ctrl.Force_Handle();

                if (begin_invoke)
                    ctrl.BeginInvoke((MethodInvoker)(() => action()));
                else
                    ctrl.Invoke((MethodInvoker)(() => action()));

            }
            else {
                action();
            }

        }

        public static bool Select_Next_Safe(this Control ctrl, bool forward) {

            var parent = ctrl.FindForm();

            if (parent == null)
                return false;

            var ret = parent.SelectNextControl(ctrl, forward, true, true, true);

            return ret;

        }

        public static bool Select_Focus(this TextBoxBase tb, bool select_all) {

            bool ret = tb.Select_Focus();
            if (!ret)
                return false;

            if (select_all)
                tb.SelectAll();
            return ret;
        }

        public static bool Select_Focus(this Control ctrl) {

            ctrl.Select();
            return ctrl.Focus();

        }



        public static IEnumerable<Control>
        All_Children(this Control ctrl) {

            var ret = new[] { ctrl }
                      .Explode2(_ctrl => _ctrl.Controls
                                              .Cast<Control>());

            //var ret = FlatMap<Control>(
            //            ctrl.Controls,
            //            (_) => true,
            //            (_ctrl) => (_ctrl as Control).Controls,
            //            (_ctrl) => _ctrl as Control);

            return ret;

            // return ret.ToArray();


        }



        public static void Force_Handle(this Control ctrl) {

            if (ctrl.IsDisposed)
                return;

            IntPtr handle = ctrl.Handle;

            string temp = handle.ToString();

            H.Void(temp);

            if (ctrl.Parent != null)
                ctrl.Parent.Force_Handle();

        }

        /// <summary> Flawed
        /// </summary>
        static public bool IsTypeable(this Keys keycode) {

            char temp;
            if (Char.TryParse(keycode.Get_String(), out temp))
                return true;

            if (Keys.NumPad0 <= keycode && keycode <= Keys.NumPad9)
                return true;

            if (Keys.D0 <= keycode && keycode <= Keys.D9)
                return true;

            return false;
        }



        static public void Center_To(this Control source,
                                          Control target,
                                          bool horizontal,
                                          bool vertical) {

            Point p1 = source.ClientRectangle.Center();
            p1 = source.PointToScreen(p1);

            Point p2 = target.ClientRectangle.Center();
            p2 = target.PointToScreen(p2);

            int x = target.Location.X;
            int y = target.Location.Y;

            if (horizontal) {

                int h_offset = p1.X - p2.X;
                x += h_offset;
            }

            if (vertical) {

                int v_offset = p1.Y - p2.Y;
                y += v_offset;
            }

            Point loc = new Point(x, y);
            target.Location = loc;
        }

        static public void SetTextIfEmpty(this TextBoxBase tb, string value) {

            if (String.IsNullOrEmpty(tb.Text))
                tb.Text = value;
        }

        static public void
        SetTextIfEmpty(this TextBoxBase[] tbs, string value) {

            foreach (var tb in tbs) {
                SetTextIfEmpty(tb, value);
            }

        }

        static public void
        Set_Text(this TextBoxBase[] tbs, string value) {

            foreach (var tb in tbs) {
                tb.Text = value;
            }

        }

        static void Get_Measurements(this Rectangle rect,
            out int left, out int top, out int right, out int bottom,
            out int width, out int height) {

            left = rect.Left;
            top = rect.Top;
            height = rect.Height;
            bottom = rect.Bottom;
            right = rect.Right;
            width = rect.Width;
        }

        static public Action Lock_To_Parent(this Control ctrl, Direction_LURD direction) {

            var dirs = ((int)direction).List_Of_Bits();

            Action act1 = null, act2;
            EventHandler handler1 = null, handler2;

            var lock_to = ctrl.Parent;

            foreach (var dir in dirs) {

                var dir_1 = (Direction_LURD)(1 << dir);
                Lock_To(ctrl, lock_to, dir_1, dir_1, out handler2, out act2);

                handler1 += handler2;
                act1 += act2;

            }

            lock_to.Resize += handler1;
            lock_to.Move += handler1;

            return act1;
        }


        static public Action Lock_To(this Control ctrl, Control lock_to, Direction_LURD edge_1, Direction_LURD edge_2) {

            Action act1 = null;
            EventHandler handler1 = null;
            Lock_To(ctrl, lock_to, edge_1, edge_2, out handler1, out act1);

            lock_to.Resize += handler1;
            lock_to.Move += handler1;

            return act1;

        }

        /// <summary> Ensures the control keeps its current absolute position
        /// relative to the other control's borders.
        /// 
        /// Returns an action which can be used to unlock the control
        /// </summary>
        static public void Lock_To(this Control ctrl, Control lock_to, Direction_LURD edge_1, Direction_LURD edge_2,
            out EventHandler handler, out Action unsubscriber) {

            (B.Hamming_Weight((int)edge_1) == 1).tiff();
            (B.Hamming_Weight((int)edge_2) == 1).tiff();

            ctrl.tifn();
            lock_to.tifn();

            ctrl.Parent.tifn();

            (ctrl.Parent == lock_to || (ctrl.Parent == lock_to.Parent)).tiff();

            Func<int> distance = () =>
                {
                    var rect1 = ctrl.Bounds_On_Screen();
                    var rect2 = lock_to.Bounds_On_Screen();
                    int ret = Get_Edge_Distance(rect1, rect2, edge_1, edge_2);
                    return ret;
                };


            var init_distance = distance();

            EventHandler tmp_handler = (_1, _2) =>
            {
                var new_distance = distance();
                if (new_distance == init_distance)
                    return;

                Fix_Distance(ctrl, init_distance - new_distance, edge_1);
                //   lock_to.Invalidate(ctrl.Bounds);
            };

            unsubscriber = () =>
            {
                if (lock_to != null) {
                    lock_to.Resize -= tmp_handler;
                    lock_to.Move -= tmp_handler;
                }
            };

            handler = tmp_handler;
        }

        static public void
        Fix_Distance(Control ctrl1,
                     int diff,
                     Direction_LURD edge1) {

            int xx = edge1.LR() ? diff : 0;
            int yy = edge1.UD() ? diff : 0;

            var target = ctrl1.Bounds.Translate(xx, yy);
            ctrl1.Bounds = target;

        }

        static public bool Is_Parent_Of_Or_Same(this Control parent, Control child) {

            if (child == parent)
                return true;

            if (parent.Controls == null)
                return false;

            foreach (Control sub_parent in parent.Controls)
                if (sub_parent.Is_Parent_Of_Or_Same(child))
                    return true;

            return false;
        }

        static public bool Is_Child_Of_Or_Same(this Control child, Control parent) {

            bool ret = Is_Parent_Of_Or_Same(parent, child);

            return false;
        }

        static public Rectangle Bounds_On_Control(this Control source, Control target) {

            var ret = source.Rectangle_On_Control(target, source.ClientRectangle);

            return ret;
        }

        static public Rectangle Bounds_On_Screen(this Control ctrl) {

            var ret = ctrl.RectangleToScreen(ctrl.ClientRectangle);

            return ret;
        }

        static public bool Is_Under_Mouse(this Control ctrl) {

            var ret = ctrl.Bounds_On_Screen().Contains(Cursor.Position);

            return ret;
        }

        static public Point Get_Cursor_Location(this Control ctrl) {

            Point ret = ctrl.PointToClient(Cursor.Position);

            return ret;
        }


        static public Rectangle Rectangle_On_Control(
              this Control source,
              Control target,
              Rectangle rect) {

            var rect1 = source.RectangleToScreen(rect);
            var ret = target.RectangleToClient(rect1);

            return ret;
        }

        static public Point Point_On_Control(this Control source, Control target, Point pt) {

            var rect1 = source.PointToScreen(pt);
            var ret = target.PointToClient(rect1);

            return ret;
        }

    }
}
#endif

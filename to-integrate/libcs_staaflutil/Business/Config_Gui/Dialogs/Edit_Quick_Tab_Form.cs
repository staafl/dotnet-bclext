using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Common;
using Common.Dialogs;
using Fairweather.Service;
using Standardization;
namespace Config_Gui
{
    public partial class Edit_Quick_Tab_Form : Form_Base
    {
        public Edit_Quick_Tab_Form()
            : base(Form_Kind.Modal_Dialog) {
            InitializeComponent();
            this.Text = "Edit 'Quick Items' Tab";
            dgv.Flat_Style();
            foreach (DataGridViewColumn col in dgv.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            Set_Event_Handlers();

            Refresh_Row_Count(true);

            file_dialog.Filter = "All Files (*.*)|*.*|BMP Files|*.BMP|JPG Files|*.JPG,*.JPEG|TIFF Files|*.TIFF|PNG Files|*.PNG|GIF Files|*.GIF";
            //"Image Files (*.BMP;*.JPG;*.JPEG;*.tiff;*.PNG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*|A|*.A|B|*.B|C|*.C|D|*.D|E|*.E";
            //"*.jpg|*.bmp|*.tiff|*.gif|*.png"; // *.jpeg|
        }

        void Set_Event_Handlers() {
            but_ok.Click += (_1, _2) =>
                        {
                            if (!Validate_Name() || !Validate_Grid())
                                return;
                            DialogResult = DialogResult.OK;
                        };
            but_cancel.Click += (_1, _2) => { DialogResult = DialogResult.Cancel; Try_Close(); };
            tb_name.TextChanged += (_1, _2) => { but_ok.Enabled = !tb_name.Text.strdef().Trim().IsNullOrEmpty(); };
            // dgv.RowPrePaint += (_1, _args) => { _args.PaintParts &= ~DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.Focus; };
            dgv.CellEnter += (_1, _args) =>
            {
                but_browse.Enabled = _args.ColumnIndex == col_img.Index;
                if (dgv.AllowUserToAddRows &&
                    _args.RowIndex == dgv.NewRowIndex &&
                    _args.ColumnIndex != 0) {
                    this.Force_Handle();
                    BeginInvoke((MethodInvoker)(() => dgv.CurrentCell = dgv[0, dgv.NewRowIndex]));
                }
            };
            dgv.CellValueChanged += (_1, _args) =>
            {
                if (_args.ColumnIndex != col_img.Index)
                    return;


                var _cell = dgv[_args.ColumnIndex, _args.RowIndex];
                var _val = _cell.Value.strdef();
                if (Check_Image(ref _val, false)) {
                    // note the possibility of endless looping
                    if (!_cell.Value.Safe_Equals(_val))
                        _cell.Value = _val;
                }
                else {
                    _cell.Value = "";
                }

            };
            dgv.CellParsing += (_1, _args) =>
            {
                if (_args.ColumnIndex != col_img.Index)
                    return;

                _args.ParsingApplied = true;

                var _cell = dgv[_args.ColumnIndex, _args.RowIndex];
                var _val = _args.Value.strdef();

                if (!Check_Image(ref _val, true)) {
                    _val = _cell.Value.strdef();
                    if (!Check_Image(ref _val, false))
                        _val = "";
                }
                _args.Value = _val;

            };
            dgv.RowsAdded += (_1, _2) => Refresh_Row_Count(true);
            dgv.RowsRemoved += (_1, _2) => Refresh_Row_Count(false);

            but_browse.Click += (_1, _2) =>
            {
                var _cell = dgv.CurrentCell;
                if (_cell == null)
                    return;
                var _before = _cell.Value.strdef().Trim();

                if (File.Exists(_before))
                    file_dialog.InitialDirectory = Path.GetDirectoryName(_before);

                var _result = file_dialog.ShowDialog();
                if (!_result.Positive())
                    return;
                var _file = file_dialog.FileName;

                if (!Check_Image(ref _file, true))
                    return;

                _cell.Value = _file;
                dgv.CurrentCell = null;
                Select_And_Highlight(_cell);

            };
            but_up.Click += (_1, _2) => Move_Row(true);
            but_down.Click += (_1, _2) => Move_Row(false);
            but_delete.Click += (_1, _2) =>
            {
                var _cell = dgv.CurrentCell;
                if (_cell == null)
                    return;
                var _row = _cell.OwningRow;
                var _to_del = _row.Index;
                if (dgv.RowCount == 1) {
                    foreach (DataGridViewCell _cell_2 in _row.Cells)
                        _cell_2.Value = null;
                    return;
                }
                if (_to_del == dgv.NewRowIndex)
                    --_to_del;

                dgv.Rows.RemoveAt(_to_del);
            };
        }


        bool b_Refresh_Row_Count = false;
        void Refresh_Row_Count(bool add) {

            if (b_Refresh_Row_Count)
                return;

            b_Refresh_Row_Count = true;
            try {
                this.Force_Handle();

                if (add) {
                    if (dgv.RowCount > cst_max_row_cnt)
                        // hide 'new' row
                        BeginInvoke((MethodInvoker)(() =>
                        dgv.AllowUserToAddRows = false));
                }
                else {
                    if (dgv.RowCount < cst_max_row_cnt)
                        // show 'new' row
                        dgv.AllowUserToAddRows = true;
                }

            }
            finally {
                b_Refresh_Row_Count = false;
            }

        }

        bool Validate_Grid() {

            foreach (DataGridViewRow row in dgv.Rows.Cast<DataGridViewRow>().Take(dgv.RowCount - 1)) {

                if (row.Cells.Cast<DataGridViewCell>().Take(2).Any(_cell => _cell.Value.strdef().Trim().IsNullOrEmpty())) {
                    if (b_loaded)
                        Standard.Warn("One or more rows do not have Product Name or Barcode information.\n\nPlease fill the required cells or delete the rows before proceeding.");
                    return false;
                }
            }

            return true;


        }

        bool Validate_Name() {

            var name = tb_name.Text.strdef();
            if (name.Trim().IsNullOrEmpty()) {
                if (b_loaded)
                    Standard.Warn("You must specify a name for the Tab.");
                return false;
            }
            return true;
        }

        void Select_And_Highlight(DataGridViewCell _cell) {
            dgv.CurrentCell = _cell;

            var _ctrl = (DataGridViewTextBoxEditingControl)dgv.EditingControl;
            if (_ctrl == null)
                return;

            _ctrl.SelectionStart = 0;
            _ctrl.SelectionLength = _ctrl.TextLength;
        }

        void Move_Row(bool up) {
            var cell = dgv.CurrentCell;
            if (cell == null)
                return;

            var row = cell.OwningRow;

            var from = row.Index;

            var to = from + (up ? -1 : +1);
            if (to >= dgv.RowCount - 1 /* mind the new row */ || to < 0)
                return;


            dgv.CurrentCell = null;
            dgv.Rows.RemoveAt(from);
            dgv.Rows.Insert(to, row);
            Select_And_Highlight(cell);

        }

        bool Check_Image(ref string file, bool show) {

            file = file.Trim();

            if (file.IsNullOrEmpty())
                return true;

            if (!File.Exists(file)) {
                if (b_loaded && show)
                    Standard.Warn("The file '{0}' was not found.".spf(file));
                return false;
            }


            if (H.Validate_Image(file))
                return true;

            if (b_loaded && show)
                Standard.Warn("The file '{0}' is not recognized as a valid image file.".spf(file));

            return false;

        }

        readonly OpenFileDialog file_dialog = new OpenFileDialog();

        public Pair<string, List<Quick_Item_Data>>?
        Show_And_Collect(Quick_Tab_File tab_file) {

            tb_name.Text = tab_file.Name;

            b_loaded = false;
            Enter_Grid_Data(tab_file);
            b_loaded = true;
            var result = this.ShowDialog();

            if (!result.Positive())
                return null;

            var data = Get_Grid_Data();

            var dir = tab_file.Dir;
            var pic_dir = dir.Cpath("pics");

            if (!Directory.Exists(pic_dir))
                Directory.CreateDirectory(pic_dir);

            Func<Quick_Item_Data, Quick_Item_Data> map =
            _qid =>
            {
                var _pic_path_1 = _qid.Image_Src;
                var _name = Path.GetFileName(_pic_path_1); // _qid.Name doesn't contain the extension!

                if (_pic_path_1.IsNullOrEmpty())
                    return _qid;

                string _pic_path_2 = "";

                if (File.Exists(_pic_path_1)) {

                    _pic_path_2 = pic_dir.Cpath(_name);

                    if (!H.File_Compare(_pic_path_1, _pic_path_2, null))
                        File.Delete(_pic_path_2);

                    if (!File.Exists(_pic_path_2))
                        File.Copy(_pic_path_1, _pic_path_2);

                }

                return new Quick_Item_Data(_name, _qid.Barcode, _pic_path_2);

            };

            data = data.Select(map).lst();

            return Pair.Make(tb_name.Text, data);

        }

        List<Quick_Item_Data> Get_Grid_Data() {
            var list = new List<Quick_Item_Data>();

            foreach (DataGridViewRow row in dgv.Rows) {
                var cells = row.Cells
                               .Cast<DataGridViewCell>()
                               .Take(3)
                               .Select(_cell => _cell.Value.strdef())
                               .arr();

                if (cells.Take(2).Any(string.IsNullOrEmpty))
                    continue;

                var qld = new Quick_Item_Data(cells[0], cells[1], cells[2]);
                list.Add(qld);


            }

            return list;
        }

        void Enter_Grid_Data(Quick_Tab_File tab_file) {
            List<Quick_Item_Data> list;

            tab_file.Read(out list).tiff();
            foreach (var qld in list) {
                var row = dgv.Rows[dgv.Rows.Add()];
                row.Cells[0].Value = qld.Name;
                row.Cells[1].Value = qld.Barcode;
                row.Cells[2].Value = qld.Image_Src;

            }

            //throw new NotImplementedException();
        }



        bool b_loaded;

        const int cst_max_row_cnt = Quick_Tab_File.cst_max_items_per_tab;



    }
}

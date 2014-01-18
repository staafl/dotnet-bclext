using System;
using System.Windows.Forms;

namespace Common.Controls
{
    public class Validating_Box_Editing_Control : ValidatingBox, IDataGridViewEditingControl
    {
        protected int rowIndex;
        protected DataGridView dataGridView;
        protected bool valueChanged = false;

        protected override void OnTextChanged(EventArgs e) {
            base.OnTextChanged(e);
            if (dataGridView != null)
                dataGridView.NotifyCurrentCellDirty(true);
        }

        public Validating_Box_Editing_Control() : base() { }

        //Interface members
        public Cursor EditingPanelCursor {
            get {
                return Cursors.IBeam;
            }
        }
        public DataGridView EditingControlDataGridView {
            get {
                return dataGridView;
            }
            set {
                dataGridView = value;
            }
        }


        public bool EditingControlValueChanged {
            get {
                return valueChanged;
            }
            set {
                valueChanged = value;
            }
        }
        public int EditingControlRowIndex {
            get { return rowIndex; }
            set { rowIndex = value; }
        }
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey) {
            switch (keyData & Keys.KeyCode) {
            case Keys.Right: {
                    if (SelectionLength != 0 || SelectionStart != Text.Length) {
                        return true;
                    }
                    break;
                }
            case Keys.Left:
                if (!(this.SelectionLength == 0
                      && this.SelectionStart == 0)) {
                    return true;
                }
                break;

            case Keys.Home:
            case Keys.End:
                if (this.SelectionLength != this.ToString().Length) {
                    return true;
                }
                break;

            case Keys.Prior:
            case Keys.Next:
                if (this.valueChanged) {
                    return true;
                }
                break;

            case Keys.Delete:
                if (this.SelectionLength > 0 || this.SelectionStart < this.ToString().Length) {
                    return true;
                }
                break;
            }
            return !dataGridViewWantsInputKey;
        }
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
            this.TextAlign = translateAlignment(dataGridViewCellStyle.Alignment);
        }

        public bool RepositionEditingControlOnValueChange {
            get { return false; }
        }
        public void PrepareEditingControlForEdit(bool selectAll) {
            if (selectAll)
                SelectAll();
            else
                SelectionStart = Text.Length;
        }
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) {
            return this.Text;
        }
        public object EditingControlFormattedValue {
            set {
                this.Text = value.ToString();
                NotifyDataGridViewOfValueChange();
            }
            get {
                return this.Text;
            }

        }
        protected virtual void NotifyDataGridViewOfValueChange() {
            this.valueChanged = true;
            if (this.dataGridView != null) {
                this.dataGridView.NotifyCurrentCellDirty(true);
            }
        }
        static HorizontalAlignment translateAlignment(DataGridViewContentAlignment align) {
            switch (align) {
            case DataGridViewContentAlignment.TopLeft:
            case DataGridViewContentAlignment.MiddleLeft:
            case DataGridViewContentAlignment.BottomLeft:
                return HorizontalAlignment.Left;

            case DataGridViewContentAlignment.TopCenter:
            case DataGridViewContentAlignment.MiddleCenter:
            case DataGridViewContentAlignment.BottomCenter:
                return HorizontalAlignment.Center;

            case DataGridViewContentAlignment.TopRight:
            case DataGridViewContentAlignment.MiddleRight:
            case DataGridViewContentAlignment.BottomRight:
                return HorizontalAlignment.Right;
            }

            throw new ArgumentException("Error: Invalid Content Alignment!");
        }

        void InitializeComponent() {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
    public class ValidatingBoxCell : DataGridViewTextBoxCell
    {
        public ValidatingBoxCell()
            : base() {
        }
        public override Type EditType {
            get {
                return typeof(Validating_Box_Editing_Control);
            }
        }
        public override void InitializeEditingControl(int rowIndex,
                                                      object initialFormattedValue,
                                                      DataGridViewCellStyle dataGridViewCellStyle) {
            Validating_Box_Editing_Control nb_ctrl;
            ValidatingBoxColumn nb_col;
            DataGridViewColumn dgv_col;

            base.InitializeEditingControl(rowIndex,
                                          initialFormattedValue,
                                          dataGridViewCellStyle);

            nb_ctrl = DataGridView.EditingControl as Validating_Box_Editing_Control;


            dgv_col = OwningColumn;   // this.DataGridView.Columns[this.ColumnIndex];
            if (dgv_col is ValidatingBoxColumn) {
                nb_col = dgv_col as ValidatingBoxColumn;

                nb_ctrl.Text = (string)this.Value;
            }
        }
        protected static bool BoolFromTri(DataGridViewTriState tri) {
            return (tri == DataGridViewTriState.True) ? true : false;
        }
    }
    public class ValidatingBoxColumn : DataGridViewColumn
    {
        public ValidatingBoxColumn()
            : base(new ValidatingBoxCell()) {
        }
        static DataGridViewTriState TriBool(bool value) {
            return value ? DataGridViewTriState.True
                         : DataGridViewTriState.False;
        }

        public override DataGridViewCell CellTemplate {
            get { return base.CellTemplate; }
            set {
                if ((value == null) || !(value is ValidatingBoxCell)) {
                    throw new ArgumentException("Invalid cell type, ValidatingBoxColumns can only contain ValidatingBoxCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
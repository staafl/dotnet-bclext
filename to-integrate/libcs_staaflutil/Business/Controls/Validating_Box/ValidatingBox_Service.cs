using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Fairweather.Service;
using Colors = Standardization.Colors;

namespace Common.Controls
{
    public partial class Validating_Box
    {
        /// <summary>
        /// Replaces "" text with the default text.
        /// Replaces invalid text with the default text.
        /// Normalizes valid text by using double.Parse
        /// </summary>
        public void Normalize_Input() {

            if (Text.Replace(".", "").IsNullOrEmpty()) {
                base.Text = Default_Text;
            }
            else {
                double result;

                if (double.TryParse(Text, out result))
                    base.Text = result.ToString(Default_Format);
                else
                    base.Text = Default_Text;
            }
        }

        [DebuggerStepThrough]
        [Obsolete]
        public new void Undo() {
            Text = last_valid_text;
            // Optionally revert ChangedByUser
        }

        [DebuggerStepThrough]
        public override void ResetText() {
            Text = Default_Text;
            Has_User_Typed_Text = false;
        }

        public bool ChangeAlignmentOnEnter { get; set; }

        bool _read_only_mode;

        public bool Read_Only_Mode {
            [DebuggerStepThrough]
            get { return _read_only_mode; }
            [DebuggerStepThrough]
            set {
                if (_read_only_mode == value)
                    return;

                _read_only_mode = value;

                if (_read_only_mode) {

                    this.BackColor = Colors.TextBoxes.ReadOnlyBackGround;
                    ChangeAlignmentOnEnter = false;
                    this.Cursor = Cursors.Arrow;

                }
                else {
                    this.BackColor = Colors.TextBoxes.NormalBackGround;
                    this.Cursor = Cursors.Default;
                }
            }
        }

        [DefaultValue(true)]
        public bool AutoHighlight {
            get;
            set;
        }

    }

}
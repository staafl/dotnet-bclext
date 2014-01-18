using System;
using System.Diagnostics;
using Fairweather.Service;

namespace Common
{
    [DebuggerStepThrough]
    public class ColumnEditingEventArgs : ColumnEventArgs
    {
        static public void OnEditCancelled(EditingCancelledEventArgs e) {

            EditingCancelled.Raise(e.Owner, e);
        }
        static public event EventHandler<EditingCancelledEventArgs> EditingCancelled;

        public ColumnEditingEventArgs(int col_index, int row_index, object old_value, object new_value, object owner)
            : base(col_index, row_index, old_value, new_value) {


            this.owner = owner;
        }

        bool cancel;
        public bool Cancel {
            get { return cancel; }
            set {
                if (cancel)
                    throw new ArgumentException("Editing already cancelled.");

                if (value) {
                    var e = new EditingCancelledEventArgs(this);
                    OnEditCancelled(e);
                }

                cancel = value;
            }
        }

        readonly object owner;

        public class EditingCancelledEventArgs : EventArgs
        {
            readonly object owner;
            public object Owner {
                get { return owner; }
            }

            public ColumnEditingEventArgs EventArgs {
                get { return eventArgs; }
            }
            readonly ColumnEditingEventArgs eventArgs;

            public EditingCancelledEventArgs(ColumnEditingEventArgs e) {
                this.owner = e.owner;
                this.eventArgs = e;
            }


        }
    }
}

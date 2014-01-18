using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Our_Combo_Box : UserControl, ICalculatorTextBox
    {
        readonly public static Size def_min_size = new Size(40, 20);

        protected Our_Combo_Box() {

            InitializeComponent();

            MinimumSize = def_min_size;

            Setup_Shortlist();

            Set_Handlers();

        }

        void Setup_Shortlist() {
            First_Shortlist_Column_Width = 100;
            Minimum_Shortlist_Items = 1;
            Short_List_Alignment = new Rectangle_Vertex(3);

            Minimum_Shortlist_Items = 1;

            short_list.TabStop = false;

            short_list.First_Column_Width = 100;

            short_list.Size = def_min_shlist_size;
            short_list.Name = Name + "_shortList";
        }

        void Set_Handlers() {

            comboBox.TextChanged +=
                (sender, e)
                    => OnTextChanged(e);

            comboBox.DropDown +=
                (sender, e)
                    => BeginInvoke((MethodInvoker)(() => comboBox.DroppedDown = false));

            comboBox.KeyPress +=
                (sender, e)
                    => OnKeyPress(e);


            short_list.SelectedIndexChanged += shortList_SelectedIndexChanged;

            ////
            short_list.AcceptChanges += shortList_AcceptChanges;

            short_list.RejectChanges +=
                (sender, e) =>
                    OnRejectChanges(e);
            //

            short_list.VisibleChanged +=
                (sender, e) =>
                    OnListViewVisibleChanged(e);

        }

        bool b_ProcessTabKey = false;

        protected override bool
        ProcessTabKey(bool forward) {

            if (b_ProcessTabKey) {
                return base.ProcessTabKey(forward);
            }

            var ret = Handle_Changes(false, false, true);

            if (ret == Selection_Result.Already_Selected) {
                Focus();
                return base.ProcessTabKey(forward);
            }

            if (ret == Selection_Result.Empty_Account_OK)
                return base.ProcessTabKey(forward);

            if (ret.Contains(Selection_Result.cst_ok))
                return true;  //base.ProcessTabKey(forward);
            // ODD: using base.ProcessTabKey(forward); seems to return focus to this control

            return true;

        }

        public override Size MinimumSize {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }


        //
    }
}

using System.Windows.Forms;

namespace Common.Controls
{
    public partial class Unselectable_Button : System.Windows.Forms.Button
    {

        public Unselectable_Button() {

            this.SetStyle(ControlStyles.Selectable, false);

            this.UpdateStyles();


        }
    }
}
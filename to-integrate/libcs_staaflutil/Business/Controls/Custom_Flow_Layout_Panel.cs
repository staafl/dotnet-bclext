using System;
using System.Windows.Forms;

namespace Common.Controls
{
    public class Custom_Flow_Layout_Panel : FlowLayoutPanel
    {
         public void Do_Scroll(int amount) {
            
            if (this.VScroll) {
                
                int newval = this.VerticalScroll.Value + amount;

                if (amount > 0)
                    newval = Math.Min(newval, this.VerticalScroll.Maximum);

                else if (amount < 0)
                    newval = Math.Max(newval, this.VerticalScroll.Minimum);

                else 
                    return;

                //thisis required 
                //for some strange reason the value only gets updated
                //every other time
                int oldval = this.VerticalScroll.Value;
                this.VerticalScroll.Value = newval;

                if(this.VerticalScroll.Value == oldval)
                    this.VerticalScroll.Value = newval;


            }

        }
    }
}

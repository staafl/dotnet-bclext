using System;
using System.Windows.Forms;

using Fairweather.Service;

namespace Activation
{
      public partial class Initial_Company_Form
      {

            protected override void
            OnFormClosing(FormClosingEventArgs e) {


                  base.OnFormClosing(e);

                  if (!e.Cancel)
                        Activation_Helpers.Delete_Temp_Dir();

            }

            void verCB_SelectedIndexChanged(object sender, EventArgs e) {

                  groupBox2.Enabled = true;

            }

            void verCB_Leave(object sender, EventArgs e) {

                  if (verCB.SelectedIndex == -1)
                        return;

                  int[] vers = Magic.range(min_ver, max_ver).arr();

                  int version = vers[verCB.SelectedIndex];

                  Begin_Installation_Prep(version);

            }

      }
}
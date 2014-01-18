using System;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    partial class Our_Calendar : UserControl
    {
        Button but_prev_year;
        Button but_prev_month;

        Button but_next_month;
        Button but_next_year;

        const int button_side = 23;
        Size button_size = new Size(button_side, button_side);

        const int cst_button_top_margin = 12;
        const int cst_button_outer_margin = 6;
        const int cst_button_inner_margin = 4;

        static readonly Point[] button_offsets = { //Buttons on the left
                                                   new Point(cst_border + cst_button_outer_margin, 

                                                             cst_button_top_margin), 

                                                   new Point(cst_border + cst_button_outer_margin 
                                                                        + button_side 
                                                                        + cst_button_inner_margin,

                                                             cst_button_top_margin),

                                                   // Buttons on the right
                                                   new Point(- cst_border - cst_button_outer_margin 
                                                                          - button_side 
                                                                          - button_side 
                                                                          - cst_button_inner_margin,

                                                             cst_button_top_margin),

                                                   new Point(- cst_border - cst_button_outer_margin 
                                                                          - button_side,

                                                             cst_button_top_margin), };

        public Size Button_Size {
            get { return button_size; }
            set {
                if (button_size != value) {

                    button_size = value;
                    Set_Button_Sizes();
                }
            }
        }

        void Set_Button_Sizes() {

            act_but_manip_ind((ind, b) => b.Size = button_size);
        }
        void Set_Button_Locations() {

            act_but_manip_ind((ind, b) =>
            {

                Point offset = button_offsets[ind];
                bool left = ind < 2;
                b.Location = this.ClientRectangle
                                 .Vertex(left, true)
                                 .Translate(offset, true);
            });
        }

        Action<Action<int, Button>> act_but_manip_ind;
        Action<Action<Button>> act_but_manip;

        void Set_Button_Delegates() {

            act_but_manip = (act) =>
            {
                act(but_prev_year);
                act(but_prev_month);

                act(but_next_month);
                act(but_next_year);
            };
            act_but_manip_ind = (act) =>
            {
                act(0, but_prev_year);
                act(1, but_prev_month);

                act(2, but_next_month);
                act(3, but_next_year);
            };

        }
        partial void Create_Buttons() {

            Set_Button_Delegates();

            but_prev_year = new Our_Button();
            but_prev_month = new Our_Button();

            but_next_month = new Our_Button();
            but_next_year = new Our_Button();

            Set_Button_Sizes();
            Set_Button_Locations();

            act_but_manip(b => b.FlatStyle = FlatStyle.Flat);
            act_but_manip(b => b.Visible = true);
            act_but_manip(b => this.Controls.Add(b));
            act_but_manip(b => b.MouseDown += b_MouseDown);
            act_but_manip(b => b.Enter += b_Enter);
            act_but_manip_ind((id, b) =>
            {
                switch (id) {
                case 0:
                    b.Tag = -12;
                    return;
                case 1:
                    b.Tag = -1;
                    return;
                case 2:
                    b.Tag = 1;
                    return;
                case 3:
                    b.Tag = 12;
                    return;
                }
            });
        }
        Label label = new Label();
        void b_Enter(object sender, EventArgs e) {

            this.Controls.Add(label);
            label.Focus();
        }

        void b_MouseDown(object sender, MouseEventArgs e) {

            Button but = sender as Button;
            Value = Value.AddMonths((int)but.Tag);
        }
    }
}
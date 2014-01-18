using System;
using System.Windows.Forms;

using Fairweather.Service;
using Common.Dialogs;

namespace Common.Controls
{

    public partial class Advanced_Combo_Box : Our_Combo_Box, IQuick_Search_Form_Host
    {
        public Advanced_Combo_Box() {

            this.CharacterCasing = CharacterCasing.Upper;
            this.comboBox.DropDown += (_1, _2) => QSF_Visible = !QSF_Visible;
            this.QSF_Orientation = new Rectangle_Vertex(2);
            //var border = Border.Create(this);
            //border.Border_Color = Color.Black;
        }

        static readonly Set<Pair<Record_Type, string>>
        dict_restricted_accs = new Set<Pair<Record_Type, string>>();

        public static Set<Pair<Record_Type, string>> Restricted_Accs {
            get {
                return dict_restricted_accs;
            }
        }

        public override Record_Type Record_Type {
            get;
            protected set;
        }


        public void Set_Alignment(Rectangle_Vertex rv) {
            this.Short_List_Alignment =
            this.QSF_Orientation = rv;

        }

        protected override void OnLoad(EventArgs e) {

            base.OnLoad(e);

            this.Force_Handle();

            /*       small-time hack        */

        }

        protected override void OnParentChanged(EventArgs e) {

            base.OnParentChanged(e);

            m_border.Try_Dispose();
            m_border = new Moving_Border(this);

        }


        // ****************************


        public event Handler_RO<string> New_Button_Clicked;

#pragma warning disable // never used
        public event EventHandler<EventArgs> Search_Button_Clicked;

        public event Handler_RO<string> New_Form_Shown;
        public event Handler_RO<string> New_Form_Hidden;
#pragma warning restore


        public event EventHandler<EventArgs> Search_Form_Shown;
        public event EventHandler<EventArgs> Search_Form_Hidden;


        // ****************************



        public void Show_Search_Dialog() {

            Search_Form_Shown.Raise(this);

            qsf.Visible = false;
            var frm = this.FindForm();
            frm.Refresh();

            using (this.FindForm().Dim_Form())
            using (var srch_form = new Search_Form(Record_Type, Data.SDR)) {

                srch_form.Activate();
                srch_form.ShowDialog(this.Parent);


                var null_result = srch_form.Result;

                if (null_result != null)
                    (this as IQuick_Search_Form_Host).Collect_QSF_Result(null_result.Value, this.m_mode);


            }

            Search_Form_Hidden.Raise(this);


        }





        protected override void Dispose(bool disposing) {

            if (disposing)
                qsf.Try_Dispose();

            base.Dispose(disposing);
        }

        void InitializeComponent() {
            this.SuspendLayout();
            // 
            // Our_Advanced_Combo_Box
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.MinimumSize = new System.Drawing.Size(80, 21);
            this.Name = "Our_Advanced_Combo_Box";
            this.ResumeLayout(false);

        }

    }
}

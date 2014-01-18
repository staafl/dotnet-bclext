using System;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{

    class Fake_Header : UserControl
    {
        public Fake_Header() {
            InitializeComponent();
            SetEventHandlers();
        }

        void SetEventHandlers() {

            label1.TextChanged += label1_TextChanged;
        }

        void label1_TextChanged(object sender, EventArgs e) {

            using (var g = this.CreateGraphics())
                label1.Width += (int)g.MeasureString(label1.Text, label1.Font).Width;
        }

        public Font LabelFont {
            get { return this.label1.Font; }
            set { label1.Font = value; }
        }
        HorizontalAlignment lab_align;
        public HorizontalAlignment LabelAlignment {
            get { return lab_align; }
            set {
                if (lab_align != value) {
                    SetAlign(value);
                }
            }
        }
        public string LabelText {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public override Color BackColor {
            get {
                return base.BackColor;
            }
            set {
                label1.BackColor = value;
                base.BackColor = value;
            }
        }

        const int left_offset = 10;
        const int right_offset = 10;
        GroupBox groupBox1;
        const int vert_offset = 0;
        void SetAlign(HorizontalAlignment align) {

            if (align == HorizontalAlignment.Left)
                label1.Location = new Point(left_offset, vert_offset);

            else if (align == HorizontalAlignment.Center)
                label1.Location = new Point(this.Bounds.Center().LeftFromCenter(label1.Width),
                                            vert_offset);

            else if (align == HorizontalAlignment.Right)
                label1.Location = new Point(this.Bounds.Right - right_offset,
                                            vert_offset);

            lab_align = align;

        }

        #region Designer
        Label label1;

        void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(147, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 118);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // FauxHeader
            // 
            this.Controls.Add(this.groupBox1);
            this.Name = "FauxHeader";
            this.Size = new System.Drawing.Size(370, 116);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }

}
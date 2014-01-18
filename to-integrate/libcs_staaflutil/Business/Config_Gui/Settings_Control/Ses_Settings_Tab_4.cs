
using System.Collections.Generic;
using System.Windows.Forms;
using Common.Controls;
using DTA;
using Fairweather.Service;

namespace Config_Gui
{
    public partial class Ses_Settings_Tab_4 : DTA_Tab
    {
        public Ses_Settings_Tab_4() {

            InitializeComponent();


        }

        public override Dictionary<Control, Ini_Field> Get_Fields() {
            return new Dictionary<Control, Ini_Field>
            {
                {nupd_id, DTA_Fields.TE_max_details_history_length},
            };
        }

        internal Panel panel1;
        internal Label label33;
        internal NumericUpDown nupd_id;
        internal Button but_advanced;
        internal Button but_edit;

        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }




        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.but_edit = new System.Windows.Forms.Button();
            this.nupd_id = new System.Windows.Forms.NumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.but_advanced = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_id)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.but_advanced);
            this.panel1.Controls.Add(this.but_edit);
            this.panel1.Controls.Add(this.nupd_id);
            this.panel1.Controls.Add(this.label33);
            this.panel1.Location = new System.Drawing.Point(10, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(351, 140);
            this.panel1.TabIndex = 34;
            // 
            // but_edit
            // 
            this.but_edit.Location = new System.Drawing.Point(266, 32);
            this.but_edit.Name = "but_edit";
            this.but_edit.Size = new System.Drawing.Size(75, 23);
            this.but_edit.TabIndex = 50;
            this.but_edit.Text = "Edit...";
            this.but_edit.UseVisualStyleBackColor = true;
            // 
            // nupd_id
            // 
            this.nupd_id.Location = new System.Drawing.Point(293, 6);
            this.nupd_id.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nupd_id.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupd_id.Name = "nupd_id";
            this.nupd_id.Size = new System.Drawing.Size(48, 20);
            this.nupd_id.TabIndex = 10;
            this.nupd_id.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(3, 10);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(170, 12);
            this.label33.TabIndex = 30;
            this.label33.Text = "Maximum Entries in Details History:";
            // 
            // but_advanced
            // 
            this.but_advanced.Location = new System.Drawing.Point(265, 106);
            this.but_advanced.Name = "but_advanced";
            this.but_advanced.Size = new System.Drawing.Size(75, 23);
            this.but_advanced.TabIndex = 50;
            this.but_advanced.Text = "Advanced...";
            this.but_advanced.UseVisualStyleBackColor = true;
            // Settings_Tab_4
            // 
            this.Controls.Add(this.panel1);
            this.Name = "Settings_Tab_4";
            this.Size = new System.Drawing.Size(376, 156);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nupd_id)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion




    }
}


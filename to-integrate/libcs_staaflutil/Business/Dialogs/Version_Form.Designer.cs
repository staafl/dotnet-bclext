using System;
using System.Windows.Forms;

using Common;
using Common.Controls;
using Common.Dialogs;

namespace Common.Dialogs
{
    partial class Version_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lab_version_1 = new Common.Controls.Text_Label();
            this.lab_version_2 = new Common.Controls.Text_Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lab_version_3 = new Common.Controls.Text_Label();
            this.lab_version_4 = new Common.Controls.Text_Label();
            this.lab_version_5 = new Common.Controls.Text_Label();
            this.lab_version_6 = new Common.Controls.Text_Label();
            this.lab_version_8 = new Common.Controls.Text_Label();
            this.but_close = new System.Windows.Forms.Button();
            this.lab_version_7 = new Common.Controls.Text_Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.sitdtaTB = new Common.Controls.Text_Label();
            this.sgedtaTB = new Common.Controls.Text_Label();
            this.label10 = new System.Windows.Forms.Label();
            this.but_save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "SIT.EXE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "CSV.DLL";
            // 
            // lab_version_1
            // 
            this.lab_version_1.AutoEllipsis = true;
            this.lab_version_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_version_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_version_1.Location = new System.Drawing.Point(138, 10);
            this.lab_version_1.Name = "lab_version_1";
            this.lab_version_1.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_version_1.Size = new System.Drawing.Size(100, 21);
            this.lab_version_1.TabIndex = 2;
            this.lab_version_1.UseMnemonic = false;
            // 
            // lab_version_2
            // 
            this.lab_version_2.AutoEllipsis = true;
            this.lab_version_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_version_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_version_2.Location = new System.Drawing.Point(138, 36);
            this.lab_version_2.Name = "lab_version_2";
            this.lab_version_2.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_version_2.Size = new System.Drawing.Size(100, 21);
            this.lab_version_2.TabIndex = 3;
            this.lab_version_2.UseMnemonic = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "CRYPT.EXE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "CODEGENGUI.EXE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "CODEGEN.EXE";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "SITGUI.EXE";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "SITCFG.EXE";
            // 
            // lab_version_3
            // 
            this.lab_version_3.AutoEllipsis = true;
            this.lab_version_3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_version_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_version_3.Location = new System.Drawing.Point(138, 88);
            this.lab_version_3.Name = "lab_version_3";
            this.lab_version_3.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_version_3.Size = new System.Drawing.Size(100, 21);
            this.lab_version_3.TabIndex = 10;
            this.lab_version_3.UseMnemonic = false;
            // 
            // lab_version_4
            // 
            this.lab_version_4.AutoEllipsis = true;
            this.lab_version_4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_version_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_version_4.Location = new System.Drawing.Point(138, 62);
            this.lab_version_4.Name = "lab_version_4";
            this.lab_version_4.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_version_4.Size = new System.Drawing.Size(100, 21);
            this.lab_version_4.TabIndex = 9;
            this.lab_version_4.UseMnemonic = false;
            // 
            // lab_version_5
            // 
            this.lab_version_5.AutoEllipsis = true;
            this.lab_version_5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_version_5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_version_5.Location = new System.Drawing.Point(138, 166);
            this.lab_version_5.Name = "lab_version_5";
            this.lab_version_5.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_version_5.Size = new System.Drawing.Size(100, 21);
            this.lab_version_5.TabIndex = 12;
            this.lab_version_5.UseMnemonic = false;
            // 
            // lab_version_6
            // 
            this.lab_version_6.AutoEllipsis = true;
            this.lab_version_6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_version_6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_version_6.Location = new System.Drawing.Point(138, 140);
            this.lab_version_6.Name = "lab_version_6";
            this.lab_version_6.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_version_6.Size = new System.Drawing.Size(100, 21);
            this.lab_version_6.TabIndex = 11;
            this.lab_version_6.UseMnemonic = false;
            // 
            // lab_version_8
            // 
            this.lab_version_8.AutoEllipsis = true;
            this.lab_version_8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_version_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_version_8.Location = new System.Drawing.Point(138, 191);
            this.lab_version_8.Name = "lab_version_8";
            this.lab_version_8.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_version_8.Size = new System.Drawing.Size(100, 21);
            this.lab_version_8.TabIndex = 13;
            this.lab_version_8.UseMnemonic = false;
            // 
            // but_close
            // 
            this.but_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.but_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_close.Location = new System.Drawing.Point(163, 285);
            this.but_close.Name = "but_close";
            this.but_close.Size = new System.Drawing.Size(75, 21);
            this.but_close.TabIndex = 0;
            this.but_close.Text = "Close";
            // 
            // lab_version_7
            // 
            this.lab_version_7.AutoEllipsis = true;
            this.lab_version_7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_version_7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_version_7.Location = new System.Drawing.Point(138, 114);
            this.lab_version_7.Name = "lab_version_7";
            this.lab_version_7.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_version_7.Size = new System.Drawing.Size(100, 21);
            this.lab_version_7.TabIndex = 16;
            this.lab_version_7.UseMnemonic = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "SDO.DLL";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 221);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "SIT.DTA";
            // 
            // sitdtaTB
            // 
            this.sitdtaTB.AutoEllipsis = true;
            this.sitdtaTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.sitdtaTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sitdtaTB.Location = new System.Drawing.Point(138, 217);
            this.sitdtaTB.Name = "sitdtaTB";
            this.sitdtaTB.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.sitdtaTB.Size = new System.Drawing.Size(100, 21);
            this.sitdtaTB.TabIndex = 18;
            this.sitdtaTB.UseMnemonic = false;
            // 
            // sgedtaTB
            // 
            this.sgedtaTB.AutoEllipsis = true;
            this.sgedtaTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.sgedtaTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sgedtaTB.Location = new System.Drawing.Point(138, 242);
            this.sgedtaTB.Name = "sgedtaTB";
            this.sgedtaTB.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.sgedtaTB.Size = new System.Drawing.Size(100, 21);
            this.sgedtaTB.TabIndex = 20;
            this.sgedtaTB.UseMnemonic = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 246);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "SGE.DTA";
            // 
            // but_save
            // 
            this.but_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.but_save.Location = new System.Drawing.Point(10, 285);
            this.but_save.Name = "but_save";
            this.but_save.Size = new System.Drawing.Size(75, 21);
            this.but_save.TabIndex = 1;
            this.but_save.Text = "Write...";
            this.but_save.Click += new System.EventHandler(this.but_save_Click);
            // 
            // VersionForm
            // 
            this.AcceptButton = this.but_close;
            this.CancelButton = this.but_close;
            this.ClientSize = new System.Drawing.Size(247, 315);
            this.ControlBox = false;
            this.Controls.Add(this.but_close);
            this.Controls.Add(this.but_save);
            this.Controls.Add(this.sgedtaTB);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.sitdtaTB);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lab_version_7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lab_version_8);
            this.Controls.Add(this.lab_version_5);
            this.Controls.Add(this.lab_version_6);
            this.Controls.Add(this.lab_version_3);
            this.Controls.Add(this.lab_version_4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lab_version_1);
            this.Controls.Add(this.lab_version_2);
            this.MaximumSize = new System.Drawing.Size(253, 340);
            this.MinimumSize = new System.Drawing.Size(253, 340);
            this.Name = "VersionForm";
            this.Text = "Component File Versions";
            this.Controls.SetChildIndex(this.lab_version_2, 0);
            this.Controls.SetChildIndex(this.lab_version_1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.lab_version_4, 0);
            this.Controls.SetChildIndex(this.lab_version_3, 0);
            this.Controls.SetChildIndex(this.lab_version_6, 0);
            this.Controls.SetChildIndex(this.lab_version_5, 0);
            this.Controls.SetChildIndex(this.lab_version_8, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.lab_version_7, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.sitdtaTB, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.sgedtaTB, 0);
            this.Controls.SetChildIndex(this.but_save, 0);
            this.Controls.SetChildIndex(this.but_close, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        Button but_close;
        Button but_save;
        Label label1;
        Label label10;
        Label label2;
        Label label3;
        Label label4;
        Label label5;
        Label label6;
        Label label7;
        Label label8;
        Label label9;
        Text_Label lab_version_1;
        Text_Label lab_version_2;
        Text_Label lab_version_3;
        Text_Label lab_version_4;
        Text_Label lab_version_5;
        Text_Label lab_version_6;
        Text_Label lab_version_7;
        Text_Label lab_version_8;
        Text_Label sgedtaTB;
        Text_Label sitdtaTB;

    }
}

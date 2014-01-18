namespace Activation
{
    using System.Windows.Forms;

    partial class SDO_Activation_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.tb_ser = new System.Windows.Forms.TextBox();
            this.tb_act = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.but_ok = new System.Windows.Forms.Button();
            this.but_cancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_ver = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tb_ser
            // 
            this.tb_ser.BackColor = System.Drawing.Color.White;
            this.tb_ser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ser.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_ser.Location = new System.Drawing.Point(14, 238);
            this.tb_ser.Name = "tb_ser";
            this.tb_ser.Size = new System.Drawing.Size(110, 20);
            this.tb_ser.TabIndex = 0;
            // 
            // tb_act
            // 
            this.tb_act.BackColor = System.Drawing.Color.White;
            this.tb_act.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_act.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_act.Location = new System.Drawing.Point(14, 283);
            this.tb_act.Name = "tb_act";
            this.tb_act.Size = new System.Drawing.Size(110, 20);
            this.tb_act.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 222);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "SDO Serial Number:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 267);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SDO Activation Key:";
            // 
            // but_ok
            // 
            this.but_ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_ok.Enabled = false;
            this.but_ok.Location = new System.Drawing.Point(78, 315);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 4;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            this.but_ok.Click += new System.EventHandler(this.but_ok_Click);
            // 
            // but_cancel
            // 
            this.but_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_cancel.Location = new System.Drawing.Point(157, 315);
            this.but_cancel.Name = "but_cancel";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 5;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            this.but_cancel.Click += new System.EventHandler(this.but_cancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(140, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 39);
            this.label3.TabIndex = 14;
            this.label3.Text = "0845 111 6666\r\n0845 245 0280\r\n01 642 0863";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Sage Version:";
            // 
            // verTB
            // 
            this.tb_ver.BackColor = System.Drawing.Color.LightGray;
            this.tb_ver.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ver.Location = new System.Drawing.Point(14, 192);
            this.tb_ver.Name = "verTB";
            this.tb_ver.ReadOnly = true;
            this.tb_ver.Size = new System.Drawing.Size(74, 20);
            this.tb_ver.TabIndex = 17;
            // 
            // SDO_Activation_Form
            // 
            this.AcceptButton = this.but_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.but_cancel;
            this.ClientSize = new System.Drawing.Size(237, 359);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_ver);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_act);
            this.Controls.Add(this.tb_ser);
            this.MaximumSize = new System.Drawing.Size(243, 384);
            this.MinimumSize = new System.Drawing.Size(243, 384);
            this.Name = "SDO_Activation_Form";
            this.Text = "Sage Interface Tools SDO Registration";
            this.Controls.SetChildIndex(this.tb_ser, 0);
            this.Controls.SetChildIndex(this.tb_act, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.but_ok, 0);
            this.Controls.SetChildIndex(this.but_cancel, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.tb_ver, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        TextBox tb_ser;
        TextBox tb_act;
        Label label1;
        Label label2;
        Button but_ok;
        Button but_cancel;
        Label label4;
        Label label3;
        Label label5;
        TextBox tb_ver;
    }
}
using System;
using System.IO;
using System.Windows.Forms;
using Common;
using Fairweather.Service;
using Standardization;
using Versioning;

namespace Activation
{
    public partial class SDO_Activation_Form : Common.Dialogs.Form_Base
    {

        public SDO_Activation_Form(int ver,
                       Initial_Company_Form parent,
                       string common_folder)
            : base(Form_Kind.Modal_Dialog) {


            InitializeComponent();

            this.label4.Text =
@"Sage Data Objects (SDO) is not registered
and thus, 3rd party integration is not enabled.

To register the SDO, a Serial Number and an 
Activation Key will be issued free of charge. 
Please call Sage Customer Services on one
of the following numbers:

Great Britain:
Northern Ireland:
Republic of Ireland:";

            this.Text = "{0} SDO Registration".spf(Data.Default_Title);
            this.common_folder = common_folder;

            this.tb_ser.MaxLength = 7;
            this.tb_act.MaxLength = 7;


            this.ver = ver;

            tb_ver.Text = Data.Get_Sage_Versions()[ver - 11];

            this.parent = parent;

            this.Result = null;

            H.Select_On_Enter(tb_act, tb_ser);

            this.tb_ser.TextChanged += (_1, _2) => Refresh_OK_Button();
            this.tb_act.TextChanged += (_1, _2) => Refresh_OK_Button();

        }

        SDO_Engine sdo;
        readonly Initial_Company_Form parent;
        readonly int ver;
        readonly string common_folder;

        public SDO_Registration_Result? Result {
            get;
            private set;
        }


        void but_ok_Click(object sender, EventArgs e) {

            string ser = tb_ser.Text;
            string act = tb_act.Text;

            sdo = new SDO_Engine(ver);

            if (ver >= 14)
                Directory.CreateDirectory(common_folder);

            bool temp = sdo.Register(ser, act);

            if (!temp) {

                if (Activation_Helpers.cst_debug)
                    Console.WriteLine("SDORegisterForm: Last Sage Error {0}", sdo.Last_Error_Text);

                var disp = this.Make_Unclosable();
                try {
                    Message_Type.Error.Show(this, "SDO Registration error.");

                    tb_ser.Text = "";
                    tb_act.Text = "";
                    tb_ser.Select_Focus();

                    this.Delay(disp.Dispose, 2);
                }
                catch {
                    disp.Dispose();
                    throw;
                }
                return;

            }

            Result = new SDO_Registration_Result(ser, act, ver);

            Try_Close();

        }

        void but_cancel_Click(object sender, EventArgs e) {

            Result = null;
            Try_Close();

        }

        void Refresh_OK_Button() {

            var ok = (!tb_act.Text.IsNullOrEmpty() && !tb_ser.Text.IsNullOrEmpty());
            but_ok.Enabled = ok;

        }


    }
}

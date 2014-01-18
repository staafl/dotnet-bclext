using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Common;

using Fairweather.Service;
using Standardization;


namespace Activation
{
    public partial class Initial_Company_Form : Common.Dialogs.Company_Registration_Form
    {
        public Initial_Company_Form() {

            InitializeComponent();

            this.Text = Data.Initial_Setup_Title;

            this.Size = cst_def_size;

            // Tweak
            this.Bounds = this.Bounds.Expand(0, 0, 1, 0);

            this.StartPosition = FormStartPosition.Manual;

            SetEventHandlers();

            H.Select_On_Enter(tb_pass1, tb_pass2, tb_path);

        }

        void Begin_Installation_Prep(int version) {

            selected_ver = version;

            sageusr_thread = new Thread(() => Activation_Helpers.Find_sage_usr(selected_ver, out sageusr_path));
            sageusr_thread.IsBackground = true;
            sageusr_thread.Start();

            extract_thread = new Thread(() => Activation_Helpers.Extract_Files_To_Temp(selected_ver));
            extract_thread.IsBackground = true;
            extract_thread.Start();

            Thread.Sleep(25);

        }

        /// <summary>
        /// This function copies the sage.usr file to the provided location
        /// </summary>
        void sageusr_callback(string sparent_user) {

            sageusr_thread.Join();

            if (File.Exists(sageusr_path))
                File.Copy(sageusr_path, sparent_user, true);

        }

        /// <summary>
        /// This function installs Microsoft Visual C++ Redistributable,
        /// extracts the necessary SDO files from the archive
        /// and registers them using Regsvr32
        /// </summary>
        /// <returns></returns>
        Triple<SDO_Installation_Status, int, int> install_sdo_callback() {

            int regsvr_return1;
            int regsvr_return2;

            if (extract_thread.IsAlive)
                extract_thread.Join();

            var sdo_result = Activation_Helpers.Install_SDO(this,
                                                       selected_ver,
                                                       ref b_first,
                                                       out regsvr_return1,
                                                       out regsvr_return2);

            if (cst_debug)
                Console.WriteLine("Activate: regsvr1 = {0} regsvr2 = {1}",
                                  regsvr_return1, regsvr_return2);



            if (sdo_result == SDO_Installation_Status.All_OK)
                installed.Add(selected_ver);

            var ret = Triple.Make(sdo_result, regsvr_return1, regsvr_return2);

            return ret;
        }

        protected override void
        Accept(string password) {

            var is_installed = installed[selected_ver];

            Company_Registration_Result? result;
            bool close;

            Activation_Helpers.Accept
                        (tb_path.Text,
                         tb_user.Text,
                         password,
                         selected_ver,
                         false,
                         is_installed,

                         install_sdo_callback,
                         sageusr_callback,
                         Try_Activate_SDO,

                         out close,
                         out result);

            if (result.HasValue) {

                this.Result = result.Value;

                Standard.Show(Message_Type.Success, "Program successfully activated.");
                Try_Close();

                return;

            }

            if (close)
                Try_Close();


        }

        /// <summary>
        /// This function prompts the user to enter the SDO activation data for the
        /// specified version
        /// </summary>
        bool Try_Activate_SDO(int ver) {

            if (ver <= 13) {

                if (!H.Is_User_Admin()) {

                    Activation_Helpers.Show_Admin_Error();

                    return false;

                }
            }


            SDO_Registration_Result? null_result;


            var common_dir = Activation_Helpers.Get_Sage_Data_Folder(ver);

            using (var sdoform = new SDO_Activation_Form(ver, this, common_dir)) {

                sdoform.ShowDialog(this);

                null_result = sdoform.Result;

            }

            var ret = null_result.HasValue;

            if (ret) {

                var result = null_result.Value;

                set_sdoeng_usr.Add(ver);

                ret = true;

            }


            return ret;
        }

        public void Rollback() {

            // VCRed uninstallation removed - see scraps 000

            for (int ver = min_ver; ver < max_ver; ver++) {

                if (set_sdoeng_usr.Contains(ver))
                    Activation_Helpers.Remove_sdoeng_usr(ver);

                if (set_sdoeng.Contains(ver))
                    Activation_Helpers.Unregister_sdoeng_dll(ver);

                if (set_sgregister.Contains(ver))
                    Activation_Helpers.Unregister_sgregister_dll(ver);

            }
        }
    }
}
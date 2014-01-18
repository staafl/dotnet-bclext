using System;
using System.IO;
using System.Windows.Forms;

using Common;
using DTA;
using Fairweather.Service;

namespace Config_Gui
{
   public class Code : Activation.Program
    {

        static Logging logger;

        
        public static int Run(string[] args) {

            try {
                M.Set_Culture();
                logger = M.Get_Logger(true, true);
                M.Set_Winforms_Exception_Handlers(logger);

                // D.Muted = false;
                // D.Add_Writer(Console.Out);

                Ini_File _;
                M.Get_Ini_File(true, true, out _);

                if (!M.Check_For_Files(true))
                    return (int)Return_Code.Files_Missing;

                Application.SetCompatibleTextRenderingDefault(false);

        

                instance = new Code();
                return Activation.Program.Run_Activation_1();

            }
            catch (FileNotFoundException ex) {

                return M.Top_Level_FNF(ex);

            }
        }

        protected override void Handle_Exception(Exception ex) {

            logger.Log(ex);

        }

        protected override Return_Code Accept() {

            form = new Main_Form();

            if (form.IsDisposed)
                return Return_Code.Other_Error;

            Application.Run(form);
            return Return_Code.All_OK;

        }

        static Main_Form form;


        protected override bool
        Perform_Activation(License_Result license_result,
                           Company_Registration_Result? null_company_result) {

            var ini = Data.Ini_File;

            var helper = new Activation_Helper(ini);

            bool first_company = !helper.Any_Registered_Companies;

            (first_company == null_company_result.HasValue).tiff();

            helper.Activate_Application(license_result, first_company);

            if (first_company) {

                var company_result = null_company_result.Value;

                helper.Register_Company(company_result);

            }



            ini.Write_Data();
            bool ret = true;

            return ret;
        }





    }
}
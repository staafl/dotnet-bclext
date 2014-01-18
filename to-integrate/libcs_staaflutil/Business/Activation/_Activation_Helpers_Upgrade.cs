
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Common;
using DTA;
using Fairweather.Service;
using Versioning;

namespace Activation
{
    public static partial class Activation_Helpers
    {
        static public bool Perform_Upgrade(Activation_Helper helper) {

            var ini = helper.Ini_File;

            string SAGE_USR = "sage.usr";
            string ACCDATA_SAGE_USER = "ACCDATA\\sage.usr";

            bool ok = false;
            var current_company = new Assign_Once<Company_Number>();

            var triples = helper.Companies
                .Select(company =>
                {
                    string company_as_string = company.As_String;

                    // c:/data/sage.usr
                    string usr_path = ini[company_as_string, DTA_Fields.USR_FILE_PATH];

                    // c:/data
                    string usr_dir = Directory.GetParent(usr_path).ToString();

                    // c:/data/sage.usr.bak
                    string backup = usr_dir.Cpath(Constants.SesCfg.Backup_Usr_File);

                    return Triple.Make(new Company_Number(company_as_string), usr_path, backup);
                });


            try {

                foreach (var triple in triples) {

                    var company = triple.First;

                    string usr_path = triple.Second;  // c:/data/sage.usr

                    string backup = triple.Third;     // c:/data/sage.usr.bak

                    if (!File.Exists(usr_path)) {     // If we have a leftover copy

                        if (File.Exists(backup))
                            File.Move(backup, usr_path);

                    }

                    if (File.Exists(usr_path)) {

                        current_company.Assign(company);

                        File.Delete(backup);
                        File.Move(usr_path, backup);

                    }
                }

                // ??? At least one company needs to have had a sage.usr file available
                current_company.Assigned.tiff();

                string path;
                string user;
                string pass;
                string _usr_path;
                string _usr_parent;

                using (var company_helper = helper.Get_Company_Helper(current_company.Value)) {

                    path = company_helper.Company_Path;
                    user = company_helper.Username;
                    pass = company_helper.Password;
                    _usr_path = company_helper.Company_Sage_Usr;
                    _usr_parent = Directory.GetParent(_usr_path).ToString();

                }

                /*       Since the .usr files are now moved, connecting to SDO        */
                /*       will pop a dialog        */

                var sdo = new SDO_Engine(helper.Version);

                var WS = sdo.WSAdd(Data.Workspace);
                try {
                    WS.Connect(path, user, pass);
                }
                finally {
                    WS.Try_Disconnect();
                }

                string new_file;  // The file which was created by the user 
                string parent = Directory.GetParent(path).ToString(); // c:/data

                /*       Locate new file        */

                new_file = _usr_parent.Cpath(SAGE_USR);               // c:/data/sage.usr


                if (!File.Exists(new_file)) {                         // This is the most probable location
                    new_file = parent.Cpath(SAGE_USR);                // c:/data/sage.usr

                }

                if (!File.Exists(new_file)) {
                    new_file = parent.Cpath(ACCDATA_SAGE_USER);       // c:/data/accdata/sage.usr

                }



                /*       If we haven't found a file, we assume the registration       */
                /*       has failed        */

                ok = File.Exists(new_file);

                if (ok) {

                    /*       Copy the created file to all company directories        */

                    File.Move(new_file, _usr_path);
                    new_file = _usr_path;

                    foreach (var triple in triples) {

                        string usr_path = triple.Second;

                        string backup = triple.Third;

                        File.Delete(backup);

                        if (triple.First == current_company.Value)
                            continue;

                        if (new_file != usr_path)
                            File.Copy(new_file, usr_path);

                    }
                }
            }
            catch (XSage_Conn ex) {
                Logging.Notify(ex);
                ok = false;
            }
            catch (COMException ex) {
                Logging.Notify(ex);
                ok = false;

            }
            catch (IOException ex) {
                Logging.Notify(ex);
                ok = false;
            }

            if (ok)
                return true;

            /*       Rollback        */
            /*       Revert the sage.usr files for each company        */
            /*       using the backups we made in the beginning        */

            var first_usr = new Assign_Once<string>();

            foreach (var triple in triples) {

                string usr_path = triple.Second;

                string backup = triple.Third;

                if (File.Exists(backup)) {

                    // ???
                    if (File.Exists(usr_path))
                        File.Delete(usr_path);

                    first_usr.Assign(usr_path);
                    File.Move(backup, usr_path);
                    File.Delete(backup);
                }
                else
                    if (!File.Exists(usr_path)) {

                        // this clause is in case the user has somehow   
                        // managed to slide two registrations of the same
                        // company past us

                        File.Copy(first_usr.Value, usr_path);

                    }
            }

            return false;
        }
    }
}
using System;
using System.ComponentModel;
using System.IO;

using Common;
using Fairweather.Service;
using Standardization;

namespace Activation
{
    public static partial class Activation_Helpers
    {

        public static void
        Accept(string path,
               string user,
               string pass,

               int ver,
               bool already_activated,
               bool installed,

               Func<Triple<SDO_Installation_Status, int, int>> install_callback,
               Action<string> sageusr_callback,
               Func<int, bool> sdo_callback,

               out bool close,
               out Company_Registration_Result? result) {



            Directory.Exists(path).tiff("path: {0}".spf(path));


            close = false;
            result = null;


            // ****************************


            string parent = "";
            string gparent = "";

            var parent_info = Directory.GetParent(path);

            if (parent_info != null)
                parent = parent_info.FullName;

            if (Directory.Exists(parent)) {

                var gparent_info = Directory.GetParent(parent);

                if (gparent_info != null)
                    gparent = gparent_info.FullName;

            }

            var parent_usr = parent.Cpath("sage.usr");
            var gparent_usr = gparent.Cpath("sage.usr");

            if (!File.Exists(parent_usr) && !File.Exists(gparent_usr))
                sageusr_callback(gparent_usr);


            // ****************************


            if (!installed) {

                var triple = install_callback();

                if (triple.First != SDO_Installation_Status.All_OK) {

                    bool repeat;

                    Handle_SDO_Installation_Status(triple.First,
                                                    triple.Second,
                                                    triple.Third,
                                                    out repeat,
                                                    out close);

                    if (repeat)
                        Accept(path,
                               user,
                               pass,
                               ver,
                               already_activated,
                               installed,

                               install_callback,
                               sageusr_callback,
                               sdo_callback,

                               out close,
                               out result);


                    return;

                }
            }

            // ****************************

            var sdo_act_stat = Activation_Helpers.Get_SDO_Activation_Status(ver, path, user, pass);

            if (sdo_act_stat != SDO_Activation_Status.OK) {

                if (sdo_act_stat != SDO_Activation_Status.Not_Activated) {
                    Handle_SDO_Activation_Status(sdo_act_stat, ver);
                    return;
                }

                if (!sdo_callback(ver))
                    return;

            }


            // ****************************

            
            var sageusr = gparent_usr;

            Shuffle_Usr_File(parent, gparent, parent_usr, gparent_usr, ref sageusr);


            // ****************************


            var creds_stat = Activation_Helpers.Check_User_Creds(ver, path, user, pass);

            bool valid_user = creds_stat == User_Credentials_Status.Valid_User;

            if (!valid_user) {
                Handle_User_Credentials_Status(creds_stat);
                return;

            }


            // ****************************


            string name, period, bank_control;

            var access = new Sage_Access(path, new Credentials(user, pass), ver);

            var ok = Activation_Helpers.Get_Name_Period_Control(
                access,
                out name, out period, out bank_control);

            if (!ok) // error is examined in 'Get_Name_Period_Control'
                return;


            // easy as pie!


            result = new Company_Registration_Result(name, path,
                                                     user, pass,
                                                     period,
                                                     sageusr,
                                                     ver,
                                                     bank_control);


        }


        static void
        Shuffle_Usr_File(string parent,
                         string gparent,
                         string parent_usr,
                         string gparent_usr,
                         ref string sageusr) {

            var tmp = parent.Cpath("ACCDATA\\sage.usr");

            if (File.Exists(tmp))
                H.Overwrite(tmp, parent_usr);

            if (File.Exists(parent_usr)) {

                if (File.Exists(gparent_usr)) {
                    var backup_path = gparent.Cpath("sageusr.bak");
                    H.Overwrite(gparent_usr, backup_path);

                }

                var exe_path = parent.Cpath("sage.exe");

                if (File.Exists(exe_path)) {
                    sageusr = parent_usr;

                }
                else {
                    H.Overwrite(parent_usr, gparent_usr);
                    sageusr = gparent_usr;

                }
            }
        }

        public static void
        Handle_SDO_Installation_Status(SDO_Installation_Status result,
                                      int regsvr_return_code1,
                                      int regsvr_return_code2,
                                      out bool repeat,
                                      out bool close) {

            close = false;
            switch (result) {

                case SDO_Installation_Status.Try_Again:
                    repeat = true;

                    return;

                case SDO_Installation_Status.All_OK:

                    throw new InvalidOperationException();

                case SDO_Installation_Status.User_Clicked_NO:

                    D.wl("Entering shutdown sequence");

                    close = true;
                    repeat = false;

                    return;

                case SDO_Installation_Status.Regsvr32_Error:
                case SDO_Installation_Status.Wrong_Version:
                case SDO_Installation_Status.Unknown_Error:
                case SDO_Installation_Status.Firewall:
                case SDO_Installation_Status.Access_Error:

                    repeat = false;
                    return;

                default:
                    throw new InvalidEnumArgumentException("SDO_Installation_Status");
            }

        }

        public static void
        Handle_SDO_Activation_Status(SDO_Activation_Status reg, int version) {

            switch (reg) {

                case SDO_Activation_Status.Not_Activated: {
                        throw new InvalidOperationException();

                    }
                case SDO_Activation_Status.COM_Exception:
                case SDO_Activation_Status.Unknown_Error: { // (wrong sage version or wrong credentials)
                        Named_Message.Connection_To_The_Sage_Data_Files_Cannot_Be_Established.Show(version);

                        return;
                    }
                case SDO_Activation_Status.Sage_Not_Registered_User_Refuses:
                    Standard.Show(Message_Type.Error, "You need to register Sage in order to use the program.");

                    return;

                default:

                    throw new InvalidEnumArgumentException("SDO_Registration_Status");

            }
        }

        public static void
        Handle_User_Credentials_Status(User_Credentials_Status cred) {

            switch (cred) {

                case User_Credentials_Status.Incorrect_Credentials: {

                        Standard.Show(Message_Type.Error, "The credentials you have entered are not valid.");

                        return;
                    }

                case User_Credentials_Status.Unknown_Error: {
                        Named_Message.An_Unexpected_Error.Show();
                        return;
                    }

                default:
                    throw new InvalidEnumArgumentException("User_Credentials_Status");
            }
        }





    }
}
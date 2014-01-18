using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Fairweather.Service;
using Microsoft.PointOfService;
using Microsoft.Win32;
using System.IO;
namespace Fairweather.Service
{
    public static class Pos_For_Net
    {
        //#if OPOS

        public static List<string> 
        Get_OPOS_Logical_Names(ISynchronizeInvoke obj) {

            var devices = Get_Devices(null);
            var ret = devices.Select(info => info.LogicalNames.FirstOrDefault(info.ServiceObjectName))
                             .OrderBy(_s => _s == simulator ? "" : _s);

            return ret.lst();

        }

        public static bool Pos_For_Net_Present {
            get {
                return Pos_For_Net_Assembly && Is_Pos_For_Net_Installed;
            }
        }

        public static bool Pos_For_Net_Assembly {
            get {

                if (!File.Exists("Microsoft.PointOfService.dll"))
                    return false;

                return true;

                //return H.Test_Assembly("Microsoft.PointOfService.dll");

            }
        }

        public static bool Is_Pos_For_Net_Installed {
            get {
                var reg = Registry.LocalMachine;
                // HKEY_LOCAL_MACHINE\SOFTWARE\POSfor.NET\Setup 
                
                var path = @"SOFTWARE\POSfor.NET\Setup";

                reg = reg.OpenSubKey(path);

                if (reg == null)
                    return false;

                var value = reg.GetValue("ProductVersion");

                if (value == null)
                    return false;

                if (H.Compare_Versions("1.12.1296.00", value.ToString()) < 0)
                    return false;

                return true;
            }
        }

        public static IEnumerable<string> 
        Names(this DeviceInfo info) {

            return info.LogicalNames.Pend(info.ServiceObjectName, true);

        }

        const string simulator = "Microsoft PosPrinter Simulator";

        static bool Is_Valid(DeviceInfo info) {

            return true;
            /*
            if (!info.HardwarePath.IsNullOrEmpty())
                return true;

            if (info.ServiceObjectName == simulator)
                return true;

            return false;*/
        }

        public static List<DeviceInfo> 
        Get_Devices(ISynchronizeInvoke obj) {

            var pos_explorer = new PosExplorer(obj);

            var devices = pos_explorer.GetDevices(DeviceType.PosPrinter);

            var infos = devices.OfType<DeviceInfo>();

            infos = infos.Where(Is_Valid);

            return infos.lst();
        }

    }

}
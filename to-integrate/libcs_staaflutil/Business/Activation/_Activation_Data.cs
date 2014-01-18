using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common;
using Fairweather.Service;

namespace Activation
{
    /* Serial numbers generation and validation */
    public static class Activation_Data
    {
        const int PIN_LENGTH = 16;
        const string HEX_FORMAT = "{0:X2}";

        //{ 0x0D, 0xAB, 0x93, 0x84, 0x71, 0x7D, 0xC1, 0x12, 0xF4, 0x3D }
        static int[] Hex_Digits {
            get {
                //                    "0",  "1",  "2",  "3",  "4",  "5",  "6",  "7",  "8",  "9"  };
                var hex = new int[] { 0x0D, 0xAB, 0x93, 0x84, 0x71, 0x7D, 0xC1, 0x12, 0xF4, 0x3D };
                return hex;
            }
        }

        //{ "0D", "AB", "93", "84", "71", "7D", "C1", "12", "F4", "3D" };
        static string[] Hex_Strings {
            get {
                var ret = Hex_Digits.Select(hex => String.Format(HEX_FORMAT, hex)).ToArray();
                return ret;
            }
        }
        /// <summary> Depends on:
        ///     Windows directory date
        ///     Program directory date
        ///     
        /// "bool request" -> if it is a request key as opposed to activation
        /// key
        /// </summary>
        public static string
        Generate_Request(bool request) {

            string windir = H.Get_Win_Dir();
            string progdir = Environment.CurrentDirectory;

            string ret = "";
            int temp;

            DateTime windate = Directory.GetCreationTimeUtc(windir);
            DateTime progdate = Directory.GetCreationTimeUtc(progdir);

            temp = (windate.Year % 100) + (progdate.Year % 100) + 20;
            if (request)
                temp += 11;

            ret += String.Format("{0:x}", temp).ToUpper();
            ret += "-";

            temp = windate.Month + progdate.Month + 40;
            if (request)
                temp += 44;

            ret += String.Format("{0:x}", temp).ToUpper();
            ret += "-";

            temp = windate.Day + progdate.Day + 30;
            if (request)
                temp += 22;

            ret += String.Format("{0:x}", temp).ToUpper();
            ret += "-";

            temp = windate.Hour + progdate.Hour + 110;      // Removed: + 2 * DateTimeOffset.Now.Offset.Hours;
            if (request)
                temp -= 55;

            ret += String.Format("{0:x}", temp).ToUpper();
            ret += "-";

            temp = windate.Minute + progdate.Minute + 50;
            if (request)
                temp -= 33;

            ret += String.Format("{0:x}", temp).ToUpper();

            return ret;

        }

        public static string
        De_Hyphen(string input) {

            string ret = input.Replace("-", "");

            return ret;

        }

        public static bool
        ValidateKey(string key) {

            License_Data? temp;
            bool ret = ValidateKey(key, false, out temp);

            return ret;

        }

        /// <summary> If "validate_modules" is enabled, 
        /// the result depends on the current time.
        /// 
        /// Otherwise "data" is always null.
        /// </summary>
        public static bool
        ValidateKey(string key, bool validate_modules,
                    out License_Data? data) {

            data = null;

            Regex rx_act = new Regex(@"[A-F0-9]{20}");

            Regex rx_whyp = new Regex(@"(?:[A-F0-9][A-F0-9]-){9}[A-F0-9][A-F0-9]");

            if (rx_whyp.Match(key).Success) {

                key = De_Hyphen(key);

            }

            if (!rx_act.Match(key).Success)
                return false;


            if (key.Length != 20)
                return false;


            {
                string temp_1 = "";

                for (int i = 0; i < key.Length; i += 4)  // XX__XX__XX__XX__XX__
                    temp_1 += key.Substring(i, 2);

                var temp_2 = Generate_Request(false);
                temp_2 = De_Hyphen(temp_2);


                if (temp_1 != temp_2)
                    return false;
            }

            Func<int, int> substring_as_hex =
                (start) =>
                {
                    var substring = key.Substring(start, 2);

                    return Convert.ToInt32(substring, 16);
                };

            int NC1 = substring_as_hex(2);     // Number of companies #1
            int MM = substring_as_hex(4);      // Month
            int CE = substring_as_hex(6);      // Record Module
            int DD = substring_as_hex(8);      // Day
            int RE = substring_as_hex(10);     // Transactions Module
            int HH = substring_as_hex(12);     // Hour
            int NC2 = substring_as_hex(14);    // Number of companies #2
            int LL = substring_as_hex(16);     // Minutes
            int MC = substring_as_hex(18);     // Document Module

            bool rec_module = (CE == HH + DateTime.Today.Day + 62);

            var today_day = DateTime.Today.Day;

            if (validate_modules && !rec_module) {

                if (CE != HH + today_day + 50)
                    return false;

            }

            bool tran_module = (RE == LL + today_day + 50);

            if (validate_modules && !tran_module) {

                if (RE != LL + today_day + 38)
                    return false;

            }

            bool doc_module = (MC == MM + today_day + 82);

            if (validate_modules && !doc_module) {

                if (MC != MM + today_day + 96)
                    return false;

            }

            if (validate_modules) {

                int NC = NC1 * 100 + NC2;
                NC /= 2;
                NC -= 19;
                NC -= today_day;
                NC -= DD;

                if (NC < 1)
                    return false;

                data = new License_Data(rec_module, tran_module, doc_module, NC);

            }

            return true;
        }

        public static bool
        ValidatePIN(string activation, string pin) {

            activation = De_Hyphen(activation);

            string rexpin = @"[A-F0-9]{16}";

            string rx_pin_whyp = "(?:[A-F0-9][A-F0-9]-){7}[A-F0-9][A-F0-9]";


            if (pin.Match(rx_pin_whyp).Success)
                pin = De_Hyphen(pin);


            if (!pin.Match(rexpin).Success)
                return false;


            string serial = "";

            /*       Check if digit groups 0-1, 4-5, etc., are permitted        */
            /*       During activation these digits are generated from the serial number        */

            var serial_inds = new int[] { 0, 2, 4, 5, 7 };
            var allowed = Hex_Strings;

            for (int ii = 0; ii < serial_inds.Length; ++ii) {

                var start = 2 * serial_inds[ii];
                var temp_str = pin.Substring(start, 2);

                int index = allowed.IndexOf(temp_str);
                if (index == -1)
                    return false;

                serial += index.ToString();

            }

            string generated_pin;
            Generate_PIN(activation, serial, out generated_pin).tiff();

            return generated_pin == pin;

            #region old
            // temp now contains integer elements from allowed

            /*       This is where will generate the new PIN        */

            //var pin_n = new string[10];

            //for (int ii = 0; ii < 8; ++ii) {
            //    pin_n[ii] = pin.Substring(2 * ii, 2);
            //}

            //int num1 = 4 * (temp[0] % 16) +
            //           temp[1] / 16 +
            //           temp[2] % 16 +
            //           2 * (temp[3] / 16) +
            //           3 * (temp[4] % 16);

            //int num2 = (Convert.ToInt32(activation[1].ToString(), 16) +
            //        3 * Convert.ToInt32(activation[3].ToString(), 16) +
            //            Convert.ToInt32(activation[8].ToString(), 16) +
            //        2 * Convert.ToInt32(activation[16].ToString(), 16));

            //int num3 = 2 * (temp[0] / 16) +
            //           4 * temp[1] % 16 +
            //           temp[2] / 16 +
            //           2 * (temp[3] % 16) +
            //           3 * (temp[4] % 16);

            //pin_n[1] = String.Format("{0:X2}", num1);

            //pin_n[3] = String.Format("{0:X2}", num2);

            //pin_n[6] = String.Format("{0:X2}", num3);


            //string pin_t = "";

            //foreach (var str in pin_n)
            //    pin_t += str;

            //return pin == pin_t; 
            #endregion
        }

        public static bool
        Generate_Activation(string request, int nco, bool rec, bool tran, bool doc,
                            out string act) {
            act = null;

            try {
                request = De_Hyphen(request);
                int LL, HH, DD, MM, YY, NC1, NC2, RM, TM, DM;

                YY = Convert.ToInt32(request.Substring(0, 2), 16);
                YY -= 11;

                MM = Convert.ToInt32(request.Substring(2, 2), 16);
                MM -= 44;

                DD = Convert.ToInt32(request.Substring(4, 2), 16);
                DD -= 22;

                HH = Convert.ToInt32(request.Substring(6, 2), 16);
                HH += 55;

                LL = Convert.ToInt32(request.Substring(8, 2), 16);
                LL += 33;

                int today = DateTime.Today.Day;

                NC1 = ((today + 19 + DD + nco) * 2) / 100;
                NC2 = ((today + 19 + DD + nco) * 2) % 100;

                if (rec)
                    RM = HH + today + 62;
                else
                    RM = HH + today + 50;

                if (tran)
                    TM = LL + today + 50;
                else
                    TM = LL + today + 38;

                if (doc)
                    DM = MM + today + 82;
                else
                    DM = MM + today + 96;

                act = String.Format("{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}{7:X2}{8:X2}{9:X2}",
                                       YY, NC1, MM, RM, DD, TM, HH, NC2, LL, DM).ToUpper();

                return true;
            }
#pragma warning disable
            catch (ArgumentOutOfRangeException ex) {
                return false;
            }
#pragma warning restore
        }

        public static string
        Get_Serial(string pin) {

            if (pin.Length != 16)
                return "";

            var hex = Hex_Strings;

            Func<int, string> get_from_index = (start) =>
                hex.IndexOf(pin.Substring(start, 2)).ToString();

            var ret = get_from_index(0) + get_from_index(4);
            ret += get_from_index(8) + get_from_index(10);
            ret += get_from_index(14);

            return ret;
        }

        public static bool
        Generate_PIN(string activation, string serial, out string pin) {

            pin = "";

            try {
                activation = De_Hyphen(activation);

                var hex = Hex_Digits;

                var serial_as_int = new int[5];
                var pin_array = new string[8];

                for (int ii = 0; ii < 5; ++ii) {
                    int index = int.Parse(serial[ii].ToString());
                    serial_as_int[ii] = hex[index];
                }

                Func<int, string> format1 = (index) => String.Format("{0:X2}", serial_as_int[index]);

                pin_array[0] = format1(0);
                // pin_array[1] = format2(num1);
                pin_array[2] = format1(1);
                // pin_array[3] = format2(num2);
                pin_array[4] = format1(2);
                pin_array[5] = format1(3);
                // pin_array[6] = format2(num3);
                pin_array[7] = format1(4);

                Func<int, int> first_hex = (index) => serial_as_int[index] / 16;
                Func<int, int> second_hex = (index) => serial_as_int[index] % 16;

                int num1 = 4 * (second_hex(0)) +
                                 first_hex(1) +
                                 second_hex(2) +
                            2 * (first_hex(3)) +
                            3 * (second_hex(4));

                Func<int, int> get_digit = (index) => Convert.ToInt16(activation[index].ToString(), 16);

                int num2 = get_digit(1) +
                           3 * get_digit(3) +
                               get_digit(8) +
                           2 * get_digit(16);

                // There is a small misfeature in the calculation - the former version was missing a pair of braces around
                // the serial_as_int[1] % 16. See revision 127.

                int num3 = 2 * (first_hex(0)) +
                          (4 * serial_as_int[1]) % 16 + // sic.
                               first_hex(2) +
                           2 * (second_hex(3)) +
                           3 * (second_hex(4));

                Func<int, string> format2 = (value) => String.Format("{0:X2}", value);

                pin_array[1] = format2(num1);

                pin_array[3] = format2(num2);

                pin_array[6] = format2(num3);

                pin = pin_array.Aggregate((a, b) => a + b);

                return true;
            }
            catch (IndexOutOfRangeException) {
                return false;
            }
        }


    }
}
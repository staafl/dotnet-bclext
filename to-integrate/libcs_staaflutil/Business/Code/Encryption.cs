using System;
using System.Text;
namespace Common
{
    public static class Encryption
    {
        public static string Encrypt(string input) {

            var sb = new StringBuilder(input);

            Random rnd = new Random();

            for (int ii = input.Length; ii >= 0; --ii) {
                if (rnd.Next() % 12 == 0) {
                    sb.Insert(ii, "QXJ");
                }
            }

            byte[] inp = UnicodeEncoding.Unicode.GetBytes(sb.ToString());
            byte[] key = new byte[] { 77, 0, 65, 0, 82, 0, 73, 0, 65, 0 };

            int len = inp.Length;
            int key_len = key.Length;

            byte[] outp = inp;

            for (int ii = 0; ii < len; ii += 2) {

                int a1 = inp[ii];
                int a2 = key[ii % key_len] % 13;
                int a3 = ii % 10;

                int z1 = a1 - a2 + a3 + 4;

                outp[ii] = (byte)z1;

                int z2 = inp[ii + 1] + 7 + (a1 /*clear byte*/ % 3);

                // For silly comment that was here, see repository

                outp[ii + 1] = (byte)z2;

            }

            string ret = UnicodeEncoding.Unicode.GetString(outp);
            return ret;
        }

        public static string Decrypt(string a) {

            byte[] inp = UnicodeEncoding.Unicode.GetBytes(a);
            byte[] key = new byte[] { 77, 0, 65, 0, 82, 0, 73, 0, 65, 0 };

            int keylen = key.Length;
            int len = inp.Length;

            var outp = inp;
            for (int ii = 0; ii < len; ii += 2) {

                int a1 = inp[ii];
                int a2 = key[ii % keylen] % 13;
                int a3 = ii % 10;

                int z1 = a1 + a2 - a3 - 4;

                outp[ii] = (byte)z1;

                int z2 = inp[ii + 1] - 7 - (z1 /*clear byte*/ % 3);

                outp[ii + 1] = (byte)z2;
            }

            string ret = UnicodeEncoding.Unicode.GetString(outp);
            // ret = ret.Replace("\0\0QXJXLL\0\0", "");

            ret = ret.Replace("QXJ", "");

            return ret;
        }
        //static string Hash(string a)
        //{
        //    Dictionary<string, string> _settings = new Dictionary<string, string>();
        //    StringReader sr = new StringReader(a);
        //    Regex regex = new Regex(@"^(\d{4}) ([A-Za-z_0-9]+)=""?([-0-9A-Za-z_:\\\. ]+)""?\r?$", RegexOptions.Multiline);
        //    Match m;
        //    //MatchCollection mm = regex.Matches(a); <-- Keep an eye out for this
        //    string temp;
        //    while ((temp = sr.ReadLine()) != null)
        //    {
        //        m = regex.Match(temp);
        //        if (m.Success)
        //        {
        //            if (m.Groups[1].Value == "0000")
        //                _settings[m.Groups[2].Value] = m.Groups[3].Value;
        //            m.NextMatch();
        //        }
        //    }

        //    int hash = 0;
        //    for (int i = 0; i < hashables.Length; i++)
        //    {
        //        foreach (string str in _settings.Keys)
        //        {
        //            if (str.Contains(hashables[i]))
        //            {
        //                int j; int l = str.Length;
        //                for (j = 0; j < _settings[str].Length; j++)
        //                {
        //                    int deb1 = str[j % l].GetHashCode();
        //                    int deb2 = _settings[str].GetHashCode();
        //                    hash += j * deb1 + deb2;
        //                }
        //            }
        //        }
        //    }
        //    return String.Format("\r\n0000 HASH={0}", hash.ToString());
        //}
    }
}

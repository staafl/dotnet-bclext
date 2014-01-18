using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Fairweather.Service
{
    public class Ini_Group_Class : IReadWrite<Ini_Field, string>, IRead<Ini_Field, string>
    {
        readonly Ini_File ini;
        readonly string m_category;

        public Ini_Group_Class(Ini_File ini, string category) {

            this.ini = ini;
            m_category = category;

        }

        public string this[string key] {
            get { return this[new Ini_Field(key)]; }
            set { this[new Ini_Field(key)] = value; }
        }

        public string this[Ini_Field key] {
            get { return ini[m_category, key]; }
            set { ini[m_category, key] = value; }
        }

        bool IContains<Ini_Field>.Contains(Ini_Field key) {

            string _;
            return ini.Try_Get_Data(m_category, key, out _);

        }

    }
}

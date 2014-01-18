using System.Diagnostics;

using Fairweather.Service;

namespace Fairweather.Service
{

    [DebuggerStepThrough]
    public class Pos_Print_Font
    {
        const string default_generic = "Generic";

        static public Pos_Print_Font Default {
            get { return new Pos_Print_Font(default_generic, 9.5f, false, Print_Font_Type.Generic); }
        }

        //  static readonly Dictionary<Pos_Known_Font, Pos_Print_Font>
        //  sdt_known_fonts;
        //            //  static Pos_Print_Font() {
        //              sdt_known_fonts = new Dictionary<Pos_Known_Font, Pos_Print_Font>{
        //                    {}
        //              };
        //  }


        readonly string m_name;

        readonly float m_height;

        readonly bool m_bold;

        readonly Print_Font_Type m_type;


        public string Name {
            get {
                return this.m_name;
            }
        }

        public float Height {
            get {
                return this.m_height;
            }
        }

        public bool Bold {
            get {
                return this.m_bold;
            }
        }

        public Print_Font_Type Type {
            get {
                return this.m_type;
            }
        }

        public Pos_Print_Font(string name,
                    float height,
                    bool bold,
                    Print_Font_Type type) {
            this.m_name = name;
            this.m_height = height;
            this.m_bold = bold;
            this.m_type = type;
        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            //ret += "name = " + this.m_name;
            //ret += ", ";
            //ret += "height = " + this.m_height;
            //ret += ", ";
            //ret += "bold = " + this.m_bold;
            //ret += ", ";
            //ret += "type = " + this.m_type;

            //ret = "{Print_Font: " + ret + "}";

            ret = "{0}, {1} {2}{3}".spf(m_name, m_height, m_bold ? "Bold " : "", m_type);

            return ret;

        }

        public override int GetHashCode() {

#pragma warning disable
            unchecked {
                int ret = 23;
                int temp;

                if (this.m_name != null) {
                    ret *= 31;
                    temp = this.m_name.GetHashCode();
                    ret += temp;

                }

                if (this.m_height != null) {
                    ret *= 31;
                    temp = this.m_height.GetHashCode();
                    ret += temp;

                }

                if (this.m_bold != null) {
                    ret *= 31;
                    temp = this.m_bold.GetHashCode();
                    ret += temp;

                }

                if (this.m_type != null) {
                    ret *= 31;
                    temp = this.m_type.GetHashCode();
                    ret += temp;

                }

                return ret;
            }
#pragma warning restore
        }

    }

}

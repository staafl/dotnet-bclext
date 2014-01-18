using System;
using System.Diagnostics;

namespace Common
{
    [Serializable]
    [DebuggerStepThrough]
    /// Standard Vertical Receipt
    public class Receipt_Layout_SV
    {
        Receipt_Layout_SV() {
        }

        string m_name;

        string m_header;

        string m_footer;


        string m_image;

        bool m_use_cat;


        bool m_use_image;



        public string Name {
            get {
                return this.m_name ?? "";
            }
            set { this.m_name = value; }
        }

        public string Footer {
            get {
                return (this.m_footer ?? "").Replace("\n", Environment.NewLine);
            }
            set { this.m_footer = value; }


        }

        public string Header {
            get {
                return (this.m_header ?? "").Replace("\n", Environment.NewLine);
            }
            set { this.m_header = value; }


        }

        public string Image {
            get {
                return this.m_image;
            }
            set { this.m_image = value; }


        }

        public bool Use_Image {
            get {
                return this.m_use_image;
            }
            set { this.m_use_image = value; }


        }

        public bool Use_Category {
            get {
                return m_use_cat;
            }
            set { this.m_use_cat = value; }
        }

        public Receipt_Layout_SV(string name,
                          string header,
                          string footer,
                          string image,
                          bool use_image,
                          bool use_cat) {

            this.m_name = name;

            this.m_header = header;
            this.m_footer = footer;

            this.m_image = image;
            this.m_use_image = use_image;
            this.m_use_cat = use_cat;
        }


        /* Boilerplate */

        //public override string ToString() {

        //    string ret = "";

        //    ret += "name = " + this.m_name;
        //    ret += ", ";
        //    ret += "text = " + this.m_text;
        //    ret += ", ";
        //    ret += "image = " + this.m_image;
        //    ret += ", ";
        //    ret += "use_image = " + this.m_use_image;

        //    ret = "{Printing_Header: " + ret + "}";
        //    return ret;

        //}

        //            #region Printing_Header

        //            public bool Equals(Printing_Header obj2) {

        //#pragma warning disable
        //                  if (this.m_title == null) {
        //                        if (obj2.m_title != null)
        //                              return false;
        //                  }
        //                  else {
        //                        if (!this.m_title.Equals(obj2.m_title))
        //                              return false;
        //                  }


        //                  if (this.m_text == null) {
        //                        if (obj2.m_text != null)
        //                              return false;
        //                  }
        //                  else {
        //                        if (!this.m_text.Equals(obj2.m_text))
        //                              return false;
        //                  }


        //                  if (this.m_image == null) {
        //                        if (obj2.m_image != null)
        //                              return false;
        //                  }
        //                  else {
        //                        if (!this.m_image.Equals(obj2.m_image))
        //                              return false;
        //                  }


        //                  if (this.m_use_image == null) {
        //                        if (obj2.m_use_image != null)
        //                              return false;
        //                  }
        //                  else {
        //                        if (!this.m_use_image.Equals(obj2.m_use_image))
        //                              return false;
        //                  }


        //                  if (this.m_dimensions == null) {
        //                        if (obj2.m_dimensions != null)
        //                              return false;
        //                  }
        //                  else {
        //                        if (!this.m_dimensions.Equals(obj2.m_dimensions))
        //                              return false;
        //                  }


        //#pragma warning restore
        //                  return true;
        //            }

        //            public override bool Equals(object obj2) {

        //                  var ret = (obj2 != null && obj2 is Printing_Header);

        //                  if (ret)
        //                        ret = this.Equals((Printing_Header)obj2);


        //                  return ret;

        //            }

        //            public static bool operator ==(Printing_Header left, Printing_Header right) {

        //                  var ret = left.Equals(right);
        //                  return ret;

        //            }

        //            public static bool operator !=(Printing_Header left, Printing_Header right) {

        //                  var ret = !left.Equals(right);
        //                  return ret;

        //            }

        //            public override int GetHashCode() {

        //#pragma warning disable
        //                  unchecked {
        //                        int ret = 23;
        //                        int temp;

        //                        if (this.m_title != null) {
        //                              ret *= 31;
        //                              temp = this.m_title.GetHashCode();
        //                              ret += temp;

        //                        }

        //                        if (this.m_text != null) {
        //                              ret *= 31;
        //                              temp = this.m_text.GetHashCode();
        //                              ret += temp;

        //                        }

        //                        if (this.m_image != null) {
        //                              ret *= 31;
        //                              temp = this.m_image.GetHashCode();
        //                              ret += temp;

        //                        }

        //                        if (this.m_use_image != null) {
        //                              ret *= 31;
        //                              temp = this.m_use_image.GetHashCode();
        //                              ret += temp;

        //                        }

        //                        if (this.m_dimensions != null) {
        //                              ret *= 31;
        //                              temp = this.m_dimensions.GetHashCode();
        //                              ret += temp;

        //                        }

        //                        return ret;
        //                  }
        //#pragma warning restore
        //            }

        //            #endregion
    }
}

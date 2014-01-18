using System;
using System.Runtime.InteropServices;

using Fairweather.Service;

namespace Versioning
{
    /*       Obsolete "Field" class removed in rev 827        */



    public class Fields : Sage_Object, IReadWrite<string, object>
    {
        const string INDEX = "INDEX";
        const string VALUE = "VALUE";

        const string cst_invalid_index = "INVALID INDEX";

        bool IContains<string>.Contains(string key) {

            try {
                var value = this[key];
            }
            catch (COMException ex) {

                string trouble = ex.Message.ToUpper();
                if (trouble.Contains(cst_invalid_index))
                    return false;

                throw;

            }

            return true;

        }

        public object this[string str] {
            get {
                try {
                    object a = str;
                    switch (m_version) {
                        case 11:
                            return f11.Item(ref a).Value;
                        case 12:
                            return f12.Item(ref a).Value;
                        case 13:
                            return f13.Item(ref a).Value;
                        case 14:
                            return f14.Item(ref a).Value;
                        case 15:
                            return f15.Item(ref a).Value;
                        case 16:
                            return f16.Item(ref a).Value;
                        case 17:
                            return f17.Item(ref a).Value;
                    }
                }
                catch (COMException exc) {
                    exc.Data[INDEX] = str;
                    Logging.Notify(exc, INDEX);
                    throw;
                }
                throw new InvalidOperationException();
            }
            set {
                object a = str;
                try {
                    switch (m_version) {
                        case 11:
                            f11.Item(ref a).Value = value;
                            break;
                        case 12:
                            f12.Item(ref a).Value = value;
                            break;

                        case 13:
                            f13.Item(ref a).Value = value;
                            break;

                        case 14:
                            f14.Item(ref a).Value = value;
                            break;

                        case 15:
                            f15.Item(ref a).Value = value;
                            break;
                        case 16:
                            f16.Item(ref a).Value = value;
                            break;
                        case 17:
                            f17.Item(ref a).Value = value;
                            break;

                    }
                }
                catch (COMException exc) {
                    exc.Data[INDEX] = str;
                    exc.Data[VALUE] = value;
                    Logging.Notify(exc, INDEX, VALUE);
                    throw;
                }
            }
        }

        SageDataObject110.Fields f11;
        SageDataObject120.Fields f12;
        SageDataObject130.Fields f13;
        SageDataObject140.Fields f14;
        SageDataObject150.Fields f15;
        SageDataObject160.Fields f16;
        SageDataObject170.Fields f17;

        public Fields(object a, int version)
            : base(version) {

            switch (m_version) {
                case 11:
                    f11 = (SageDataObject110.Fields)a;
                    return;
                case 12:
                    f12 = (SageDataObject120.Fields)a;
                    return;
                case 13:
                    f13 = (SageDataObject130.Fields)a;
                    return;
                case 14:
                    f14 = (SageDataObject140.Fields)a;
                    return;
                case 15:
                    f15 = (SageDataObject150.Fields)a;
                    return;
                case 16:
                    f16 = (SageDataObject160.Fields)a;
                    return;
                case 17:
                    f17 = (SageDataObject170.Fields)a;
                    return;
            }
            return;
        }
    }
}

namespace Versioning
{
    using System;
    using System.Reflection;
    public class InvoicePost : Sage_Container
    {
        SageDataObject110.InvoicePost ip11;
        SageDataObject120.InvoicePost ip12;
        SageDataObject130.InvoicePost ip13;
        SageDataObject140.InvoicePost ip14;
        SageDataObject150.InvoicePost ip15;
        SageDataObject160.InvoicePost ip16;
        SageDataObject170.InvoicePost ip17;

        public InvoicePost(object a, int version)
            : base(version) {
            switch (m_version) {
            case 11:
                ip11 = (SageDataObject110.InvoicePost)a;



                break;
            case 12:
                ip12 = (SageDataObject120.InvoicePost)a;



                break;
            case 13:
                ip13 = (SageDataObject130.InvoicePost)a;




                break;
            case 14:
                ip14 = (SageDataObject140.InvoicePost)a;



                break;
            case 15:
                ip15 = (SageDataObject150.InvoicePost)a;



                break;
            case 16:
                ip16 = (SageDataObject160.InvoicePost)a;



                break;
            case 17:
                ip17 = (SageDataObject170.InvoicePost)a;



                break;
            }


        }

        public bool Update() {
            switch (m_version) {
            case 11:
                return ip11.Update();
            case 12:
                return ip12.Update();
            case 13:
                return ip13.Update();
            case 14:
                return ip14.Update();
            case 15:
                return ip15.Update();
            case 16:
                return ip16.Update();
            case 17:
                return ip17.Update();
            }
            return false;
        }

        public Fields HDGet() {

            switch (m_version) {
            case 11: {
                    Object temp = ip11.Header.GetType().InvokeMember("Fields",
                                                                    BindingFlags.GetProperty,
                                                                    null,
                                                                    ip11.Header,
                                                                    new Object[0]);
                    return new Fields(temp, m_version);
                }
            case 12: {
                    Object temp = ip12.Header.GetType().InvokeMember("Fields",
                                                                    BindingFlags.GetProperty,
                                                                    null,
                                                                    ip12.Header,
                                                                    new Object[0]);
                    return new Fields(temp, m_version);
                }
            case 13: {
                    Object temp = ip13.Header.GetType().InvokeMember("Fields",
                                                                    BindingFlags.GetProperty,
                                                                    null,
                                                                    ip13.Header,
                                                                    new Object[0]);
                    return new Fields(temp, m_version);
                }
            case 14: {
                    Object temp = ip14.Header.GetType().InvokeMember("Fields",
                                                                    BindingFlags.GetProperty,
                                                                    null,
                                                                    ip14.Header,
                                                                    new Object[0]);
                    return new Fields(temp, m_version);
                }
            case 15: {
                    Object temp = ip15.Header.GetType().InvokeMember("Fields",
                                                                    BindingFlags.GetProperty,
                                                                    null,
                                                                    ip15.Header,
                                                                    new Object[0]);
                    return new Fields(temp, m_version);
                }
            case 16: {
                    Object temp = ip16.Header.GetType().InvokeMember("Fields",
                                                                    BindingFlags.GetProperty,
                                                                    null,
                                                                    ip16.Header,
                                                                    new Object[0]);
                    return new Fields(temp, m_version);
                }
            case 17: {
                    Object temp = ip17.Header.GetType().InvokeMember("Fields",
                                                                    BindingFlags.GetProperty,
                                                                    null,
                                                                    ip17.Header,
                                                                    new Object[0]);
                    return new Fields(temp, m_version);
                }
            }
            throw new InvalidOperationException();
        }

        public InvoiceItem AddItem() {
            switch (m_version) {
            case 11: {
                    Object temp = ip11.Items.GetType().InvokeMember("Add",
                                                                    BindingFlags.InvokeMethod,
                                                                    null,
                                                                    ip11.Items,
                                                                    new Object[0]);
                    return new InvoiceItem(temp, m_version);
                }
            case 12: {
                    Object temp = ip12.Items.GetType().InvokeMember("Add",
                                                                    BindingFlags.InvokeMethod,
                                                                    null,
                                                                    ip12.Items,
                                                                    new Object[0]);
                    return new InvoiceItem(temp, m_version);
                }
            case 13: {
                    Object temp = ip13.Items.GetType().InvokeMember("Add",
                                                                    BindingFlags.InvokeMethod,
                                                                    null,
                                                                    ip13.Items,
                                                                    new Object[0]);
                    return new InvoiceItem(temp, m_version);
                }
            case 14: {
                    Object temp = ip14.Items.GetType().InvokeMember("Add",
                                                                    BindingFlags.InvokeMethod,
                                                                    null,
                                                                    ip14.Items,
                                                                    new Object[0]);
                    return new InvoiceItem(temp, m_version);
                }
            case 15: {
                    Object temp = ip15.Items.GetType().InvokeMember("Add",
                                                                    BindingFlags.InvokeMethod,
                                                                    null,
                                                                    ip15.Items,
                                                                    new Object[0]);
                    return new InvoiceItem(temp, m_version);
                }
            case 16: {
                    Object temp = ip16.Items.GetType().InvokeMember("Add",
                                                                    BindingFlags.InvokeMethod,
                                                                    null,
                                                                    ip16.Items,
                                                                    new Object[0]);
                    return new InvoiceItem(temp, m_version);
                }
            case 17: {
                    Object temp = ip17.Items.GetType().InvokeMember("Add",
                                                                    BindingFlags.InvokeMethod,
                                                                    null,
                                                                    ip17.Items,
                                                                    new Object[0]);
                    return new InvoiceItem(temp, m_version);
                }
            }
            return null;
        }

        public LedgerType Type {
            get {
                switch (m_version) {
                case 11:
                    return (LedgerType)ip11.Type;
                case 12:
                    return (LedgerType)ip12.Type;
                case 13:
                    return (LedgerType)ip13.Type;
                case 14:
                    return (LedgerType)ip14.Type;
                case 15:
                    return (LedgerType)ip15.Type;
                case 16:
                    return (LedgerType)ip16.Type;
                case 17:
                    return (LedgerType)ip17.Type;
                }
                throw new InvalidOperationException();
            }
            set {
                switch (m_version) {
                case 11:
                    ip11.Type = (SageDataObject110.InvoiceType)value; break;
                case 12:
                    ip12.Type = (SageDataObject120.InvoiceType)value; break;
                case 13:
                    ip13.Type = (SageDataObject130.InvoiceType)value; break;
                case 14:
                    ip14.Type = (SageDataObject140.InvoiceType)value; break;
                case 15:
                    ip15.Type = (SageDataObject150.InvoiceType)value; break;
                case 16:
                    ip16.Type = (SageDataObject160.InvoiceType)value; break;
                case 17:
                    ip17.Type = (SageDataObject170.InvoiceType)value; break;
                }
            }
        }

        public int GetNextNumber {
            get {
                switch (m_version) {
                case 11:
                    return ip11.GetNextNumber();
                case 12:
                    return ip12.GetNextNumber();
                case 13:
                    return ip13.GetNextNumber();
                case 14:
                    return ip14.GetNextNumber();
                case 15:
                    return ip15.GetNextNumber();
                case 16:
                    return ip16.GetNextNumber();
                case 17:
                    return ip17.GetNextNumber();
                }
                return -1;
            }
        }
    }
}

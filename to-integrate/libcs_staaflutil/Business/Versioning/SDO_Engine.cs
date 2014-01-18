namespace Versioning
{
    using System;
    using Fairweather.Service;

    public class SDO_Engine : Sage_Object
    {
        SageDataObject110.SDOEngine sdo11;
        SageDataObject120.SDOEngine sdo12;
        SageDataObject130.SDOEngine sdo13;
        SageDataObject140.SDOEngine sdo14;
        SageDataObject150.SDOEngine sdo15;
        SageDataObject160.SDOEngine sdo16;
        SageDataObject170.SDOEngine sdo17;


        public SDO_Engine(int version)
            : base(version) {
            switch (m_version) {
                case 11:
                    sdo11 = new SageDataObject110.SDOEngine();
                    break;
                case 12:
                    sdo12 = new SageDataObject120.SDOEngine();
                    break;
                case 13:
                    sdo13 = new SageDataObject130.SDOEngine();
                    break;
                case 14:
                    sdo14 = new SageDataObject140.SDOEngine();
                    break;
                case 15:
                    sdo15 = new SageDataObject150.SDOEngine();
                    break;
                case 16:
                    sdo16 = new SageDataObject160.SDOEngine();
                    break;
                case 17:
                    sdo17 = new SageDataObject170.SDOEngine();
                    break;
                default:
                    throw new InvalidOperationException();
            }

        }

        public string Last_Error_Text {
            get {
                switch (m_version) {
                    case 11:
                        return sdo11.LastError.Text;
                    case 12:
                        return sdo12.LastError.Text;
                    case 13:
                        return sdo13.LastError.Text;
                    case 14:
                        return sdo14.LastError.Text;
                    case 15:
                        return sdo15.LastError.Text;
                    case 16:
                        return sdo16.LastError.Text;
                    case 17:
                        return sdo17.LastError.Text;
                }
                return "";
            }
        }

        public Sage_Error Last_Error {
            get {
                var code = Last_Error_Code;
                return code.To_Enum_<Sage_Error>() ?? 0;
            }
        }

        public int Last_Error_Code {
            get {
                switch (m_version) {
                    case 11:
                        return (int)sdo11.LastError.Code;
                    case 12:
                        return (int)sdo12.LastError.Code;
                    case 13:
                        return (int)sdo13.LastError.Code;
                    case 14:
                        return (int)sdo14.LastError.Code;
                    case 15:
                        return (int)sdo15.LastError.Code;
                    case 16:
                        return (int)sdo16.LastError.Code;
                    case 17:
                        return (int)sdo17.LastError.Code;
                }
                return -1;
            }
        }

        public Work_Space WSAdd() {
            return WSAdd("ws");
        }
        public Work_Space WSAdd(string name) {
            name = "ws";
            Work_Space ret = null;
            switch (m_version) {
                case 11: {
                        ret = new Work_Space(sdo11.Workspaces.Add(name), this);
                        break;
                    }
                case 12: {
                        ret = new Work_Space(sdo12.Workspaces.Add(name), this);
                        break;
                    }
                case 13: {
                        ret = new Work_Space(sdo13.Workspaces.Add(name), this);
                        break;
                    }
                case 14: {
                        ret = new Work_Space(sdo14.Workspaces.Add(name), this);
                        break;
                    }
                case 15: {
                        ret = new Work_Space(sdo15.Workspaces.Add(name), this);
                        break;
                    }
                case 16: {
                        ret = new Work_Space(sdo16.Workspaces.Add(name), this);
                        break;
                    }
                case 17: {
                        ret = new Work_Space(sdo17.Workspaces.Add(name), this);
                        break;
                    }
            }

            return ret;
        }

        public Work_Space WSGet(ref object index) {
            switch (m_version) {
                case 11:
                    return new Work_Space(sdo11.Workspaces.Item(ref index), this);
                case 12:
                    return new Work_Space(sdo12.Workspaces.Item(ref index), this);
                case 13:
                    return new Work_Space(sdo13.Workspaces.Item(ref index), this);
                case 14:
                    return new Work_Space(sdo14.Workspaces.Item(ref index), this);
                case 15:
                    return new Work_Space(sdo15.Workspaces.Item(ref index), this);
                case 16:
                    return new Work_Space(sdo16.Workspaces.Item(ref index), this);
                case 17:
                    return new Work_Space(sdo17.Workspaces.Item(ref index), this);
            }
            return null;
        }

        public int WSCount {
            get {
                switch (m_version) {
                    case 11:
                        return sdo11.Workspaces.Count;
                    case 12:
                        return sdo12.Workspaces.Count;
                    case 13:
                        return sdo13.Workspaces.Count;
                    case 14:
                        return sdo14.Workspaces.Count;
                    case 15:
                        return sdo15.Workspaces.Count;
                    case 16:
                        return sdo16.Workspaces.Count;
                    case 17:
                        return sdo17.Workspaces.Count;
                }
                return 0;
            }
        }

        public bool Register(string serial, string activation) {
            switch (m_version) {
                case 11:
                    return sdo11.Register(serial, activation, "Sage Interface Tools", "InfoTrends", 0x11);
                case 12:
                    return sdo12.Register(serial, activation, "Sage Interface Tools", "InfoTrends", 0x11);
                case 13:
                    return sdo13.Register(serial, activation, "Sage Interface Tools", "InfoTrends", 0x11);
                case 14:
                    return sdo14.Register(serial, activation, "Sage Interface Tools", "InfoTrends", 0x11);
                case 15:
                    return sdo15.Register(serial, activation, "Sage Interface Tools", "InfoTrends", 0x11);
                case 16:
                    return sdo16.Register(serial, activation, "Sage Interface Tools", "InfoTrends", 0x11);
                case 17:
                    return sdo17.Register(serial, activation, "Sage Interface Tools", "InfoTrends", 0x11);
            }
            throw new InvalidOperationException();
        }

        public bool IsRegistered {
            get {
                switch (m_version) {
                    case 11:
                        return sdo11.IsRegistered;
                    case 12:
                        return sdo12.IsRegistered;
                    case 13:
                        return sdo13.IsRegistered;
                    case 14:
                        return sdo14.IsRegistered;
                    case 15:
                        return sdo15.IsRegistered;
                    case 16:
                        return sdo16.IsRegistered;
                    case 17:
                        return sdo17.IsRegistered;
                }
                return false;
            }
        }
    }
#if REFLECT
        public const BindingFlags All =
                BindingFlags.NonPublic | BindingFlags.Public |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.IgnoreCase;

        static public Func<object, Func<TReturn>>
            CreateGenericDelegate1<TTarget, TReturn>(MethodInfo mi)
            where TTarget : class {

            var func = (Func<TTarget, TReturn>)
                       Delegate.CreateDelegate(typeof(Func<TTarget, TReturn>), null, mi);

            return ((caller) =>
            {
                TTarget cast = (TTarget)caller;
                return () => func(cast);
            });
        }

        static public Func<object, Func<TParam, TReturn>>
            CreateGenericDelegate2<TTarget, TParam, TReturn>(MethodInfo mi)
            where TTarget : class {

            var func = (Func<TTarget, TParam, TReturn>)
                       Delegate.CreateDelegate(typeof(Func<TTarget, TParam, TReturn>), null, mi);

            return ((caller) =>
            {
                TTarget cast = (TTarget)caller;
                return (TParam par) => func(cast, par);
            });
        }

#endif
}
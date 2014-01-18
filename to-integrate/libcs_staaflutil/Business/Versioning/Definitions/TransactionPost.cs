using System;
using System.Reflection;

using Fairweather.Service;

namespace Versioning
{
    public class TransactionPost : Sage_Object
    {

        const int version_0 = 11;
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;


        /*       Version Specific        */

        static readonly Func<object, object>[] head_dict = new Func<object, object>[7];

        static readonly Type[] types = { typeof(SageDataObject110.IHeaderData),
                                         typeof(SageDataObject120.IHeaderData),
                                         typeof(SageDataObject130.IHeaderData),
                                         typeof(SageDataObject140.IHeaderData),
                                         typeof(SageDataObject150.IHeaderData),
                                         typeof(SageDataObject160.IHeaderData),
                                         typeof(SageDataObject170.IHeaderData),};




        SageDataObject110.ITransactionPost tp11;
        SageDataObject120.ITransactionPost tp12;
        SageDataObject130.ITransactionPost tp13;
        SageDataObject140.ITransactionPost tp14;
        SageDataObject150.ITransactionPost tp15;
        SageDataObject160.ITransactionPost tp16;
        SageDataObject170.ITransactionPost tp17;

        readonly Type items_type;
        readonly Func<object> header_producer;
        readonly Func<object> field_producer;

        //object items_obj;Type items_type;
        //

        public TransactionPost(object a, int version)
            : base(version) {


            switch (m_version) {
                case 11:
                    tp11 = (SageDataObject110.ITransactionPost)a;
                    header_producer = () => tp11.Header;
                    items_type = tp11.Items.GetType();

                    break;
                case 12:
                    tp12 = (SageDataObject120.ITransactionPost)a;
                    header_producer = () => tp12.Header;
                    items_type = tp12.Items.GetType();


                    break;
                case 13:
                    tp13 = (SageDataObject130.ITransactionPost)a;
                    header_producer = () => tp13.Header;
                    items_type = tp13.Items.GetType();



                    break;
                case 14:
                    tp14 = (SageDataObject140.ITransactionPost)a;
                    header_producer = () => tp14.Header;
                    items_type = tp14.Items.GetType();



                    break;
                case 15:
                    tp15 = (SageDataObject150.ITransactionPost)a;
                    header_producer = () => tp15.Header;
                    items_type = tp15.Items.GetType();


                    break;
                case 16:
                    tp16 = (SageDataObject160.ITransactionPost)a;
                    header_producer = () => tp16.Header;
                    items_type = tp16.Items.GetType();


                    break;
                case 17:
                    tp17 = (SageDataObject170.ITransactionPost)a;
                    header_producer = () => tp17.Header;
                    items_type = tp17.Items.GetType();


                    break;
            }

            int version_index = version - version_0;

            if (head_dict[version_index] == null) {

                Type header_type = types[version_index];
                var pi = header_type.GetProperty("Fields", flags);
                var mi = pi.GetGetMethod();

                var magic1 = _Delegates.Create_Generic_Delegate1_Info;
                var magic2 = magic1.MakeGenericMethod(header_type, typeof(object));

                var @delegate = (Func<object, object>)magic2.Invoke(null, new object[] { mi });
                head_dict[version_index] = @delegate;

            }

            field_producer = () =>
            {
                var @delegate = head_dict[version_index];
                return @delegate(header_producer());
            };

            #region MyRegion

            //MethodInfo generic_1;
            //MethodInfo generic_2;

            //MethodInfo instance_method;

            //generic_1 = this.GetType().GetMethod("CreateGenericDelegate1", All);
            //generic_2 = this.GetType().GetMethod("CreateGenericDelegate2", All);

            //PropertyInfo p;
            //MethodInfo m;

            //p = header_data_t.GetProperty("fields", All);
            //m = p.GetGetMethod();

            //instance_method = generic_1.MakeGenericMethod(header_data_t, typeof(object));

            //TransactionPost.Fact_Header = (Func<object, Func<object>>)
            //instance_method.Invoke(null, new Object[] { m });

            //items_type = items_obj.GetType();

            //int version_index = version - version_0;

            //if (split_dict[version_index] == null) {

            //    var item_type = items_obj.GetType();
            //    var mi = item_type.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
            //    var mis = item_type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreReturn | BindingFlags.NonPublic);

            //    var @delegate = (Func<object, object>)Delegate.CreateDelegate(typeof(Func<object, object>), mi);

            //    split_dict[version_index] = (items) =>
            //    {
            //        return @delegate(items);
            //    };


            //} 
            #endregion
        }


        public bool Update() {
            switch (m_version) {
                case 11:
                    return tp11.Update();
                case 12:
                    return tp12.Update();
                case 13:
                    return tp13.Update();
                case 14:
                    return tp14.Update();
                case 15:
                    return tp15.Update();
                case 16:
                    return tp16.Update();
                case 17:
                    return tp17.Update();
            }
            return false;
        }

        public Fields HDGet() {
            return new Fields(field_producer(), m_version);
        }

        public SplitData SDAdd() {

            switch (m_version) {
                case 11: {
                        Object temp = items_type.InvokeMember("Add",
                                                                        BindingFlags.InvokeMethod,
                                                                        null,
                                                                        tp11.Items,
                                                                        new Object[0]);
                        return new SplitData(temp, m_version);
                    }
                case 12: {
                        Object temp = items_type.InvokeMember("Add",
                                                                        BindingFlags.InvokeMethod,
                                                                        null,
                                                                        tp12.Items,
                                                                        new Object[0]);
                        return new SplitData(temp, m_version);
                    }
                case 13: {
                        Object temp = items_type.InvokeMember("Add",
                                                                        BindingFlags.InvokeMethod,
                                                                        null,
                                                                        tp13.Items,
                                                                        new Object[0]);
                        return new SplitData(temp, m_version);
                    }
                case 14: {
                        Object temp = items_type.InvokeMember("Add",
                                                                        BindingFlags.InvokeMethod,
                                                                        null,
                                                                        tp14.Items,
                                                                        new Object[0]);
                        return new SplitData(temp, m_version);
                    }
                case 15: {
                        Object temp = items_type.InvokeMember("Add",
                                                                        BindingFlags.InvokeMethod,
                                                                        null,
                                                                        tp15.Items,
                                                                        new Object[0]);
                        return new SplitData(temp, m_version);
                    }
                case 16: {
                        Object temp = items_type.InvokeMember("Add",
                                                                        BindingFlags.InvokeMethod,
                                                                        null,
                                                                        tp16.Items,
                                                                        new Object[0]);
                        return new SplitData(temp, m_version);
                    }
                case 17: {
                        Object temp = items_type.InvokeMember("Add",
                                                                        BindingFlags.InvokeMethod,
                                                                        null,
                                                                        tp17.Items,
                                                                        new Object[0]);
                        return new SplitData(temp, m_version);
                    }
            }
            return null;
        }

        public bool Allocate(int nInvoice, int nCredit, double dfAmount, DateTime dDate) {
            switch (m_version) {
                case 11:
                    return tp11.AllocatePayment(nInvoice, nCredit, dfAmount, dDate);
                case 12:
                    return tp12.AllocatePayment(nInvoice, nCredit, dfAmount, dDate);
                case 13:
                    return tp13.AllocatePayment(nInvoice, nCredit, dfAmount, dDate);
                case 14:
                    return tp14.AllocatePayment(nInvoice, nCredit, dfAmount, dDate);
                case 15:
                    return tp15.AllocatePayment(nInvoice, nCredit, dfAmount, dDate);
                case 16:
                    return tp16.AllocatePayment(nInvoice, nCredit, dfAmount, dDate);
                case 17:
                    return tp17.AllocatePayment(nInvoice, nCredit, dfAmount, dDate);
            }
            return false;
        }

        public int PostingNumber {
            get {
                switch (m_version) {
                    case 11:
                        return tp11.PostingNumber;
                    case 12:
                        return tp12.PostingNumber;
                    case 13:
                        return tp13.PostingNumber;
                    case 14:
                        return tp14.PostingNumber;
                    case 15:
                        return tp15.PostingNumber;
                    case 16:
                        return tp16.PostingNumber;
                    case 17:
                        return tp17.PostingNumber;
                }
                return -1;
            }
        }
    }


}

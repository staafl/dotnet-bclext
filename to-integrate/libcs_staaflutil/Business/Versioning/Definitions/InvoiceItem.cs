using System;
namespace Versioning
{
    public class InvoiceItem : Sage_Container, IMove
    {
        /* Autogenerated by sage_wrapper_generator.pl */
        SageDataObject110.InvoiceItem ii11;
        SageDataObject120.InvoiceItem ii12;
        SageDataObject130.InvoiceItem ii13;
        SageDataObject140.InvoiceItem ii14;
        SageDataObject150.InvoiceItem ii15;
        SageDataObject160.InvoiceItem ii16;
        SageDataObject170.InvoiceItem ii17;


        public InvoiceItem(object inner, int version)
            : base(version) {
            switch (m_version) {
                case 11: {
                        ii11 = (SageDataObject110.InvoiceItem)inner;
                        m_fields = new Fields(ii11.Fields,m_version);
                        return;
                    }

                case 12: {
                        ii12 = (SageDataObject120.InvoiceItem)inner;
                        m_fields = new Fields(ii12.Fields,m_version);
                        return;
                    }

                case 13: {
                        ii13 = (SageDataObject130.InvoiceItem)inner;
                        m_fields = new Fields(ii13.Fields,m_version);
                        return;
                    }

                case 14: {
                        ii14 = (SageDataObject140.InvoiceItem)inner;
                        m_fields = new Fields(ii14.Fields,m_version);
                        return;
                    }

                case 15: {
                        ii15 = (SageDataObject150.InvoiceItem)inner;
                        m_fields = new Fields(ii15.Fields,m_version);
                        return;
                    }
                case 16: {
                        ii16 = (SageDataObject160.InvoiceItem)inner;
                        m_fields = new Fields(ii16.Fields,m_version);
                        return;
                    }
                case 17: {
                        ii17 = (SageDataObject170.InvoiceItem)inner;
                        m_fields = new Fields(ii17.Fields,m_version);
                        return;
                    }

                default: throw new InvalidOperationException("ver");
            }
        }
        /* Autogenerated with record_generator.pl */
        const string ACCOUNT_REF = "ACCOUNT_REF";
        const string INVOICEITEM = "INVOICEITEM";


        public bool Deleted_Safe {
            get {
                if (m_version < 13)
                    return false;
                return Deleted;
            }
        }
        public override bool Deleted {
            get {
                bool ret;
                switch (m_version) {

                    case 13: {
                            ret = ii13.IsDeleted();
                            break;
                        }

                    case 14: {
                            ret = ii14.IsDeleted();
                            break;
                        }

                    case 15: {
                            ret = ii15.IsDeleted();
                            break;
                        }
                    case 16: {
                            ret = ii16.IsDeleted();
                            break;
                        }
                    case 17: {
                            ret = ii17.IsDeleted();
                            break;
                        }

                    case 11:
                    case 12:
                    default: throw new InvalidOperationException("ver");
                }
                return ret;
            }

        }
        ///
        //public bool AddNew() {
        //    bool ret;
        //    switch (m_version) {
        //    case 11: {
        //            ret = ii11.AddNew();
        //            break;
        //        }

        //    case 12: {
        //            ret = ii12.AddNew();
        //            break;
        //        }

        //    case 13: {
        //            ret = ii13.AddNew();
        //            break;
        //        }

        //    case 14: {
        //            ret = ii14.AddNew();
        //            break;
        //        }

        //    case 15: {
        //            ret = ii15.AddNew();
        //            break;
        //        }
        //    case 16: {
        //            ret = ii16.AddNew();
        //            break;
        //        }
        //    case 17: {
        //            ret = ii17.AddNew();
        //            break;
        //        }

        //    default: throw new InvalidOperationException("ver");
        //    }
        //    return ret;
        //}
        //public bool Update() {
        //    bool ret;
        //    switch (m_version) {
        //    case 11: {
        //            ret = ii11.Update();
        //            break;
        //        }

        //    case 12: {
        //            ret = ii12.Update();
        //            break;
        //        }

        //    case 13: {
        //            ret = ii13.Update();
        //            break;
        //        }

        //    case 14: {
        //            ret = ii14.Update();
        //            break;
        //        }

        //    case 15: {
        //            ret = ii15.Update();
        //            break;
        //        }
        //    case 16: {
        //            ret = ii16.Update();
        //            break;
        //        }
        //    case 17: {
        //            ret = ii17.Update();
        //            break;
        //        }

        //    default: throw new InvalidOperationException("ver");
        //    }
        //    return ret;
        //}
        //public bool Edit() {
        //    bool ret;
        //    switch (m_version) {
        //    case 11: {
        //            ret = ii11.Edit();
        //            break;
        //        }

        //    case 12: {
        //            ret = ii12.Edit();
        //            break;
        //        }

        //    case 13: {
        //            ret = ii13.Edit();
        //            break;
        //        }

        //    case 14: {
        //            ret = ii14.Edit();
        //            break;
        //        }

        //    case 15: {
        //            ret = ii15.Edit();
        //            break;
        //        }
        //    case 16: {
        //            ret = ii16.Edit();
        //            break;
        //        }
        //    case 17: {
        //            ret = ii17.Edit();
        //            break;
        //        }

        //    default: throw new InvalidOperationException("ver");
        //    }
        //    return ret;
        //}
        //public bool Find(bool partial) {
        //    bool ret;
        //    switch (m_version) {
        //    case 11: {
        //            ret = ii11.Find(partial);
        //            break;
        //        }

        //    case 12: {
        //            ret = ii12.Find(partial);
        //            break;
        //        }

        //    case 13: {
        //            ret = ii13.Find(partial);
        //            break;
        //        }

        //    case 14: {
        //            ret = ii14.Find(partial);
        //            break;
        //        }

        //    case 15: {
        //            ret = ii15.Find(partial);
        //            break;
        //        }
        //    case 16: {
        //            ret = ii16.Find(partial);
        //            break;
        //        }
        //    case 17: {
        //            ret = ii17.Find(partial);
        //            break;
        //        }

        //    default: throw new InvalidOperationException("ver");
        //    }
        //    return ret;
        //}
        //
        public bool MoveFirst() {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = ii11.MoveFirst();
                        break;
                    }

                case 12: {
                        ret = ii12.MoveFirst();
                        break;
                    }

                case 13: {
                        ret = ii13.MoveFirst();
                        break;
                    }

                case 14: {
                        ret = ii14.MoveFirst();
                        break;
                    }

                case 15: {
                        ret = ii15.MoveFirst();
                        break;
                    }
                case 16: {
                        ret = ii16.MoveFirst();
                        break;
                    }
                case 17: {
                        ret = ii17.MoveFirst();
                        break;
                    }

                default: throw new InvalidOperationException("ver");
            }
            return ret;
        }
        public bool MoveNext() {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = ii11.MoveNext();
                        break;
                    }

                case 12: {
                        ret = ii12.MoveNext();
                        break;
                    }

                case 13: {
                        ret = ii13.MoveNext();
                        break;
                    }

                case 14: {
                        ret = ii14.MoveNext();
                        break;
                    }

                case 15: {
                        ret = ii15.MoveNext();
                        break;
                    }
                case 16: {
                        ret = ii16.MoveNext();
                        break;
                    }
                case 17: {
                        ret = ii17.MoveNext();
                        break;
                    }

                default: throw new InvalidOperationException("ver");
            }
            return ret;
        }
        public bool MoveLast() {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = ii11.MoveLast();
                        break;
                    }

                case 12: {
                        ret = ii12.MoveLast();
                        break;
                    }

                case 13: {
                        ret = ii13.MoveLast();
                        break;
                    }

                case 14: {
                        ret = ii14.MoveLast();
                        break;
                    }

                case 15: {
                        ret = ii15.MoveLast();
                        break;
                    }
                case 16: {
                        ret = ii16.MoveLast();
                        break;
                    }
                case 17: {
                        ret = ii17.MoveLast();
                        break;
                    }

                default: throw new InvalidOperationException("ver");
            }
            return ret;
        }
        public bool MovePrev() {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = ii11.MovePrev();
                        break;
                    }

                case 12: {
                        ret = ii12.MovePrev();
                        break;
                    }

                case 13: {
                        ret = ii13.MovePrev();
                        break;
                    }

                case 14: {
                        ret = ii14.MovePrev();
                        break;
                    }

                case 15: {
                        ret = ii15.MovePrev();
                        break;
                    }
                case 16: {
                        ret = ii16.MovePrev();
                        break;
                    }
                case 17: {
                        ret = ii17.MovePrev();
                        break;
                    }

                default: throw new InvalidOperationException("ver");
            }
            return ret;
        }

        public bool Write(int RecNo) {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = ii11.Write(RecNo);
                        break;
                    }

                case 12: {
                        ret = ii12.Write(RecNo);
                        break;
                    }

                case 13: {
                        ret = ii13.Write(RecNo);
                        break;
                    }

                case 14: {
                        ret = ii14.Write(RecNo);
                        break;
                    }

                case 15: {
                        ret = ii15.Write(RecNo);
                        break;
                    }
                case 16: {
                        ret = ii16.Write(RecNo);
                        break;
                    }
                case 17: {
                        ret = ii17.Write(RecNo);
                        break;
                    }

                default: throw new InvalidOperationException("ver");
            }
            return ret;
        }
        public bool Read(int RecNo) {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = ii11.Read(RecNo);
                        break;
                    }

                case 12: {
                        ret = ii12.Read(RecNo);
                        break;
                    }

                case 13: {
                        ret = ii13.Read(RecNo);
                        break;
                    }

                case 14: {
                        ret = ii14.Read(RecNo);
                        break;
                    }

                case 15: {
                        ret = ii15.Read(RecNo);
                        break;
                    }
                case 16: {
                        ret = ii16.Read(RecNo);
                        break;
                    }
                case 17: {
                        ret = ii17.Read(RecNo);
                        break;
                    }

                default: throw new InvalidOperationException("ver");
            }
            return ret;
        }

        //public bool CanRemove() {
        //    bool ret;
        //    switch (m_version) {
        //    case 11: {
        //            ret = ii11.CanRemove();
        //            break;
        //        }

        //    case 12: {
        //            ret = ii12.CanRemove();
        //            break;
        //        }

        //    case 13: {
        //            ret = ii13.CanRemove();
        //            break;
        //        }

        //    case 14: {
        //            ret = ii14.CanRemove();
        //            break;
        //        }

        //    case 15: {
        //            ret = ii15.CanRemove();
        //            break;
        //        }
        //    case 16: {
        //            ret = ii16.CanRemove();
        //            break;
        //        }
        //    case 17: {
        //            ret = ii17.CanRemove();
        //            break;
        //        }

        //    default: throw new InvalidOperationException("ver");
        //    }
        //    return ret;
        //}
        //public bool Remove() {
        //    bool ret;
        //    switch (m_version) {
        //    case 11: {
        //            ret = ii11.Remove();
        //            break;
        //        }

        //    case 12: {
        //            ret = ii12.Remove();
        //            break;
        //        }

        //    case 13: {
        //            ret = ii13.Remove();
        //            break;
        //        }

        //    case 14: {
        //            ret = ii14.Remove();
        //            break;
        //        }

        //    case 15: {
        //            ret = ii15.Remove();
        //            break;
        //        }
        //    case 16: {
        //            ret = ii16.Remove();
        //            break;
        //        }
        //    case 17: {
        //            ret = ii17.Remove();
        //            break;
        //        }

        //    default: throw new InvalidOperationException("ver");
        //    }
        //    return ret;
        //}
        //public object Link {
        //    get {
        //        object ret;
        //        switch (m_version) {
        //        case 11: {
        //                ret = ii11.Link;
        //                break;
        //            }

        //        case 12: {
        //                ret = ii12.Link;
        //                break;
        //            }

        //        case 13: {
        //                ret = ii13.Link;
        //                break;
        //            }

        //        case 14: {
        //                ret = ii14.Link;
        //                break;
        //            }

        //        case 15: {
        //                ret = ii15.Link;
        //                break;
        //            }
        //        case 16: {
        //                ret = ii16.Link;
        //                break;
        //            }
        //        case 17: {
        //                ret = ii17.Link;
        //                break;
        //            }

        //        default: throw new InvalidOperationException("ver");
        //        }
        //        return ret;
        //    }
        //    set {
        //        switch (m_version) {
        //        case 11: {
        //                ii11.Link = value;
        //                break;
        //            }

        //        case 12: {
        //                ii12.Link = value;
        //                break;
        //            }

        //        case 13: {
        //                ii13.Link = value;
        //                break;
        //            }

        //        case 14: {
        //                ii14.Link = value;
        //                break;
        //            }

        //        case 15: {
        //                ii15.Link = value;
        //                break;
        //            }
        //        case 16: {
        //                ii16.Link = value;
        //                break;
        //            }
        //        case 17: {
        //                ii17.Link = value;
        //                break;
        //            }

        //        }
        //    }
        //}
    }
}

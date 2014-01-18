using System;
namespace Versioning
{
    public class SplitData : Sage_Container, IData, IMove
    {
        /* Autogenerated by sage_wrapper_generator.pl */
        SageDataObject110.SplitData sd11;
        SageDataObject120.SplitData sd12;
        SageDataObject130.SplitData sd13;
        SageDataObject140.SplitData sd14;
        SageDataObject150.SplitData sd15;
        SageDataObject160.SplitData sd16;
        SageDataObject170.SplitData sd17;


        public SplitData(object inner, int version)
            : base(version) {
            switch (m_version) {
                case 11: {
                        sd11 = (SageDataObject110.SplitData)inner;
                        m_fields = new Fields(sd11.Fields, m_version);
                        return;
                    }

                case 12: {
                        sd12 = (SageDataObject120.SplitData)inner;
                        m_fields = new Fields(sd12.Fields, m_version);
                        return;
                    }

                case 13: {
                        sd13 = (SageDataObject130.SplitData)inner;
                        m_fields = new Fields(sd13.Fields, m_version);
                        return;
                    }

                case 14: {
                        sd14 = (SageDataObject140.SplitData)inner;
                        m_fields = new Fields(sd14.Fields, m_version);
                        return;
                    }

                case 15: {
                        sd15 = (SageDataObject150.SplitData)inner;
                        m_fields = new Fields(sd15.Fields, m_version);
                        return;
                    }
                case 16: {
                        sd16 = (SageDataObject160.SplitData)inner;
                        m_fields = new Fields(sd16.Fields, m_version);
                        return;
                    }
                case 17: {
                        sd17 = (SageDataObject170.SplitData)inner;
                        m_fields = new Fields(sd17.Fields, m_version);
                        return;
                    }

                default: throw new InvalidOperationException("m_version");
            }
        }
        /* Autogenerated with data_generator.pl */
        const string ACCOUNT_REF = "ACCOUNT_REF";
        const string SPLITDATA = "SplitData";




        public int RecordNumber {
            get {
                switch (m_version) {
                    case 11:
                        return sd11.RecordNumber;
                    case 12:
                        return sd12.RecordNumber;
                    case 13:
                        return sd13.RecordNumber;
                    case 14:
                        return sd14.RecordNumber;
                    case 15:
                        return sd15.RecordNumber;
                    case 16:
                        return sd16.RecordNumber;
                    case 17:
                        return sd17.RecordNumber;
                }
                return -1;
            }
        }
        public ProjectCostCode CostCode {
            get {
                switch (m_version) {
                    case 12:
                        return new ProjectCostCode(sd12.CostCode, m_version);
                    case 13:
                        return new ProjectCostCode(sd13.CostCode, m_version);
                    case 14:
                        return new ProjectCostCode(sd14.CostCode, m_version);
                    case 15:
                        return new ProjectCostCode(sd15.CostCode, m_version);
                    case 16:
                        return new ProjectCostCode(sd16.CostCode, m_version);
                    case 17:
                        return new ProjectCostCode(sd17.CostCode, m_version);
                }
                return null;
            }
            set {
                switch (m_version) {
                    case 12:
                        sd12.CostCode = value.Inner;
                        break;
                    case 13:
                        sd13.CostCode = value.Inner;
                        break;
                    case 14:
                        sd14.CostCode = value.Inner;
                        break;
                    case 15:
                        sd15.CostCode = value.Inner;
                        break;
                    case 16:
                        sd16.CostCode = value.Inner;
                        break;
                    case 17:
                        sd17.CostCode = value.Inner;
                        break;
                }
            }
        }
        public Project Project {
            get {
                switch (m_version) {
                    case 12:
                        return new Project(sd12.Project, m_version);
                    case 13:
                        return new Project(sd13.Project, m_version);
                    case 14:
                        return new Project(sd14.Project, m_version);
                    case 15:
                        return new Project(sd15.Project, m_version);
                    case 16:
                        return new Project(sd16.Project, m_version);
                    case 17:
                        return new Project(sd17.Project, m_version);
                }
                return null;
            }
            set {
                switch (m_version) {
                    case 12:
                        sd12.Project = value.Inner;
                        break;
                    case 13:
                        sd13.Project = value.Inner;
                        break;
                    case 14:
                        sd14.Project = value.Inner;
                        break;
                    case 15:
                        sd15.Project = value.Inner;
                        break;
                    case 16:
                        sd16.Project = value.Inner;
                        break;
                    case 17:
                        sd17.Project = value.Inner;
                        break;
                }
            }
        }
        public bool Open(OpenMode mode) {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = sd11.Open((SageDataObject110.OpenMode)mode);
                        break;
                    }

                case 12: {
                        ret = sd12.Open((SageDataObject120.OpenMode)mode);
                        break;
                    }

                case 13: {
                        ret = sd13.Open((SageDataObject130.OpenMode)mode);
                        break;
                    }

                case 14: {
                        ret = sd14.Open((SageDataObject140.OpenMode)mode);
                        break;
                    }

                case 15: {
                        ret = sd15.Open((SageDataObject150.OpenMode)mode);
                        break;
                    }
                case 16: {
                        ret = sd16.Open((SageDataObject160.OpenMode)mode);
                        break;
                    }
                case 17: {
                        ret = sd17.Open((SageDataObject170.OpenMode)mode);
                        break;
                    }

                default: throw new InvalidOperationException("m_version");
            }
            return ret;
        }
        public void Close() {
            switch (m_version) {
                case 11: {
                        sd11.Close();
                        break;
                    }

                case 12: {
                        sd12.Close();
                        break;
                    }

                case 13: {
                        sd13.Close();
                        break;
                    }

                case 14: {
                        sd14.Close();
                        break;
                    }

                case 15: {
                        sd15.Close();
                        break;
                    }
                case 16: {
                        sd16.Close();
                        break;
                    }
                case 17: {
                        sd17.Close();
                        break;
                    }

                default: throw new InvalidOperationException("m_version");
            }

        }
        public bool Read(int IRecNo) {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = sd11.Read(IRecNo);
                        break;
                    }

                case 12: {
                        ret = sd12.Read(IRecNo);
                        break;
                    }

                case 13: {
                        ret = sd13.Read(IRecNo);
                        break;
                    }

                case 14: {
                        ret = sd14.Read(IRecNo);
                        break;
                    }

                case 15: {
                        ret = sd15.Read(IRecNo);
                        break;
                    }
                case 16: {
                        ret = sd16.Read(IRecNo);
                        break;
                    }
                case 17: {
                        ret = sd17.Read(IRecNo);
                        break;
                    }

                default: throw new InvalidOperationException("m_version");
            }
            return ret;
        }
        public bool Write(int IRecNo) {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = sd11.Write(IRecNo);
                        break;
                    }

                case 12: {
                        ret = sd12.Write(IRecNo);
                        break;
                    }

                case 13: {
                        ret = sd13.Write(IRecNo);
                        break;
                    }

                case 14: {
                        ret = sd14.Write(IRecNo);
                        break;
                    }

                case 15: {
                        ret = sd15.Write(IRecNo);
                        break;
                    }
                case 16: {
                        ret = sd16.Write(IRecNo);
                        break;
                    }
                case 17: {
                        ret = sd17.Write(IRecNo);
                        break;
                    }

                default: throw new InvalidOperationException("m_version");
            }
            return ret;
        }
        public bool Seek(int IRecNo) {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = sd11.Seek(IRecNo);
                        break;
                    }

                case 12: {
                        ret = sd12.Seek(IRecNo);
                        break;
                    }

                case 13: {
                        ret = sd13.Seek(IRecNo);
                        break;
                    }

                case 14: {
                        ret = sd14.Seek(IRecNo);
                        break;
                    }

                case 15: {
                        ret = sd15.Seek(IRecNo);
                        break;
                    }
                case 16: {
                        ret = sd16.Seek(IRecNo);
                        break;
                    }
                case 17: {
                        ret = sd17.Seek(IRecNo);
                        break;
                    }

                default: throw new InvalidOperationException("m_version");
            }
            return ret;
        }
        public bool Lock(int IRecNo) {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = sd11.Lock(IRecNo);
                        break;
                    }

                case 12: {
                        ret = sd12.Lock(IRecNo);
                        break;
                    }

                case 13: {
                        ret = sd13.Lock(IRecNo);
                        break;
                    }

                case 14: {
                        ret = sd14.Lock(IRecNo);
                        break;
                    }

                case 15: {
                        ret = sd15.Lock(IRecNo);
                        break;
                    }
                case 16: {
                        ret = sd16.Lock(IRecNo);
                        break;
                    }
                case 17: {
                        ret = sd17.Lock(IRecNo);
                        break;
                    }

                default: throw new InvalidOperationException("m_version");
            }
            return ret;
        }
        public bool Unlock(int IRecNo) {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = sd11.Unlock(IRecNo);
                        break;
                    }

                case 12: {
                        ret = sd12.Unlock(IRecNo);
                        break;
                    }

                case 13: {
                        ret = sd13.Unlock(IRecNo);
                        break;
                    }

                case 14: {
                        ret = sd14.Unlock(IRecNo);
                        break;
                    }

                case 15: {
                        ret = sd15.Unlock(IRecNo);
                        break;
                    }
                case 16: {
                        ret = sd16.Unlock(IRecNo);
                        break;
                    }
                case 17: {
                        ret = sd17.Unlock(IRecNo);
                        break;
                    }

                default: throw new InvalidOperationException("m_version");
            }
            return ret;
        }
        public bool FindFirst(object varField, object varSearch) {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = sd11.FindFirst(varField, varSearch);
                        break;
                    }

                case 12: {
                        ret = sd12.FindFirst(varField, varSearch);
                        break;
                    }

                case 13: {
                        ret = sd13.FindFirst(varField, varSearch);
                        break;
                    }

                case 14: {
                        ret = sd14.FindFirst(varField, varSearch);
                        break;
                    }

                case 15: {
                        ret = sd15.FindFirst(varField, varSearch);
                        break;
                    }
                case 16: {
                        ret = sd16.FindFirst(varField, varSearch);
                        break;
                    }
                case 17: {
                        ret = sd17.FindFirst(varField, varSearch);
                        break;
                    }

                default: throw new InvalidOperationException("m_version");
            }
            return ret;
        }
        public bool FindNext(object varField, object varSearch) {
            bool ret;
            switch (m_version) {
                case 11: {
                        ret = sd11.FindNext(varField, varSearch);
                        break;
                    }

                case 12: {
                        ret = sd12.FindNext(varField, varSearch);
                        break;
                    }

                case 13: {
                        ret = sd13.FindNext(varField, varSearch);
                        break;
                    }

                case 14: {
                        ret = sd14.FindNext(varField, varSearch);
                        break;
                    }

                case 15: {
                        ret = sd15.FindNext(varField, varSearch);
                        break;
                    }
                case 16: {
                        ret = sd16.FindNext(varField, varSearch);
                        break;
                    }
                case 17: {
                        ret = sd17.FindNext(varField, varSearch);
                        break;
                    }

                default: throw new InvalidOperationException("m_version");
            }
            return ret;
        }
        public int Count {
            get {
                int ret;
                switch (m_version) {
                    case 11: {
                            ret = sd11.Count;
                            break;
                        }

                    case 12: {
                            ret = sd12.Count;
                            break;
                        }

                    case 13: {
                            ret = sd13.Count;
                            break;
                        }

                    case 14: {
                            ret = sd14.Count;
                            break;
                        }

                    case 15: {
                            ret = sd15.Count;
                            break;
                        }
                    case 16: {
                            ret = sd16.Count;
                            break;
                        }
                    case 17: {
                            ret = sd17.Count;
                            break;
                        }

                    default: throw new InvalidOperationException("m_version");
                }
                return ret;
            }
        }


        //IMove
        public bool MoveFirst() {
            switch (m_version) {
                case 11:
                    return sd11.MoveFirst();
                case 12:
                    return sd12.MoveFirst();
                case 13:
                    return sd13.MoveFirst();
                case 14:
                    return sd14.MoveFirst();
                case 15:
                    return sd15.MoveFirst();
                case 16:
                    return sd16.MoveFirst();
                case 17:
                    return sd17.MoveFirst();
            }
            return false;
        }
        public bool MoveNext() {
            switch (m_version) {
                case 11:
                    return sd11.MoveNext();
                case 12:
                    return sd12.MoveNext();
                case 13:
                    return sd13.MoveNext();
                case 14:
                    return sd14.MoveNext();
                case 15:
                    return sd15.MoveNext();
                case 16:
                    return sd16.MoveNext();
                case 17:
                    return sd17.MoveNext();
            }
            return false;
        }
        public bool MoveLast() {
            switch (m_version) {
                case 11:
                    return sd11.MoveLast();
                case 12:
                    return sd12.MoveLast();
                case 13:
                    return sd13.MoveLast();
                case 14:
                    return sd14.MoveLast();
                case 15:
                    return sd15.MoveLast();
                case 16:
                    return sd16.MoveLast();
                case 17:
                    return sd17.MoveLast();
            }
            return false;
        }
        public bool MovePrev() {
            switch (m_version) {
                case 11:
                    return sd11.MovePrev();
                case 12:
                    return sd12.MovePrev();
                case 13:
                    return sd13.MovePrev();
                case 14:
                    return sd14.MovePrev();
                case 15:
                    return sd15.MovePrev();
                case 16:
                    return sd16.MovePrev();
                case 17:
                    return sd17.MovePrev();
            }
            return false;
        }
        public HeaderData Link {
            get {
                switch (m_version) {
                    case 11:
                        return new HeaderData(sd11.Link, m_version);
                    case 12:
                        return new HeaderData(sd12.Link, m_version);
                    case 13:
                        return new HeaderData(sd13.Link, m_version);
                    case 14:
                        return new HeaderData(sd14.Link, m_version);
                    case 15:
                        return new HeaderData(sd15.Link, m_version);
                    case 16:
                        return new HeaderData(sd16.Link, m_version);
                    case 17:
                        return new HeaderData(sd17.Link, m_version);
                }
                return null;
            }
        }

    }
}

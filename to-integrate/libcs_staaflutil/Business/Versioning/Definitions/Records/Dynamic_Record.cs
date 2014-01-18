using System;

using Fairweather.Service;

namespace Versioning
{
    class Dynamic_Record : ISageRecord
    {
        // C:\Users\Fairweather\Desktop\decorator.pl

        readonly ISageRecord inner;
        public Dynamic_Record(ISageRecord inner) {
            this.inner = inner;
        }

        public Fields Fields {
            get {
                return inner.Fields;
            }
        }

        public string Index_String {
            get {
                return inner.Index_String;
            }
        }
        public virtual object this[string ix] {
            get {
                return inner[ix];
            }
            set {
                inner[ix] = value;
            }
        }
        public bool Contains(string ix) {
            return inner.Contains(ix);
        }

        public string Key {
            get {
                return inner.Key;
            }
            set {
                inner.Key = value;
            }
        }

        public int Count {
            get { return inner.Count; }
        }

        public bool AddNew() {

            return inner.AddNew();
        }

        public bool Update() {

            return inner.Update();

        }
        public bool Edit() {

            return inner.Edit();

        }
        public bool Find(bool partial) {

            return inner.Find(partial);

        }
        public bool MoveFirst() {

            return inner.MoveFirst();

        }
        public bool MoveNext() {

            return inner.MoveNext();

        }
        public bool MoveLast() {

            return inner.MoveLast();

        }
        public bool MovePrev() {

            return inner.MovePrev();

        }

        public bool Deleted { get { return inner.Deleted; } }
    }

    class Dynamic_Dept
    {

    }

    class Dynamic_Bank : Dynamic_Record
    {
        const string STR_NAME = "NAME";
        const string STR_ACCOUNT_REF = "ACCOUNT_REF";

        Lazy_Dict<string, string> cache;
        readonly BankRecord inner;
        // readonly NominalRecord nominal;

        public Dynamic_Bank(BankRecord inner, NominalRecord nominal)
            : base(inner) {
            this.inner = inner;
            cache = new Lazy_Dict<string, string>(acc_ref =>
            {
                nominal[STR_ACCOUNT_REF] = acc_ref;
                nominal.Find(false).tiff();
                return nominal[STR_NAME].StringOrDefault();
            });

        }
        public override object this[string ix] {
            get {
                if (ix == STR_NAME)
                    return cache[inner[STR_ACCOUNT_REF].ToString()];

                return base[ix];
            }
            set {
                base[ix] = value;
            }
        }

    }
}

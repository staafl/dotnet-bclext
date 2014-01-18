using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fairweather.Service;
namespace Versioning
{
    public class Projects : Sage_Object
    {
        SageDataObject120.Projects projs12;
        SageDataObject130.Projects projs13;
        SageDataObject140.Projects projs14;
        SageDataObject150.Projects projs15;
        SageDataObject160.Projects projs16;
        SageDataObject170.Projects projs17;

        //object inner;



        public Projects(object inner, int version)
            : base(version) {
            // this.inner = inner;

            switch (m_version) {
                case 12: {
                        projs12 = (SageDataObject120.Projects)inner;
                        return;
                    }

                case 13: {
                        projs13 = (SageDataObject130.Projects)inner;
                        return;
                    }

                case 14: {
                        projs14 = (SageDataObject140.Projects)inner;
                        return;
                    }

                case 15: {
                        projs15 = (SageDataObject150.Projects)inner;
                        return;
                    }
                case 16: {
                        projs16 = (SageDataObject160.Projects)inner;
                        return;
                    }
                case 17: {
                        projs17 = (SageDataObject170.Projects)inner;
                        return;
                    }

                default: throw new InvalidOperationException("ver");
            }
        }

        public int Count {
            get {
                //return inner.GetType()
                //            .InvokeMember(
                //            "Count", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase, null, inner, new object[0]).ToInt32();

                switch (m_version) {
                    case 12: return projs12.Count;
                    case 13: return projs13.Count;
                    case 14: return projs14.Count;
                    case 15: return projs15.Count;
                    case 16: return projs16.Count;
                    case 17: return projs17.Count;
                    default: throw new InvalidOperationException("ver");
                }
            }
        }

        public Projects Item(int index) {
            object temp = index;
            object a;
            switch (m_version) {
                case 12: { a = projs12.Item(ref temp); break; }
                case 13: { a = projs13.Item(ref temp); break; }
                case 14: { a = projs14.Item(ref temp); break; }
                case 15: { a = projs15.Item(ref temp); break; }
                case 16: { a = projs16.Item(ref temp); break; }
                case 17: { a = projs17.Item(ref temp); break; }
                default: throw new InvalidOperationException("ver");
            }
            return new Projects(a, m_version);
        }

        public IEnumerable GetEnumerator() {

            IEnumerator a;

            switch (m_version) {
                case 12: { a = projs12.GetEnumerator(); break; }
                case 13: { a = projs13.GetEnumerator(); break; }
                case 14: { a = projs14.GetEnumerator(); break; }
                case 15: { a = projs15.GetEnumerator(); break; }
                case 16: { a = projs16.GetEnumerator(); break; }
                case 17: { a = projs17.GetEnumerator(); break; }
                default: throw new InvalidOperationException("ver");
            }

            while (a.MoveNext())
                yield return new Projects(a.Current, m_version);

        }

    }
}

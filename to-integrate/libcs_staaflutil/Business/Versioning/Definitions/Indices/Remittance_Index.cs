using System;
using System.Reflection;


namespace Versioning
{
      public class RemittanceIndex : Sage_Container, IData, IMove
      {
            SageDataObject110.RemittanceIndex rd11;
            SageDataObject120.RemittanceIndex rd12;
            SageDataObject130.RemittanceIndex rd13;
            SageDataObject140.RemittanceIndex rd14;
            SageDataObject150.RemittanceIndex rd15;
            SageDataObject160.RemittanceIndex rd16;
            SageDataObject170.RemittanceIndex rd17;
            Type _type;
            Object _object;


            public RemittanceIndex(object a, int version)
                  : base(version) {
                  switch (m_version) {
                        case 11:
                              rd11 = (SageDataObject110.RemittanceIndex)a;
                              _type = rd11.GetType();
                              _object = rd11;
                              break;
                        case 12:
                              rd12 = (SageDataObject120.RemittanceIndex)a;
                              _type = rd12.GetType();
                              _object = rd12;

                              break;
                        case 13:
                              rd13 = (SageDataObject130.RemittanceIndex)a;
                              _type = rd13.GetType();
                              _object = rd13;

                              break;
                        case 14:
                              rd14 = (SageDataObject140.RemittanceIndex)a;
                              _type = rd14.GetType();
                              _object = rd14;

                              break;
                        case 15:
                              rd15 = (SageDataObject150.RemittanceIndex)a;
                              _type = rd15.GetType();
                              _object = rd15;

                              break;
                        case 16:
                              rd16 = (SageDataObject160.RemittanceIndex)a;
                              _type = rd16.GetType();
                              _object = rd16;

                              break;
                        case 17:
                              rd17 = (SageDataObject170.RemittanceIndex)a;
                              _type = rd17.GetType();
                              _object = rd17;

                              break;
                  }
                  m_fields = new Fields(_type.InvokeMember("Fields", BindingFlags.GetProperty, null, _object, new Object[0]), m_version);

            }

            public bool Open(OpenMode mode) {
                  return (bool)_type.InvokeMember("Open", BindingFlags.InvokeMethod, null, _object, new Object[] { (SageDataObject110.OpenMode)mode });
            }

            public void Close() {
                  _type.InvokeMember("Close", BindingFlags.InvokeMethod, null, _object, new Object[0]);
            }

            public int Count {
                  get {
                        return (int)_type.InvokeMember("Count", BindingFlags.InvokeMethod, null, _object, new Object[0]);
                  }
            }
            public bool Add() {
                  return (bool)_type.InvokeMember("Add", BindingFlags.InvokeMethod, null, _object, new Object[] { });
            }
            public bool Read(int IRecNo) {
                  return (bool)_type.InvokeMember("Read", BindingFlags.InvokeMethod, null, _object, new Object[] { IRecNo });

            }
            public bool Write(int IRecNo) {
                  return (bool)_type.InvokeMember("Write", BindingFlags.InvokeMethod, null, _object, new Object[] { IRecNo });
            }
            public bool MoveFirst() {
                  return (bool)_type.InvokeMember("MoveFirst", BindingFlags.InvokeMethod, null, _object, new Object[0]);
            }
            public bool MoveNext() {
                  return (bool)_type.InvokeMember("MoveNext", BindingFlags.InvokeMethod, null, _object, new Object[0]);
            }
            public bool MoveLast() {
                  return (bool)_type.InvokeMember("MoveLast", BindingFlags.InvokeMethod, null, _object, new Object[0]);
            }
            public bool MovePrev() {
                  return (bool)_type.InvokeMember("MovePrev", BindingFlags.InvokeMethod, null, _object, new Object[0]);
            }

            public int DataRecordNumber {
                  get {
                        return (int)_type.InvokeMember("DataRecordNumber", BindingFlags.GetProperty, null, _object, new Object[0]);
                  }
            }

            public int IndexRecordNumber {
                  get {
                        return (int)_type.InvokeMember("IndexRecordNumber", BindingFlags.GetProperty, null, _object, new Object[0]);
                  }
            }

            public override string Key {
                  get {
                        return (string)_type.InvokeMember("Key", BindingFlags.GetProperty, null, _object, new Object[0]);
                  }
                set {
                    throw new NotImplementedException();
                }
            }

      }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Fairweather.Service;


namespace Common.Queries
{
      public struct Clause
      {

            static void Ensure_Type(Operation operation, Argument arg_1,
                                                         Argument arg_2) {

                  var type_1 = arg_1.Type;
                  var type_2 = arg_2.Type;
                  Argument_Type type;

                  if (type_1 == Argument_Type.Place_Holder ||
                      type_2 == Argument_Type.Place_Holder) {

                        (type_1 == type_2).tiff();
                        return;

                  }

                  if (type_1 == Argument_Type.Entity ||
                      type_2 == Argument_Type.Entity) {

                        (type_1 == Argument_Type.Clause ||
                         type_2 == Argument_Type.Clause).tift();

                        if (type_1 == type_2) // Two entities is always legal
                              return;

                        // otherwise, we'll have to verify the type of the other 
                        // argument
                        type = (type_1 != Argument_Type.Entity) ? type_1 : type_2;

                  }
                  else {

                        // other operations make no sense
                        (type_1 == type_2).tiff();

                        type = type_1;
                  }

                  bool ok;

                  if (type == Argument_Type.Clause) {
                        ok = (Operation.And == operation) ||
                                (Operation.Or == operation);
                  }
                  else {
                        var set = Query.Get_Supported_Operations(type);
                        ok = set[operation];
                  }

                  ok.tiff();
            }

            readonly List<Argument> subordinates;
            readonly List<Clause> sub_clauses;

            public List<Argument> Subordinates {
                  get { return subordinates; }
            }

            public List<Clause> Sub_Clauses {
                  get { return sub_clauses; }
            }


            readonly Argument m_arg_1;

            readonly Argument m_arg_2;

            readonly Operation m_operation;


            public Argument Arg_1 {
                  get {
                        return this.m_arg_1;
                  }
            }

            public Argument Arg_2 {
                  get {
                        return this.m_arg_2;
                  }
            }

            public Operation Operation {
                  get {
                        return this.m_operation;
                  }
            }


            public Clause(Clause clause1,
                          Clause clause2,
                          Operation operation)
                  : this(new Argument(Argument_Type.Clause, clause1),
                         new Argument(Argument_Type.Clause, clause2),
                         operation) { }

            public Clause(Argument arg_1,
                          Argument arg_2,
                          Operation operation) {

                  Ensure_Type(operation, arg_1, arg_2);

                  this.m_arg_1 = arg_1;
                  this.m_arg_2 = arg_2;
                  this.m_operation = operation;

                  subordinates = new List<Argument>();
                  sub_clauses = new List<Clause>();

                  subordinates.Add(arg_1);
                  subordinates.Add(arg_2);

                  foreach (var clause in from arg in new Argument[] { arg_1, arg_2 }
                                         where arg.Type == Argument_Type.Clause
                                         let clause = (Clause)arg.Data
                                         select clause) {

                        subordinates.AddRange(clause.subordinates);
                        sub_clauses.Add(clause);

                        // I should comment this line out if I ever need
                        // to access the arguments 
                        clause.subordinates.Clear();

                  }

            }

            #region Clause


            /* Boilerplate */

            public override string ToString() {

                  string ret = "";

                  ret += "arg_1 = " + this.m_arg_1;
                  ret += ", ";
                  ret += "arg_2 = " + this.m_arg_2;
                  ret += ", ";
                  ret += "operation = " + this.m_operation;

                  ret = "{Clause: " + ret + "}";
                  return ret;

            }

            public bool Equals(Clause obj2) {

                  if (!this.m_arg_1.Equals(obj2.m_arg_1))
                        return false;

                  if (!this.m_arg_2.Equals(obj2.m_arg_2))
                        return false;

                  if (!this.m_operation.Equals(obj2.m_operation))
                        return false;

                  return true;
            }

            public override bool Equals(object obj2) {

                  var ret = (obj2 != null && obj2 is Clause);

                  if (ret)
                        ret = this.Equals((Clause)obj2);


                  return ret;

            }

            public static bool operator ==(Clause left, Clause right) {

                  var ret = left.Equals(right);
                  return ret;

            }

            public static bool operator !=(Clause left, Clause right) {

                  var ret = !left.Equals(right);
                  return ret;

            }

            public override int GetHashCode() {

                  unchecked {
                        int ret = 23;
                        int temp;

                        ret *= 31;
                        temp = this.m_arg_1.GetHashCode();
                        ret += temp;

                        ret *= 31;
                        temp = this.m_arg_2.GetHashCode();
                        ret += temp;

                        ret *= 31;
                        temp = this.m_operation.GetHashCode();
                        ret += temp;

                        return ret;
                  }
            }

            #endregion
      }



      public struct Argument
      {
            internal const int arg_type_constant = 0x1000;


            static void Ensure_Type(Argument_Type type, ref object data) {

                  switch (type) {
                        case Argument_Type.String:
                        case Argument_Type.Entity:
                              (data is string).tiff();
                              break;

                        case Argument_Type.Clause:
                              (data is Clause).tiff();
                              break;

                        // 18 oct
                        case Argument_Type.Decimal:
                              if (data is double)
                                    data = Convert.ToDecimal(data);
                              (data is decimal).tiff();

                              break;

                        // 18 oct
                        case Argument_Type.Integer:
                        case Argument_Type.Bool:
                              data = Convert.ToInt32(data);
                              break;

                        case Argument_Type.Date:
                              data = Convert.ToDateTime(data).Date;
                              break;

                        case Argument_Type.Null:
                              break;

                        default:
                              throw new ApplicationException();
                  }
            }

            readonly Argument_Type m_type;

            readonly object m_data;


            public Argument_Type Type {
                  get {
                        return this.m_type;
                  }
            }

            public object Data {
                  get {
                        return this.m_data;
                  }
            }


            public Argument(Argument_Type type,
                            object data) {

                  Ensure_Type(type, ref data);
                  this.m_type = type;
                  this.m_data = data;
            }

            #region Argument

            /* Boilerplate */

            public override string ToString() {

                  string ret = "";

                  ret += "type = " + this.m_type;
                  ret += ", ";
                  ret += "data = " + this.m_data;

                  ret = "{Argument: " + ret + "}";
                  return ret;

            }

            public bool Equals(Argument obj2) {


#pragma warning disable
                  if (this.m_type == null) {
                        if (obj2.m_type != null)
                              return false;
                  }
                  else {
                        if (!this.m_type.Equals(obj2.m_type))
                              return false;
                  }

                  if (this.m_data == null) {
                        if (obj2.m_data != null)
                              return false;
                  }
                  else {
                        if (!this.m_data.Equals(obj2.m_data))
                              return false;
                  }
#pragma warning restore



                  return true;
            }

            public override bool Equals(object obj2) {

                  var ret = (obj2 != null && obj2 is Argument);

                  if (ret)
                        ret = this.Equals((Argument)obj2);


                  return ret;

            }

            public static bool operator ==(Argument left, Argument right) {

                  var ret = left.Equals(right);
                  return ret;

            }

            public static bool operator !=(Argument left, Argument right) {

                  var ret = !left.Equals(right);
                  return ret;

            }

            public override int GetHashCode() {

                  unchecked {
                        int ret = 23;
                        int temp;

                        ret *= 31;
                        temp = this.m_type.GetHashCode();
                        ret += temp;

                        ret *= 31;
                        temp = this.m_data.GetHashCode();
                        ret += temp;

                        return ret;
                  }
            }

            #endregion
      }
}
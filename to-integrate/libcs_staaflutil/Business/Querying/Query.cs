using System;
using System.Collections.Generic;


using Fairweather.Service;
using Fairweather.Service.Syntax;

namespace Common.Queries
{
    public static class Query
    {
        const int arg_type_constant = Argument.arg_type_constant;

        static bool Compare(Argument arg1, Argument arg2, bool less_than, bool strict) {

            var data1 = arg1.Data;
            var data2 = arg2.Data;
            int cmp;

            switch (arg1.Type) {
                case Argument_Type.String:
                    cmp = String.Compare(data1.ToString(),
                                         data2.ToString());
                    break;

                case Argument_Type.Decimal:
                    cmp = decimal.Compare((decimal)data1, (decimal)data2);

                    break;

                case Argument_Type.Integer:
                    cmp = (int)data1 < (int)data2 ? -1 :
                               data1.Equals(data2) ? 0 :
                               -1;


                    break;
                case Argument_Type.Date: {
                        var day1 = ((DateTime)data1).Comparable_Day();
                        var day2 = ((DateTime)data2).Comparable_Day();
                        cmp = day1.CompareTo(day2);
                        break;
                    }
                case Argument_Type.Bool:
                case Argument_Type.Entity:
                case Argument_Type.Clause:
                default:
                    throw new ApplicationException();
            }


            var ret = (!strict && cmp == 0) || (less_than == (cmp == -1));

            return ret;

        }

        static bool Equate(Argument arg1, Argument arg2, bool equals) {

            bool ret;

            var data1 = arg1.Data;
            var data2 = arg2.Data;

            switch (arg1.Type) {

                case Argument_Type.String:
                    ret = data1.ToString() == data2.ToString();
                    break;

                case Argument_Type.Decimal:
                case Argument_Type.Bool:
                case Argument_Type.Integer:
                case Argument_Type.Date:
                    ret = data1.Equals(data2);
                    break;

                case Argument_Type.Entity:
                case Argument_Type.Clause:
                default:
                    throw new ApplicationException();

            }

            ret = (ret == equals);

            return ret;

        }

        static bool Evaluate_Operation(Operation op, Argument arg1,
                                                     Argument arg2) {

            var data1 = arg1.Data;
            var data2 = arg2.Data;

            switch (op) {
                case Operation.And:
                    return ((bool)data1 && (bool)data2);

                case Operation.Or:
                    return ((bool)data1 || (bool)data2);

                case Operation.Greater_Than:
                case Operation.Less_Than:

                    return Compare(arg1, arg2, op == Operation.Less_Than, true);

                case Operation.Greater_Or_Equal:
                case Operation.Less_Or_Equal:


                    return Compare(arg1, arg2, op == Operation.Less_Or_Equal, false);

                case Operation.Equal_To:
                case Operation.Not_Equal_To:

                    return Equate(arg1, arg2, op == Operation.Equal_To);

                case Operation.Contains:

                    return data1.ToString().Contains(data2.ToString());

                case Operation.Contains_Caseless:
                    return data1.ToString()
                                .ToUpper().Contains(data2.ToString()
                                                         .ToUpper());

                default:
                    throw new ApplicationException();
            }


        }

        static
        Query_Predicate Combine(Operation operation, Clause clause1,
                                                                       Clause clause2)
            //where TEntity : IEntity 
        {

            var pred1 = Compile(clause1);
            var pred2 = Compile(clause2);

            if (operation == Operation.And) {

                return (entity) =>
                {
                    return pred1(entity) && pred2(entity);
                };

            }

            if (operation == Operation.Or) {

                return (entity) =>
                {
                    return pred1(entity) || pred2(entity);
                };

            }

            throw new ApplicationException();
        }

        public static
        Query_Predicate Compile(Clause clause) {

            var arg1 = clause.Arg_1;
            var arg2 = clause.Arg_2;

            var type1 = arg1.Type;
            var type2 = arg2.Type;

            var data1 = arg1.Data;
            var data2 = arg2.Data;

            var operation = clause.Operation;

            bool cst1 = type1.Contains(arg_type_constant);
            bool cst2 = type2.Contains(arg_type_constant);

            bool clauses = (arg1.Type == Argument_Type.Clause);

            bool ent1 = (type1 == Argument_Type.Entity);
            bool ent2 = (type2 == Argument_Type.Entity);


            if (cst1 && cst2) {

                bool _ret = Evaluate_Operation(operation, arg1, arg2);
                return _ => _ret;

            }

            if (ent1 || ent2) {

                return entity =>
                {
                    var _arg1 = entity.Instantiate(arg1);
                    var _arg2 = entity.Instantiate(arg2);
                    var _type1 = _arg1.Type;
                    var _type2 = _arg2.Type;

                    if (_type1 == Argument_Type.Null || _type2 == Argument_Type.Null) {

                        bool both_are_null = (_type1 == Argument_Type.Null) &&
                              (_type2 == Argument_Type.Null);

                        if (operation == Operation.Equal_To)
                            return both_are_null;

                        if (operation == Operation.Not_Equal_To)
                            return !both_are_null;

                    }
                    return (_type1 == _type2) && Evaluate_Operation(operation, _arg1, _arg2);
                };

            }

            if (clauses) {

                return Combine(operation, (Clause)data1, (Clause)data2);

            }
            throw new ApplicationException();

        }

        //public static
        //Clause Make_Or_Equal_Clause(Argument arg1,
        //                            Argument arg2,
        //                            bool less) {

        //    var clause1 = new Clause(arg1, arg2, less ? Operation.Less_Than :
        //                                                Operation.Greater_Than);
        //    var clause2 = new Clause(arg1, arg2, Operation.Equal_To);

        //    var ret = new Clause(clause1, clause2, Operation.Or);

        //    return ret;

        //}

        public static
        Clause Make_Between_Clause(Argument middle,
                                   Argument left,
                                   Argument right) {

            var ge = new Clause(left, middle, Operation.Greater_Or_Equal);
            var le = new Clause(middle, right, Operation.Less_Or_Equal);

            var ret = new Clause(le, ge, Operation.And);

            return ret;

        }

        public static
        Clause Make_Chained_Clause(Argument_Type left_argument,
                                   Argument_Type right_argument,
                                   Operation op,
                                   bool and,
                                   List<Pair<object>> arguments) {

            Operation join = and ? Operation.And : Operation.Or;
            Clause ret;

            Clause? main_clause = null;

            foreach (var pair in arguments) {

                var arg1 = new Argument(left_argument, pair.First);
                var arg2 = new Argument(right_argument, pair.Second);

                var temp_clause = new Clause(arg1, arg2, op);

                if (!main_clause.HasValue) {
                    main_clause = temp_clause;
                    continue;
                }

                var arg_clause_1 = new Argument(Argument_Type.Clause, main_clause);
                var arg_clause_2 = new Argument(Argument_Type.Clause, temp_clause);

                main_clause = new Clause(arg_clause_1, arg_clause_2, join);

            }

            ret = main_clause.Value;

            return ret;

        }
        public static Clause
        Create_Structured_Query(IToken[] tokens) {

            var ast = BinaryAST.Make(tokens);

            var clauses = ast.Tree.Transform(node =>
            {
                Clause clause;

                if (node.Type == Token_Type.Binary_Operator) {
                    clause = new Clause(default(Argument), default(Argument), (Operation)node.Data);
                }
                else if (node.Type == Token_Type.Argument) {
                    clause = (Clause)node.Data;
                }
                else {
                    true.tift();
                    throw new InvalidOperationException();
                }

                return clause;

            });

            Func<Clause, Clause?, Clause?, Clause>
            walker = (par, lch, rch) =>
            {
                if (!lch.HasValue) {

                    rch.HasValue.tift();
                    return par;

                }

                var arg1 = new Argument(Argument_Type.Clause, lch.Value);
                var arg2 = new Argument(Argument_Type.Clause, rch.Value);

                return new Clause(arg1, arg2, par.Operation);
            };

            /*       Construct the individual clauses        */

            var clauses2 = clauses.Walk_From_Bottom(walker);

            /*       Tie them together                       */


            var ret = clauses2.Value;

            return ret;
        }


        static Dictionary<Argument_Type, Set<Operation>> dict_supported_operations;

        static bool Prepare_Dict_Supported_Operations() {

            if (dict_supported_operations != null)
                return false;

            var for_int = new Set<Operation>{Operation.Equal_To,
                                                     Operation.Not_Equal_To,
                                                     Operation.Greater_Than,
                                                     Operation.Less_Than,
                                                     Operation.Greater_Or_Equal,
                                                     Operation.Less_Or_Equal};

            var for_string = new Set<Operation>{Operation.Contains,
                                                    Operation.Contains_Caseless};

            for_string.AddRange(for_int);


            var for_bool = new Set<Operation>{Operation.Equal_To,
                                                                        Operation.Not_Equal_To};

            var for_date = for_int;

            var dict = new Dictionary<Argument_Type, Set<Operation>>
            {
                  {Argument_Type.String,  for_string},
                  {Argument_Type.Integer, for_int},
                  {Argument_Type.Bool, for_bool},
                  {Argument_Type.Date, for_date},
            };

            dict[Argument_Type.Decimal] = for_int;

            dict_supported_operations = dict;

            return true;
        }

        public static Set<Operation>
        Get_Supported_Operations(Argument_Type argtype) {


            Set<Operation> ret;

            Prepare_Dict_Supported_Operations();

            bool ok = dict_supported_operations.TryGetValue(argtype, out ret);

            if (ok)
                return ret;

            true.tift("Invalid argument type: " + argtype.Get_String());
            return null;
        }



        public static
        Clause Make_Clause(Operation operation,
                                       Argument arg1,
                                       params Argument[] args) {

            Clause ret;
            var arg2 = args[0];

            if (operation == Operation.Between) {
                var arg3 = args[1];

                ret = Query.Make_Between_Clause(arg1, arg2, arg3);

                return ret;
            }

            ret = new Clause(arg1, arg2, operation);

            return ret;
        }

    }




}

using System;

namespace Common
{
    public class XCyclic_Dependency : XOur
    {
        const string str_format = "{0}\n" +
                                  "({1}, {2})\n" +
                                  "{3} to {4}\n" +
                                  "[{5} {6} {7}]";

        public XCyclic_Dependency(string type, int column, int row,
                                         object old_value, object new_value,
                                         int column1, int column2,
                                         string direction_indicator)

            : base(String.Format(str_format,
                    type,
                /*(*/ column, /*,*/ row /*)*/,
                    old_value, /*to*/ new_value,
                /*[*/ column1, direction_indicator, column2 /*]*/ )) { }
    }
}

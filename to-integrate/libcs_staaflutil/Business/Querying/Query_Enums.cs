

namespace Common.Queries
{

      public enum Argument_Type
      {
            Place_Holder = 0x0,
            Entity = 0x1,
            Clause,

            String = 0x1 | Argument.arg_type_constant,
            Decimal = 0x2 | Argument.arg_type_constant,
            Bool = 0x3 | Argument.arg_type_constant,
            Integer = 0x4 | Argument.arg_type_constant,
            Date = 0x5 | Argument.arg_type_constant,
            Null,
      }

      public enum Operation
      {
            And = 0x1,
            Or,

            Contains,
            Contains_Caseless,

            Greater_Than,
            Less_Than,
            Greater_Or_Equal,
            Less_Or_Equal,
            Equal_To,
            Not_Equal_To,

            Between,
      }





}
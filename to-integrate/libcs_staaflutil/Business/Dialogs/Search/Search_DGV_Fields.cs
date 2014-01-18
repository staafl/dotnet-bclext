using System.Collections.Generic;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    partial class Search_DGV
    {
        Triple<int, string, string>[] m_sage_fields;

        const int MAX_ROWS = 4;

        Numeric_Box nb_integer;
        Our_Date_Time dt_date;
        TextBox tb_string;
        Numeric_Box nb_decimal;
        ComboBox cb_yesno;

        ComboBox cb_field;

        ComboBox cb_precedence;

        ComboBox cb_where;
        ComboBox cb_andor;

        ComboBox[] m_cond_cbxs;

        public const int
        COL_JOIN = 0,
        COL_PRECEDENCE = 1,
        COL_FIELD = 2,
        COL_CONDITION = 3,
        COL_VALUE = 4;

        public const string
        WHERE = "Where",
        AND = "And",
        OR = "Or",
        NONE = "None",
        YES = "Yes",
        NO = "No",

        EQUAL = "Equal",
        NOT_EQUAL = "Not Equal",
        GREATER_THAN = "Greater Than",
        LESS_THAN = "Less Than",
        GREATER_OR_EQ = "Greater Than or Equal",
        LESS_OR_EQ = "Less Than or Equal",
        BETWEEN = "Between",
        CONTAINS = "Contains";


        const int
         CBX_WHERE = 0
        , CBX_ANDOR = 1

        // Indices of condition combo-boxes
        , CBX_STRING = 0
        , CBX_DATE = 1
        , CBX_DECIMAL = 2
        , CBX_INT32 = 3
        , CBX_BOOL = 4;




    }
}

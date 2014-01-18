namespace Common
{
    using System;
    using System.Windows.Forms;
    public interface IFree_Form
    {
        int COL_TT { get;  }
        int COL_ACCOUNT { get;  }
        int COL_DATE { get;  }
        int COL_REF { get;  }
        int COL_EX_REF { get;  }
        int COL_NOMINAL { get;  }
        int COL_DEPT { get;  }
        int COL_PROJ_REF { get;  }
        int COL_COST_CODE { get;  }
        int COL_DETAILS { get;  }
        int COL_NET { get;  }
        int COL_TAX_CODE { get;  }
        int COL_TAX { get;  }
        int COL_ALLOC { get;  }

        int ColumnCount { get; }

    }
}

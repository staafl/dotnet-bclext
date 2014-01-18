using System;
using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public class XStack_Unrolling : XOur
    {
        readonly Action action;

        public Action Action {
            get { return action; }
        }

        public XStack_Unrolling(Action action_p) {

            this.action = action_p;
        }

    }
}

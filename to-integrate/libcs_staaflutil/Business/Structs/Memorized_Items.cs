using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Common
{
    [Serializable]
    [DebuggerStepThrough]
    // Only used for serialization
    public class Memorized_Items : List<Memorized_Item>
    {
        public Memorized_Items(int capacity)
            : base(capacity) {
        }

    }
}

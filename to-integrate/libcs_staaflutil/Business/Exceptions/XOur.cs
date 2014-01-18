using System;
using System.Diagnostics;

namespace Common
{
      [global::System.Serializable]
      [DebuggerStepThrough]
      public abstract class XOur : ApplicationException
      {
            protected XOur() { }
            protected XOur(string message) : base(message) { }
            protected XOur(string message, Exception inner) : base(message, inner) { }
            protected XOur(
                System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context)
                  : base(info, context) { }
      }
      
}
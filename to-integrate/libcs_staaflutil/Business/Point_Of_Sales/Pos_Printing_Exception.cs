using System;

using Fairweather.Service;

namespace Fairweather.Service
{

      [Serializable]
      public class Pos_Printing_Exception : XPrinting
      {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public Pos_Printing_Exception() : base() { }
            public Pos_Printing_Exception(string message) : base(message) { }
            public Pos_Printing_Exception(string message, Exception inner) : base(message, inner) { }

            protected Pos_Printing_Exception(
                System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context)
                  : base(info, context) { }
      }
}

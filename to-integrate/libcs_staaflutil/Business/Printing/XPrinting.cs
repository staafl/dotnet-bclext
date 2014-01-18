using System;

namespace Fairweather.Service
{
    //[global::System.Serializable]
    public class XPrinting : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public XPrinting() { }
        public XPrinting(string message) : base(message) { }
        public XPrinting(string message, Exception inner) : base(message, inner) { }
        protected XPrinting(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

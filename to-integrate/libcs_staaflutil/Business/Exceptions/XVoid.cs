using System;

namespace Common
{
    [global::System.Serializable]
    public class XVoid : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public XVoid() { }
        public XVoid(string message) : base(message) { }
        public XVoid(string message, Exception inner) : base(message, inner) { }
        protected XVoid(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

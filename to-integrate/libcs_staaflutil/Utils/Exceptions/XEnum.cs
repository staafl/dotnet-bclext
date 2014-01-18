using System;

namespace Fairweather.Service
{
    [global::System.Serializable]
    public class XEnum : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public XEnum() { }
        public XEnum(string name, Enum value) : base("Invalid enum value: " + name + "=" + value) { }

        public XEnum(string message) : base(message) { }
        public XEnum(string message, Exception inner) : base(message, inner) { }
        protected XEnum(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

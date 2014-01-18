namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    public interface ICsvFormatter<T>
    {
        T Deserialize(IEnumerable<string> fields);
        IEnumerable<object> Serialize(T obj);
    }
}

namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    public interface ICsvFormatter2<T>
    {
        T Deserialize(IDictionary<string, object> fields);
        IDictionary<string, object> Serialize(T obj);
    }
}

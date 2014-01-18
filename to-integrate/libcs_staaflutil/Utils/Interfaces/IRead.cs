namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    public interface IRead<TKey, TValue> : IContains<TKey>
    {
        TValue this[TKey index] { get; }
    }
}

namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    public interface IContains<TKey>
    {
        bool Contains(TKey key);
    }
}

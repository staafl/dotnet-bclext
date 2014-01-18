namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    /* Use this if one of the indexers is going to be 
         * implemented explicitly.
         */
    public interface IReadWrite<TKey, TValue> : IContains<TKey>
    {
        TValue this[TKey index] { get; set; }
    }
}

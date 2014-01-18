namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    public interface IValue<TValue>
    {
        TValue Value { get; set; }
    }
}

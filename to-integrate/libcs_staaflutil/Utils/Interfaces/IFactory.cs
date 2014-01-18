namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;

    public interface IFactory<TClass, TArg>
    {
        TClass Create(TArg value);
    }
}
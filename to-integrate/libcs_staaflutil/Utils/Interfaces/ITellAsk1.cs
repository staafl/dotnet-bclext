namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    public interface ITellAsk<T> : ITellAsk
    {
        void Tell(string text, T data);
        bool Ask(string question, T data);
    }
}

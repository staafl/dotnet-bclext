namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    public interface ITellAsk
    {
        void Tell(string text);
        bool Ask(string question);
    }
}

using System;

namespace Fairweather.Service
{
    public delegate bool Exception_Handler<T>(T ex) where T : Exception;
}

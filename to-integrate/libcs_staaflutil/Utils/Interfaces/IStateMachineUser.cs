namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    public interface IState_Machine_User<TUser> where TUser : IState_Machine_User<TUser>
    {
        string InitialState { get; }
        char[] Steps { get; }
        string[] Nodes { get; }

        Dictionary<string, Dictionary<string, Action<TUser>>>
            Transitions { get; }

        Dictionary<string, string> Jumps { get; }
    }
}

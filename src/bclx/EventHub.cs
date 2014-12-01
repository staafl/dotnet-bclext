using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace bclx
{
    static public class EventHub
    {
        static readonly object padlock = new object();
        static readonly Dictionary<object, Action<object>> callbacks = new Dictionary<object, Action<object>>();

        public static void Subscribe(object eventId, Action<object> callback)
        {
            lock (padlock)
            {
                callbacks[eventId] += callback;
            }
        }

        public static void Unsubscribe(object eventId, Action<object> callback)
        {
            lock (padlock)
            {
                // ReSharper disable once DelegateSubtraction
                callbacks[eventId] -= callback;
            }
        }

        public static void Raise(object eventId, object argument, Action<Action<object>, object> invoker = null)
        {
            Action<object> callback;
            lock(padlock)
            {
                callback = callbacks[eventId];
            }
            if (callback != null)
            {
                if (invoker == null)
                {
                    callback(argument);
                }
                else
                {
                    foreach (Action<object> action in callback.GetInvocationList())
                    {
                        invoker(action, argument);
                    }
                }
            }
        }

        public static void Pulse(object eventId)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace bclx
{
    public class State
    {
        public static IDisposable Push<T>(Func<T> getter, [CallerMemberName] string methodName = null)
        {
            threadStack.Push(new StateItem<T>(getter, methodName));
            poppedStack.Clear();

            return new Popper();
        }

        public static void Log(Exception ex)
        {
            Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId);
            foreach (var stateItem in poppedStack.Concat(threadStack))
            {
                Console.WriteLine(stateItem.MethodName);
                var state = stateItem.GetObject();
                foreach (var prop in stateItem.GetProperties())
                {
                    Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(state));
                }
            }
            Console.WriteLine(ex);
        }

        [ThreadStatic]
        static readonly Stack<StateItem> threadStack = new Stack<StateItem>();
        [ThreadStatic]
        static readonly Stack<StateItem> poppedStack = new Stack<StateItem>();

        class Popper : IDisposable
        {
            public void Dispose()
            {
                poppedStack.Push(threadStack.Pop());
            }
        }

        abstract class StateItem
        {
            public StateItem(string methodName)
            {
                MethodName = methodName;
            }

            public abstract object GetObject();
            public abstract PropertyInfo[] GetProperties();

            public string MethodName { get; private set; }
        }

        class StateItem<T> : StateItem
        {
            public Func<T> Getter { get; private set; }

            [ThreadStatic]
            public static PropertyInfo[] properties;

            public StateItem(Func<T> getter, string methodName)
                : base(methodName)
            {
                Getter = getter;
            }

            public override PropertyInfo[] GetProperties()
            {
                properties = properties ?? typeof(T).GetProperties();
                return properties;
            }

            public override object GetObject()
            {
                return Getter();
            }
        }
    }
}
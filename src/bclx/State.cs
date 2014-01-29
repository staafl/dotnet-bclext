// MIT Software License / Expat License
// 
// Copyright (C) 2014 Velko Nikolov
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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

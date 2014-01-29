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
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

class Program
{
  interface IFoo
  {
    void Bar();
  }
  
  class Foo : IFoo
  {
    public void Bar() 
    {
      Console.WriteLine("Inside method");
    }
  }
  
  static void Main(string[] args) 
  {
      var proxy = new DebugProxy<IFoo>(new Foo());
      IFoo fooProxy = (IFoo)proxy.GetTransparentProxy();
      proxy.MethodInvoked += name => Console.WriteLine("{0} invoked", name);
      fooProxy.Bar();
  }
  
  public class DebugProxy<T> : RealProxy
  {
    readonly T inner;
    
    public DebugProxy(T inner)
        :base(typeof(T))
    {
      if (inner == null)
          throw new ArgumentNullException("inner");
      this.inner = inner;
    }
    
    public event Action<string> MethodInvoked;

    // [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    public override IMessage Invoke(IMessage msg)
    {
      if (msg == null)
          throw new ArgumentNullException("msg");
          
      var methodCallMessage = msg as IMethodCallMessage;

      if (methodCallMessage == null)
          throw new ArgumentException("msg as IMethodCallMessage == null");
          
      ReturnMessage responseMessage;
      Object response = null;
      Exception caughtException = null;

      try
      {
          String methodName = (String)msg.Properties["__MethodName"];
          Type[] parameterTypes = (Type[])msg.Properties["__MethodSignature"];
          var method = typeof(T).GetMethod(methodName, parameterTypes);

          object[] parameters = (object[])msg.Properties["__Args"];
          
          var invoked = MethodInvoked;
          if (invoked != null)
            invoked(methodName);
          
          response = method.Invoke(inner, parameters);
      }
      catch (Exception ex)
      {
          // Store the caught exception
          caughtException = ex;
      }


      if (caughtException == null)
          responseMessage = new ReturnMessage(response, null, 0, null, methodCallMessage);
      else
          responseMessage = new ReturnMessage(caughtException, methodCallMessage);

      return responseMessage;
    }
  }
}
  

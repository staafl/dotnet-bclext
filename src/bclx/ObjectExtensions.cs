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
namespace bclx
{
  public static class ObjectExtensions
  {
    public static void NullCheck<T>(this T obj) {
      foreach (var prop in typeof(T).GetPropertiesCache())
      {
        if (null == prop.GetValue(obj))
          throw new ArgumentNullException(prop.Name);
      }
    }
    
    public static void Log<T>(this Func<T> obj, TextWriter tw = null, [CallerMemberName] string methodName = null)
    {
        tw = tw ?? Console.Out;
        tw .WriteLine(methodName);
        foreach (var prop in typeof(T).GetProperties())
        {
            tw .WriteLine("{0}={1}", prop.Name, prop.GetValue(obj));
        }
    }

    public static void LogExpression(Expression<Func<object>> l, TextWriter tw = null)
    {
        tw = tw ?? Console.Out;
        tw.WriteLine("{0}={1}", l.Body.ToString(), l.Compile()());
    }
  }
}

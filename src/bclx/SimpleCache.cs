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
  public static class SimpleCache
  {
    public static T GetOrCreate<T>(Func<T> ctor, params object[] ctorArgs) {
      return SimpleCacheOfT<T>.GetOrCreate(ctorArgs);
    }
    
    public static T GetOrCreate<T>(params object[] ctorArgs) {
      return SimpleCacheOfT<T>.GetOrCreate(ctorArgs);
    }
    
    static class SimpleCacheOfT<T>
    {
      static readonly Dictionary<ArrayComparer, T> dict = new Dictionary<ArrayComparer, T>();
      
      public static T GetOrCreate(Func<T> ctor, params object[] ctorArgs)
      {
        T obj;
        var comparer = new ArrayComparer(ctorArgs);
        if (!dict.TryGetValue(comparer, out obj))
          dict[comparer] = obj = ctor();
        return obj;
      }
      
      public static T GetOrCreate(params object[] ctorArgs)
      {
        T obj;
        var comparer = new ArrayComparer(ctorArgs);
        if (!dict.TryGetValue(comparer, out obj))
          dict[comparer] = obj = (T)Activator.CreateInstance(typeof(T), ctorArgs);
        return obj;
      }
    }
  }
}

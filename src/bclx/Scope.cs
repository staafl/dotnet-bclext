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
using System.Threading;

namespace bclx
{
  // single threaded
  public abstract class Scope<TChild, TKey> where TChild : Scope<TChild, TKey>, new()
  {
    protected abstract TChild CreateChild(TKey key);
    
    public static IDisposable Open(TKey key) {
      if (Current(key) == null)
      {
        openedScopes = openedScopes ?? new Dictionary<TKey,TChild>();
        openedScopes[key] = new TChild().CreateChild(key);
        return Disposable.Action(() => Close(key));
      }
      else
      {
        return Disposable.Empty;
      }
    }
    
    private static bool Close(TKey key) {
      if (openedScopes == null)
        return false;
      if (openedScopes.Remove(key))
      {
        if (openedScopes.Count == 0)
          openedScopes = null;
        return true;
      }
      else 
      {
        return false;
      }
    }
    
    public static TChild Current(TKey key) {
      if (openedScopes == null)
        return null;
      TChild child;
      if (openedScopes.TryGetValue(key, out child))
        return child;
      else
        return null;
    }
    
    [ThreadStatic]
    static Dictionary<TKey, TChild> openedScopes;
  }
}

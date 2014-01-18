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
  class ArrayComparer
  {
    public ArrayComparer(object[] array)
    {
      if (array == null)
        throw new ArgumentNullException();
      this.array = array;
    }
    public override bool Equals(object other) {
      var asArrayComparer = other as ArrayComparer;
      if (asArrayComparer == null)
        return false;
      return this.array.Length == asArrayComparer.array.Length && 
        this.array.SequenceEqual(asArrayComparer.array);
    }
    
    readonly object[] array;
    int hash;
    
    public override int GetHashCode() {
      if (this.hash != 0)
        return this.hash;
      if (this.array.Length == 0)
        return 0;
      this.hash = int.MinValue;
      foreach(var elem in array)
        if (elem != null) {
          this.hash = elem.GetHashCode();
          break;
        }
        
      return int.MinValue;
    }
  }
}

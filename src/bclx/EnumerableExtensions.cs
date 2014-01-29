// MIT Software License / Expat License
// 
// Copyright (C) 2014 Velko Nikolov
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace bclx
{
    public static class EnumerableExtensions
    {
        // orders according to another sequence's order
        public static IEnumerable<T> OrderBy<T, T2>(
            this IEnumerable<T> seq, 
                IEnumerable<T2> seq2, 
                Func<T, T2> mapping)
        {
            var dict = new Dictionary<T2, int>();
            int counter = 0;
            foreach (var elem in seq2)
            {
              dict[elem] = counter;
              counter += 1;
            }
            return seq.OrderBy(elem => { 
                dict[mapping(elem)];
            });
        }

        public static IEnumerable<Tuple<T, TTag>> Tag<T, TTag>(this IEnumerable<T> seq, TTag tag)
        {
            foreach (var elem in seq)
                yield return Tuple.Create(elem, tag);
        }

        public static IEnumerable<dynamic> DuckType<T>(this IEnumerable<object> seq, params string[] propNames)
        {
            foreach (object elem in seq)
            {
                if (elem == null)
                    continue;
          
                var type = elem.GetType();
          
                bool ok = true;
                foreach (var propName in propNames)
                {
                    var prop = type.GetProperty(propName);
                    if (prop == null || !typeof(T).IsAssignableFrom(prop.PropertyType))
                    {
                        ok = false;
                        break;
                    }
                }
                if (!ok)
                    continue;
                yield return elem;
            }
        }
    }
}

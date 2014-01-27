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
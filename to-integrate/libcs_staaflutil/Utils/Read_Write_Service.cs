
namespace Fairweather.Service
{
    public static class ReadWrite
    {
        class Proxy<TKey, TValue> : IRead<TKey, TValue>
        {
            readonly IReadWrite<TKey, TValue> rd_inner;

            public TValue this[TKey index] {
                get { return rd_inner[index]; }
            }

            public Proxy(IReadWrite<TKey, TValue> inner) {
                rd_inner = inner;
            }

            public bool Contains(TKey key) {
                return rd_inner.Contains(key);
            }
        }

        public static IRead<TKey, TValue>
        As_Readonly<TKey, TValue>(this IReadWrite<TKey, TValue> irw) {

            return new Proxy<TKey, TValue>(irw);

        }
    }
}
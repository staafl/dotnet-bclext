using System.Collections.Generic;

namespace Fairweather.Service
{

    // Credit: Jared Parsons' BCL Helpers


    /// <summary>
    /// Used to simply the creation of certain collection classses.  Allows the caller
    /// to rely on type inference rather than having to type out the specific classes 
    /// 
    /// This is really helpful for anonymous types where it's not possible to specify 
    /// the actual type name.  You must rely on inference to create the type
    /// </summary>
    public static class CollectionFactory
    {
        
        public static T GetType<T>(T value) {
            return default(T);
        }

        // List<T>

        
        public static List<T> CreateList<T>(T type) {
            return new List<T>();
        }

        public static List<T> CreateList<T>(IEnumerable<T> enumerable) {
            return new List<T>(enumerable);
        }

        

        // Stack<T>

        
        public static Stack<T> CreateStack<T>(T type) {
            return new Stack<T>();
        }

        public static Stack<T> CreateStack<T>(IEnumerable<T> enumerable) {
            return new Stack<T>(enumerable);
        }

        

        // Queue<T>

        
        public static Queue<T> CreateQueue<T>(T type) {
            return new Queue<T>();
        }

        public static Queue<T> CreateQueue<T>(IEnumerable<T> enumerable) {
            return new Queue<T>(enumerable);
        }

        

        // Dictionary<TKey,TValue>


        public static Dictionary<TKey, TValue> Create<TKey, TValue>(TKey keyType, TValue valueType) {
            return new Dictionary<TKey, TValue>();
        }

        
    }
}

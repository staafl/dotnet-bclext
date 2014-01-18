using System;

namespace Fairweather.Service
{
    // Credit: Jared Parsons' BCL Helpers
    public static class Singleton
    {
        static class Storage<T>
        {
            public static T s_instance;
        }

        
        public static T GetInstance<T>(Func<T> op) {
            if (Storage<T>.s_instance == null) {
                lock (typeof(Storage<T>)) {
                    if (Storage<T>.s_instance == null) {
                        T temp = op();
                        System.Threading.Thread.MemoryBarrier();
                        Storage<T>.s_instance = temp;
                    }
                }
            }
            return Storage<T>.s_instance;
        }

        public static T GetInstance<T>()
            where T : new() {
            return GetInstance(() => new T());
        }
    }
}

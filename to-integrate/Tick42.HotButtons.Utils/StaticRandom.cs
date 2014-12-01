using System;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    /// Provides a per-thread Random instance.
    /// </summary>
    public static class StaticRandom
    {
        [ThreadStatic]
        private static Random rand_;

        public static Random Instance
        {
            get
            {
                return rand_ = (rand_ ?? new Random());
            }
        }
    }
}
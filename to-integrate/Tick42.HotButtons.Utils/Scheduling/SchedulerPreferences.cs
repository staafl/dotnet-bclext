using System;
using System.Reactive.Concurrency;

namespace Tick42.HotButtons.Utils.Scheduling
{
    /// <summary>
    ///     Allows a sequence of users to specify ISchedulerCollection and SchedulerType preferences
    ///     (see SchedulerProvideHelpers.Cascade).
    ///     Returns an IScheduler that honors the earliest-specified preferences, or reverts to
    ///     sensible defaults if none.
    /// </summary>
    public class SchedulerPreferences
    {
        private static ISchedulerCollection explicitDefaultCollection_;

        public SchedulerPreferences(
            ISchedulerCollection schedulerCollection = null,
            SchedulerType? schedulerType = null)
        {
            ExplicitSchedulerCollection = schedulerCollection;
            ExplicitSchedulerType = schedulerType;
        }

        public static ISchedulerCollection DefaultCollection
        {
            get { return explicitDefaultCollection_ ?? new StandardSchedulerCollection(); }
            set { explicitDefaultCollection_ = value; }
        }

        public SchedulerType? ExplicitSchedulerType { get; private set; }
        public ISchedulerCollection ExplicitSchedulerCollection { get; private set; }

        /// <summary>
        /// Retrieves an IScheduler instance respecting the preferences specified so far. If no ExplicitSchedulerType has been
        /// specified, 'suggestedType' is used. If no ExplicitSchedulerCollection has been specified, 
        /// SchedulerPreferences.DefaultCollection is used.
        /// </summary>
        /// <param name="suggestedType"></param>
        /// <returns></returns>
        [Obsolete("Not meant to be called directly. Use SchedulerPreferencesHelpers.GetSchedulerSafe instead.")]
        public IScheduler GetScheduler(SchedulerType suggestedType)
        {
            SchedulerType realType = ExplicitSchedulerType ?? suggestedType;
            ISchedulerCollection realCollection = ExplicitSchedulerCollection ?? DefaultCollection;
            return realCollection.GetScheduler(realType);
        }
    }
}
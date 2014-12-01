using System;
using System.Reactive.Concurrency;

namespace Tick42.HotButtons.Utils.Scheduling
{
    /// <summary>
    ///     Helper methods for schedulerPreferences.
    /// </summary>
    public static class SchedulerPreferencesHelpers
    {
        /// <summary>
        ///     Allows you to retrieve an IScheduler instance even if the schedulerPreferences is null. See SchedulerPreferences.GetScheduler()
        /// </summary>
        public static IScheduler GetSchedulerSafe(
            this SchedulerPreferences schedulerPreferences,
            SchedulerType schedulerType)
        {
#pragma warning disable 618 // schedulerPreferences.GetScheduler is obsolete
            // schedulerPreferences.GetScheduler is obsolete precisely so users will call GetSchedulerSafe
            return (schedulerPreferences ?? new SchedulerPreferences()).GetScheduler(schedulerType);
#pragma warning restore 618
        }

        /// <summary>
        ///     Allows you to specify preferences for a scheduler type for the IScheduler which will be returned. Can be called on null.
        /// </summary>
        /// <returns></returns>
        public static SchedulerPreferences Cascade(
            this SchedulerPreferences schedulerPreferences,
            SchedulerType schedulerType)
        {
            if (schedulerPreferences == null)
            {
                return new SchedulerPreferences(null, schedulerType);
            }
            if (schedulerPreferences.ExplicitSchedulerType != null)
            {
                return schedulerPreferences;
            }
            return new SchedulerPreferences(schedulerPreferences.ExplicitSchedulerCollection, schedulerType);
        }

        /// <summary>
        ///     Allows you to specify preferences for a scheduler collection containing the IScheduler which will be returned. Can be called on null.
        /// </summary>
        [Obsolete("Using this method specifies an explicit schedulerCollection, which will prevent testing injection")]
        public static SchedulerPreferences Cascade(
            this SchedulerPreferences schedulerPreferences,
            ISchedulerCollection schedulerCollection)
        {
            if (schedulerPreferences == null)
            {
                return new SchedulerPreferences(schedulerCollection, null);
            }
            if (schedulerPreferences.ExplicitSchedulerCollection != null)
            {
                return schedulerPreferences;
            }
            return new SchedulerPreferences(schedulerCollection, schedulerPreferences.ExplicitSchedulerType);
        }
    }
}
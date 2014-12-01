using System;
using System.Reactive.Concurrency;

namespace Tick42.HotButtons.Utils.Scheduling
{
    /// <summary>
    ///     Represents a scheduler collection using the standard Scheduler/DispatcherScheduler static properties.
    /// </summary>
    public sealed class StandardSchedulerCollection : ISchedulerCollection
    {
        public IScheduler GetScheduler(SchedulerType schedulerType)
        {
            switch (schedulerType)
            {
                case SchedulerType.CurrentThread:
#pragma warning disable 618 // Obsoleted scheduler properties
                    return Scheduler.CurrentThread;
                case SchedulerType.Dispatcher:
                    return DispatcherScheduler.Instance;
                case SchedulerType.Immediate:
                    return Scheduler.Immediate;
                case SchedulerType.NewThread:
                    return Scheduler.NewThread;
                case SchedulerType.ThreadPool:
                    return Scheduler.ThreadPool;
#if !NET35
                case SchedulerType.TaskPool:
                    return Scheduler.TaskPool;
#endif
#pragma warning restore 618
                default:
                    throw new ArgumentOutOfRangeException("schedulerType: " + schedulerType);
            }
        }
    }
}
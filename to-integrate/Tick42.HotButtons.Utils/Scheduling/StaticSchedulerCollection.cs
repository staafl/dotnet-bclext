using System;
using System.Reactive.Concurrency;

namespace Tick42.HotButtons.Utils.Scheduling
{
    /// <summary>
    ///     Represents a scheduler collection using a single scheduler. Instantiate with TestScheduler for testing.
    /// </summary>
    public sealed class StaticSchedulerCollection : ISchedulerCollection
    {
        private readonly IScheduler scheduler_;

        public StaticSchedulerCollection(IScheduler scheduler)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException("scheduler");
            }
            scheduler_ = scheduler;
        }

        public IScheduler GetScheduler(SchedulerType schedulerType)
        {
            switch (schedulerType)
            {
                case SchedulerType.CurrentThread:
                case SchedulerType.Dispatcher:
                case SchedulerType.Immediate:
                case SchedulerType.NewThread:
                case SchedulerType.ThreadPool:
#if !NET35
                case SchedulerType.TaskPool:
#endif
                    return scheduler_;
                default:
                    throw new ArgumentOutOfRangeException("schedulerType: " + schedulerType);
            }
        }
    }
}
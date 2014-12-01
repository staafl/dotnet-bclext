using System.Reactive.Concurrency;

namespace Tick42.HotButtons.Utils.Scheduling
{
    /// <summary>
    ///     Represents a set of IScheduler instances.
    /// </summary>
    public interface ISchedulerCollection
    {
        IScheduler GetScheduler(SchedulerType schedulerType);
    }
}
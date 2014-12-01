namespace Tick42.HotButtons.Utils.Scheduling
{
    /// <summary>
    ///     Types of schedulers returned by an ISchedulerCollection.GetScheduler
    /// </summary>
    public enum SchedulerType
    {
        CurrentThread,
        Dispatcher,
        Immediate,
        NewThread,
        ThreadPool,
#if !NET35
        TaskPool,
#endif
    }
}
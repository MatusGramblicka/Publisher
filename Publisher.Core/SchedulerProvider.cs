using Publisher.Interface;
using System.Reactive.Concurrency;

namespace Publisher.Core;

public class SchedulerProvider : ISchedulerProvider
{
    public IScheduler CurrentThread => Scheduler.CurrentThread;

    public IScheduler Immediate => Scheduler.Immediate;

    public IScheduler Default => Scheduler.Default;

    public IScheduler NewThread => NewThreadScheduler.Default;
}
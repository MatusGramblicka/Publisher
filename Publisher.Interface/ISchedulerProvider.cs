using System.Reactive.Concurrency;

namespace Publisher.Interface;

public interface ISchedulerProvider
{
    IScheduler CurrentThread { get; }
    IScheduler Immediate { get; }
    IScheduler Default { get; }
    IScheduler NewThread { get; }
}
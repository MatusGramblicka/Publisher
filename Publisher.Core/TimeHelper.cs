using Publisher.Interface;

namespace Publisher.Core;

public class TimeHelper : ITimeHelper
{
    private readonly ISchedulerProvider _schedulerProvider;

    public TimeHelper(ISchedulerProvider schedulerProvider)
    {
        _schedulerProvider = schedulerProvider;
    }

    public DateTime GetNow() => _schedulerProvider.CurrentThread.Now.DateTime;

    public int GetSecondsDifferenceFromNow(DateTime dateCreated)
    {
       return Math.Abs((GetNow() - dateCreated).Seconds);
    }
}
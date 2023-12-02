namespace Publisher.Interface;

public interface ITimeHelper
{
    DateTime GetNow();
    int GetSecondsDifferenceFromNow(DateTime dateCreated);
}
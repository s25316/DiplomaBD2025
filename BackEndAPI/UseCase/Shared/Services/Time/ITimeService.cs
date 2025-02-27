namespace UseCase.Shared.Services.Time
{
    public interface ITimeService
    {
        DateTime GetNow();
        DateTime FromDateOnly(DateOnly dateOnly);
    }
}

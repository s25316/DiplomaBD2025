﻿
namespace UseCase.Shared.Services.Time
{
    public class TimeService : ITimeService
    {
        public DateTime GetNow() => DateTime.Now;
        public DateTime FromDateOnly(DateOnly dateOnly) => dateOnly.ToDateTime(TimeOnly.MinValue);
    }
}

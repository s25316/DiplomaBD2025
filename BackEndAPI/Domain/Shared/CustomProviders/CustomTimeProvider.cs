namespace Domain.Shared.CustomProviders
{
    public static class CustomTimeProvider
    {
        public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));//DateTime.Now.ToLocalTime();
        public static DateOnly Today => DateOnly.FromDateTime(Now);
        //public static DateTime GetDateTime(DateOnly dateOnly) => dateOnly.ToDateTime(TimeOnly.MinValue);
        //public static DateOnly GetDateOnly(DateTime dateTime) => DateOnly.FromDateTime(dateTime);
    }
}

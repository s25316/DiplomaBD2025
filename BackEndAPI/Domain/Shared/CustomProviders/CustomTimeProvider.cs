namespace Domain.Shared.CustomProviders
{
    public static class CustomTimeProvider
    {
        public static DateTime GetDateTimeNow() => DateTime.Now.ToLocalTime();
        public static DateTime GetDateTime(DateOnly dateOnly) => dateOnly.ToDateTime(TimeOnly.MinValue);
        public static DateOnly GetDateOnly(DateTime dateTime) => DateOnly.FromDateTime(dateTime);
    }
}

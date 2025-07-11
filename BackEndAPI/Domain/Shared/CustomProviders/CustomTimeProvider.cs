﻿namespace Domain.Shared.CustomProviders
{
    public static class CustomTimeProvider
    {
        public static DateTime Now => ConvertToPoland(DateTime.UtcNow);
        public static DateOnly Today => DateOnly.FromDateTime(Now);

        public static DateTime ConvertToPoland(DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
            return TimeZoneInfo.ConvertTimeFromUtc(
            dateTime,
            TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
        }

        public static DateOnly GetDateOnly(DateTime dateTime) => DateOnly.FromDateTime(dateTime);

        public static int GetDays(DateTime dateTime1, DateTime? dateTime2 = null)
        {
            dateTime2 ??= Now;

            if (dateTime1 > dateTime2)
            {
                var min = dateTime2;
                dateTime2 = dateTime1;
                dateTime1 = min.Value;
            }

            return GetDateOnly(dateTime2.Value).DayNumber -
                GetDateOnly(dateTime1).DayNumber;
        }

        public static int GetYears(DateOnly dateOnly1, DateOnly? dateOnly2 = null)
        {
            dateOnly2 ??= Today;

            if (dateOnly2.HasValue && dateOnly1 > dateOnly2)
            {
                var min = dateOnly2.Value;
                dateOnly2 = dateOnly1;
                dateOnly1 = min;
            }


            return DateOnly
                .MinValue
                .AddDays(dateOnly2.Value.DayNumber - dateOnly1.DayNumber)
                .Year - 1;
        }

        public static DateTime GetDateTime(DateOnly dateOnly) => dateOnly.ToDateTime(TimeOnly.MinValue);
    }
}

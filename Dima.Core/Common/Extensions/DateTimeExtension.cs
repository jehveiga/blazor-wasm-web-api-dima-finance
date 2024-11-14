namespace Dima.Core.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime GetFirstDay(this DateTime date,
                                                int? year = null,
                                                int? month = null) =>
            new(year: year ?? date.Year,
                month: month ?? date.Month,
                day: 1,
                hour: date.Hour,
                minute: date.Minute,
                second: date.Second,
                DateTimeKind.Local);

        public static DateTime GetLastDay(this DateTime date,
                                               int? year = null,
                                               int? month = null) =>
            new DateTime(
                year: year ?? date.Year,
                month: month ?? date.Month,
                day: 1,
                hour: date.Hour,
                minute: date.Minute,
                second: date.Second,
                DateTimeKind.Local).AddMonths(1)
                                   .AddDays(-1);
    }
}
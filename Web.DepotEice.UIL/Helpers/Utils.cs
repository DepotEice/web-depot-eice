using System.Text.RegularExpressions;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace Web.DepotEice.UIL.Helpers
{
    /// <summary>
    /// Static class of utilities
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// The range of the date that can be used in the project
        /// </summary>
        public enum DateRange
        {
            /// <summary>
            /// The day range
            /// </summary>
            Day,

            /// <summary>
            /// The week range
            /// </summary>
            Week,

            /// <summary>
            /// The month range
            /// </summary>
            Month,

            /// <summary>
            /// The year range
            /// </summary>
            Year
        }

        /// <summary>
        /// Regex to detect the HTML <img> node
        /// </summary>
        /// <returns></returns>
        public static readonly Regex HTMLImgRegex = new(@"<img[^>]+>");

        public static readonly Regex HtmlEntityRemovalRegex = new(@"&nbsp;|&lt;|&gt;|&amp;|&euro;|&pound;|&quot;");

        /// <summary>
        /// API Base url
        /// </summary>
        public static string API_BASE_URL { get; set; }
#if DEBUG
           = "https://localhost:7205/api/";
#else
           = "https://www.api.tfe-depot-eice.be/api/";
#endif

        public static string BASE_URL { get; set; }
#if DEBUG
           = "https://localhost:7205/";
#else
           = "https://www.api.tfe-depot-eice.be/";
#endif

        /// <summary>
        /// Helper method to iterate through each day within a range
        /// </summary>
        /// <param name="start">The start datetime</param>
        /// <param name="end">The end datetime</param>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> where T is <see cref="DateTime"/>
        /// </returns>
        public static IEnumerable<DateTime> EachDay(DateTime start, DateTime end)
        {
            for (DateTime dt = start.Date; dt <= end.Date; dt = dt.AddDays(1))
            {
                yield return dt;
            }
        }

        /// <summary>
        /// Determine how the date time comparison will be done
        /// </summary>
        public enum DateTimeComparison
        {
            Year,
            Month,
            Day,
            YearMonth,
            MonthDay,
            YearMonthDay,
            Hour,
            Minute,
            Second,
            HourMinute,
            MinuteSecond,
            HourMinuteSecond,
            All
        }

        /// <summary>
        /// Verify if the date is equal to the other date
        /// </summary>
        /// <param name="date">The datetime</param>
        /// <param name="other">The datetime to compare to</param>
        /// <param name="dateTimeComparison">The comparison type</param>
        /// <returns>
        /// true If the date is equal to the other date, false otherwise
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static bool Equals(this DateTime date, DateTime other, DateTimeComparison dateTimeComparison = DateTimeComparison.All)
        {
            switch (dateTimeComparison)
            {
                case DateTimeComparison.Year:
                    return date.Year == other.Year;
                case DateTimeComparison.Month:
                    return date.Month == other.Month;
                case DateTimeComparison.Day:
                    return date.Day == other.Day;
                case DateTimeComparison.YearMonth:
                    return date.Year == other.Year && date.Month == other.Month;
                case DateTimeComparison.MonthDay:
                    return date.Month == other.Month && date.Day == other.Day;
                case DateTimeComparison.YearMonthDay:
                    return date.Year == other.Year && date.Month == other.Month && date.Day == other.Day;
                case DateTimeComparison.Hour:
                    return date.Hour == other.Hour;
                case DateTimeComparison.Minute:
                    return date.Minute == other.Minute;
                case DateTimeComparison.Second:
                    return date.Second == other.Second;
                case DateTimeComparison.HourMinute:
                    return date.Hour == other.Hour && date.Minute == other.Minute;
                case DateTimeComparison.MinuteSecond:
                    return date.Minute == other.Minute && date.Second == other.Second;
                case DateTimeComparison.HourMinuteSecond:
                    return date.Hour == other.Hour && date.Minute == other.Minute && date.Second == other.Second;
                case DateTimeComparison.All:
                    return date.Year == other.Year && date.Month == other.Month && date.Day == other.Day &&
                           date.Hour == other.Hour && date.Minute == other.Minute && date.Second == other.Second;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(dateTimeComparison),
                        dateTimeComparison,
                        "The comparison value doesn't exist"
                    );
            }
        }
    }
}

using System.Text.RegularExpressions;

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
        public static string API_BASE_URL = Environment.GetEnvironmentVariable("API_BASE_URL")
            ?? "https://localhost:7205/api";
    }
}

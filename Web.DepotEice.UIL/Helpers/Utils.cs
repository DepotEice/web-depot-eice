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
        public static string API_BASE_URL { get; set; } = "https://localhost:7205/api";

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

        public static async Task<string> GetSecret()
        {
            string secretName = "prod/web-depot-eice";
            string region = "eu-west-3";

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
            };

            GetSecretValueResponse response;

            try
            {
                response = await client.GetSecretValueAsync(request);
            }
            catch (Exception e)
            {
                // For a list of the exceptions thrown, see
                // https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
                throw e;
            }

            string secret = response.SecretString;

            return secret;
        }
    }
}

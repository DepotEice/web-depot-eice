using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.IServices;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.Services
{
    public class OpeningHoursService : IOpeningHoursService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpeningHoursService> _logger;
        private readonly ISyncLocalStorageService _localStorageService;

        public OpeningHoursService(HttpClient httpClient, ILogger<OpeningHoursService> logger, ISyncLocalStorageService localStorageService)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            _httpClient = httpClient;
            _logger = logger;
            _localStorageService = localStorageService;

            string token = _localStorageService.GetItemAsString("token");

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Get the opening hours of the API, if no parameters are given, it will return all the opening hours
        /// </summary>
        /// <param name="day">The day date</param>
        /// <param name="month">The month</param>
        /// <param name="year">The year</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="OpeningHoursModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<OpeningHoursModel>>> GetOpeningHoursAsync(int? day = null, int? month = null, int? year = null)
        {
            string queryUri = "OpeningHours?";

            if (day.HasValue)
            {
                queryUri += $"day={day}&";
            }

            if (month.HasValue)
            {
                queryUri += $"month={month}&";
            }

            if (year.HasValue)
            {
                queryUri += $"year={year}&";
            }

            HttpResponseMessage response = await _httpClient.GetAsync(queryUri);

            ResultModel<IEnumerable<OpeningHoursModel>> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<OpeningHoursModel>>();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{nameof(GetOpeningHoursAsync)}: An exception was thrown, cannot " +
                        $"read the result as json.\n{e.Message}");
            }

            return result;
        }

        public async Task<IEnumerable<OpeningHoursModel>> GetWeekOpeningHoursAsync(DateTime dateTime)
        {
            IEnumerable<OpeningHoursModel>? openingHours = (await GetOpeningHoursAsync()).Data;

            int actualDay = (int)dateTime.DayOfWeek;

            return openingHours.Where(oh =>
                (oh.OpenAt.Year == dateTime.Year && oh.OpenAt.Month == dateTime.Month) &&
                (oh.OpenAt.Day >= dateTime.Day - actualDay && oh.OpenAt.Day <= dateTime.Day + (7 - actualDay)));
        }

        public async Task<IEnumerable<DateTime>> GetClosedDatesAsync()
        {
            IEnumerable<OpeningHoursModel> openingHours = (await GetOpeningHoursAsync()).Data;

            if (openingHours.Count() == 0)
            {
                return Enumerable.Empty<DateTime>();
            }

            var firstOpeningHours = openingHours.First();
            var lastOpeningHours = openingHours.Last();

            DateTime firstDayOpen = new DateTime(
                firstOpeningHours.OpenAt.Year,
                firstOpeningHours.OpenAt.Month,
                firstOpeningHours.OpenAt.Day);

            DateTime lastDayOpen = new DateTime(
                lastOpeningHours.CloseAt.Year,
                lastOpeningHours.CloseAt.Month,
                lastOpeningHours.CloseAt.Day);

            List<DateTime> closedDateTimes = new List<DateTime>();

            for (DateTime dti = firstDayOpen; dti <= lastDayOpen; dti.AddDays(1))
            {
                if (dti.DayOfWeek != DayOfWeek.Sunday)
                {
                    closedDateTimes.Append(dti);
                }
            }

            return closedDateTimes;
        }

        public async Task<OpeningHoursModel> CreateOpeningHoursAsync(OpeningHoursCreateModel openingHours)
        {
            if (openingHours is null)
            {
                throw new ArgumentNullException(nameof(openingHours));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("OpeningHours", openingHours);

            response.EnsureSuccessStatusCode();

            OpeningHoursModel? createdOpeningHours = await response.Content.ReadFromJsonAsync<OpeningHoursModel>();

            if (createdOpeningHours is null)
            {
                throw new NullReferenceException(nameof(createdOpeningHours));
            }

            return createdOpeningHours;
        }

        public async Task<bool> DeleteOpeningHoursAsync(OpeningHoursModel openingHours)
        {
            if (openingHours is null)
            {
                throw new ArgumentNullException(nameof(openingHours));
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync($"OpeningHours/{openingHours.Id}");

            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}

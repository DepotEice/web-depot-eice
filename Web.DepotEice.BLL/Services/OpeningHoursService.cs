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
    /// <summary>
    /// Opening hours service
    /// </summary>
    public class OpeningHoursService : IOpeningHoursService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpeningHoursService> _logger;
        private readonly ISyncLocalStorageService _localStorageService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <param name="localStorageService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public OpeningHoursService(HttpClient httpClient, ILogger<OpeningHoursService> logger,
            ISyncLocalStorageService localStorageService)
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
        public async Task<ResultModel<IEnumerable<OpeningHoursModel>>> GetOpeningHoursAsync(
            int? day = null,
            int? month = null,
            int? year = null
        )
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
                _logger.LogInformation(
                    "{fn}: An exception was thrown, cannot read the result as json.\n{eMsg}",
                    nameof(GetOpeningHoursAsync),
                    e.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Create an opening hours by sending a POST request to the API
        /// </summary>
        /// <param name="openingHours">The data to create the opening hours</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="OpeningHoursModel"/>. The created opening hours if it 
        /// was created, null otherwise
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<OpeningHoursModel>> CreateOpeningHoursAsync(OpeningHoursCreateModel openingHours)
        {
            if (openingHours is null)
            {
                throw new ArgumentNullException(nameof(openingHours));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("OpeningHours", openingHours);

            ResultModel<OpeningHoursModel> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<OpeningHoursModel>();
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                    "{fn}: An exception was thrown, cannot read the result as json.\n{eMsg}",
                    nameof(CreateOpeningHoursAsync),
                    e.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Delete an opening hours by sending a DELETE request to the API
        /// </summary>
        /// <param name="id">The id of the opening hours</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>. True if the opening hours was deleted, false otherwise
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ResultModel<bool>> DeleteOpeningHoursAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync($"OpeningHours/{id}");

            ResultModel<bool> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = response.IsSuccessStatusCode
            };

            return result;
        }
    }
}

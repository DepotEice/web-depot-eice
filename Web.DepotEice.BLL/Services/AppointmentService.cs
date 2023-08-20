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
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(ISyncLocalStorageService localStorageService, HttpClient httpClient,
            ILogger<AppointmentService> logger)
        {
            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _localStorageService = localStorageService;
            _httpClient = httpClient;
            _logger = logger;

            string token = _localStorageService.GetItemAsString("token");

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Create an appointment by sending a POST request to the API
        /// </summary>
        /// <param name="appointmentCreate">The model to send to the API</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AppointmentModel"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<AppointmentModel>> CreateAppointmentAsync(AppointmentCreateModel appointmentCreate)
        {
            if (appointmentCreate is null)
            {
                throw new ArgumentNullException(nameof(appointmentCreate));
            }

            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync("Appointments", appointmentCreate);

            ResultModel<AppointmentModel> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<AppointmentModel>();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{nameof(CreateAppointmentAsync)}: An exception was thrown, cannot " +
                                                           $"read the result as json.\n{e.Message}");
            }

            return result;
        }

        public async Task<AppointmentModel?> GetAppointmentAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Appointments/{id}");

            var result = await response.Content.ReadFromJsonAsync<AppointmentModel>();

            return result;
        }

        public async Task<ResultModel<IEnumerable<AppointmentModel>>> GetAppointmentsAsync(DateTime? date = null)
        {
            string queryUri = "Appointments";

            if (date.HasValue)
            {
                queryUri += $"?date={date.Value:s}";
            }

            HttpResponseMessage response = await _httpClient.GetAsync(queryUri);

            ResultModel<IEnumerable<AppointmentModel>> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<AppointmentModel>>();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{nameof(GetAppointmentsAsync)}: An exception was thrown, cannot " +
                        $"read the result as json.\n{e.Message}");
            }

            return result;
        }
    }
}

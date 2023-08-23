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
    /// Appointment service to communicate with the API
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;
        private readonly ILogger<AppointmentService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="localStorageService"></param>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Update an appointment by sending a PUT request to the API
        /// </summary>
        /// <param name="id">The id of the appointment to update</param>
        /// <param name="appointment">The appointment form</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AppointmentModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<AppointmentModel?>> UpdateAppointmentAsync(int id, AppointmentCreateModel appointment)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (appointment is null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Appointments/{id}", appointment);

            ResultModel<AppointmentModel?> result = new()
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
                _logger.LogInformation($"{nameof(UpdateAppointmentAsync)}: An exception was thrown, cannot " +
                                       $"read the result as json.\n{e.Message}");
            }

            return result;
        }

        /// <summary>
        /// Get an appointment by sending a GET request to the API
        /// </summary>
        /// <param name="id">Id of the appointment</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AppointmentModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ResultModel<AppointmentModel?>> GetAppointmentAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Appointments/{id}");

            ResultModel<AppointmentModel?> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<AppointmentModel>();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{nameof(GetAppointmentAsync)}: An exception was thrown, cannot " +
                    $"read the result as json.\n{e.Message}");
            }

            return result;
        }

        /// <summary>
        /// Get all appointments by sending a GET request to the API
        /// </summary>
        /// <param name="date">The datetime</param>
        /// <param name="dateRange">The range to select</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{AppointmentModel}"/> where T is <see cref="AppointmentModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<AppointmentModel>>> GetAppointmentsAsync(DateTime? date = null, int? dateRange = null)
        {
            string queryUri = "Appointments";

            if (date.HasValue)
            {
                queryUri += $"?date={date.Value:s}";

                if (dateRange.HasValue)
                {
                    queryUri += $"&range={dateRange.Value}";
                }
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

        /// <summary>
        /// Delete an appointment by sending a DELETE request to the API
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ResultModel<bool>> DeleteAppointmentAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Appointments/{id}");

            ResultModel<bool> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{nameof(DeleteAppointmentAsync)}: An exception was thrown, cannot " +
                                       $"read the result as json.\n{e.Message}");
            }

            return result;
        }
    }
}

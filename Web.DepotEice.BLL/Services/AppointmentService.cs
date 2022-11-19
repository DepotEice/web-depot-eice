using Blazored.LocalStorage;
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
        private readonly ILocalStorageService _localStorageService;

        public AppointmentService(ILocalStorageService localStorageService)
        {
            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            _localStorageService = localStorageService;

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7205/api/"),
            };
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> CreateAppointmentAsync(AppointmentCreateModel appointmentCreate)
        {
            if (appointmentCreate is null)
            {
                throw new ArgumentNullException(nameof(appointmentCreate));
            }

            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync("Appointments", appointmentCreate);

            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<AppointmentModel?> GetAppointmentAsync(int id)
        {
            string? token = await _localStorageService.GetItemAsStringAsync("token");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"Appointments/{id}");

            var result = await response.Content.ReadFromJsonAsync<AppointmentModel>();

            return result;
        }

        public async Task<IEnumerable<AppointmentModel>> GetAppointmentsAsync()
        {
            string? token = await _localStorageService.GetItemAsStringAsync("token");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Appointments");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<AppointmentModel>>();

            if (result is null)
            {
                return Enumerable.Empty<AppointmentModel>();
            }

            return result;
        }
    }
}

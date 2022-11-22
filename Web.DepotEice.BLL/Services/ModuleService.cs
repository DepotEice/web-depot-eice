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
    public class ModuleService : IModuleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public ModuleService(ILocalStorageService localStorageService)
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

        public async Task<ModuleModel?> CreateModuleAsync(ModuleCreationModel moduleCreation)
        {
            if (moduleCreation is null)
            {
                throw new ArgumentNullException(nameof(moduleCreation));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Modules", moduleCreation);

            response.EnsureSuccessStatusCode();

            ModuleModel? result = await response.Content.ReadFromJsonAsync<ModuleModel>();

            return result;
        }

        public async Task<ModuleModel?> GetModuleAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/{id}");

            var result = await response.Content.ReadFromJsonAsync<ModuleModel>();

            return result;
        }

        public async Task<IEnumerable<ModuleModel>> GetModulesAsync()
        {
            string? token = await _localStorageService.GetItemAsStringAsync("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("Modules");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ModuleModel>>();

            if (result is null)
            {
                return Enumerable.Empty<ModuleModel>();
            }

            return result;
        }

        public async Task<ScheduleModel?> CreateScheduleAsync(int moduleId, ScheduleCreateModel scheduleCreate)
        {
            if (scheduleCreate is null)
            {
                throw new ArgumentNullException(nameof(scheduleCreate));
            }

            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync($"Modules/{moduleId}/Schedules", scheduleCreate);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ScheduleModel>();
        }

        public async Task<IEnumerable<ScheduleModel>> GetModuleSchedulesAsync(int moduleId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/{moduleId}/Schedules");

            response.EnsureSuccessStatusCode();

            IEnumerable<ScheduleModel>? result = await response.Content.ReadFromJsonAsync<IEnumerable<ScheduleModel>>();

            if (result is null)
            {
                return Enumerable.Empty<ScheduleModel>();
            }

            return result;
        }

        public async Task<bool> DeleteScheduleAsync(int moduleId, int scheduleId)
        {
            HttpResponseMessage response =
                await _httpClient.DeleteAsync($"Modules/{moduleId}/Schedules/{scheduleId}");

            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<UserModuleRequestModel>> GetUsersRequestingModules()
        {
            HttpResponseMessage response =
                await _httpClient.GetAsync("Modules/RequestingUsers");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserModuleRequestModel>>();

            if (result is null)
            {
                return Enumerable.Empty<UserModuleRequestModel>();
            }

            return result;
        }

        public async Task<bool?> UserIsAccepted(int moduleId)
        {
            string? token = await _localStorageService.GetItemAsStringAsync("token");

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response =
                await _httpClient.GetAsync($"Modules/{moduleId}/UserRequestStatus");

            bool? result = await response.Content.ReadFromJsonAsync<bool>();

            if (result is null)
            {
                return false;
            }

            return result.Value;
        }

        public async Task<bool?> UserIsAccepted(int moduleId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string? token = await _localStorageService.GetItemAsStringAsync("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response =
                await _httpClient.GetAsync($"Modules/{moduleId}/UserRequestStatus/{userId}");

            bool? result = await response.Content.ReadFromJsonAsync<bool>();

            if (result is null)
            {
                return false;
            }

            return result.Value;
        }

        public async Task<bool> RequestAcceptance(int moduleId)
        {
            string? token = await _localStorageService.GetItemAsStringAsync("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response =
                await _httpClient.PostAsync($"Modules/{moduleId}/RequestAcceptance", null);

            return response.IsSuccessStatusCode;
        }
    }
}

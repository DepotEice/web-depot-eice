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

namespace Web.DepotEice.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public RoleService(ILogger<RoleService> logger, ILocalStorageService localStorageService)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            _logger = logger;
            _localStorageService = localStorageService;

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7205/api/"),
            };
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> UserHasRoleAsync(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException(nameof(role));
            }

            string? token = await _localStorageService.GetItemAsStringAsync("token");

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("User is not authenticated!");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"Roles/HasRole/{role}");

            response.EnsureSuccessStatusCode();

            bool? result = await response.Content.ReadFromJsonAsync<bool>();

            if (result is null)
            {
                return false;
            }

            return result.Value;
        }

        public async Task<bool> UserHasRoleAsync(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException(nameof(role));
            }

            string? token = await _localStorageService.GetItemAsStringAsync("token");

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("User is not authenticated!");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"Roles/HasRole/{userId}/{role}");

            response.EnsureSuccessStatusCode();

            bool? result = await response.Content.ReadFromJsonAsync<bool>();

            if (result is null)
            {
                return false;
            }

            return result.Value;
        }
    }
}

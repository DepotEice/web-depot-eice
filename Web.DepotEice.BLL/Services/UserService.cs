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
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public UserService(ILogger<UserService> logger, HttpClient httpClient, ILocalStorageService localStorageService)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            _logger = logger;
            _httpClient = httpClient;
            _localStorageService = localStorageService;

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<UserModel>> GetTeachersAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Users/Teachers");

            response.EnsureSuccessStatusCode();

            IEnumerable<UserModel>? result = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();

            if (result is null)
            {
                return Enumerable.Empty<UserModel>();
            }

            return result;
        }

        public async Task<bool> UpdatePassword(PasswordUpdateModel passwordUpdate, string? token = null)
        {
            if (passwordUpdate is null)
            {
                throw new ArgumentNullException(nameof(passwordUpdate));
            }

            string requestString = "Users/Password" + (token is not null ? $"?token={token}" : string.Empty);

            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync(requestString, passwordUpdate);

            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<UserModel?> GetUserAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            string token = await _localStorageService.GetItemAsync<string>("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"Users/{userId}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserModel>();
        }

        public async Task<UserModel?> GetUserAsync()
        {
            string token = await _localStorageService.GetItemAsync<string>("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"Users/me");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserModel>();
        }
    }
}

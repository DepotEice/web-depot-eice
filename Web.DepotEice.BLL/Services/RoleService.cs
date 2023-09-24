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
    public class RoleService : IRoleService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;

        public RoleService(ILogger<RoleService> logger, ISyncLocalStorageService localStorageService, HttpClient httpClient)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            _logger = logger;
            _localStorageService = localStorageService;
            _httpClient = httpClient;

            string token = _localStorageService.GetItemAsString("token");

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Get all the roles of the current user if the id is not provided or get the roles of the user with the provided id
        /// </summary>
        /// <param name="userId">The id</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{RoleModel}"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<RoleModel>>> GetRolesAsync(string? userId = null)
        {
            string queryUrl = string.IsNullOrEmpty(userId)
                ? "Roles/me"
                : $"Users/{userId}/Roles";

            HttpResponseMessage response = await _httpClient.GetAsync(queryUrl);

            ResultModel<IEnumerable<RoleModel>> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<RoleModel>>();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{nameof(GetRolesAsync)}: An exception was thrown, cannot read the " +
                    $"result as json.\n{e.Message}");
            }

            return result;
        }

        /// <summary>
        /// Get all the available roles by sending a GET request to the API
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="RoleModel"/>.
        /// The list of available roles in the API
        /// </returns>
        public async Task<ResultModel<IEnumerable<RoleModel>>> GetAvailableRolesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Roles");

            ResultModel<IEnumerable<RoleModel>> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<RoleModel>>();
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                    "{fn}: An exception was thrown, cannot read the result as json.\n{eMsg}",
                    nameof(GetRolesAsync),
                    e.Message
                );
            }

            return result;
        }

        public async Task<bool> UserHasRoleAsync(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException(nameof(role));
            }

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

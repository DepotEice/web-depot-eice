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
    public class AuthService : IAuthService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;

        public AuthService(ILogger<AuthService> logger, HttpClient httpClient, ISyncLocalStorageService localStorageService)
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

            string token = _localStorageService.GetItemAsString("token");

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string> SignInAsync(SignInModel signInModel)
        {
            if (signInModel is null)
            {
                throw new ArgumentNullException(nameof(signInModel));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"Auth/Login", signInModel);
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<SignInResponseModel>();

            if (token is null)
            {
                return string.Empty;
            }

            return token.Token;
        }

        public async Task<bool> RequestNewPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            HttpResponseMessage response =
                await _httpClient.GetAsync($"Auth/RequestNewPassword?email={email}");

            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> SignUpAsync(SignUpModel signUpModel)
        {
            if (signUpModel is null)
            {
                throw new ArgumentNullException(nameof(signUpModel));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"Auth/SignUp", signUpModel);

            // TODO : Change NoContent in API to OK()
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Activate(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            HttpResponseMessage response =
                await _httpClient.PostAsync($"Auth/Activate?id={userId}&token={token}", null);

            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> ResetPassword(PasswordResetModel passwordResetModel, string token)
        {
            _logger.LogInformation("UpdatePassword");

            if (passwordResetModel is null)
            {
                _logger.LogWarning("UpdatePassword: passwordUpdate is null");

                throw new ArgumentNullException(nameof(passwordResetModel));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                $"Users/Password?token={token}",
                passwordResetModel
            );

            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}

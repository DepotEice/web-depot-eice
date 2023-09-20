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

        /// <summary>
        /// Request a new password by sending a GET request to the API
        /// </summary>
        /// <param name="email">The email address associated to the account</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is a <see cref="bool"/>. If the request was successful, the data will 
        /// be true, otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<bool>> RequestPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            HttpResponseMessage response =
                await _httpClient.GetAsync($"Auth/RequestPassword?email={email}");

            ResultModel<bool> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = response.IsSuccessStatusCode
            };

            return result;
        }

        public async Task<bool> SignUpAsync(SignUpModel signUpModel)
        {
            if (signUpModel is null)
            {
                throw new ArgumentNullException(nameof(signUpModel));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"Auth/Register", signUpModel);

            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Activate the user account by sending a POST request to the API
        /// </summary>
        /// <param name="tokenId">The id of the user</param>
        /// <param name="tokenValue">The activation token</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is a <see cref="bool"/>. If the activation (status code 200) was 
        /// successful, the data will be true, otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<bool>> ActivateAsync(string tokenId, string tokenValue)
        {
            if (string.IsNullOrEmpty(tokenId))
            {
                throw new ArgumentNullException(nameof(tokenId));
            }

            if (string.IsNullOrEmpty(tokenValue))
            {
                throw new ArgumentNullException(nameof(tokenValue));
            }

            HttpResponseMessage response =
                await _httpClient.PostAsync($"Auth/Activate?tokenId={tokenId}&tokenValue={tokenValue}", null);

            ResultModel<bool> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = response.IsSuccessStatusCode
            };

            return result;
        }

        /// <summary>
        /// Reset the user password by sending a POST request to the API
        /// </summary>
        /// <param name="passwordResetModel">The password form to reset the password</param>
        /// <param name="token">The token to validate the password reset</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is a <see cref="bool"/>. The data is valid if the request is successful.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<bool>> ResetPasswordAsync(PasswordResetModel passwordResetModel, string token)
        {
            if (passwordResetModel is null)
            {
                throw new ArgumentNullException(nameof(passwordResetModel));
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                $"Auth/ResetPassword?token={token}",
                passwordResetModel
            );

            ResultModel<bool> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = response.IsSuccessStatusCode
            };

            return result;
        }

        /// <summary>
        /// Validate the current token
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is a <see cref="bool"/>
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<ResultModel<bool>> AuthorizeAsync()
        {
            string? token = _localStorageService.GetItem<string>("token");

            if (string.IsNullOrEmpty(token))
            {
                throw new NullReferenceException(nameof(token));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Auth/Authorize/{token}");

            ResultModel<bool> result = new ResultModel<bool>()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(AuthorizeAsync)}: an exception was thrown while converting result to json.\n{ex.Message}");
            }

            return result;
        }
    }
}

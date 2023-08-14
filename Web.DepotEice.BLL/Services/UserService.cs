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
        private readonly ISyncLocalStorageService _localStorageService;

        public UserService(ILogger<UserService> logger, HttpClient httpClient, ISyncLocalStorageService localStorageService)
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

        /// <summary>
        /// Update the user profile picture
        /// </summary>
        /// <param name="imageContent">byte content of the image</param>
        /// <param name="contentType">content type</param>
        /// <returns><see cref="ResultModel{UserModel?}"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ResultModel<Stream?>> UpdateProfilePictureAsync(byte[] imageContent, string contentType)
        {
            _logger.LogInformation($"{nameof(UpdateProfilePictureAsync)}");

            if (imageContent is null)
            {
                throw new ArgumentNullException(nameof(imageContent));
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(contentType));
            }

            try
            {
                using MultipartFormDataContent content = new MultipartFormDataContent();

                using MemoryStream memoryStream = new MemoryStream(imageContent);

                StreamContent streamContent = new StreamContent(memoryStream);

                streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                content.Add(content: streamContent, name: "image", fileName: "profilePicture");

                HttpResponseMessage response = await _httpClient.PostAsync("Users/UpdateProfilePicture", content);

                ResultModel<Stream?> result = new ResultModel<Stream?>()
                {
                    Code = response.StatusCode,
                    Message = await response.Content.ReadAsStringAsync(),
                    Success = response.IsSuccessStatusCode
                };

                try
                {
                    result.Data = await response.Content.ReadAsStreamAsync();
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"{nameof(UpdateProfilePictureAsync)}: An exception was thrown, cannot " +
                        $"read the result as json.\n{e.Message}");
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogWarning($"{nameof(UpdateProfilePictureAsync)}: An exception was thrown when trying " +
                    $"to upload profile picture to the server.\n{e.Message}");

                throw;
            }
        }

        /// <summary>
        /// Retrieves a collection of teacher users asynchronously.
        /// </summary>
        /// <returns>The collection of teacher users.</returns>
        public async Task<IEnumerable<UserModel>> GetTeachersAsync()
        {
            _logger.LogInformation($"{nameof(GetTeachersAsync)}");

            HttpResponseMessage response = await _httpClient.GetAsync("Users/Teachers");

            response.EnsureSuccessStatusCode();

            IEnumerable<UserModel>? result = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();

            if (result is null)
            {
                _logger.LogWarning("GetTeachersAsync: result is null");

                return Enumerable.Empty<UserModel>();
            }

            _logger.LogInformation("GetTeachersAsync: {resultNumber}", result.Count());

            return result;
        }

        public async Task<ResultModel<UserModel>> UpdatePasswordAsync(PasswordUpdateModel passwordUpdate)
        {
            _logger.LogInformation($"{nameof(UpdatePasswordAsync)}");

            if (passwordUpdate is null)
            {
                throw new ArgumentNullException(nameof(passwordUpdate));
            }

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Users/UpdatePassword", passwordUpdate);

                ResultModel<UserModel> result = new ResultModel<UserModel>()
                {
                    Code = response.StatusCode,
                    Message = await response.Content.ReadAsStringAsync(),
                    Success = response.IsSuccessStatusCode
                };

                try
                {
                    result.Data = await response.Content.ReadFromJsonAsync<UserModel>();
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"{nameof(UpdatePasswordAsync)}: An exception was thrown when trying to " +
                        $"read from json.\n{e.Message}");
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{nameof(UpdatePasswordAsync)}: An exception was thrown.\n{e.Message}");
                throw;
            }
        }

        public async Task<UserModel?> GetUserAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Users/{userId}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserModel>();
        }

        public async Task<UserModel?> GetUserAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Users/me");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserModel>();
        }

        public async Task<UserModel?> UpdateUserAsync(UserUpdateModel userUpdateModel)
        {
            if (userUpdateModel is null)
            {
                throw new NullReferenceException(nameof(userUpdateModel));
            }

            HttpResponseMessage responseMessage = await _httpClient.PutAsJsonAsync(
                "Users",
                userUpdateModel
            );

            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<UserModel>();
        }
    }
}

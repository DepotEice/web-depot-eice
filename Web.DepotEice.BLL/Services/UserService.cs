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
        /// Get the list of all teachers
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<UserModel>>> GetTeachersAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Users/Teachers");

            ResultModel<IEnumerable<UserModel>> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{nameof(GetTeachersAsync)}: An exception was thrown, cannot " +
                    $"read the result as json.\n{e.Message}");
            }

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

        /// <summary>
        /// Get the user by sending a GET request to the API and by providing the user ID
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<UserModel>> GetUserAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Users/{userId}");

            var result = new ResultModel<UserModel>()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<UserModel>();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{nameof(GetUserAsync)}: An exception was thrown, cannot " +
                    $"read the result as json.\n{e.Message}");
            }

            return result;
        }

        /// <summary>
        /// Get the current user by sending a GET request to the API
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        public async Task<ResultModel<UserModel>> GetUserAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Users/me");

            ResultModel<UserModel> result = new ResultModel<UserModel>()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<UserModel>();
            }
            catch (Exception e)
            {
                _logger.LogError(
                    "{fn}: An exception was thrown, cannot read the result as json.\n{eMsg}\n{eStr}",
                    nameof(GetUserAsync),
                    e.Message,
                    e.StackTrace
                );
            }

            return result;
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

        public async Task<ResultModel<bool>> DeleteUserAsync(string? userId = null)
        {
            string url = string.IsNullOrEmpty(userId) ? "Users" : $"Users?id={userId}";

            HttpResponseMessage response = await _httpClient.DeleteAsync(url);

            ResultModel<bool> result = new ResultModel<bool>()
            {
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Success = response.IsSuccessStatusCode
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception e)
            {
                _logger.LogWarning($"{nameof(DeleteUserAsync)}: An exception was thrown when trying to " +
                                       $"read from json.\n{e.Message}");
            }

            return result;
        }

        /// <summary>
        /// Get all the available users from the API by sending a GET request to the API
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<UserModel>>> GetUsersAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Users/available");

            ResultModel<IEnumerable<UserModel>> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                    "{fn}: An exception was thrown, cannot read the result as json.\n{eMsg}",
                    nameof(GetUsersAsync),
                    e.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get all the students by sending a GET request to the API
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<UserModel>>> GetStudentsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Users/students");

            ResultModel<IEnumerable<UserModel>> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                    "{fn}: An exception was thrown, cannot read the result as json.\n{eMsg}",
                    nameof(GetStudentsAsync),
                    e.Message
                );
            }

            return result;
        }
    }
}

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
    /// Address service
    /// </summary>
    public class AddressService : IAddressService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpClient"></param>
        /// <param name="localStorageService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AddressService(ILogger<AddressService> logger, HttpClient httpClient, ISyncLocalStorageService localStorageService)
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
        /// Create a new address by sending a POST request to the API with the address data
        /// </summary>
        /// <param name="addressCreate">The new address data</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AddressModel"/> which is the created address data. Null if
        /// nothing was created or if the request failed
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<AddressModel>> CreateAddressAsync(AddressCreateModel addressCreate)
        {
            if (addressCreate is null)
            {
                throw new ArgumentNullException(nameof(addressCreate));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Addresses", addressCreate);

            ResultModel<AddressModel> result = new ResultModel<AddressModel>()
            {
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = await response.Content.ReadFromJsonAsync<AddressModel>(),
                Success = response.IsSuccessStatusCode
            };

            return result;
        }

        /// <summary>
        /// Delete the address by sending a DELETE request to the API with the address ID
        /// </summary>
        /// <param name="id">The id of the address to delete</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>. True if the address was deleted, false otherwise
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task<ResultModel<bool>> DeleteAddressAsync(int id)
        {
            if (id <= 0)
            {
                throw new IndexOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Addresses/{id}");

            ResultModel<bool> result = new ResultModel<bool>()
            {
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = response.IsSuccessStatusCode,
                Success = response.IsSuccessStatusCode
            };

            return result;
        }

        /// <summary>
        /// Get an address by sending a GET request to the API with the address ID
        /// </summary>
        /// <param name="id">The id of the address to retrieve</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AddressModel"/> which is the address data. Null if the
        /// data was not found or if the request failed
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task<ResultModel<AddressModel>> GetAddressAsync(int id)
        {
            if (id <= 0)
            {
                throw new IndexOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Addresses/{id}");

            ResultModel<AddressModel> result = new ResultModel<AddressModel>()
            {
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Success = response.IsSuccessStatusCode
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<AddressModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{eMsg}",
                    nameof(GetAddressAsync),
                    ex.Message
                );

                result.Data = null;
            }

            return result;
        }

        /// <summary>
        /// Get the current user's addresses by sending a GET request to the API
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="AddressModel"/>.
        /// The list of addresses. Null if the request failed
        /// </returns>
        public async Task<ResultModel<IEnumerable<AddressModel>>> GetAddressesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Addresses");

            ResultModel<IEnumerable<AddressModel>> result = new ResultModel<IEnumerable<AddressModel>>()
            {
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Success = response.IsSuccessStatusCode
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<AddressModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{eMsg}",
                    nameof(GetAddressesAsync),
                    ex.Message
                );

                result.Data = null;
            }

            return result;
        }

        /// <summary>
        /// Get a user's addresses by sending a GET request to the API. Requires admin privileges
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="AddressModel"/>.
        /// The list of addresses. Null if the request failed
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<IEnumerable<AddressModel>>> GetAddressesAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Users/{userId}/Addresses");

            ResultModel<IEnumerable<AddressModel>> result = new ResultModel<IEnumerable<AddressModel>>()
            {
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Success = response.IsSuccessStatusCode
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<AddressModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{eMsg}",
                    nameof(GetAddressesAsync),
                    ex.Message
                );

                result.Data = null;
            }

            return result;
        }

        /// <summary>
        /// Set an address as primary by sending a POST request to the API with the address ID
        /// </summary>
        /// <param name="id">The id of the address to set as primary</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AddressModel"/> which is the address data. Null if the
        /// data is not found or if the request failed
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task<ResultModel<AddressModel>> SetPrimaryAddressAsync(int id)
        {
            if (id <= 0)
            {
                throw new IndexOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.PostAsync($"Addresses/SetPrimary/{id}", null);

            ResultModel<AddressModel> result = new ResultModel<AddressModel>()
            {
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Success = response.IsSuccessStatusCode
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<AddressModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"{nameof(SetPrimaryAddressAsync)}: an exception was thrown while converting result to json.\n{ex.Message}");
                result.Data = null;
            }

            return result;
        }

        /// <summary>
        /// Update an address by sending a PUT request to the API with the address data
        /// </summary>
        /// <param name="addressUpdate">The data of the address</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AddressModel"/> which is the updated address data. Null if
        /// data was not updated or if the request failed
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<AddressModel>> UpdateAddressAsync(AddressUpdateModel addressUpdate)
        {
            if (addressUpdate is null)
            {
                throw new ArgumentNullException(nameof(addressUpdate));
            }

            HttpResponseMessage response =
                await _httpClient.PutAsJsonAsync($"Addresses/{addressUpdate.Id}", addressUpdate);

            ResultModel<AddressModel> result = new ResultModel<AddressModel>()
            {
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = await response.Content.ReadFromJsonAsync<AddressModel>(),
                Success = response.IsSuccessStatusCode
            };

            return result;
        }
    }
}

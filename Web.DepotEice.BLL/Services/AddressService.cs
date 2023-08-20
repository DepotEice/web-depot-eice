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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.DepotEice.BLL.Services
{
    public class AddressService : IAddressService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;

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

        public async Task<ResultModel<AddressModel>> DeleteAddressAsync(int id)
        {
            if (id <= 0)
            {
                throw new IndexOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Addresses/{id}");

            ResultModel<AddressModel> result = new ResultModel<AddressModel>()
            {
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = null,
                Success = response.IsSuccessStatusCode
            };

            return result;
        }

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
                    $"{nameof(GetAddressAsync)}: an exception was thrown while converting result to json.\n{ex.Message}");
                result.Data = null;
            }

            return result;
        }

        public async Task<ResultModel<IEnumerable<AddressModel>>> GetAddressesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Addresses");

            ResultModel<IEnumerable<AddressModel>> result = new ResultModel<IEnumerable<AddressModel>>()
            {
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = await response.Content.ReadFromJsonAsync<IEnumerable<AddressModel>>(),
                Success = response.IsSuccessStatusCode
            };

            return result;
        }

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

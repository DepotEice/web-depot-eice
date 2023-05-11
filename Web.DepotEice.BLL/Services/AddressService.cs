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
    public class AddressService : IAddressService
    {
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;

        private readonly string _token;

        public AddressService(HttpClient httpClient, ISyncLocalStorageService localStorageService)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            _httpClient = httpClient;
            _localStorageService = localStorageService;

            _token = _localStorageService.GetItemAsString("token");

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<AddressModel?> CreateAddress(AddressCreateModel addressCreate)
        {
            if (addressCreate is null)
            {
                throw new ArgumentNullException(nameof(addressCreate));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Addresses", addressCreate);

            AddressModel? createdAddress = await response.Content.ReadFromJsonAsync<AddressModel>();

            return createdAddress;
        }

        public async Task<bool> DeleteAddress(int id)
        {
            if (id <= 0)
            {
                throw new IndexOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Addresses/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<AddressModel?> GetAddress(int id)
        {
            if (id <= 0)
            {
                throw new IndexOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Addresses/{id}");

            AddressModel? address = await response.Content.ReadFromJsonAsync<AddressModel>();

            return address;
        }

        public async Task<IEnumerable<AddressModel>> GetAddresses()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Addresses");

            IEnumerable<AddressModel>? addresses = await response.Content.ReadFromJsonAsync<IEnumerable<AddressModel>>();

            if (addresses is null)
            {
                return Enumerable.Empty<AddressModel>();
            }

            return addresses;
        }

        public async Task<AddressModel?> UpdateAddress(AddressUpdateModel addressUpdate)
        {
            if (addressUpdate is null)
            {
                throw new ArgumentNullException(nameof(addressUpdate));
            }

            HttpResponseMessage response =
                await _httpClient.PutAsJsonAsync($"Addresses/${addressUpdate.Id}", addressUpdate);

            AddressModel? address = await response.Content.ReadFromJsonAsync<AddressModel>();

            return address;
        }
    }
}

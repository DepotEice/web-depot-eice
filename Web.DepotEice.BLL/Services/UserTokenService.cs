using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.IServices;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.Services
{
    public class UserTokenService : IUserTokenService
    {
        private readonly HttpClient _httpClient;

        public UserTokenService(HttpClient httpClient)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Validate the current token by sending a GET request to the API
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<bool>> IsValidAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Tokens/IsValid?token={token}");

            ResultModel<bool> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = response.IsSuccessStatusCode
            };

            return result;
        }
    }
}

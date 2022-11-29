using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.IServices;

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

        public async Task<bool> IsValid(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Tokens/IsValid?token={token}");

            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}

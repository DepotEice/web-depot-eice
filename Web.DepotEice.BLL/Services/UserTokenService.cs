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

        public UserTokenService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7205/api/"),
            };
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

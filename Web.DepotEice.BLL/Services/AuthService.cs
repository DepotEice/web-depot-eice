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
        private readonly HttpClient _httpClient;

        public AuthService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7205/api/"),
            };
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<string> SignInAsync(SignInModel signInModel)
        {
            if (signInModel is null)
            {
                throw new ArgumentNullException(nameof(signInModel));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"Auth/SignIn", signInModel);
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

            HttpResponseMessage response = await _httpClient.GetAsync($"Auth/RequestNewPassword?email={email}");

            response.EnsureSuccessStatusCode();

            return true;
        }

        public Task<string> SignUpAsync(SignUpModel signUpModel)
        {
            throw new NotImplementedException();
        }
    }
}

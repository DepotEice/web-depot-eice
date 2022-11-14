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
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7205/api/"),
            };
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> UpdatePassword(PasswordUpdateModel passwordUpdate, string? token = null)
        {
            if (passwordUpdate is null)
            {
                throw new ArgumentNullException(nameof(passwordUpdate));
            }

            string requestString = "Users/Password" + (token is not null ? $"?token={token}" : string.Empty);

            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync(requestString, passwordUpdate);

            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}

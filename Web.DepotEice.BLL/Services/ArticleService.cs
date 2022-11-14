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
    public class ArticleService : IArticleService
    {
        private readonly HttpClient _httpClient;

        public ArticleService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7205/api/"),
            };
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ArticleModel?> GetArticleAsync(int id)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Articles/{id}");

            response.EnsureSuccessStatusCode();

            ArticleModel? article = await response.Content.ReadFromJsonAsync<ArticleModel>();

            return article;
        }

        public async Task<IEnumerable<ArticleModel>> GetArticlesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Articles");

            response.EnsureSuccessStatusCode();

            IEnumerable<ArticleModel>? articles = await response.Content.ReadFromJsonAsync<IEnumerable<ArticleModel>>();

            if (articles is null)
            {
                return Enumerable.Empty<ArticleModel>();
            }

            return articles;
        }
    }
}

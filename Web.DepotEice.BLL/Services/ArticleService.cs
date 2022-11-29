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
    public class ArticleService : IArticleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public ArticleService(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            _localStorageService = localStorageService;
            _httpClient= httpClient;

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

        public async Task<bool> CanPinArticleAsync()
        {
            IEnumerable<ArticleModel> articlesFromApi = await GetArticlesAsync();

            return articlesFromApi.Count(a => a.IsPinned) < 12;
        }

        public async Task<ArticleModel?> CreateArticleAsync(ArticleCreateModel articleCreate)
        {
            if (articleCreate is null)
            {
                throw new ArgumentNullException(nameof(articleCreate));
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await _localStorageService.GetItemAsStringAsync("token"));

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Articles", articleCreate);

            response.EnsureSuccessStatusCode();

            ArticleModel? createdArticle = await response.Content.ReadFromJsonAsync<ArticleModel>();

            return createdArticle;
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await _localStorageService.GetItemAsStringAsync("token"));

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Articles/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RestoreArticleAsync(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await _localStorageService.GetItemAsStringAsync("token"));

            HttpResponseMessage response = await _httpClient.PutAsync($"Articles/restore/{id}", null);

            return response.IsSuccessStatusCode;
        }

        public async Task<ArticleModel?> UpdateArticleAsync(int id, ArticleCreateModel articleUpdate)
        {
            if (articleUpdate is null)
            {
                throw new ArgumentNullException(nameof(articleUpdate));
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await _localStorageService.GetItemAsStringAsync("token"));

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Articles/{id}", articleUpdate);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ArticleModel>();
        }
    }
}

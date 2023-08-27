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
    public class ArticleService : IArticleService
    {
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;
        private readonly ILogger _logger;

        public ArticleService(ISyncLocalStorageService localStorageService, HttpClient httpClient,
            ILogger<ArticleService> logger)
        {
            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _localStorageService = localStorageService;
            _httpClient = httpClient;
            _logger = logger;

            string token = _localStorageService.GetItemAsString("token");

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Get the article by sending a GET requesting to the API
        /// </summary>
        /// <param name="id">The id of the article</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="ArticleModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ResultModel<ArticleModel>> GetArticleAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Articles/{id}");

            var result = new ResultModel<ArticleModel>()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<ArticleModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"{nameof(GetArticlesAsync)}: an exception was thrown while converting result to json.\n{ex.Message}");
                result.Data = null;
            }

            return result;
        }

        /// <summary>
        /// Get articles by sending a GET request to the API
        /// </summary>
        /// <param name="pinned">Specify if only the pinned articles must be retrieved</param>
        /// <param name="skip">The number of elements to skip</param>
        /// <param name="top">The number of elements to keep</param>
        /// <param name="descending">The descending order</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="ArticleModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<ArticleModel>>> GetArticlesAsync(bool pinned = false, int skip = 0,
            int top = 100, bool descending = false)
        {
            string queryUri = $"Articles?onlyPinned={pinned}&skip={skip}&top={top}&descending={descending}";

            HttpResponseMessage response = await _httpClient.GetAsync(queryUri);

            ResultModel<IEnumerable<ArticleModel>> result = new ResultModel<IEnumerable<ArticleModel>>()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<ArticleModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"{nameof(GetArticlesAsync)}: an exception was thrown while converting result to json.\n{ex.Message}");
                result.Data = Enumerable.Empty<ArticleModel>();
            }

            return result;
        }

        public async Task<bool> CanPinArticleAsync()
        {
            var result = await GetArticlesAsync(pinned: true);

            if (!result.Success)
            {
                _logger.LogError("Getting articles failed");

                return false;
            }

            IEnumerable<ArticleModel>? articlesFromApi = result.Data;

            if (articlesFromApi is null)
            {
                return true;
            }

            return articlesFromApi.Count(a => a.IsPinned) < 12;
        }

        public async Task<ArticleModel?> CreateArticleAsync(ArticleCreateModel articleCreate)
        {
            if (articleCreate is null)
            {
                throw new ArgumentNullException(nameof(articleCreate));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Articles", articleCreate);

            response.EnsureSuccessStatusCode();

            ArticleModel? createdArticle = await response.Content.ReadFromJsonAsync<ArticleModel>();

            return createdArticle;
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"Articles/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RestoreArticleAsync(int id)
        {


            HttpResponseMessage response = await _httpClient.PutAsync($"Articles/restore/{id}", null);

            return response.IsSuccessStatusCode;
        }

        public async Task<ArticleModel?> UpdateArticleAsync(int id, ArticleCreateModel articleUpdate)
        {
            if (articleUpdate is null)
            {
                throw new ArgumentNullException(nameof(articleUpdate));
            }

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Articles/{id}", articleUpdate);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ArticleModel>();
        }
    }
}

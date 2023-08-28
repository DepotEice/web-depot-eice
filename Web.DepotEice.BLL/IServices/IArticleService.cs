using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IArticleService
    {
        /// <summary>
        /// Get articles by sending a GET request to the API
        /// </summary>
        /// <param name="pinned">Specify if only the pinned articles must be retrieved</param>
        /// <param name="skip">The number of elements to skip</param>
        /// <param name="top">The number of elements to keep</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="ArticleModel"/>
        /// </returns>
        Task<ResultModel<IEnumerable<ArticleModel>>> GetArticlesAsync(bool pinned = false, int skip = 0, int top = 100,
            bool descending = false);

        /// <summary>
        /// Get the article by sending a GET requesting to the API
        /// </summary>
        /// <param name="id">The id of the article</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="ArticleModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task<ResultModel<ArticleModel>> GetArticleAsync(int id);

        Task<bool> CanPinArticleAsync();

        /// <summary>
        /// Create an article by sending a POST request to the API with the form
        /// </summary>
        /// <param name="articleCreate">The form to create an article</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is the newly created <see cref="ArticleModel"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<ArticleModel>> CreateArticleAsync(ArticleCreateModel articleCreate);
        Task<ArticleModel?> UpdateArticleAsync(int id, ArticleCreateModel articleUpdate);
        Task<bool> DeleteArticleAsync(int id);
        Task<bool> RestoreArticleAsync(int id);
    }
}

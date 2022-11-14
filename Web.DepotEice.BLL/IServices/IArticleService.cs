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
        Task<IEnumerable<ArticleModel>> GetArticlesAsync();
        Task<ArticleModel?> GetArticleAsync(int id);
    }
}

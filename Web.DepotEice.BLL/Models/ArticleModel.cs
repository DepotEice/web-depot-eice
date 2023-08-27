using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.DepotEice.BLL.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }
        public int MainImageId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsPinned { get; set; }
        public string UserId { get; set; } = string.Empty;

        public string ArticleUrl
        {
            get
            {
                return $"depot/manage-article/{Id}";
            }
        }
    }
}

using Web.DepotEice.UIL.Helpers;

namespace Web.DepotEice.UIL.Models
{
    public class ArticleDetailsModel
    {
        public int Id { get; set; }
        public int MainImageId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Author { get; set; } = string.Empty;

        public string ImageUrl
        {
            get
            {
                return $"{Utils.API_BASE_URL}/Files/ById/{Id}";
            }
        }
    }
}

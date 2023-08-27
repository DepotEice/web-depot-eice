namespace Web.DepotEice.UIL.Models
{
    public class ArticleIndexModel
    {
        public int Id { get; set; }

        public string MainImageUrl { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string BodyNoHTML { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsPinned { get; set; }
    }
}

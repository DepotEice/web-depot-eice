namespace Web.DepotEice.UIL.Models
{
    public class ArticlesModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsPinned { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}

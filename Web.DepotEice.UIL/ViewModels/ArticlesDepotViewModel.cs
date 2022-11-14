namespace Web.DepotEice.UIL.ViewModels
{
    public class ArticlesDepotViewModel
    {
        public string Search { get; set; } = string.Empty;
        public bool SelectDeleted { get; set; } = false;
        public int ActualPage { get; set; } = 1;
        public int ArticlesToDisplay { get; set; } = 10;
    }
}

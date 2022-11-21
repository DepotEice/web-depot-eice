namespace Web.DepotEice.UIL.ViewModels
{
    public class ArticlesDepotViewModel
    {
        public string Search { get; set; } = string.Empty;
        public bool SelectDeleted { get; set; } = false;
        public DateTime From { get; set; }
        public DateTime To { get; set; } = DateTime.Now;
        public int ActualPage { get; set; } = 1;
        public int ArticlesToDisplay { get; set; } = 20;

        public ArticlesDepotViewModel()
        {
            From = DateTime.Now.AddYears(-1);
        }
    }
}

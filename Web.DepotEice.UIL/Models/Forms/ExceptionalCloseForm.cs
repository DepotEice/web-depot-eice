namespace Web.DepotEice.UIL.Models.Forms
{
    public class ExceptionalCloseForm
    {
        public int Id { get; set; }
        public DateTime From { get; set; } = DateTime.Now;
        public DateTime To { get; set; } = DateTime.Now;

        public ExceptionalCloseForm()
        {

        }

        public ExceptionalCloseForm(int id)
        {
            Id = id;
        }
    }
}

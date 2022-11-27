namespace Web.DepotEice.UIL.Models.Forms
{
    public class ExceptionalScheduleForm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public ExceptionalScheduleForm(int id)
        {
            Id = id;
        }
    }
}

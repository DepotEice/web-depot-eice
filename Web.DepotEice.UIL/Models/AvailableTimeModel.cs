namespace Web.DepotEice.UIL.Models
{
    public class AvailableTimeModel
    {
        public TimeOnly From { get; set; }
        public TimeOnly To { get; private set; }
        public bool IsSelected { get; set; }

        public AvailableTimeModel(TimeOnly from)
        {
            From = from;
            To = from.AddMinutes(30);
            IsSelected = false;
        }
    }
}

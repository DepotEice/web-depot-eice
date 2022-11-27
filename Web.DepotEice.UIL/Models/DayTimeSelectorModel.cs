using Web.DepotEice.Helpers;

namespace Web.DepotEice.UIL.Models
{
    public class DayTimeSelectorModel
    {
        public DaysOfTheWeekFr DayOfWeek { get; set; }
        public bool IsSelected { get; set; } = false;
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MinValue;

        public DayTimeSelectorModel()
        {

        }

        public DayTimeSelectorModel(DaysOfTheWeekFr dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
        }

        public override string ToString()
        {
            return $"{DayOfWeek} | {IsSelected} | {From} | {To}";
        }
    }
}

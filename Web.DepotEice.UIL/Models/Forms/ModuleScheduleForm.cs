using Web.DepotEice.Helpers;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class ModuleScheduleForm
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public List<DayTimeSelectorModel> DayTimeSelectors { get; set; }
        public List<ExceptionalCloseForm> ExceptionalCloses { get; set; } = new();

        public ModuleScheduleForm()
        {
            DayTimeSelectors = new List<DayTimeSelectorModel>()
            {
                new DayTimeSelectorModel(DaysOfTheWeekFr.Lundi),
                new DayTimeSelectorModel(DaysOfTheWeekFr.Mardi),
                new DayTimeSelectorModel(DaysOfTheWeekFr.Mercredi),
                new DayTimeSelectorModel(DaysOfTheWeekFr.Jeudi),
                new DayTimeSelectorModel(DaysOfTheWeekFr.Vendredi),
                new DayTimeSelectorModel(DaysOfTheWeekFr.Samedi),
            };
        }
    }
}

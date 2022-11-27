using Web.DepotEice.Helpers;
using Web.DepotEice.UIL.Models;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.ViewModels
{
    public class CreateOpeningHoursViewModel
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public List<DayTimeSelectorModel> DayTimeSelectors { get; set; }
        public List<ExceptionalCloseForm> ExceptionalCloses { get; set; } = new();

        public CreateOpeningHoursViewModel()
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

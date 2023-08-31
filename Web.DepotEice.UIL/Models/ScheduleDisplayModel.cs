using Syncfusion.Blazor.HeatMap;

namespace Web.DepotEice.UIL.Models
{
    public class ScheduleDisplayModel
    {
        public int Id { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; }  = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string RGBColor { get; private set; } = string.Empty;

        public ScheduleDisplayModel()
        {
            int red = new Random().Next(0, 200);
            int green = new Random().Next(0, 200);
            int blue = new Random().Next(0, 200);

            RGBColor = $"rgb({red},{green},{blue})";
        }

        public string ScheduleDetailsUrl
        {
            get
            {
                return $"depot/schedules/{Id}/details";
            }
        }
    }
}

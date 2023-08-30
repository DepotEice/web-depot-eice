namespace Web.DepotEice.UIL.Models
{
    public class ScheduleDisplayModel
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string TeacherName { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string ScheduleDetailsUrl
        {
            get
            {
                return $"depot/schedules/{Id}/details";
            }
        }
    }
}

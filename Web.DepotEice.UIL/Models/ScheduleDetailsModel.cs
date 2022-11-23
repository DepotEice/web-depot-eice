using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.UIL.Models
{
    public class ScheduleDetailsModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public List<ScheduleFileModel> ScheduleFiles { get; set; } = new();
    }
}

namespace Web.DepotEice.UIL.Models
{
    public class ModulePresentationModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public string ModuleUrl
        {
            get
            {
                return $"/modules/details/{Id}";
            }
        }

        public string ModuleManageUrl
        {
            get
            {
                return $"/depot/manage-module/{Id}";
            }

        }
    }
}

namespace Web.DepotEice.UIL.Models
{
    public class UserModuleRequestDisplayModel
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
    }
}

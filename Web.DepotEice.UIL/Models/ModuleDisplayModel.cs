namespace Web.DepotEice.UIL.Models
{
    public class ModuleDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TeacherFullName { get; set; } = string.Empty;

        public string RGBColor { get; private set; }

        public ModuleDisplayModel()
        {
            int red = new Random().Next(0, 200);
            int green = new Random().Next(0, 200);
            int blue = new Random().Next(0, 200);

            RGBColor = $"rgb({red},{green},{blue})";
        }
    }
}

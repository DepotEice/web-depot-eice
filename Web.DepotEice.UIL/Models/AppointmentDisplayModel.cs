namespace Web.DepotEice.UIL.Models
{
    public class AppointmentDisplayModel
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsAccepted { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string UserFullName { get; set; } = string.Empty;

        public string RGBColor { get; private set; } = string.Empty;

        public AppointmentDisplayModel()
        {
            int red = new Random().Next(0, 200);
            int green = new Random().Next(0, 200);
            int blue = new Random().Next(0, 200);

            RGBColor = $"rgb({red},{green},{blue})";
        }
    }
}

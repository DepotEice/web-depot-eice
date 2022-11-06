using System.ComponentModel.DataAnnotations;
using Web.DepotEice.Helpers;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class AppointmentForm
    {
        [Required(ErrorMessage = "Veuillez choisir une heure de rendez-vous")]
        [DateTimeGreaterThanToday(ErrorMessage = "La date et l'heure de rendez-vous ne peuvent pas être dans le passé")]
        public DateTime From { get; set; } = DateTime.Now;
    }
}

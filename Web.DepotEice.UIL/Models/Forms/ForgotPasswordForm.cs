using System.ComponentModel.DataAnnotations;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class ForgotPasswordForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer une adresse email !")]
        [EmailAddress(ErrorMessage = "Veuillez entrer une adresse email valide !")]
        public string Email { get; set; } = string.Empty;
    }
}

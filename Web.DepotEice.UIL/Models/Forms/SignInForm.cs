using System.ComponentModel.DataAnnotations;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class SignInForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer une adresse email valide !")]
        public string Email { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer votre mot de passe !")]
        public string Password { get; set; } = string.Empty;
    }
}

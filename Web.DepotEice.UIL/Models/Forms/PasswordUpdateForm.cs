using System.ComponentModel.DataAnnotations;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class PasswordUpdateForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez saisir votre mot de passe actuel")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer le nouveau mot de passe !")]
        [RegularExpression("(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_+]).{8,20}$",
            ErrorMessage = "Le mot de passe doit faire entre 8 et 20 caractères et doit contenir au moins une minuscule, une majuscule, un nombre et un caractère spécial!")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez confirmer le nouveau mot de passe !")]
        [Compare("NewPassword", ErrorMessage = "Les mots de passe ne correspondent pas !")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}

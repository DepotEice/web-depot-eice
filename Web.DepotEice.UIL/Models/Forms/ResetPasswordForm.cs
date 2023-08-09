using System.ComponentModel.DataAnnotations;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class ResetPasswordForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer un mot de passe !")]
        [RegularExpression("(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_+]).{8,20}$",
            ErrorMessage = "Le mot de passe doit faire entre 8 et 20 caractères de longs, contenir une lettre " +
            "minuscule, une majuscule et un caractère spéciale !")]
        public string Password { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez confirmer votre mot de passe !")]
        [Compare(nameof(Password), ErrorMessage = "Les mots de passes ne concordent pas !")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

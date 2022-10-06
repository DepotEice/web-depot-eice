using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class RegisterForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "L'adresse email est requise!")]
        [EmailAddress(ErrorMessage = "Le format de l'adresse email est incorrect!")]
        public string? Email { get; set; }

        [Compare(nameof(Email), ErrorMessage = "Les adresses emails ne correspondent pas!")]
        public string? ConfirmEmail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer un mot de passe !")]
        [RegularExpression("(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_+]).{8,20}$",
            ErrorMessage = "Le mot de passe doit faire entre 8 et 20 caractères et doit contenir au moins une minuscule, une majuscule, un nombre et un caractère spécial!")]
        public string? Password { get; set; }

        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer votre prénom !")]
        public string? FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer votre nom de famille !")]
        public string? LastName { get; set; }
    }
}
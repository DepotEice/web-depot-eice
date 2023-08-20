using System.ComponentModel.DataAnnotations;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class AddressForm
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La rue est requise !")]
        [MaxLength(150, ErrorMessage = "La rue doit faire au plus 150 caractères !")]
        public string Street { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Le numéro de rue est requis !")]
        [MaxLength(15, ErrorMessage = "Le numéro de rue doit faire au plus 15 caractères !")]
        public string HouseNumber { get; set; } = string.Empty;

        [MaxLength(15, ErrorMessage = "Le numéro d'appartement doit faire au plus 15 caractères !")]
        public string? Appartment { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La ville est requise !")]
        [MaxLength(100, ErrorMessage = "La ville doit faire au plus 100 caractères !")]
        public string City { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "L'état/La province doit faire au plus 100 caractères !")]
        public string State { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Le code postal est requis !")]
        [MaxLength(15, ErrorMessage = "Le code postal doit faire au plus 15 caractères !")]
        public string Zipcode { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Le pays est requis !")]
        [MaxLength(100, ErrorMessage = "Le pays doit faire au plus 100 caractères !")]
        public string Country { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
    }
}

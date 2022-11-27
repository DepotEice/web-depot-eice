using System.ComponentModel.DataAnnotations;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class ModuleForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer un titre")]
        [MaxLength(100, ErrorMessage = "La longueur maximale du titre est de 100 caractères")]
        public string Title { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer une description")]
        [MaxLength(1000, ErrorMessage = "La longueur maximale de la description est de 1000 caractères")]
        public string Description { get; set; } = string.Empty;

        public string? TeacherId { get; set; }
    }
}

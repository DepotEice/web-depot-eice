using System.ComponentModel.DataAnnotations;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class ArticleForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Le titre est requis")]
        [MaxLength(100, ErrorMessage = "Le titre ne peut pas dépasser les 100 caractères")]
        public string Title { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Veuillez entrer un contenu pour l'article")]
        public string Body { get; set; } = string.Empty;

        public bool Pinned { get; set; }
    }
}

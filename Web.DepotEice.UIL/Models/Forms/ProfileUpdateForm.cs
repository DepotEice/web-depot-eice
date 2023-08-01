using Microsoft.AspNetCore.Components.Forms;

namespace Web.DepotEice.UIL.Models.Forms
{
    public class ProfileUpdateForm
    {
        public string ImgUrl { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SchoolEmail { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}

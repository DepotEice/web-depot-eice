using AutoMapper;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UpdatePasswordForm, PasswordUpdateModel>();
        }
    }
}

using AutoMapper;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<SignInForm, SignInModel>();
            CreateMap<RegisterForm, SignUpModel>();
        }
    }
}

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
            CreateMap<RegisterForm, SignUpModel>()
                .ForMember(
                    dest => dest.BirthDate,
                    opt => opt.MapFrom(
                        src => new DateTime(src.BirthDate.Year, src.BirthDate.Month, src.BirthDate.Day)));
        }
    }
}

using AutoMapper;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.UIL.Models;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ResetPasswordForm, PasswordResetModel>();

            CreateMap<UserModel, UserSelectionModel>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(
                        src => $"{src.LastName.ToUpper()} {src.FirstName}"));

            CreateMap<UserModel, ProfileUpdateForm>();

            CreateMap<ProfileUpdateForm, UserUpdateModel>();

            CreateMap<PasswordUpdateForm, PasswordUpdateModel>();

            CreateMap<UserModel, UserUpdateModel>();
        }
    }
}

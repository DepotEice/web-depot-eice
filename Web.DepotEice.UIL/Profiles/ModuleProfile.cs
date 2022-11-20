using AutoMapper;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.Profiles
{
    public class ModuleProfile : Profile
    {
        public ModuleProfile()
        {
            CreateMap<ModuleForm, ModuleCreationModel>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(
                        src => src.Title));

            CreateMap<ModuleModel, ModuleForm>()
                .ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(
                        src => src.Name));

            CreateMap<ScheduleForm, ScheduleCreateModel>();
        }
    }
}

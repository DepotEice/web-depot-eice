using AutoMapper;
using Web.DepotEice;
using Web.DepotEice.UIL.Models;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.Profiles
{
    public class ModuleProfile : Profile
    {
        public ModuleProfile()
        {
            CreateMap<ModuleForm, BLL.Models.ModuleCreationModel>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(
                        src => src.Title));

            CreateMap<BLL.Models.ModuleModel, ModuleForm>()
                .ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(
                        src => src.Name));

            CreateMap<ScheduleForm, BLL.Models.ScheduleCreateModel>();

            CreateMap<BLL.Models.ModuleModel, ModulePresentationModel>()
                .ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(
                        src => src.Name))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(
                        src => src.Description.Substring(0, src.Description.Length > 250 ? 250 : src.Description.Length)));

            CreateMap<BLL.Models.ModuleModel, UIL.Models.ModuleModel>();

            CreateMap<BLL.Models.ScheduleModel, UIL.Models.ScheduleModel>()
                .ForMember(
                    dest => dest.StartTime,
                    opt => opt.MapFrom(
                        src => src.StartAt))
                .ForMember(
                    dest => dest.EndTime,
                    opt => opt.MapFrom(
                        src => src.EndAt));
        }
    }
}

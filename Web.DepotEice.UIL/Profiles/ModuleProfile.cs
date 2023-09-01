using AutoMapper;
using Web.DepotEice;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.UIL.Models;
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

            CreateMap<ModuleModel, ModulePresentationModel>()
                .ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(
                        src => src.Name))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(
                        src => src.Description.Substring(0, src.Description.Length > 250 ? 250 : src.Description.Length)));

            CreateMap<ModuleModel, ModuleDisplayModel>();

            CreateMap<ScheduleModel, ScheduleDisplayModel>()
                .ForMember(
                    dest => dest.StartTime,
                    opt => opt.MapFrom(
                        src => src.StartAt))
                .ForMember(
                    dest => dest.EndTime,
                    opt => opt.MapFrom(
                        src => src.EndAt));

            CreateMap<ScheduleModel, ScheduleDetailsModel>();

            CreateMap<ScheduleDetailsModel, ScheduleCreateModel>();
        }
    }
}

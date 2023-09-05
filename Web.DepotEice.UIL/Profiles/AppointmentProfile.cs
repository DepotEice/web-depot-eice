using AutoMapper;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.UIL.Models;

namespace Web.DepotEice.UIL.Profiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<AppointmentModel, AppointmentDisplayModel>()
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

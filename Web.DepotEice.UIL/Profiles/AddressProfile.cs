using AutoMapper;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressModel, AddressForm>();
            CreateMap<AddressForm, AddressCreateModel>();
            CreateMap<AddressForm, AddressUpdateModel>();
        }
    }
}

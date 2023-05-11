using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressModel>> GetAddresses();
        Task<AddressModel?> GetAddress(int id);
        Task<AddressModel?> CreateAddress(AddressCreateModel addressCreate);
        Task<AddressModel?> UpdateAddress(AddressUpdateModel addressUpdate);
        Task<bool> DeleteAddress(int id);
    }
}

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
        Task<ResultModel<IEnumerable<AddressModel>>> GetAddressesAsync();
        Task<ResultModel<AddressModel>> GetAddressAsync(int id);
        Task<ResultModel<AddressModel>> CreateAddressAsync(AddressCreateModel addressCreate);
        Task<ResultModel<AddressModel>> UpdateAddressAsync(AddressUpdateModel addressUpdate);
        Task<ResultModel<AddressModel>> DeleteAddressAsync(int id);
        Task<ResultModel<AddressModel>> SetPrimaryAddress(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IUserService
    {
        Task<bool> UpdatePassword(PasswordUpdateModel passwordUpdate, string? token = null);
        Task<IEnumerable<UserModel>> GetTeachersAsync();

        Task<UserModel?> GetUserAsync(string? id);
        Task<UserModel?> GetUserAsync();
    }
}

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
        /// <summary>
        /// Update user password
        /// </summary>
        /// <param name="passwordUpdate">The password update form</param>
        /// <returns>Result Model of UserModel</returns>
        Task<ResultModel<UserModel>> UpdatePasswordAsync(PasswordUpdateModel passwordUpdate);
        Task<IEnumerable<UserModel>> GetTeachersAsync();

        Task<UserModel?> GetUserAsync(string? id);
        Task<UserModel?> GetUserAsync();
        Task<UserModel?> UpdateUserAsync(UserUpdateModel userUpdateModel);
        Task<dynamic> GetProfilePictureAsync(string? userId = null);
    }
}

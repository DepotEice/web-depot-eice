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

        /// <summary>
        /// Get the list of all teachers
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        Task<ResultModel<IEnumerable<UserModel>>> GetTeachersAsync();

        /// <summary>
        /// Get the user by sending a GET request to the API and by providing the user ID
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<UserModel>> GetUserAsync(string? userId);

        Task<UserModel?> GetUserAsync();
        Task<UserModel?> UpdateUserAsync(UserUpdateModel userUpdateModel);

        /// <summary>
        /// Update the user profile picture
        /// </summary>
        /// <param name="imageContent">byte content of the image</param>
        /// <param name="contentType">content type</param>
        /// <returns><see cref="ResultModel{UserModel?}"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        Task<ResultModel<Stream?>> UpdateProfilePictureAsync(byte[] imageContent, string contentType);
        Task<ResultModel<bool>> DeleteUserAsync(string? userId = null);
    }
}

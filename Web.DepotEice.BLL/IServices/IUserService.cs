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

        /// <summary>
        /// Get the current user by sending a GET request to the API
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        Task<ResultModel<UserModel>> GetUserAsync();

        /// <summary>
        /// Update the user by sending a PUT request to the API
        /// </summary>
        /// <param name="userUpdateModel">The user form</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="UserModel"/> which is the updated user data. Null if 
        /// the update failed
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        Task<ResultModel<UserModel>> UpdateUserAsync(UserUpdateModel userUpdateModel);

        /// <summary>
        /// Update the user profile picture
        /// </summary>
        /// <param name="imageContent">byte content of the image</param>
        /// <param name="contentType">content type</param>
        /// <returns><see cref="ResultModel{UserModel?}"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        Task<ResultModel<Stream>> UpdateProfilePictureAsync(byte[] imageContent, string contentType);

        /// <summary>
        /// Delete the user by sending a DELETE request to the API
        /// </summary>
        /// <param name="userId">(Optional) The id of the user to delete</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/> which is true if the user was deleted, false otherwise
        /// </returns>
        Task<ResultModel<bool>> DeleteUserAsync(string? userId = null);

        /// <summary>
        /// Get all users from the API by sending a GET request to the API. <br />
        /// Get all the available users if the admin parameter is false, otherwise get all the users. <br />
        /// To retrieve all users, including the deleted ones, the user must be admin
        /// </summary>
        /// <param name="all">Specify if all the users (including the deleted one) should be loaded</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        Task<ResultModel<IEnumerable<UserModel>>> GetUsersAsync(bool all = false);

        /// <summary>
        /// Get all the students by sending a GET request to the API
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        Task<ResultModel<IEnumerable<UserModel>>> GetStudentsAsync();
    }
}

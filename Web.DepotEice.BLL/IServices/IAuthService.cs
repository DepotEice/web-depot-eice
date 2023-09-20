using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IAuthService
    {
        Task<string> SignInAsync(SignInModel signInModel);
        Task<bool> SignUpAsync(SignUpModel signUpModel);
        Task<bool> RequestNewPassword(string email);

        /// <summary>
        /// Activate the user account by sending a POST request to the API
        /// </summary>
        /// <param name="tokenId">The id of the user</param>
        /// <param name="tokenValue">The activation token</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is a <see cref="bool"/>. If the activation (status code 200) was 
        /// successful, the data will be true, otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<bool>> ActivateAsync(string tokenId, string tokenValue);
        Task<bool> ResetPassword(PasswordResetModel passwordResetModel, string token);

        /// <summary>
        /// Validate the current token
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is a <see cref="bool"/>
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        Task<ResultModel<bool>> AuthorizeAsync();
    }
}

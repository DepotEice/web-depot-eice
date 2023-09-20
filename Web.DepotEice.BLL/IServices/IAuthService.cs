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
        /// <summary>
        /// Sign in the user by sending a POST request to the API and return the JWT token if the request is successful
        /// </summary>
        /// <param name="signInModel">The login form</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is a <see cref="string"/>. If the request is successful, get the JWT
        /// token
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<SignInResponseModel>> SignInAsync(SignInModel signInModel);
        Task<bool> SignUpAsync(SignUpModel signUpModel);

        /// <summary>
        /// Request a new password by sending a GET request to the API
        /// </summary>
        /// <param name="email">The email address associated to the account</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is a <see cref="bool"/>. If the request was successful, the data will 
        /// be true, otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<bool>> RequestPasswordAsync(string email);

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

        /// <summary>
        /// Reset the user password by sending a POST request to the API
        /// </summary>
        /// <param name="passwordResetModel">The password form to reset the password</param>
        /// <param name="token">The token to validate the password reset</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is a <see cref="bool"/>. The data is valid if the request is successful.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<bool>> ResetPasswordAsync(PasswordResetModel passwordResetModel, string token);

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

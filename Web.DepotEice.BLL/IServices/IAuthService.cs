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
        Task<bool> Activate(string userId, string token);
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

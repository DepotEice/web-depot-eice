using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IUserTokenService
    {
        /// <summary>
        /// Validate the current token by sending a GET request to the API
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<bool>> IsValidAsync(string token);
    }
}

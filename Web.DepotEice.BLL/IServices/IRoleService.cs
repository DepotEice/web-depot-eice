using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IRoleService
    {
        Task<bool> UserHasRoleAsync(string role);
        Task<bool> UserHasRoleAsync(string userId, string role);

        /// <summary>
        /// Get all the roles of the current user if the id is not provided or get the roles of the user with the provided id
        /// </summary>
        /// <param name="userId">The id</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{RoleModel}"/>
        /// </returns>
        Task<ResultModel<IEnumerable<RoleModel>>> GetRolesAsync(string? userId = null);
    }
}

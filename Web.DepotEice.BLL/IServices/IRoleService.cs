using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.DepotEice.BLL.IServices
{
    public interface IRoleService
    {
        Task<bool> UserHasRoleAsync(string role);
        Task<bool> UserHasRoleAsync(string userId, string role);
    }
}

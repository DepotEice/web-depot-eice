using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.DepotEice.BLL.Models
{
    /// <summary>
    /// The role model from the API
    /// </summary>
    public class RoleModel
    {
        /// <summary>
        /// The id of the role
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The name of the role
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}

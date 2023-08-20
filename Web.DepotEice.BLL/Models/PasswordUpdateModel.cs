using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.DepotEice.BLL.Models
{
    public class PasswordUpdateModel
    {
        /// <summary>
        /// The user's current password
        /// </summary>
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// The user's new password
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Confirmation of the new password
        /// </summary>
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}

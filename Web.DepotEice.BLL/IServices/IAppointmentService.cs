using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IAppointmentService
    {
        Task<bool> CreateAppointmentAsync(AppointmentCreateModel appointmentCreate);
        Task<IEnumerable<AppointmentModel>> GetAppointmentsAsync();
        Task<AppointmentModel?> GetAppointmentAsync(int id);
    }
}

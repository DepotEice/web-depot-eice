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
        Task<ResultModel<AppointmentModel>> CreateAppointmentAsync(AppointmentCreateModel appointmentCreate);
        Task<ResultModel<IEnumerable<AppointmentModel>>> GetAppointmentsAsync(DateTime? date = null);
        Task<AppointmentModel?> GetAppointmentAsync(int id);
    }
}

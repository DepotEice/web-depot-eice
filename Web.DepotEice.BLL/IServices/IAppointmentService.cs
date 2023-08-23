using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    /// <summary>
    /// Appointment service interface
    /// </summary>
    public interface IAppointmentService
    {
        /// <summary>
        /// Create an appointment by sending a POST request to the API
        /// </summary>
        /// <param name="appointmentCreate">The model to send to the API</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AppointmentModel"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<AppointmentModel>> CreateAppointmentAsync(AppointmentCreateModel appointmentCreate);

        /// <summary>
        /// Update an appointment by sending a PUT request to the API
        /// </summary>
        /// <param name="id">The id of the appointment to update</param>
        /// <param name="appointment">The appointment form</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AppointmentModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<AppointmentModel?>> UpdateAppointmentAsync(int id, AppointmentCreateModel appointment);

        /// <summary>
        /// Get all appointments by sending a GET request to the API
        /// </summary>
        /// <param name="date">The datetime</param>
        /// <param name="dateRange">The range to select</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{AppointmentModel}"/> where T is <see cref="AppointmentModel"/>
        /// </returns>
        Task<ResultModel<IEnumerable<AppointmentModel>>> GetAppointmentsAsync(DateTime? date = null, int? dateRange = null);

        /// <summary>
        /// Get an appointment by sending a GET request to the API
        /// </summary>
        /// <param name="id">Id of the appointment</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AppointmentModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task<ResultModel<AppointmentModel?>> GetAppointmentAsync(int id);

        /// <summary>
        /// Delete an appointment by sending a DELETE request to the API
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task<ResultModel<bool>> DeleteAppointmentAsync(int id);
    }
}

using AutoMapper;
using Microsoft.Extensions.Logging;
using Web.DepotEice.BLL.IServices;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.BLL.Services;
using Web.DepotEice.UIL.Models;
using Web.DepotEice.UIL.Pages.Profile;
using static Web.DepotEice.UIL.Helpers.Utils;

namespace Web.DepotEice.UIL.Managers
{
    public class AppointmentManager
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;

        public AppointmentManager(ILogger<AppointmentManager> logger, IMapper mapper, IAppointmentService appointmentService,
            IUserService userService)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (appointmentService is null)
            {
                throw new ArgumentNullException(nameof(appointmentService));
            }

            if (userService is null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            _logger = logger;
            _mapper = mapper;
            _appointmentService = appointmentService;
            _userService = userService;
        }

        /// <summary>
        /// Get appointments with the user information
        /// </summary>
        /// <param name="dateTime">The date to select</param>
        /// <param name="range">The range based on the date</param>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> where T is <see cref="AppointmentDisplayModel"/>
        /// </returns>
        public async Task<IEnumerable<AppointmentDisplayModel>> GetDisplayAppointmentsAsync(DateTime dateTime, DateRange range)
        {
            ResultModel<IEnumerable<AppointmentModel>> resultModel =
                await _appointmentService.GetAppointmentsAsync(dateTime, (int)range);

            if (!resultModel.Success)
            {
                _logger.LogError($"The appointments couldn't be loaded:\n{resultModel.Message}");

                return Enumerable.Empty<AppointmentDisplayModel>();
            }

            if (resultModel.Data is null)
            {
                _logger.LogError($"The list of appointment is null");

                return Enumerable.Empty<AppointmentDisplayModel>();
            }

            var appointments = _mapper.Map<List<AppointmentDisplayModel>>(resultModel.Data);

            foreach (var appointment in appointments)
            {
                var userFromService = await _userService.GetUserAsync(appointment.UserId);

                if (userFromService is null)
                {
                    _logger.LogError("The user for this appointment is null");

                    continue;
                }

                appointment.UserFullName = $"{userFromService.LastName.ToUpperInvariant()} {userFromService.FirstName}";
            }

            return appointments;
        }
    }
}

using AutoMapper;
using BlazorBootstrap;
using Microsoft.Extensions.Logging;
using Web.DepotEice.BLL.IServices;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.BLL.Services;
using Web.DepotEice.UIL.Models;
using static Web.DepotEice.UIL.Helpers.Utils;

namespace Web.DepotEice.UIL.Managers
{
    public class ScheduleManager
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IModuleService _moduleService;
        private readonly IUserService _userService;

        public ScheduleManager(ILogger<ScheduleManager> logger, IMapper mapper, IModuleService moduleService,
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

            if (moduleService is null)
            {
                throw new ArgumentNullException(nameof(moduleService));
            }

            if (userService is null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            _logger = logger;
            _mapper = mapper;
            _moduleService = moduleService;
            _userService = userService;
        }

        /// <summary>
        /// Get the list of schedules with the module and teacher names.
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> where T is <see cref="ScheduleDisplayModel"/>
        /// </returns>
        public async Task<IEnumerable<ScheduleDisplayModel>> GetSchedulesAsync(DateTime selectedDate, DateRange dateRange)
        {
            ResultModel<IEnumerable<ScheduleModel>> result = await _moduleService.GetSchedulesAsync(selectedDate, (int)dateRange);

            if (!result.Success)
            {
                _logger.LogError("The request failed with status code {code}.\n{msg}", result.Code, result.Message);

                return Enumerable.Empty<ScheduleDisplayModel>();
            }

            if (result.Data is null)
            {
                _logger.LogError("The request succeeded but the returned data is null.\n{msg}", result.Message);

                return Enumerable.Empty<ScheduleDisplayModel>();
            }

            List<int> moduleIds = result.Data.Select(s => s.ModuleId).Distinct().ToList();
            List<ModuleModel> modules = new List<ModuleModel>();
            List<UserModel> teachers = new List<UserModel>();

            foreach (int moduleId in moduleIds)
            {
                ResultModel<ModuleModel> moduleResult = await _moduleService.GetModuleAsync(moduleId);

                if (!moduleResult.Success)
                {
                    _logger.LogError(
                        "The request failed with status code {code}.\n{msg}",
                        moduleResult.Code,
                        moduleResult.Message
                    );

                    continue;
                }

                if (moduleResult.Data is null)
                {
                    _logger.LogError(
                        "The request succeeded but the returned data is null.\n{msg}",
                        moduleResult.Message
                    );

                    continue;
                }

                modules.Add(moduleResult.Data);

                ResultModel<UserModel> teacherResult = await _moduleService.GetTeacherAsync(moduleId);

                if (!teacherResult.Success)
                {
                    _logger.LogError(
                        "The request failed with status code {code}.\n{msg}",
                        teacherResult.Code,
                        teacherResult.Message
                    );

                    continue;
                }

                if (teacherResult.Data is null)
                {
                    _logger.LogError(
                        "The request succeeded but the returned data is null.\n{msg}",
                        teacherResult.Message
                    );

                    continue;
                }

                teachers.Add(teacherResult.Data);
            }

            IEnumerable<ScheduleDisplayModel> schedules = _mapper.Map<IEnumerable<ScheduleDisplayModel>>(result.Data);

            foreach (ScheduleDisplayModel schedule in schedules)
            {
                ModuleModel? scheduleModule = modules.FirstOrDefault(m => m.Id == schedule.ModuleId);

                if (scheduleModule is null)
                {
                    _logger.LogError("The module with id {id} was not found.", schedule.ModuleId);

                    continue;
                }

                schedule.ModuleName = scheduleModule.Name;

                UserModel? scheduleTeacher = teachers.FirstOrDefault(t => t.Id == scheduleModule.TeacherId);

                if (scheduleTeacher is null)
                {
                    _logger.LogError("The teacher with id {id} was not found.", scheduleModule.TeacherId);

                    continue;
                }

                schedule.TeacherName = $"{scheduleTeacher.LastName}, {scheduleTeacher.FirstName}";
            }

            return schedules;
        }

        /// <summary>
        /// Get the schedule details with the module information and the teacher name.
        /// </summary>
        /// <param name="scheduleId">
        /// The id of the schedule
        /// </param>
        /// <returns>
        /// <see cref="ScheduleDetailsModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ScheduleDetailsModel> GetScheduleAsync(int scheduleId)
        {
            if (scheduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scheduleId));
            }

            ResultModel<ScheduleModel> result = await _moduleService.GetScheduleAsync(scheduleId);

            if (!result.Success)
            {
                _logger.LogError("The request failed with status code {code}.\n{msg}", result.Code, result.Message);

                return new ScheduleDetailsModel();
            }

            if (result.Data is null)
            {
                _logger.LogError("The request succeeded but the returned data is null.\n{msg}", result.Message);

                return new ScheduleDetailsModel();
            }

            ScheduleDetailsModel schedule = _mapper.Map<ScheduleDetailsModel>(result.Data);

            ResultModel<ModuleModel> moduleResult = await _moduleService.GetModuleAsync(schedule.ModuleId);

            if (!moduleResult.Success)
            {
                _logger.LogError(
                    "The request failed with status code {code}.\n{msg}",
                    moduleResult.Code,
                    moduleResult.Message
                );

                return schedule;
            }

            if (moduleResult.Data is null)
            {
                _logger.LogError(
                    "The request succeeded but the returned data is null.\n{msg}",
                    moduleResult.Message
                );

                return schedule;
            }

            schedule.ModuleName = moduleResult.Data.Name;

            ResultModel<UserModel> teacherResult = await _moduleService.GetTeacherAsync(schedule.ModuleId);

            if (!teacherResult.Success)
            {
                _logger.LogError(
                    "The request failed with status code {code}.\n{msg}",
                    teacherResult.Code,
                    teacherResult.Message
                );

                return schedule;
            }

            if (teacherResult.Data is null)
            {
                _logger.LogError(
                    "The request succeeded but the returned data is null.\n{msg}",
                    teacherResult.Message
                );

                return schedule;
            }

            schedule.TeacherName = $"{teacherResult.Data.LastName}, {teacherResult.Data.FirstName}";

            return schedule;
        }
    }
}

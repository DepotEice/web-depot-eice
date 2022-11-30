using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleModel>> GetModulesAsync();
        Task<ModuleModel?> CreateModuleAsync(ModuleCreationModel moduleCreation);
        Task<ModuleModel?> UpdateModuleAsync(int id, ModuleCreationModel moduleCreation);
        Task<ModuleModel?> GetModuleAsync(int id);
        Task<ScheduleModel?> CreateScheduleAsync(int moduleId, ScheduleCreateModel scheduleCreate);
        Task<IEnumerable<ScheduleModel>> GetModuleSchedulesAsync(int moduleId);
        Task<bool> DeleteScheduleAsync(int moduleId, int scheduleId);
        Task<bool> DeleteScheduleFileAsync(int moduleId, int scheduleId, int scheduleFileId);
        Task<IEnumerable<UserModuleRequestModel>> GetUsersRequestingModules();
        Task<bool?> UserIsAccepted(int moduleId);
        Task<bool?> UserIsAccepted(int moduleId, string userId);
        Task<bool> RequestAcceptance(int moduleId);
        Task<IEnumerable<ScheduleModel>> GetSchedulesAsync();
        Task<IEnumerable<ScheduleModel>> GetSchedulesAsync(int moduleId);
        Task<ScheduleModel?> GetScheduleAsync(int id);
        Task<IEnumerable<ScheduleFileModel>> GetScheduleFilesAsync(int scheduleId);
        Task<ScheduleFileModel?> GetScheduleFileAsync(int id);
        Task<bool> UserHasRoleAsync(string role, int moduleId);
    }
}

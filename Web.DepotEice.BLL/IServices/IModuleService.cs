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
        Task<ModuleModel?> GetModuleAsync(int id);
        Task<ScheduleModel?> CreateScheduleAsync(int moduleId, ScheduleCreateModel scheduleCreate);
        Task<IEnumerable<ScheduleModel>> GetModuleSchedulesAsync(int moduleId);
        Task<bool> DeleteScheduleAsync(int moduleId, int scheduleId);
    }
}

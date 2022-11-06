using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IOpeningHoursService
    {
        Task<IEnumerable<OpeningHoursModel>> GetOpeningHoursAsync();
        Task<IEnumerable<OpeningHoursModel>> GetWeekOpeningHoursAsync(DateTime dateTime);
        Task<IEnumerable<DateTime>> GetClosedDatesAsync();
        Task<OpeningHoursModel> CreateOpeningHoursAsync(OpeningHoursCreateModel openingHours);
    }
}

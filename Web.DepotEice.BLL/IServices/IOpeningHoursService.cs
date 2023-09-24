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
        /// <summary>
        /// Get the opening hours of the API, if no parameters are given, it will return all the opening hours
        /// </summary>
        /// <param name="day">The day date</param>
        /// <param name="month">The month</param>
        /// <param name="year">The year</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="OpeningHoursModel"/>
        /// </returns>
        Task<ResultModel<IEnumerable<OpeningHoursModel>>> GetOpeningHoursAsync(
            int? day = null,
            int? month = null,
            int? year = null
        );

        /// <summary>
        /// Create an opening hours by sending a POST request to the API
        /// </summary>
        /// <param name="openingHours">The data to create the opening hours</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="OpeningHoursModel"/>. The created opening hours if it 
        /// was created, null otherwise
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<OpeningHoursModel>> CreateOpeningHoursAsync(OpeningHoursCreateModel openingHours);

        /// <summary>
        /// Delete an opening hours by sending a DELETE request to the API
        /// </summary>
        /// <param name="id">The id of the opening hours</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>. True if the opening hours was deleted, false otherwise
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task<ResultModel<bool>> DeleteOpeningHoursAsync(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.IServices;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.Services
{
    public class OpeningHoursService : IOpeningHoursService
    {
        private readonly HttpClient _httpClient;

        public OpeningHoursService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7205/api/"),
            };
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<IEnumerable<OpeningHoursModel>> GetOpeningHoursAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("OpeningHours");

            IEnumerable<OpeningHoursModel>? openingHours =
                await response.Content.ReadFromJsonAsync<IEnumerable<OpeningHoursModel>>();

            if (openingHours is null)
            {
                return Enumerable.Empty<OpeningHoursModel>();
            }

            return openingHours;
        }

        public async Task<IEnumerable<OpeningHoursModel>> GetWeekOpeningHoursAsync(DateTime dateTime)
        {
            IEnumerable<OpeningHoursModel> openingHours = await GetOpeningHoursAsync();

            int actualDay = (int)dateTime.DayOfWeek;

            return openingHours.Where(oh =>
                (oh.OpenAt.Year == dateTime.Year && oh.OpenAt.Month == dateTime.Month) &&
                (oh.OpenAt.Day >= dateTime.Day - actualDay && oh.OpenAt.Day <= dateTime.Day + (7 - actualDay)));
        }

        public async Task<IEnumerable<DateTime>> GetClosedDatesAsync()
        {
            IEnumerable<OpeningHoursModel> openingHours = await GetOpeningHoursAsync();

            if (openingHours.Count() == 0)
            {
                return Enumerable.Empty<DateTime>();
            }

            var firstOpeningHours = openingHours.First();
            var lastOpeningHours = openingHours.Last();

            DateTime firstDayOpen = new DateTime(
                firstOpeningHours.OpenAt.Year,
                firstOpeningHours.OpenAt.Month,
                firstOpeningHours.OpenAt.Day);

            DateTime lastDayOpen = new DateTime(
                lastOpeningHours.CloseAt.Year,
                lastOpeningHours.CloseAt.Month,
                lastOpeningHours.CloseAt.Day);

            List<DateTime> closedDateTimes = new List<DateTime>();

            for (DateTime dti = firstDayOpen; dti <= lastDayOpen; dti.AddDays(1))
            {
                if (dti.DayOfWeek != DayOfWeek.Sunday)
                {
                    closedDateTimes.Append(dti);
                }
            }

            return closedDateTimes;
        }

        public async Task<OpeningHoursModel> CreateOpeningHoursAsync(OpeningHoursCreateModel openingHours)
        {
            if (openingHours is null)
            {
                throw new ArgumentNullException(nameof(openingHours));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("OpeningHours", openingHours);

            response.EnsureSuccessStatusCode();

            OpeningHoursModel? createdOpeningHours = await response.Content.ReadFromJsonAsync<OpeningHoursModel>();

            if (createdOpeningHours is null)
            {
                throw new NullReferenceException(nameof(createdOpeningHours));
            }

            return createdOpeningHours;
        }

        public async Task<bool> DeleteOpeningHoursAsync(OpeningHoursModel openingHours)
        {
            if (openingHours is null)
            {
                throw new ArgumentNullException(nameof(openingHours));
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync($"OpeningHours/{openingHours.Id}");

            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}

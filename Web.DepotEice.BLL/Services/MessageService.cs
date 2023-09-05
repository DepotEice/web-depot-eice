using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
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
    public class MessageService : IMessageService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;

        public MessageService(ILogger<AddressService> logger, HttpClient httpClient, ISyncLocalStorageService localStorageService)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            _logger = logger;
            _httpClient = httpClient;
            _localStorageService = localStorageService;

            string token = _localStorageService.GetItemAsString("token");

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Get the current user's conversations
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="ConversationModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<ConversationModel>>> GetConversationsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Messages/Conversations/Me");

            ResultModel<IEnumerable<ConversationModel>> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<ConversationModel>>();
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                    "{fn}: An exception was thrown, cannot read the result as json.\n{eMesg}",
                    nameof(GetConversationsAsync),
                    e.Message
                );
            }

            return result;
        }
    }
}

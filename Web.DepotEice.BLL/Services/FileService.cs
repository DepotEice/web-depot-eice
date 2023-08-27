using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.IServices;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;

        public FileService(ILogger<FileService> logger, HttpClient httpClient, ISyncLocalStorageService localStorageService)
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
        /// Get the default picture URL
        /// </summary>
        /// <returns>The default picture URL</returns>
        public string GetDefaultPictureURL()
        {
            return $"${_httpClient.BaseAddress}/Files/DefaultProfilePicture";
        }

        public void GetFileAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upload a file by sending a POST request to the server
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="stream">The stream content of the file</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="int?"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<IEnumerable<int>?>> UploadFileAsync(string fileName, string contentType, Stream stream)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            try
            {
                using MultipartFormDataContent content = new MultipartFormDataContent();

                StreamContent streamContent = new StreamContent(stream);

                streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

                content.Add(content: streamContent, name: "uploadFiles", fileName: fileName);

                HttpResponseMessage response = await _httpClient.PostAsync("Files", content);

                ResultModel<IEnumerable<int>?> resultModel = new ResultModel<IEnumerable<int>?>()
                {
                    Code = response.StatusCode,
                    Success = response.IsSuccessStatusCode,
                    Message = await response.Content.ReadAsStringAsync()
                };

                try
                {
                    resultModel.Data = await response.Content.ReadFromJsonAsync<IEnumerable<int>?>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                    $"{nameof(UploadFileAsync)}: an exception was thrown while converting result to json.\n{ex.Message}");

                    resultModel.Data = null;
                }

                return resultModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while uploading file");

                return new ResultModel<IEnumerable<int>?>()
                {
                    Success = false,
                    Code = 0,
                    Data = null,
                    Message = ex.Message
                };
            }
        }
    }
}

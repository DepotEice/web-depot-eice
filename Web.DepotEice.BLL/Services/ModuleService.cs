using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.IServices;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.Services
{
    public class ModuleService : IModuleService
    {
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;
        private readonly ILogger _logger;

        public ModuleService(ISyncLocalStorageService localStorageService, HttpClient httpClient, ILogger<ModuleService> logger)
        {
            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _localStorageService = localStorageService;
            _httpClient = httpClient;
            _logger = logger;

            string token = _localStorageService.GetItemAsString("token");

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Create a new module by sending a POST request to the API with the given form in parameter
        /// </summary>
        /// <param name="moduleCreation">The form to create</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is the newly created <see cref="ModuleModel"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<ModuleModel>> CreateModuleAsync(ModuleCreationModel moduleCreation)
        {
            if (moduleCreation is null)
            {
                throw new ArgumentNullException(nameof(moduleCreation));
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Modules", moduleCreation);

            ResultModel<ModuleModel> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<ModuleModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(CreateModuleAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Update a module by sending a PUT request to the API with the module ID and the form
        /// </summary>
        /// <param name="id">The id of the module to update</param>
        /// <param name="moduleCreation">The form to update the module</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="ModuleModel"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<ModuleModel>> UpdateModuleAsync(int id, ModuleCreationModel moduleCreation)
        {
            if (moduleCreation is null)
            {
                throw new ArgumentNullException(nameof(moduleCreation));
            }

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Modules/{id}", moduleCreation);

            ResultModel<ModuleModel> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<ModuleModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(UpdateModuleAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get the module with the given id by sending a GET request to the API
        /// </summary>
        /// <param name="id">The id of the module</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="ModuleModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ResultModel<ModuleModel>> GetModuleAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/{id}");

            ResultModel<ModuleModel> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<ModuleModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(GetModuleAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get the modules by sending a GET request to the API
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="ModuleModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<ModuleModel>>> GetModulesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Modules");

            ResultModel<IEnumerable<ModuleModel>> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<ModuleModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(GetModulesAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Create a Schedule for a module by sending a POST request to the API
        /// </summary>
        /// <param name="moduleId">The id of the module to which the schedule is created</param>
        /// <param name="scheduleCreate">The body form of the schedule to create</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="ScheduleModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<ScheduleModel>> CreateScheduleAsync(int moduleId, ScheduleCreateModel scheduleCreate)
        {
            if (moduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(moduleId));
            }

            if (scheduleCreate is null)
            {
                throw new ArgumentNullException(nameof(scheduleCreate));
            }

            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync($"Modules/{moduleId}/Schedules", scheduleCreate);

            ResultModel<ScheduleModel> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<ScheduleModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(GetModuleAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get the module schedules from the give module id by sending a GET request to the API
        /// </summary>
        /// <param name="moduleId">The id of the module</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="ScheduleModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<ScheduleModel>>> GetModuleSchedulesAsync(int moduleId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/{moduleId}/Schedules");

            ResultModel<IEnumerable<ScheduleModel>> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<ScheduleModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(GetModuleSchedulesAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Delete the schedule of a module
        /// </summary>
        /// <param name="moduleId">The id of the module</param>
        /// <param name="scheduleId">The id of the schedule</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ResultModel<bool>> DeleteScheduleAsync(int moduleId, int scheduleId)
        {
            if (moduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(moduleId));
            }

            if (scheduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scheduleId));
            }

            HttpResponseMessage response =
                await _httpClient.DeleteAsync($"Modules/{moduleId}/Schedules/{scheduleId}");

            ResultModel<bool> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(DeleteScheduleAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get the users requesting to join a modules
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="UserModuleRequestModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<UserModuleRequestModel>>> GetUsersRequestingModules()
        {
            HttpResponseMessage response =
                await _httpClient.GetAsync("Modules/RequestingUsers");

            ResultModel<IEnumerable<UserModuleRequestModel>> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<UserModuleRequestModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(GetUsersRequestingModules),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get the user status for a module by sending a GET request to the API. If the user is accepted or not
        /// </summary>
        /// <param name="moduleId">
        /// The id of the module
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>. true if the user is accepted, false otherwise
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task<ResultModel<bool?>> GetUserModuleStatus(int moduleId)
        {
            if (moduleId <= 0)
            {
                throw new IndexOutOfRangeException(nameof(moduleId));
            }

            HttpResponseMessage response =
                 await _httpClient.GetAsync($"Modules/{moduleId}/UserRequestStatus");

            ResultModel<bool?> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(GetUserModuleStatus),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get the user status for a module by sending a GET request to the API. If the user is accepted or not
        /// </summary>
        /// <param name="moduleId">
        /// The id of the module
        /// </param>
        /// <param name="userId">
        /// The id of the user
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>. true if the user is accepted, false otherwise
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<bool>> GetUserModuleStatus(int moduleId, string userId)
        {
            if (moduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(moduleId));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            HttpResponseMessage response =
                await _httpClient.GetAsync($"Modules/{moduleId}/UserRequestStatus/{userId}");

            ResultModel<bool> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(GetUserModuleStatus),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Request acceptance for a module by sending a POST request to the API
        /// </summary>
        /// <param name="moduleId">
        /// The id of the module to which the user want to be accepted
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>. true if the request was successful, false otherwise
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ResultModel<bool>> RequestAcceptance(int moduleId)
        {
            if (moduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(moduleId));
            }

            HttpResponseMessage response =
                 await _httpClient.PostAsync($"Modules/{moduleId}/Request", null);

            ResultModel<bool> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(RequestAcceptance),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get all the schedules by sending a GET request to the API
        /// </summary>
        /// <param name="selectedDate">
        /// The date to get the schedules from
        /// </param>
        /// <param name="range">
        /// The range to get the schedules from
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="ScheduleModel"/>
        /// </returns>
        public async Task<ResultModel<IEnumerable<ScheduleModel>>> GetSchedulesAsync(DateTime? selectedDate = null, int? range = null)
        {
            string queryUrl = "Modules/Schedules?";

            if (selectedDate.HasValue)
            {
                queryUrl += $"selectedDate={selectedDate.Value:s}&";
            }

            if (range.HasValue)
            {
                queryUrl += $"range={range.Value}&";
            }

            HttpResponseMessage response = await _httpClient.GetAsync(queryUrl);

            ResultModel<IEnumerable<ScheduleModel>> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<ScheduleModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(DeleteScheduleAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get all the schedules for a module by sending a GET request to the API
        /// </summary>
        /// <param name="moduleId">
        /// The id of the module to which the schedules belong
        /// </param>
        /// <param name="selectedDate">
        /// The date to get the schedules from
        /// </param>
        /// <param name="range">
        /// The date range to get the schedules from
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="ScheduleModel"/>
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task<ResultModel<IEnumerable<ScheduleModel>>> GetSchedulesAsync(int moduleId,
            DateTime? selectedDate = null, int? range = null)
        {
            if (moduleId <= 0)
            {
                throw new IndexOutOfRangeException(nameof(moduleId));
            }

            string queryUrl = $"Modules/{moduleId}/Schedules?";

            if (selectedDate.HasValue)
            {
                queryUrl += $"selectedDate={selectedDate.Value:s}&";
            }

            if (range.HasValue)
            {
                queryUrl += $"range={range.Value}&";
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/{moduleId}/Schedules");

            ResultModel<IEnumerable<ScheduleModel>> result = new()
            {
                Success = response.IsSuccessStatusCode,
                Code = response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<ScheduleModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(DeleteScheduleAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// The the schedule details by sending a GET request to the API
        /// </summary>
        /// <param name="id">
        /// The id of the schedule
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="ScheduleModel"/>
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task<ResultModel<ScheduleModel>> GetScheduleAsync(int id)
        {
            if (id <= 0)
            {
                throw new IndexOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/Schedules/{id}");

            ResultModel<ScheduleModel> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<ScheduleModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(DeleteScheduleAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get the files of a schedule by sending a GET request to the API
        /// </summary>
        /// <param name="mId">
        /// The id of the module to which belongs the schedule
        /// </param>
        /// <param name="sId">
        /// The id of the schedule to which belongs the files
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="ScheduleFileModel"/>
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task<ResultModel<IEnumerable<ScheduleFileModel>>> GetScheduleFilesAsync(int mId, int sId)
        {
            if (mId <= 0)
            {
                throw new IndexOutOfRangeException(nameof(mId));
            }

            if (sId <= 0)
            {
                throw new IndexOutOfRangeException(nameof(sId));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/{mId}/Schedules/{sId}/Files");

            ResultModel<IEnumerable<ScheduleFileModel>> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<IEnumerable<ScheduleFileModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(DeleteScheduleAsync),
                    ex.Message
                );
            }

            return result;
        }

        public Task<ScheduleFileModel?> GetScheduleFileAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserHasRoleAsync(string role, int moduleId)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (moduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(moduleId));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/{moduleId}/HasRole/{role}");

            response.EnsureSuccessStatusCode();

            bool? result = await response.Content.ReadFromJsonAsync<bool>();

            return result is null ? false : result.Value;
        }

        /// <summary>
        /// Delete a schedule's file by sending a DELETE request to the API
        /// </summary>
        /// <param name="moduleId">
        /// The id of the module to which belong the schedule
        /// </param>
        /// <param name="scheduleId">
        /// The id of the schedule to which belong the file
        /// </param>
        /// <param name="scheduleFileId">
        /// The id of the file to delete
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ResultModel<bool>> DeleteScheduleFileAsync(int moduleId, int scheduleId, int scheduleFileId)
        {
            if (moduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(moduleId));
            }

            if (scheduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scheduleId));
            }

            if (scheduleFileId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scheduleFileId));
            }

            HttpResponseMessage response =
                            await _httpClient.DeleteAsync($"Modules/{moduleId}/Schedules/{scheduleId}/Files/{scheduleFileId}");

            ResultModel<bool> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(DeleteScheduleFileAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Get the teacher of a module by sending a GET request to the API
        /// </summary>
        /// <param name="moduleId">
        /// The id of the module
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="UserModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ResultModel<UserModel>> GetTeacherAsync(int moduleId)
        {
            if (moduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(moduleId));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/{moduleId}/Teacher");

            ResultModel<UserModel> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<UserModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(GetTeacherAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Update a schedule by sending a PUT request to the API
        /// </summary>
        /// <param name="moduleId">
        /// The id of the module to which belong the schedule
        /// </param>
        /// <param name="scheduleId">
        /// The id of the schedule to update
        /// </param>
        /// <param name="scheduleCreate">
        /// The new values of the schedule
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="ScheduleModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<ScheduleModel>> UpdateScheduleAsync(int moduleId, int scheduleId, ScheduleCreateModel scheduleCreate)
        {
            if (scheduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scheduleId));
            }

            if (scheduleCreate is null)
            {
                throw new ArgumentNullException(nameof(scheduleCreate));
            }

            HttpResponseMessage response =
                await _httpClient.PutAsJsonAsync($"Modules/{moduleId}/Schedules/{scheduleId}", scheduleCreate);

            ResultModel<ScheduleModel> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };

            try
            {
                result.Data = await response.Content.ReadFromJsonAsync<ScheduleModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{fn}: an exception was thrown while converting result to json.\n{exMsg}",
                    nameof(UpdateScheduleAsync),
                    ex.Message
                );
            }

            return result;
        }

        /// <summary>
        /// Update a schedule's file by sending a POST request to the API
        /// </summary>
        /// <param name="mId">
        /// The id of the module to which belong the schedule
        /// </param>
        /// <param name="sId">
        /// The id of the schedule to which belong the file
        /// </param>
        /// <param name="files">
        /// The list of files to upload
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>. The API response is NoContent. 
        /// So the data is true if the request is successful, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ResultModel<bool>> UploadFilesAsync(int mId, int sId, IEnumerable<IBrowserFile> files)
        {
            if (mId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(mId));
            }

            if (sId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sId));
            }

            string query = $"Modules/{mId}/Schedules/{sId}/Files";

            try
            {
                using MultipartFormDataContent content = new();

                foreach (IBrowserFile file in files)
                {
                    StreamContent streamContent = new(file.OpenReadStream(file.Size), (int)file.Size);

                    streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "files",
                        FileName = file.Name
                    };

                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                    content.Add(streamContent, file.Name, file.Name);
                }

                HttpResponseMessage response = await _httpClient.PostAsync(query, content);

                ResultModel<bool> result = new()
                {
                    Code = response.StatusCode,
                    Success = response.IsSuccessStatusCode,
                    Message = await response.Content.ReadAsStringAsync(),
                    Data = response.IsSuccessStatusCode
                };

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(
                    "{fn}: An exception was thrown when trying to upload a shcedule file.\n{e.msg}\n{e.str}",
                    nameof(UploadFilesAsync),
                    e.Message,
                    e.StackTrace
                );

                throw;
            }
        }

        /// <summary>
        /// Set the status of the user in the module
        /// </summary>
        /// <param name="moduleId">
        /// The id of the module to which belong the user
        /// </param>
        /// <param name="userId">
        /// The id of the user for which to set the status
        /// </param>
        /// <param name="status">
        /// The value of the status
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>. The API response is NoContent, so the data is 
        /// true if the request is successful, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ResultModel<bool>> SetUserModuleStatusAsync(int moduleId, string userId, bool status)
        {
            if (moduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(moduleId));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string queryUri = $"Modules/{moduleId}/Students/{userId}/Status";

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(queryUri, status);

            ResultModel<bool> result = new()
            {
                Code = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                Data = response.IsSuccessStatusCode
            };

            return result;
        }
    }
}

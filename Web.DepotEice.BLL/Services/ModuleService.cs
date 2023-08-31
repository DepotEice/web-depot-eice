﻿using Blazored.LocalStorage;
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

        public async Task<IEnumerable<ModuleModel>> GetModulesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Modules");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ModuleModel>>();

            if (result is null)
            {
                return Enumerable.Empty<ModuleModel>();
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

        public async Task<IEnumerable<UserModuleRequestModel>> GetUsersRequestingModules()
        {
            HttpResponseMessage response =
                await _httpClient.GetAsync("Modules/RequestingUsers");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserModuleRequestModel>>();

            if (result is null)
            {
                return Enumerable.Empty<UserModuleRequestModel>();
            }

            return result;
        }

        public async Task<bool?> UserIsAccepted(int moduleId)
        {
            if (moduleId <= 0)
            {
                throw new IndexOutOfRangeException(nameof(moduleId));
            }

            HttpResponseMessage response =
                 await _httpClient.GetAsync($"Modules/{moduleId}/UserRequestStatus");

            bool? result = await response.Content.ReadFromJsonAsync<bool>();

            if (result is null)
            {
                return false;
            }

            return result.Value;
        }

        public async Task<bool?> UserIsAccepted(int moduleId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            HttpResponseMessage response =
                await _httpClient.GetAsync($"Modules/{moduleId}/UserRequestStatus/{userId}");

            bool? result = await response.Content.ReadFromJsonAsync<bool>();

            if (result is null)
            {
                return false;
            }

            return result.Value;
        }

        public async Task<bool> RequestAcceptance(int moduleId)
        {
            HttpResponseMessage response =
                 await _httpClient.PostAsync($"Modules/{moduleId}/RequestAcceptance", null);

            return response.IsSuccessStatusCode;
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

        public async Task<IEnumerable<ScheduleModel>> GetSchedulesAsync(int moduleId)
        {
            if (moduleId <= 0)
            {
                throw new IndexOutOfRangeException(nameof(moduleId));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/{moduleId}/Schedules");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ScheduleModel>>();

            if (result is null)
            {
                return Enumerable.Empty<ScheduleModel>();
            }

            return result;
        }

        public async Task<ScheduleModel?> GetScheduleAsync(int id)
        {
            if (id <= 0)
            {
                throw new IndexOutOfRangeException(nameof(id));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/Schedules/{id}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ScheduleModel>();

            return result;
        }

        public async Task<IEnumerable<ScheduleFileModel>> GetScheduleFilesAsync(int scheduleId)
        {
            if (scheduleId <= 0)
            {
                throw new IndexOutOfRangeException(nameof(scheduleId));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"Modules/{scheduleId}/Files");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ScheduleFileModel>>();

            if (result is null)
            {
                return Enumerable.Empty<ScheduleFileModel>();
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

        public async Task<bool> DeleteScheduleFileAsync(int moduleId, int scheduleId, int scheduleFileId)
        {
            if (scheduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scheduleId));
            }

            HttpResponseMessage response =
                            await _httpClient.DeleteAsync($"Modules/{moduleId}/Schedules/{scheduleId}/Files/{scheduleFileId}");

            return response.IsSuccessStatusCode;
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
    }
}

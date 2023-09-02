﻿using Microsoft.AspNetCore.Components.Forms;
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

        /// <summary>
        /// Create a new module by sending a POST request to the API with the given form in parameter
        /// </summary>
        /// <param name="moduleCreation">The form to create</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is the newly created <see cref="ModuleModel"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<ModuleModel>> CreateModuleAsync(ModuleCreationModel moduleCreation);

        /// <summary>
        /// Update a module by sending a PUT request to the API with the module ID and the form
        /// </summary>
        /// <param name="id">The id of the module to update</param>
        /// <param name="moduleCreation">The form to update the module</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="ModuleModel"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<ModuleModel>> UpdateModuleAsync(int id, ModuleCreationModel moduleCreation);

        /// <summary>
        /// Get the module with the given id by sending a GET request to the API
        /// </summary>
        /// <param name="id">The id of the module</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="ModuleModel"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task<ResultModel<ModuleModel>> GetModuleAsync(int id);

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
        Task<ResultModel<ScheduleModel>> CreateScheduleAsync(int moduleId, ScheduleCreateModel scheduleCreate);

        /// <summary>
        /// Get the module schedules from the give module id by sending a GET request to the API
        /// </summary>
        /// <param name="moduleId">The id of the module</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="ScheduleModel"/>
        /// </returns>
        Task<ResultModel<IEnumerable<ScheduleModel>>> GetModuleSchedulesAsync(int moduleId);

        /// <summary>
        /// Delete the schedule of a module
        /// </summary>
        /// <param name="moduleId">The id of the module</param>
        /// <param name="scheduleId">The id of the schedule</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task<ResultModel<bool>> DeleteScheduleAsync(int moduleId, int scheduleId);

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
        Task<ResultModel<bool>> DeleteScheduleFileAsync(int moduleId, int scheduleId, int scheduleFileId);
        Task<IEnumerable<UserModuleRequestModel>> GetUsersRequestingModules();

        /// <summary>
        /// Verify the user status for a module. If the user is accepted or not
        /// </summary>
        /// <param name="moduleId">
        /// The id of the module
        /// </param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>. true if the user is accepted, false otherwise
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        Task<ResultModel<bool?>> ModuleUserStatus(int moduleId);
        Task<bool?> UserIsAccepted(int moduleId, string userId);

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
        Task<ResultModel<bool>> RequestAcceptance(int moduleId);

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
        Task<ResultModel<IEnumerable<ScheduleModel>>> GetSchedulesAsync(DateTime? selectedDate = null, int? dateRange = null);
        Task<IEnumerable<ScheduleModel>> GetSchedulesAsync(int moduleId);

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
        Task<ResultModel<ScheduleModel>> GetScheduleAsync(int id);

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
        Task<ResultModel<IEnumerable<ScheduleFileModel>>> GetScheduleFilesAsync(int mId, int sId);
        Task<ScheduleFileModel?> GetScheduleFileAsync(int id);
        Task<bool> UserHasRoleAsync(string role, int moduleId);

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
        Task<ResultModel<UserModel>> GetTeacherAsync(int moduleId);

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
        Task<ResultModel<ScheduleModel>> UpdateScheduleAsync(int moduleId, int scheduleId, ScheduleCreateModel scheduleCreate);

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
        Task<ResultModel<bool>> UploadFilesAsync(int mId, int sId, IEnumerable<IBrowserFile> files);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    /// <summary>
    /// The address service interface
    /// </summary>
    public interface IAddressService
    {
        /// <summary>
        /// Get the current user's addresses by sending a GET request to the API
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="AddressModel"/>.
        /// The list of addresses. Null if the request failed
        /// </returns>
        Task<ResultModel<IEnumerable<AddressModel>>> GetAddressesAsync();

        /// <summary>
        /// Get a user's addresses by sending a GET request to the API. Requires admin privileges
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="AddressModel"/>.
        /// The list of addresses. Null if the request failed
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<IEnumerable<AddressModel>>> GetAddressesAsync(string userId);

        /// <summary>
        /// Get an address by sending a GET request to the API with the address ID
        /// </summary>
        /// <param name="id">The id of the address to retrieve</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AddressModel"/> which is the address data. Null if the
        /// data was not found or if the request failed
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        Task<ResultModel<AddressModel>> GetAddressAsync(int id);

        /// <summary>
        /// Create a new address by sending a POST request to the API with the address data
        /// </summary>
        /// <param name="addressCreate">The new address data</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AddressModel"/> which is the created address data. Null if
        /// nothing was created or if the request failed
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<AddressModel>> CreateAddressAsync(AddressCreateModel addressCreate);

        /// <summary>
        /// Update an address by sending a PUT request to the API with the address data
        /// </summary>
        /// <param name="addressUpdate">The data of the address</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AddressModel"/> which is the updated address data. Null if
        /// data was not updated or if the request failed
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<AddressModel>> UpdateAddressAsync(AddressUpdateModel addressUpdate);

        /// <summary>
        /// Delete the address by sending a DELETE request to the API with the address ID
        /// </summary>
        /// <param name="id">The id of the address to delete</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="bool"/>. True if the address was deleted, false otherwise
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        Task<ResultModel<bool>> DeleteAddressAsync(int id);

        /// <summary>
        /// Set an address as primary by sending a POST request to the API with the address ID
        /// </summary>
        /// <param name="id">The id of the address to set as primary</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="AddressModel"/> which is the address data. Null if the
        /// data is not found or if the request failed
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        Task<ResultModel<AddressModel>> SetPrimaryAddressAsync(int id);
    }
}

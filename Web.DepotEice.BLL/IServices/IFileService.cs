using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IFileService
    {
        /// <summary>
        /// Upload a file by sending a POST request to the server
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="contentType">The content type of the file</param>
        /// <param name="stream">The stream of the file</param>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="int?"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ResultModel<IEnumerable<int>?>> UploadFileAsync(string fileName, string contentType, Stream stream);
    }
}

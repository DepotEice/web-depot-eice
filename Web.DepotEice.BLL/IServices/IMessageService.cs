using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IMessageService
    {

        /// <summary>
        /// Get the current user's conversations
        /// </summary>
        /// <returns>
        /// <see cref="ResultModel{T}"/> where T is <see cref="IEnumerable{T}"/> where T is <see cref="ConversationModel"/>
        /// </returns>
        Task<ResultModel<IEnumerable<ConversationModel>>> GetConversationsAsync();
    }
}

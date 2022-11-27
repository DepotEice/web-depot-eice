using AutoMapper;
using Blazored.LocalStorage;

namespace Web.DepotEice.UIL.Managers
{
    public class RoleManager
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ILocalStorageService _localStorageService;
        private readonly ISyncLocalStorageService _syncLocalStorageService;

    }
}

using AutoMapper;
using Blazored.LocalStorage;
using Web.DepotEice.BLL.IServices;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.Helpers;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.Managers
{
    public class UserManager
    {
        public const string GUEST_ROLE = "Guest";
        public const string STUDENT_ROLE = "Student";
        public const string TEACHER_ROLE = "Teacher";
        public const string DIRECTION_ROLE = "Direction";

        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ILocalStorageService _localStorageService;
        private readonly ISyncLocalStorageService _syncLocalStorageService;
        private readonly IAuthService _authService;
        private readonly IRoleService _roleService;
        private readonly IModuleService _moduleService;

        public bool IsConnected
        {
            get
            {
                string? result = _syncLocalStorageService.GetItemAsString("token");

                return !string.IsNullOrEmpty(result);
            }
        }

        public UserManager(ILogger<UserManager> logger, IMapper mapper, ILocalStorageService localStorageService,
            ISyncLocalStorageService syncLocalStorageService, IAuthService authService, IRoleService roleService,
            IModuleService moduleService)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            if (syncLocalStorageService is null)
            {
                throw new ArgumentNullException(nameof(syncLocalStorageService));
            }

            if (authService is null)
            {
                throw new ArgumentNullException(nameof(authService));
            }

            if (roleService is null)
            {
                throw new ArgumentNullException(nameof(roleService));
            }

            if (moduleService is null)
            {
                throw new ArgumentNullException(nameof(moduleService));
            }

            _logger = logger;
            _mapper = mapper;
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            _authService = authService;
            _roleService = roleService;
            _moduleService = moduleService;
        }

        public async Task<bool> SignInAsync(SignInForm signInForm)
        {
            if (signInForm is null)
            {
                throw new ArgumentNullException(nameof(signInForm));
            }

            try
            {
                SignInModel signInModel = _mapper.Map<SignInModel>(signInForm);

                string result = await _authService.SignInAsync(signInModel);

                if (string.IsNullOrEmpty(result))
                {
                    return false;
                }

                await _localStorageService.SetItemAsStringAsync("token", result);

                return !string.IsNullOrEmpty(await _localStorageService.GetItemAsStringAsync("token"));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> SignOutAsync()
        {
            try
            {
                await _localStorageService.RemoveItemAsync("token");

                return string.IsNullOrEmpty(await _localStorageService.GetItemAsStringAsync("token"));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> SignUpAsync(RegisterForm registerForm)
        {
            if (registerForm is null)
            {
                throw new ArgumentNullException(nameof(registerForm));
            }

            try
            {
                SignUpModel signUpModel = _mapper.Map<SignUpModel>(registerForm);

                bool result = await _authService.SignUpAsync(signUpModel);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> RequestNewPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await _authService.RequestNewPassword(email);
        }

        public async Task<bool> Activate(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            return await _authService.Activate(userId, token);
        }

        public async Task<bool> IsInRoleAsync(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException(nameof(role));
            }

            bool result = await _roleService.UserHasRoleAsync(role);

            return result;
        }

        public async Task<bool> HasRole(string role, int moduleId)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (moduleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(moduleId));
            }

            var result = await _moduleService.UserHasRoleAsync(role, moduleId);

            return result;
        }
    }
}

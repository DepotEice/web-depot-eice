using AutoMapper;
using Blazored.LocalStorage;
using Web.DepotEice.BLL.IServices;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.Helpers;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.Managers
{
    /// <summary>
    /// User manager handling user authentication, logout, if he is logged in, etc.
    /// </summary>
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
        private readonly IUserService _userService;

        /// <summary>
        /// Specify if the user is connected or not
        /// </summary>
        public bool IsConnected
        {
            get
            {
                string? result = _syncLocalStorageService.GetItemAsString("token");

                return !string.IsNullOrEmpty(result);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="localStorageService"></param>
        /// <param name="syncLocalStorageService"></param>
        /// <param name="authService"></param>
        /// <param name="roleService"></param>
        /// <param name="moduleService"></param>
        /// <param name="userService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserManager(ILogger<UserManager> logger, IMapper mapper, ILocalStorageService localStorageService,
            ISyncLocalStorageService syncLocalStorageService, IAuthService authService, IRoleService roleService,
            IModuleService moduleService, IUserService userService)
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

            if (userService is null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            _logger = logger;
            _mapper = mapper;
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            _authService = authService;
            _roleService = roleService;
            _moduleService = moduleService;
            _userService = userService;
        }

        /// <summary>
        /// Sign in the user and save the JWT token in the local storage
        /// </summary>
        /// <param name="signInForm">The login form</param>
        /// <returns>
        /// true If the sign in is successful, false Otherwise
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> SignInAsync(SignInForm signInForm)
        {
            if (signInForm is null)
            {
                throw new ArgumentNullException(nameof(signInForm));
            }

            try
            {
                SignInModel signInModel = _mapper.Map<SignInModel>(signInForm);

                ResultModel<SignInResponseModel> result = await _authService.SignInAsync(signInModel);

                if (!result.Success)
                {
                    _logger.LogError(
                        "Signing in failed with status code {code} and message {msg}",
                        result.Code,
                        result.Message
                    );

                    return false;
                }

                string? token = result.Data?.Token;

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError(
                        "Signing in succeeded but the data is null and message is {msg}",
                        result.Message
                    );

                    return false;
                }

                await _localStorageService.SetItemAsStringAsync("token", token);

                return !string.IsNullOrEmpty(await _localStorageService.GetItemAsStringAsync("token"));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    "An exception was thrown during {fn}\n{eMsg}\n{eStr}",
                    nameof(SignInAsync),
                    e.Message,
                    e.StackTrace
                );

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

        /// <summary>
        /// Verify if the current user is in the role
        /// </summary>
        /// <param name="role">
        /// The name of the role
        /// </param>
        /// <returns>
        /// <c>true</c> If the user has the role, <c>false</c> Otherwise
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> IsInRoleAsync(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException(nameof(role));
            }

            var resultModel = await _roleService.GetRolesAsync();

            if (!resultModel.Success)
            {
                _logger.LogError($"Getting current user's role failed");

                return false;
            }

            if (resultModel.Data is null)
            {
                _logger.LogError($"Current user's roles data is null");

                return false;
            }

            bool result = resultModel.Data.Any(r => r.Name.Equals(role));

            return result;
        }

        /// <summary>
        /// Verify if the current user is in one of the roles
        /// </summary>
        /// <param name="roles">The array of roles to check</param>
        /// <returns>
        /// true If the user has one of the roles, false Otherwise
        /// </returns>
        public async Task<bool> IsInRoleAsync(string[] roles)
        {
            var resultModel = await _roleService.GetRolesAsync();

            if (!resultModel.Success)
            {
                _logger.LogError($"Getting current user's role failed");

                return false;
            }

            if (resultModel.Data is null)
            {
                _logger.LogError($"Current user's roles data is null");

                return false;
            }

            bool result = resultModel.Data.Any(r => roles.Contains(r.Name));

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

        /// <summary>
        /// Get the id of the current user
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetCurrentUserId()
        {
            ResultModel<UserModel> result = await _userService.GetUserAsync();

            if (!result.Success)
            {
                _logger.LogError(
                    "Getting current user failed with status code {code} and message {msg}",
                    result.Code,
                    result.Message
                );

                return null;
            }

            if (result.Data is null)
            {
                _logger.LogError(
                    "Getting current user succeeded but the data is null and message is {msg}",
                    result.Message
                );

                return null;
            }

            return result.Data.Id;
        }
    }
}

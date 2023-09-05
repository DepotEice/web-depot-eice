using System.Data;
using BlazorBootstrap;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Web.DepotEice.BLL.IServices;
using Web.DepotEice.BLL.Services;
using Web.DepotEice.UIL.Managers;

namespace Company.WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<Web.DepotEice.UIL.App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddLogging();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddBlazorBootstrap();

#if DEBUG
            builder.Services.AddScoped(
                sp => new HttpClient { BaseAddress = new Uri("https://localhost:7205/api/") }
            );
#else
            builder.Services.AddScoped(
                sp =>
                    new HttpClient
                    {
                        BaseAddress = new Uri("https://api-depot-eice.azurewebsites.net/api/")
                    }
            );
#endif
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserTokenService, UserTokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOpeningHoursService, OpeningHoursService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IModuleService, ModuleService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IFileService, FileService>();

            builder.Services.AddScoped<UserManager>();
            builder.Services.AddScoped<AppointmentManager>();
            builder.Services.AddScoped<ScheduleManager>();

            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();

            var host = builder.Build();

            var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogInformation(
                "{date} | Logged after the app is built in Program.cs.",
                DateTime.Now
            );

            await host.RunAsync();
        }
    }
}

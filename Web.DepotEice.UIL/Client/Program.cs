using System.Data;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<UserManager>();

            var host = builder.Build();

            var logger = host.Services
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogInformation("{date} | Logged after the app is built in Program.cs.", DateTime.Now);

            await host.RunAsync();
        }
    }
}
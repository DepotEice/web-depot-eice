using System.Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Company.WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<Web.DepotEice.UIL.App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var host = builder.Build();

            var logger = host.Services
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogInformation("{date} | Logged after the app is built in Program.cs.");

            await host.RunAsync();
        }
    }
}
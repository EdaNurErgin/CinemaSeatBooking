using Blazored.LocalStorage;
using CinemaSeatBooking.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CinemaSeatBooking
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<MovieService>();
            builder.Services.AddScoped<SeatService>();
            builder.Services.AddBlazoredLocalStorage();
            await builder.Build().RunAsync();

        }
    }
}

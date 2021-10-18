using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using EngineerNotebook.BlazorAdmin.Services;
using EngineerNotebook.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EngineerNotebook.BlazorAdmin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#admin");

            var baseUrlConfig = new BaseUrlConfiguration();
            builder.Configuration.Bind(BaseUrlConfiguration.CONFIG_NAME, baseUrlConfig);
            builder.Services.AddScoped<BaseUrlConfiguration>(sp => baseUrlConfig);
            builder.Services.AddScoped(sp => new HttpClient()
                { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<HttpService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddScoped(sp =>
                (CustomAuthStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
            builder.Services.AddBlazorServices();
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

            await ClearLocalStorageCache(builder.Services);

            await builder.Build().RunAsync();
        }

        static async Task ClearLocalStorageCache(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var localStorageCache = sp.GetRequiredService<ILocalStorageService>();
            await localStorageCache.RemoveItemAsync(StorageConstants.TAGS);
            await localStorageCache.RemoveItemAsync(StorageConstants.DOCS);
        }
    }
}
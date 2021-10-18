using EngineerNotebook.BlazorAdmin.Services;
using EngineerNotebook.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EngineerNotebook.BlazorAdmin
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IDocService, DocService>();
            return services;
        }
    }
}
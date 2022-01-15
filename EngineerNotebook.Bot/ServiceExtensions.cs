using Discord.WebSocket;

namespace EngineerNotebook.Bot;

public static class ServiceExtensions
{
    public static IServiceCollection AddEngineeringBot(this IServiceCollection services)
    {
        services.AddSingleton<DiscordSocketClient>();
        services.AddHostedService<Bot>();
        
        return services;
    }
}
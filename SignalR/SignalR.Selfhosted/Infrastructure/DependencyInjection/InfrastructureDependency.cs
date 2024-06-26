using Microsoft.Extensions.DependencyInjection;
using SignalR.Api.Infrastructure.Services;

namespace SignalR.Api.Infrastructure.DependencyInjection;

public static class InfrastructureDependency
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddScoped<IAzureSignalRService, AzureSignalRService>();

        return services;
    }
}

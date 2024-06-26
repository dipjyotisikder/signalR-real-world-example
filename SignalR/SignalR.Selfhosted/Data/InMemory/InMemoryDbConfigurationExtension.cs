using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SignalR.Api.Data.InMemory
{
    /// <summary>
    /// Represents the class for in memory database configuration.
    /// </summary>
    public static class InMemoryDbConfigurationExtension
    {
        /// <summary>
        /// Injects In Memory database configurations into DI container.
        /// </summary>
        /// <param name="services">DI service collection.</param>
        /// <param name="configuration">Application Configuration.</param>
        /// <returns>DI service collection.</returns>
        public static IServiceCollection AddInMemoryDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InMemoryDataContext>(x => x.UseInMemoryDatabase(configuration.GetConnectionString("InMemoryDb")));
            services.AddScoped<IDataContext, InMemoryDataContext>();

            return services;
        }
    }
}

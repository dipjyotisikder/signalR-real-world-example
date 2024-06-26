using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SignalR.Api.Data.SqLite
{
    /// <summary>
    /// Presents configuration class for SqLite database.
    /// </summary>
    public static class SqLiteDbConfigurationExtension
    {
        /// <summary>
        /// Injects SqLite database configurations into DI container.
        /// </summary>
        /// <param name="services">DI service collection.</param>
        /// <param name="configuration">Application Configuration.</param>
        /// <returns>DI service collection.</returns>
        public static IServiceCollection AddSqLiteDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqLiteDataContext>(x => x.UseSqlite(configuration.GetConnectionString("SqLiteDb")));
            services.AddScoped<IDataContext, SqLiteDataContext>();

            return services;
        }
    }
}

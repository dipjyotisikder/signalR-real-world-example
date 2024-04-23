using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SignalR.SelfHosted.Data.SqLite
{
    public static class SqLiteDbConfigurationExtension
    {
        public static IServiceCollection AddSqLiteDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqLiteDataContext>(x => x.UseSqlite(configuration.GetConnectionString("SqLiteDb")));
            services.AddScoped<IDataContext, SqLiteDataContext>();

            return services;
        }
    }
}

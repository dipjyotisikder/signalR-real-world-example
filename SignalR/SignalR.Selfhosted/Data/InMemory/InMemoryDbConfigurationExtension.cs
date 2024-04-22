using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SignalR.SelfHosted.Data.InMemory
{
    public static class InMemoryDbConfigurationExtension
    {
        public static IServiceCollection AddInMemoryDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InMemoryDataContext>(x => x.UseInMemoryDatabase(configuration.GetConnectionString("InMemoryDb")));
            services.AddScoped<IDataContext, InMemoryDataContext>();

            return services;
        }
    }
}

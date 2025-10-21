using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Valoriza.Infrastructure.Extensions;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValorizaDb(this IServiceCollection services, IConfiguration config)
    {
        var provider = config.GetValue<string>("Database:Provider") ?? "sqlite";
        if (provider.Equals("postgres", StringComparison.OrdinalIgnoreCase))
        {
            services.AddDbContext<ValorizaDbContext>(opt =>
            opt.UseNpgsql(config.GetConnectionString("Postgres")));
        }
        else
        {
            services.AddDbContext<ValorizaDbContext>(opt =>
            opt.UseSqlite(config.GetConnectionString("Sqlite")));
        }
        return services;
    }
}
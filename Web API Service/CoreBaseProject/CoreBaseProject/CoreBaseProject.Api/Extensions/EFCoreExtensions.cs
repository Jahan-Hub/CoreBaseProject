using CoreBaseProject.Database.DatabaseContexts;
using CoreBaseProject.Repository.Base;
using CoreBaseProject.Repository.Contracts;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace CoreBaseProject.Api.Extensions
{
    public static class EfCoreExtensions
    {
        public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Add database connection string
            services.AddDbContext<DatabaseContext>(option => option.UseSqlServer(configuration.GetConnectionString("Default")));

             services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add hang fire on the database
            services.AddHangfire((serviceProvider, globalConfiguration) =>
            {
                var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("Default");
                globalConfiguration.UseSqlServerStorage(connectionString);
            });
            services.AddHangfireServer();

            return services;
        }
    }
}
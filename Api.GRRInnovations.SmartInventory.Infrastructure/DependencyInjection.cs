using Api.GRRInnovations.SmartInventory.Infrastructure.Helpers;
using Api.GRRInnovations.SmartInventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.SmartInventory.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = ConnectionHelper.GetConnectionString(configuration);
            services.AddDbContextPool<ApplicationDbContext>(options => ConfigureDatabase(options, connection));

            return services;
        }

        private static void ConfigureDatabase(DbContextOptionsBuilder options, string connection)
        {
            options.UseSqlServer(connection, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );

                sqlOptions.CommandTimeout(60);
            });

#if DEBUG
            options.LogTo(Console.WriteLine, LogLevel.Information)
                   .EnableSensitiveDataLogging(); // CUIDADO: isso mostra dados sensíveis no log
#endif
        }
    }
}

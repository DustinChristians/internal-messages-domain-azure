using System;
using Internal.Messages.Repository.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Internal.Messages.Configuration
{
    public static class DatabaseConfig
    {
        public static void SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<InternalMessagesContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<DependencyConfig>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }

        public static void AddDatabase(string connectionString, IServiceCollection services, IWebHostEnvironment env)
        {
            if (connectionString.IsSqlLiteConnectionString())
            {
                services.AddDbContext<InternalMessagesContext>(options =>
                    options
                    .UseSqlite(
                        connectionString,
                        sqlliteOptions => sqlliteOptions.CommandTimeout(30))
                    .EnableSensitiveDataLogging());
            }
            else
            {
                services.AddDbContext<InternalMessagesContext>(options =>
                    options
                    .UseSqlServer(
                        connectionString,
                        sqlServerOptions => sqlServerOptions.CommandTimeout(30))
                    .EnableSensitiveDataLogging());
            }
        }

        private static bool IsSqlLiteConnectionString(this string connectionString)
        {
            var connBuilder = new SqlConnectionStringBuilder(connectionString);

            return connBuilder.DataSource.EndsWith(".db");
        }
    }
}

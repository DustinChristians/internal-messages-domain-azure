using System.Reflection;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Internal.Messages.Core.Abstractions.Repositories;
using Internal.Messages.Core.Abstractions.Services;
using Internal.Messages.Core.Utilities;
using Internal.Messages.Infrastructure.Services;
using Internal.Messages.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Internal.Messages.Mapping
{
    public class DependencyConfig
    {
        public static void Register(IServiceCollection services, IConfiguration configuration, string projectAssemblyName)
        {
            DatabaseConfig.AddDatabases(services, configuration);
            AddDependenciesAutomatically(services);
            ConfigureAutomapper(services, projectAssemblyName);
            LoggerConfig.AddDependencies(services);
        }

        // Add any Assembly Names that need to be scanned for AutoMapper Mapping Profiles here
        private static void ConfigureAutomapper(IServiceCollection services, string projectAssemblyName)
        {
            var mappingConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddMaps(projectAssemblyName);
                    cfg.AddMaps("Internal.Messages.Infrastructure");
                    cfg.AddMaps("Internal.Messages.Repository");
                    cfg.AddExpressionMapping();
                    cfg.ConstructServicesUsing(
                        type => ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), type));
                });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // The choice of seeding types used to get the exact assemblies is arbitrary as long as they
        // reside in the correct assemblies
        private static void AddDependenciesAutomatically(IServiceCollection services)
        {
            DependencyUtility.RegisterInterfaces("Service", services, Assembly.GetAssembly(typeof(IMessagesService)), Assembly.GetAssembly(typeof(MessagesService)));
            DependencyUtility.RegisterInterfaces("Repository", services, Assembly.GetAssembly(typeof(IMessagesRepository)), Assembly.GetAssembly(typeof(MessagesRepository)));
        }
    }
}

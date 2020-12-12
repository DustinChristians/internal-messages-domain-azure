using System.Reflection;
using Internal.Messages.Core.Abstractions.Tasks.Logging;
using Internal.Messages.Core.Types;
using Internal.Messages.Core.Utilities;
using Internal.Messages.Scheduler.Tasks.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Internal.Messages.Scheduler
{
    public class DependencyConfig
    {
        public static void Register(IServiceCollection services)
        {
            AddDependenciesAutomatically(services);
        }

        // Register all tasks
        private static void AddDependenciesAutomatically(IServiceCollection services)
        {
            DependencyUtility.RegisterInterfaces("Task", services, Assembly.GetAssembly(typeof(IDatabaseEventLogCleanupTask)), Assembly.GetAssembly(typeof(DatabaseEventLogCleanupTask)), DependencyTypes.Transient);
        }
    }
}

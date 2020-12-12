using Hangfire;
using Internal.Messages.Core.Abstractions.Tasks.Logging;
using Internal.Messages.Scheduler.Abstractions;
using Internal.Messages.Scheduler.Constants;
using Internal.Messages.Scheduler.Tasks.Logging;
using Microsoft.Extensions.Configuration;

namespace Internal.Messages.Scheduler
{
    public class TaskScheduler : ITaskScheduler
    {
        private readonly IConfiguration configuration;
        private readonly IDatabaseEventLogCleanupTask databaseEventLogCleanupTask;

        public TaskScheduler(IConfiguration configuration, IDatabaseEventLogCleanupTask databaseEventLogCleanupTask)
        {
            this.configuration = configuration;
            this.databaseEventLogCleanupTask = databaseEventLogCleanupTask;
        }

        public void ScheduleRecurringTasks()
        {
            RecurringJob.RemoveIfExists(nameof(DatabaseEventLogCleanupTask.DeleteOldEventLogs));
            RecurringJob.AddOrUpdate<IDatabaseEventLogCleanupTask>(
                nameof(DatabaseEventLogCleanupTask),
                task => task.DeleteOldEventLogs(),
                configuration.GetSection(ConfigurationKeys.DatabaseEventLogCleanupTaskCronExpression).Value);

            // Schedule more tasks here
        }
    }
}

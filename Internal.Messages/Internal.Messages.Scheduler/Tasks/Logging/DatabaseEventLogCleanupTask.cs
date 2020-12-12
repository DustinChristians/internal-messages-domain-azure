using System;
using Internal.Messages.Core.Abstractions.Repositories.Logging;
using Internal.Messages.Core.Abstractions.Tasks.Logging;
using Internal.Messages.Scheduler.Constants;
using Microsoft.Extensions.Configuration;

namespace Internal.Messages.Scheduler.Tasks.Logging
{
    public class DatabaseEventLogCleanupTask : IDatabaseEventLogCleanupTask
    {
        private readonly IConfiguration configuration;
        private readonly IEventLogRepository eventLogRepository;

        public DatabaseEventLogCleanupTask(IConfiguration configuration, IEventLogRepository eventLogRepository)
        {
            this.configuration = configuration;
            this.eventLogRepository = eventLogRepository;
        }

        public void DeleteOldEventLogs()
        {
            var days = configuration.GetValue<int>(ConfigurationKeys.DeleteDatabaseLogsOlderThanDays);

            eventLogRepository.DeleteLogsOlderThanDateTime(DateTime.Now.AddDays(-days));
        }
    }
}

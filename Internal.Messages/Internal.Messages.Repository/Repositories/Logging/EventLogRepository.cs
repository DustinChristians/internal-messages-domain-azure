using System;
using Internal.Messages.Core.Abstractions.Repositories.Logging;
using Internal.Messages.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace Internal.Messages.Repository.Repositories.Logging
{
    public class EventLogRepository : IEventLogRepository
    {
        protected InternalMessagesContext context;

        public EventLogRepository(InternalMessagesContext context)
        {
            this.context = context;
        }

        public void DeleteLogsOlderThanDateTime(DateTime dateTime)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM EventLog WHERE TimeStamp < {0}", dateTime);
        }
    }
}

using System;

namespace Internal.Messages.Core.Abstractions.Repositories.Logging
{
    public interface IEventLogRepository
    {
        void DeleteLogsOlderThanDateTime(DateTime dateTime);
    }
}

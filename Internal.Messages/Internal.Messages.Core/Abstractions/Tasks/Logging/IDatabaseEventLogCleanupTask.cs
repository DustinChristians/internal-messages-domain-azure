namespace Internal.Messages.Core.Abstractions.Tasks.Logging
{
    public interface IDatabaseEventLogCleanupTask
    {
        void DeleteOldEventLogs();
    }
}

using Internal.Messages.Core.Abstractions.Repositories;

namespace Internal.Messages.Core.Abstractions.Services
{
    public interface IMessagesService
    {
        public IMessagesRepository MessagesRepository { get; }
    }
}

using Internal.Messages.Core.Abstractions.Repositories;
using Internal.Messages.Core.Abstractions.Services;

namespace Internal.Messages.Infrastructure.Services
{
    public class MessagesService : IMessagesService
    {
        public MessagesService(IMessagesRepository messagesRepository)
        {
            this.MessagesRepository = messagesRepository;
        }

        public IMessagesRepository MessagesRepository { get; }

        // Add business logic here
    }
}

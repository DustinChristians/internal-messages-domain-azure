using System.Collections.Generic;
using System.Threading.Tasks;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.Core.Models.ResourceParameters;

namespace Internal.Messages.Core.Abstractions.Repositories
{
    public interface IMessagesRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesAsync(MessagesResourceParameters parameters);
    }
}

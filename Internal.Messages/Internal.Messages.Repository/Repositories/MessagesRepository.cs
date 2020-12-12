using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Internal.Messages.Core.Abstractions.Repositories;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.Core.Models.ResourceParameters;
using Internal.Messages.Core.Models.Search;
using Internal.Messages.Repository.Data;
using Internal.Messages.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Internal.Messages.Repository.Repositories
{
    public class MessagesRepository : BaseRepository<Message, MessageEntity>, IMessagesRepository
    {
        public MessagesRepository(InternalMessagesContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(MessagesResourceParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(MessagesResourceParameters));
            }

            var messageEntities = await Search()
                .Apply(parameters, Context.Messages as IQueryable<MessageEntity>)
                .ToListAsync();

            return Mapper.Map<IEnumerable<Message>>(messageEntities);
        }

        private SearchMutator<MessageEntity, MessagesResourceParameters> Search()
        {
            var searchMutator = new SearchMutator<MessageEntity, MessagesResourceParameters>();

            searchMutator.AddCondition(
                parameters => parameters.ChannelId > 0,
                (messages, parameters) => messages.Where(message => message.ChannelId == parameters.ChannelId));

            searchMutator.AddCondition(
                parameters => !string.IsNullOrWhiteSpace(parameters.SearchQuery),
                (messages, parameters) => messages.Where(u => u.Text.ToLower().Contains(parameters.SearchQuery)));

            return searchMutator;
        }
    }
}

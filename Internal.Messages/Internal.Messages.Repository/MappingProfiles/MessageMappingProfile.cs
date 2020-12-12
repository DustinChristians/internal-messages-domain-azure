using AutoMapper;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.Repository.Entities;

namespace Internal.Messages.Repository.MappingProfiles
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<MessageEntity, Message>().ReverseMap();
        }
    }
}

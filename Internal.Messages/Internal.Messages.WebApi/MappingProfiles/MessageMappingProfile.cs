using AutoMapper;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.WebApi.Models.Message;

namespace Internal.Messages.WebApi.MappingProfiles
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<ReadMessage, Message>().ReverseMap();
            CreateMap<CreateMessage, Message>().ReverseMap();
            CreateMap<UpdateMessage, Message>().ReverseMap();
        }
    }
}

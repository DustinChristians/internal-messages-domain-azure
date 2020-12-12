using AutoMapper;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.WebApi.Models.User;

namespace Internal.Messages.WebApi.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ReadUser, User>().ReverseMap();
            CreateMap<CreateUser, User>().ReverseMap();
        }
    }
}

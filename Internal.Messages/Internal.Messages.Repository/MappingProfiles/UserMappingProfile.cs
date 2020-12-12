using AutoMapper;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.Repository.Entities;

namespace Internal.Messages.Repository.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserEntity, User>().ReverseMap();
        }
    }
}

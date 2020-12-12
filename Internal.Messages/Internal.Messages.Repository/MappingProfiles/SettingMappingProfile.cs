using AutoMapper;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.Repository.Entities;

namespace Internal.Messages.Repository.MappingProfiles
{
    public class SettingMappingProfile : Profile
    {
        public SettingMappingProfile()
        {
            CreateMap<SettingEntity, Setting>().ReverseMap();
        }
    }
}

using AutoMapper;
using SisPlanWebCoreApi.Dtos;
using SisPlanWebCoreApi.Entities;

namespace SisPlanWebCoreApi.MappingProfiles
{
    public class clienteMappings : Profile
    {
        public clienteMappings()
        {
            CreateMap<ClienteEntity, ClienteDto>().ReverseMap();
            CreateMap<ClienteEntity, ClienteUpdateDto>().ReverseMap();
            CreateMap<ClienteEntity, ClienteCreateDto>().ReverseMap();
        }
    }
}

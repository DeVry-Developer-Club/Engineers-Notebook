using AutoMapper;
using EngineerNotebook.PublicApi.TagEndpoints;
using EngineerNotebook.PublicApi.WikiEndpoints;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Documentation, DocDto>();
            CreateMap<Tag, TagDto>();
        }
    }
}
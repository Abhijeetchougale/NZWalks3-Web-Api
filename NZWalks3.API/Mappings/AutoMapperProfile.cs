using AutoMapper;
using NZWalks3.API.DTOs;
using NZWalks3.API.Model.Domain;

namespace NZWalks3.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        { 
            CreateMap<Region, RequestRegionDto>().ReverseMap();
            CreateMap<AddRequestRegionDto, Region>().ReverseMap();
            CreateMap<UpdateRequestRegionDto, Region>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<AddWalkDto, Walk>().ReverseMap(); 
            CreateMap<UpdateWalkDto, Walk>().ReverseMap();  
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        }
    }
}

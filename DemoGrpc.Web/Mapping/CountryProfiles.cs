using AutoMapper;
using DemoGrpc.Domain.Entities;
using DemoGrpc.Protobufs;
using System.Collections.Generic;
using DemoGrpc.Protobufs.V1;

namespace DemoGrpc.Web.Mapping
{
    public class CountryProfilesV1 : Profile
    {
        public CountryProfilesV1()
        {
            CreateMap<Country, CountryReply>()
            .ForMember(dest => dest.Id, source => source.MapFrom(src => src.CountryId))
            .ForMember(dest => dest.Name, source => source.MapFrom(src => src.CountryName))
            .ForMember(dest => dest.Description, source => source.MapFrom(src => src.Description));

            CreateMap<IEnumerable<Country>, CountriesReply>()
            .ForMember(dest => dest.Countries, source => source.MapFrom(src => src));

            CreateMap<CountryCreateRequest, Country>()
            .ForMember(dest => dest.CountryName, source => source.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, source => source.MapFrom(src => src.Description));

            CreateMap<CountryRequest, Country>()
            .ForMember(dest => dest.CountryId, source => source.MapFrom(src => src.Id))
            .ForMember(dest => dest.CountryName, source => source.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, source => source.MapFrom(src => src.Description));
        }
    }
}

using Application.DTOs.Responses;
using AutoMapper;
using Domain.Entities;
using Common.Pagination;


namespace Application.Mappers
{
    public class CollectionMapper : Profile
    {
        public CollectionMapper()
        {
            CreateMap<Collection, CollectionResponse>()
                .ForMember(dest => dest.TotalExams, opt => opt.MapFrom(src => src.Exams.Count()))
                .ForMember(dest => dest.TotalDurations, opt => opt.MapFrom(src => src.Exams.Sum(e => (int?)e.Duration) ?? 0))
                ;
        }
    }
}

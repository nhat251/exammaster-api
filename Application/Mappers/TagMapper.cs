using Application.DTOs.Responses;
using AutoMapper;
using Domain.Entities;
using Common.Pagination;


namespace Application.Mappers
{
    public class TagMapper : Profile
    {
        public TagMapper()
        {
            CreateMap<Tag, TagResponse>();
        }
    }
}

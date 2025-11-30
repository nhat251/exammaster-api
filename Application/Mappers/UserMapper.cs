using Application.DTOs.Responses;
using AutoMapper;
using Domain.Entities;
using Common.Pagination;


namespace Application.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<ApplicationUser, UserResponse>();

            CreateMap(typeof(PageResult<>), typeof(PageResult<>));
        }
    }
}

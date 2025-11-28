using Application.DTOs.Responses;
using AutoMapper;
using Domain.Entities;
using Common.Pagination;


namespace Application.Mappers
{
    public class ExamMapper : Profile
    {
        public ExamMapper()
        {
            CreateMap<Exam, ExamResponse>();
            CreateMap<Tag, TagResponse>();
            CreateMap(typeof(PageResult<>), typeof(PageResult<>));
            CreateMap<AttempExam, ExamAttempedResponse>()
                .ForMember(dest => dest.ExamId, opt => opt.MapFrom(src => src.ExamId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Exam.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Exam.Description))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Exam.Duration))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Exam.Price))
                .ForMember(dest => dest.TotalQuestion, opt => opt.MapFrom(src => src.Exam.TotalQuestion))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Exam.Tags))

                .ForMember(dest => dest.AttempId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CompletedCount, opt => opt.MapFrom(src => src.CompletedCount))
                .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(src => src.StartedAt))
                .ForMember(dest => dest.ExpiredAt, opt => opt.MapFrom(src => src.ExpiredAt));

        }
    }
}

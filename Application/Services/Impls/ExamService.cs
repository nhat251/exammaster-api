using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Mappers;
using Application.Services.Interfaces;
using AutoMapper;
using Common.Exceptions;
using Common.Pagination;
using Common.Utils;
using Config;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Services.Impls
{
    public class ExamService : IExamService
    {


        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;
        private readonly IExamRepository _examRepository;
        private readonly IAttempExamRepository _attempExamRepository;
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly ICollectionRepository _collectionRepository;




        public ExamService(IMapper mapper, ILogger<AccountService> logger, IExamRepository examRepository, IAttempExamRepository attempExamRepository, IFavouriteRepository favouriteRepository, ICollectionRepository collectionRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _examRepository = examRepository;
            _attempExamRepository = attempExamRepository;
            _favouriteRepository = favouriteRepository;
            _collectionRepository = collectionRepository;
        }

        public async Task<PageResult<ExamAttempedResponse>> GetUnFinishedExams(string userId, PageRequest pageRequest)
        {
            DateTime now = DateTime.UtcNow;
            IEnumerable<ExamAttempedResponse> examAttempedResponse = Enumerable.Empty<ExamAttempedResponse>();

            _logger.LogInformation("Fetching {} attemp exams in page {} for user {} at {}", pageRequest.Size, pageRequest.Page, userId, now);

            PageResult<AttempExam> unFinishedAttempExams = await _attempExamRepository.GetPagedAsync(
                                                                        //predicate: ae => ae.UserId == userId && !ae.IsFinished && ae.ExpiredAt >= now,
                                                                        predicate: ae => ae.UserId == userId && !ae.IsFinished,
                                                                        pageRequest,
                                                                         ae => ae.User, ae => ae.Exam, ae => ae.Exam.Tags);

            var items = new List<ExamAttempedResponse>();
            foreach (var attemp in unFinishedAttempExams.Items)
            {
                var response = _mapper.Map<ExamAttempedResponse>(attemp);
                response.IsFavourite = await _favouriteRepository.IsFavouriteExists(userId, attemp.ExamId);
                items.Add(response);
            }

            var unFinishedExams = new PageResult<ExamAttempedResponse>
            {
                Items = items,
                Page = unFinishedAttempExams.Page,
                Size = unFinishedAttempExams.Size,
                TotalItems = unFinishedAttempExams.TotalItems
            };


            return unFinishedExams;
        }

        public async Task<string> MarkAsFavourited(Guid examId, string userId)
        {
            var exam = await _examRepository.GetAsync(e => e.Id == examId) ?? throw new AppException(ErrorCode.ENTITY_NOT_FOUND, "Exam not found");

            _logger.LogInformation("Marking exam {} as favourited", exam.Title);

            if (await _favouriteRepository.IsFavouriteExists(userId, examId))
            {
                _logger.LogInformation("Exam {} is already favourited", exam.Title);
                return "Exam is already favourited";
            }

            var favourite = new UserFavourite
            {
                UserId = userId,
                ExamId = examId,
            };

            await _favouriteRepository.AddAsync(favourite);

            return "Done";
        }

        public async Task<string> UnMarkAsFavourited(Guid examId, string userId)
        {
            var exam = await _examRepository.GetAsync(e => e.Id == examId) ?? throw new AppException(ErrorCode.ENTITY_NOT_FOUND, "Exam not found");

            _logger.LogInformation("Unmarking exam {} as favourited", exam.Title);

            if (!await _favouriteRepository.IsFavouriteExists(userId, examId))
            {
                _logger.LogInformation("Exam {} is not favourited", exam.Title);
                return "Exam is not favourited";
            }

            UserFavourite uf = (await _favouriteRepository.GetAsync(f => f.UserId == userId && f.ExamId == examId))!;

            await _favouriteRepository.RemoveAsync(uf);
            return "Done";
        }

        public async Task<PageResult<CollectionResponse>> GetCollectionsHasExam(PageRequest pageRequest)
        {
            _logger.LogInformation("Fetching {} exam collections in page {}", pageRequest.Size, pageRequest.Page);

            PageResult<Collection> collections = await _collectionRepository.GetPagedAsync(
                                                                        predicate: c => c.Exams.Any(),
                                                                        pageRequest,
                                                                        c => c.Exams, c => c.Tags);
            return new PageResult<CollectionResponse>
            {
                Items = [.. collections.Items.Select(c => _mapper.Map<CollectionResponse>(c))],
                Page = collections.Page,
                Size = collections.Size,
                TotalItems = collections.TotalItems
            };
        }
    }
}

using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Services.Interfaces;
using Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace exammaster_api.Controllers
{
    [Route("api/exams")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpGet("list/unfinished")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<PageResult<ExamAttempedResponse>>>> GetUnFinishedExams([FromQuery] PageRequest pageRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ApiResponse<PageResult<ExamAttempedResponse>> response = new ApiResponse<PageResult<ExamAttempedResponse>>
            {
                Result = await _examService.GetUnFinishedExams(userId!, pageRequest),
                Message = "Unfinished exams retrieved successfully",
                Code = 1000
            };

            return Ok(response);
        }

        [HttpPost("mark-as-favourited")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> MarkAsFavourited([FromQuery] Guid examId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ApiResponse<string> response = new ApiResponse<string>
            {
                Result = await _examService.MarkAsFavourited(examId, userId!),
                Message = "Exam marked as favourited successfully",
                Code = 1000
            };

            return Ok(response);
        }

        [HttpDelete("unmark-as-favourited")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> UnMarkAsFavourited([FromQuery] Guid examId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ApiResponse<string> response = new ApiResponse<string>
            {
                Result = await _examService.UnMarkAsFavourited(examId, userId!),
                Message = "Exam unmarked as favourited successfully",
                Code = 1000
            };

            return Ok(response);
        }

        [HttpGet("collections")]
        public async Task<ActionResult<ApiResponse<PageResult<CollectionResponse>>>> GetAllCollections([FromQuery] PageRequest pageRequest)
        {
            // co the them userId de lay cac collection theo user (AI RECOMMEND ~ lien quan toi lich su lam bai cua user) / neu ko co user thi fetch binh thuong

            ApiResponse<PageResult<CollectionResponse>> response = new ApiResponse<PageResult<CollectionResponse>>
            {
                Result =  await _examService.GetCollectionsHasExam(pageRequest),
                Message = "Collections retrieved successfully",
                Code = 1000
            };

            return Ok(response);
        }

    }
}

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

            return Ok(await _examService.GetUnFinishedExams(userId!, pageRequest));
        }

        [HttpPost("mark-as-favourited")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> MarkAsFavourited([FromQuery] Guid examId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(await _examService.MarkAsFavourited(examId, userId!));
        }

        [HttpDelete("unmark-as-favourited")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> UnMarkAsFavourited([FromQuery] Guid examId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(await _examService.UnMarkAsFavourited(examId, userId!));
        }

        [HttpGet("collections")]
        public async Task<ActionResult<ApiResponse<PageResult<CollectionResponse>>>> GetAllCollections([FromQuery] PageRequest pageRequest)
        {
            // co the them userId de lay cac collection theo user (AI RECOMMEND ~ lien quan toi lich su lam bai cua user) / neu ko co user thi fetch binh thuong
            return Ok(await _examService.GetCollectionsHasExam(pageRequest));
        }

    }
}

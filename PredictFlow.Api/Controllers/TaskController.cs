using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PredictFlow.Application.DTOs.Tasks;
using PredictFlow.Application.Interfaces;

namespace PredictFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequestDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(userIdClaim, out var currentUserId))
                {
                    return Unauthorized(new { message = "Invalid token: user id missing." });
                }
                var result = await _taskService.CreateAsync(currentUserId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{taskId:guid}/move")]
        public async Task<IActionResult> Move(Guid taskId, [FromBody] MoveTaskRequestDto dto)
        {
            try
            {
                await _taskService.MoveAsync(taskId, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{taskId:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid taskId, [FromBody] UpdateTaskStatusRequestDto dto)
        {
            try
            {
                await _taskService.UpdateStatusAsync(taskId, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PredictFlow.Application.DTOs.Sprint;
using PredictFlow.Application.DTOs.SprintTask;
using PredictFlow.Application.Interfaces;

namespace PredictFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SprintTaskController : ControllerBase
    {
        private readonly ISprintTaskService _sprintTaskService;

        public SprintTaskController(ISprintTaskService sprintTaskService)
        {
            _sprintTaskService = sprintTaskService;
        }
        [HttpPost("assign")]
        public async Task<IActionResult> AssignTask([FromBody] SprintTaskRequestDto dto)
        {
            try
            {
                if (dto.SprintId == Guid.Empty || dto.TaskId == Guid.Empty)
                    return BadRequest(new { message = "SprintId and TaskId are required." });

                await _sprintTaskService.AssignTaskToSprintAsync(dto.SprintId, dto.TaskId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{sprintId:guid}/tasks/{taskId:guid}")]
        public async Task<IActionResult> RemoveTask(Guid sprintId, Guid taskId)
        {
            try
            {
                if (sprintId == Guid.Empty || taskId == Guid.Empty)
                    return BadRequest(new { message = "SprintId and TaskId are required." });

                await _sprintTaskService.RemoveTaskFromSprintAsync(sprintId, taskId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{sprintId:guid}/tasks")]
        public async Task<IActionResult> GetTasks(Guid sprintId)
        {
            try
            {
                var tasks = await _sprintTaskService.GetTasksBySprintIdAsync(sprintId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
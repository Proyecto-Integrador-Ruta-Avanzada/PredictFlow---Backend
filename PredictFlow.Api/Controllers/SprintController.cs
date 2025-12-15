using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PredictFlow.Application.DTOs.Sprint;
using PredictFlow.Application.Interfaces;

namespace PredictFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SprintController : ControllerBase
    {
        private readonly ISprintService _sprintService;
        private readonly ISprintTaskService _sprintTaskService;

        public SprintController(ISprintService sprintService, ISprintTaskService sprintTaskService)
        {
            _sprintService = sprintService;
            _sprintTaskService = sprintTaskService;
        }

        #region Sprint Endpoints
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SprintRequestDto dto)
        {
            try
            {
                var result = await _sprintService.CreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{sprintId:guid}")]
        public async Task<IActionResult> Update(Guid sprintId, [FromBody] SprintRequestDto dto)
        {
            try
            {
                var result = await _sprintService.UpdateAsync(sprintId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{sprintId:guid}")]
        public async Task<IActionResult> Delete(Guid sprintId)
        {
            try
            {
                await _sprintService.DeleteAsync(sprintId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{sprintId:guid}")]
        public async Task<IActionResult> GetById(Guid sprintId)
        {
            try
            {
                var sprint = await _sprintService.GetByIdAsync(sprintId);
                if (sprint == null)
                    return NotFound(new { message = "Sprint not found" });

                return Ok(sprint);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("project/{projectId:guid}")]
        public async Task<IActionResult> GetByProjectId(Guid projectId)
        {
            try
            {
                var sprints = await _sprintService.GetByProjectIdAsync(projectId);
                return Ok(sprints);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region SprintTask Endpoints
        [HttpPost("{sprintId:guid}/tasks/{taskId:guid}")]
        public async Task<IActionResult> AssignTask(Guid sprintId, Guid taskId)
        {
            try
            {
                await _sprintTaskService.AssignTaskToSprintAsync(sprintId, taskId);
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
        #endregion
    }
}

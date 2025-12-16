using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PredictFlow.Application.Interfaces;

namespace PredictFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RiskController : ControllerBase
{
    private readonly IRiskService _riskService;

    public RiskController(IRiskService riskService)
    {
        _riskService = riskService;
    }

    [HttpGet("task/{taskId:guid}")]
    public async Task<IActionResult> GetTaskRisk(Guid taskId)
    {
        try
        {
            return Ok(await _riskService.EvaluateTaskRiskAsync(taskId));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("member/{userId:guid}/workload")]
    public async Task<IActionResult> GetWorkload(
        Guid userId,
        [FromQuery] int additionalHours = 0,
        [FromQuery] int thresholdHours = 40)
    {
        try
        {
            return Ok(await _riskService.EvaluateMemberWorkloadAsync(
                userId,
                additionalHours,
                thresholdHours));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("member/{userId:guid}/ensure")]
    public async Task<IActionResult> EnsureCapacity(
        Guid userId,
        [FromQuery] int additionalHours,
        [FromQuery] int thresholdHours = 40)
    {
        try
        {
            await _riskService.EnsureMemberCanTakeMoreWorkAsync(
                userId,
                additionalHours,
                thresholdHours);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
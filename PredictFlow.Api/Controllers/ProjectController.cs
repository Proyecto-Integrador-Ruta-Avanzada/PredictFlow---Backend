using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PredictFlow.Application.DTOs.Projects;
using PredictFlow.Application.Interfaces;
using PredictFlow.Application.DTOs;

namespace PredictFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequestDto dto)
    {
        try
        {
            var result = await _projectService.CreateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            return Ok(await _projectService.GetByIdAsync(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("team/{teamId:guid}")]
    public async Task<IActionResult> GetByTeam(Guid teamId)
    {
        try
        {
            return Ok(await _projectService.GetByTeamAsync(teamId));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{projectId:guid}/full")]
    public async Task<IActionResult> GetFull(Guid projectId)
    {
        try
        {
            return Ok(await _projectService.GetFullAsync(projectId));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProjectRequestDto dto)
    {
        try
        {
            await _projectService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _projectService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

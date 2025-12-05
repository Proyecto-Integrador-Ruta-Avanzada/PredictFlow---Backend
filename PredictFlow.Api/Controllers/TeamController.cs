using Microsoft.AspNetCore.Mvc;
using PredictFlow.Application.DTOs;
using PredictFlow.Application.DTOs.TeamMember;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;

namespace PredictFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequestDto dto)
    {
        var ownerId = Guid.Parse("00000000-0000-0000-0000-00000000001");
        
        var team = await _teamService.CreateTeamAsync(dto.Name, ownerId);
        return Ok(new
        {
            team.Id,
            team.Name,
            team.CreatedAt
        });
    }

    [HttpGet("{teamId:guid}")]
    public async Task<IActionResult> GetTeam(Guid teamId)
    {
        var team = await _teamService.GetTeamAsync(teamId);
        if (team == null)
            return NotFound("Equipo no encontrado");
        
        return Ok(team);
    }

    [HttpGet("{teamId:guid}/members")]
    public async Task<IActionResult> GetTeamForUser(Guid userId)
    {
        var teams = await _teamService.GetTeamsForUserAsync(userId);
        if (teams == null)
            return NotFound("No encontrado");
        
        return Ok(teams);
    }

    [HttpGet("{team:guid}/members")]
    public async Task<IActionResult> GetMembers(Guid teamId)
    {
        var members = await _teamService.GetMembersAsync(teamId);
        if (members == null)
            return NotFound("No hay usuarios ingresados");

        return Ok(members.Select(m => new
        {
            m.UserId,
            m.Role,
            m.Skills,
            m.Availability
            m.Workload
        }));
    }

    [HttpPost("{teamId:guid}/members")]
    public async Task<IActionResult> AddMember(Guid teamId, [FromBody] AddMemberRequestDto dto)
    {
        await _teamService.AddMemberAsync(teamId, dto.UserId, dto.Role, dto.Skills, dto.Availability);
        return Ok("Miembro agregado correctamente");
    }

    [HttpDelete("{teamId:guid}/members/{userId:guid}")]
    public async Task<IActionResult> RemoveMember(Guid teamId, Guid userId)
    {
        await _teamService.RemoveMemberAsync(teamId, userId);
        return Ok("Miembro eliminado correctamente");
    }

    [HttpPut("{teamId:guid}/members/{userId:guid}/role")]
    public async Task<IActionResult> UpdateRole(Guid teamId, Guid userId, [FromBody] UpdateRoleDto dto)
    {
        await _teamService.UpdateMemberRoleAsync(teamId, userId, dto.Role);
        return Ok("Rol Actualizado");
    }

    [HttpPut("{teamId:guid}/members/{userId:guid}/skills}")]
    public async Task<IActionResult> UpdateSkills(Guid teamId, Guid userId, [FromBody] UpdateSkillsDto dto)
    {
        await _teamService.UpdateMemberSkillsAsync(teamId, userId, dto.Skills);
        return Ok("Habilidades Actualizadas");
    }
    
    [HttpPut("{teamId:guid}/members/{userId:guid}/availability")]
    public async Task<IActionResult> UpdateAvailability(Guid teamId, Guid userId, [FromBody] UpdateAvailabilityDto dto)
    {
        await _teamService.UpdateMemberAvailabilityAsync(teamId, userId, dto.Availability);
        return Ok("Disponibilidad actualizada");
    }
    
    [HttpGet("{teamId:guid}/members/{userId:guid}/exists")]
    public async Task<IActionResult> UserIsTeamMember(Guid teamId, Guid userId)
    {
        var result = await _teamService.UserIsTeamMemberAsync(teamId, userId);
        return Ok(new { isMember = result });
    }
    
    [HttpPut("{teamId:guid}/members/{userId:guid}/workload")]
    public async Task<IActionResult> UpdateWorkload(Guid teamId, Guid userId, [FromBody] UpdateWorkloadDto dto)
    {
        var member = await _teamService.UpdateMemberWorkloadAsync(teamId, userId);

        if (member == null)
            return NotFound("Miembro no encontrado");

        member.UpdateWorkload(dto.Workload);    

        await _teamService.UpdateMemberAvailabilityAsync(teamId, userId, member.Availability);

        return Ok("Workload actualizado");
    }
}
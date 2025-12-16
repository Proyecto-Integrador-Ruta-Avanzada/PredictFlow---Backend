using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using PredictFlow.Application.DTOs;
using PredictFlow.Application.DTOs.TeamMember;
using PredictFlow.Application.Interfaces;

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
        // 1. Intentamos obtener el ID usando el nombre estándar de .NET
        var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

        // 2. Si es nulo, intentamos buscarlo como "sub" (estándar JWT)
        if (idClaim == null)
        {
            idClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
        }

        // 3. Validación de seguridad
        if (idClaim == null)
        {
            return Unauthorized(new { message = "Token inválido: No se encontró el ID del usuario." });
        }

        if (!Guid.TryParse(idClaim.Value, out var ownerId))
        {
            return Unauthorized(new { message = "Token inválido: El ID no tiene el formato correcto." });
        }

        var team = await _teamService.CreateTeamAsync(dto.Name, ownerId);

        return Ok(new TeamResponseDto
        {
            Id = team.Id,
            Name = team.Name,
            CreatedAt = team.CreatedAt
        });
    }

    [HttpGet("{teamId:guid}")]
    public async Task<IActionResult> GetTeam(Guid teamId)
    {
        var team = await _teamService.GetTeamAsync(teamId);
        return team == null ? NotFound("Team no encontrado") : Ok(team);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetTeamsForUser(Guid userId)
    {
        var teams = await _teamService.GetTeamsForUserAsync(userId);
        return Ok(teams);
    }

    [HttpGet("{teamId:guid}/members")]
    public async Task<IActionResult> GetMembers(Guid teamId)
    {
        var members = await _teamService.GetMembersAsync(teamId);
        return Ok(members.Select(m => new TeamMemberResponseDto
        {
            UserId = m.UserId, Role = m.Role, Skills = m.Skills, Availability = m.Availability, Workload = m.Workload
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
        return Ok("Rol actualizado");
    }

    [HttpPut("{teamId:guid}/members/{userId:guid}/skills")]
    public async Task<IActionResult> UpdateSkills(Guid teamId, Guid userId, [FromBody] UpdateSkillsDto dto)
    {
        await _teamService.UpdateMemberSkillsAsync(teamId, userId, dto.Skills);
        return Ok("Skills actualizadas");
    }

    [HttpPut("{teamId:guid}/members/{userId:guid}/availability")]
    public async Task<IActionResult> UpdateAvailability(Guid teamId, Guid userId, [FromBody] UpdateAvailabilityDto dto)
    {
        await _teamService.UpdateMemberAvailabilityAsync(teamId, userId, dto.Availability);
        return Ok("Availability actualizada");
    }

    [HttpPut("{teamId:guid}/members/{userId:guid}/workload")]
    public async Task<IActionResult> UpdateWorkload(Guid teamId, Guid userId, [FromBody] UpdateWorkloadDto dto)
    {
        await _teamService.UpdateMemberWorkloadAsync(teamId, userId, dto.Workload);
        return Ok("Workload actualizado");
    }
    
    [HttpGet("{teamId:guid}/members/{userId:guid}/exists")]
    public async Task<IActionResult> UserIsTeamMember(Guid teamId, Guid userId)
    {
        var result = await _teamService.UserIsTeamMemberAsync(teamId, userId);
        return Ok(new { isMember = result });
    }
}
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PredictFlow.Application.DTOs.InvitationDtos;
using PredictFlow.Application.Interfaces.InvitationsInterfaces;

namespace PredictFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvitationController : ControllerBase
{
    private readonly IInvitationService _service;

    public InvitationController(IInvitationService service)
    {
        _service = service;
    }

    [HttpPost("invite")]
    public async Task<IActionResult> InviteUser([FromBody] InviteMemberRequestDto inviteMemberRequestDto)
    {
        try
        {
            var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (idClaim == null)
            {
                idClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
            }
            if (idClaim == null)
            {
                return Unauthorized(new { message = "Token inválido: No se encontró el ID del usuario." });
            }

            if (!Guid.TryParse(idClaim.Value, out var ownerId))
            {
                return Unauthorized(new { message = "Token inválido: El ID no tiene el formato correcto." });
            }

            inviteMemberRequestDto.InvitedByUserId = ownerId;
            await _service.InviteMember(inviteMemberRequestDto);
            return Ok(new 
            {
                Succeeded = true,
                Message = "User invite succesfully"
            });
        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                Succeeded = false,
                Message = $"There was an error inviting the user, pls try again: {e.Message}"
            });
        }
    }

    [HttpGet("validate")]
    public async Task<IActionResult> ValidateInvitation([FromQuery] string code, [FromQuery] string email)
    {
        try
        {
            var result = await _service.ValidateInvitation(code, email);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"There was an unexpected error: {e.Message} ");
        }
    }

    [Authorize]
    [HttpPost("accept")]
    public async Task<IActionResult> AcceptInvitation([FromBody] string code)
    {
        try
        {
            var result = await _service.AcceptInvitation(code);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest( new
            {
                Message = $"There was an unexpected error: {e.Message}"
            });
        }
    }
}
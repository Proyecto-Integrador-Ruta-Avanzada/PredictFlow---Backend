using Microsoft.AspNetCore.Mvc;
using PredictFlow.Application.DTOs;
using PredictFlow.Application.Interfaces;

namespace PredictFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var messageResult = await _authService.RegisterAsync(dto);
            // CAMBIO CLAVE: Devolvemos un objeto JSON, no texto plano
            return Ok(new { message = messageResult });
        }
        catch (ArgumentException ex) // Captura errores de validación (ej. Email inválido)
        {
             return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Unauthorized (401) es el código correcto para login fallido
            return Unauthorized(new { message = ex.Message });
        }
    }
}
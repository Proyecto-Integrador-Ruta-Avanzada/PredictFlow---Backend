using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PredictFlow.Application.DTOs;
using PredictFlow.Application.DTOs.BoardColumns;
using PredictFlow.Application.Interfaces;

namespace PredictFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardColumnController : ControllerBase
{
    private readonly IBoardColumnService _columnService;

    public BoardColumnController(IBoardColumnService columnService)
    {
        _columnService = columnService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBoardColumnRequestDto dto)
    {
        try
        {
            return Ok(await _columnService.CreateAsync(dto));
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
            return Ok(await _columnService.GetByIdAsync(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("board/{boardId:guid}")]
    public async Task<IActionResult> GetByBoard(Guid boardId)
    {
        try
        {
            return Ok(await _columnService.GetByBoardAsync(boardId));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateBoardColumnRequestDto dto)
    {
        try
        {
            await _columnService.UpdateAsync(id, dto);
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
            await _columnService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

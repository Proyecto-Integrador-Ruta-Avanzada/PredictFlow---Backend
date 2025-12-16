using PredictFlow.Application.DTOs.BoardColumns;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Application.Services;

public class BoardColumnService : IBoardColumnService
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardColumnRepository _boardColumnRepository;

    public BoardColumnService(IBoardRepository boardRepository, IBoardColumnRepository boardColumnRepository)
    {
        _boardRepository = boardRepository;
        _boardColumnRepository = boardColumnRepository;
    }

    public async Task<BoardColumnResponseDto> CreateAsync(CreateBoardColumnRequestDto dto)
    {
        var board = await _boardRepository.GetByIdAsync(dto.BoardId);
        if (board is null) throw new InvalidOperationException("Board not found.");

        var column = new BoardColumn(dto.BoardId, dto.Name, dto.Position);
        await _boardColumnRepository.AddAsync(column);

        return Map(column);
    }

    public async Task<BoardColumnResponseDto?> GetByIdAsync(Guid id)
    {
        var column = await _boardColumnRepository.GetByIdAsync(id);
        return column is null ? null : Map(column);
    }

    public async Task<IEnumerable<BoardColumnResponseDto>> GetByBoardAsync(Guid boardId)
    {
        var cols = await _boardColumnRepository.GetByBoardIdAsync(boardId);
        return cols.OrderBy(c => c.Position).Select(Map);
    }

    public async Task<BoardColumnResponseDto> UpdateAsync(Guid id, UpdateBoardColumnRequestDto dto)
    {
        var column = await _boardColumnRepository.GetByIdAsync(id);
        if (column is null) throw new InvalidOperationException("Board column not found.");

        column.Rename(dto.Name);
        column.Reposition(dto.Position);

        await _boardColumnRepository.UpdateAsync(column);

        return Map(column);
    }

    public async Task DeleteAsync(Guid id)
    {
        var column = await _boardColumnRepository.GetByIdAsync(id);
        if (column is null) return;

        await _boardColumnRepository.DeleteAsync(column);
    }

    private static BoardColumnResponseDto Map(BoardColumn column)
    {
        return new BoardColumnResponseDto
        {
            Id = column.Id,
            BoardId = column.BoardId,
            Name = column.Name,
            Position = column.Position
        };
    }
}

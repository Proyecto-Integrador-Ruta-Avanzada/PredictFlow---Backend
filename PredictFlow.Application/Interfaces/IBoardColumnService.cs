using PredictFlow.Application.DTOs.BoardColumns;

namespace PredictFlow.Application.Interfaces;

public interface IBoardColumnService
{
    Task<BoardColumnResponseDto> CreateAsync(CreateBoardColumnRequestDto dto);
    Task<BoardColumnResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<BoardColumnResponseDto>> GetByBoardAsync(Guid boardId);
    Task<BoardColumnResponseDto> UpdateAsync(Guid id, UpdateBoardColumnRequestDto dto);
    Task DeleteAsync(Guid id);
}
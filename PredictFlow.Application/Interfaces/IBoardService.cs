using PredictFlow.Application.DTOs.Boards;

namespace PredictFlow.Application.Interfaces;

public interface IBoardService
{
    Task<BoardResponseDto> CreateAsync(CreateBoardRequestDto dto);
    Task<BoardResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<BoardResponseDto>> GetByProjectAsync(Guid projectId);
    Task<BoardResponseDto> UpdateAsync(Guid id, UpdateBoardRequestDto dto);
    Task DeleteAsync(Guid id);
    
    Task<BoardFullResponseDto?> GetFullAsync(Guid boardId); 
}
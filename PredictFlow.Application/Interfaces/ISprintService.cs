using PredictFlow.Application.DTOs.Sprint;
using PredictFlow.Application.DTOs.Sprints;

namespace PredictFlow.Application.Interfaces;

public interface ISprintService
{
    Task<SprintResponseDto> CreateAsync(SprintRequestDto dto);
    Task<SprintResponseDto> UpdateAsync(Guid sprintId, SprintRequestDto dto);
    Task DeleteAsync(Guid sprintId);
    Task<SprintResponseDto?> GetByIdAsync(Guid sprintId);
    Task<IEnumerable<SprintResponseDto>> GetByProjectIdAsync(Guid projectId);
}
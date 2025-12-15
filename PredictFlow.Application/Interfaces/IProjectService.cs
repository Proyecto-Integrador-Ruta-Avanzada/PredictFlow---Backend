using PredictFlow.Application.DTOs.Projects;

namespace PredictFlow.Application.Interfaces;

public interface IProjectService
{
    Task<ProjectResponseDto> CreateAsync(CreateProjectRequestDto dto);
    Task<ProjectResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProjectResponseDto>> GetByTeamAsync(Guid teamId);
    Task<ProjectResponseDto> UpdateAsync(Guid id, UpdateProjectRequestDto dto);
    Task DeleteAsync(Guid id);
    
    Task<ProjectFullResponseDto?> GetFullAsync(Guid projectId);

}
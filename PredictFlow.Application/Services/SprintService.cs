using PredictFlow.Application.DTOs.Sprint;
using PredictFlow.Application.DTOs.Sprints;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Application.Services;

public class SprintService : ISprintService
{
    private readonly ISprintRepository _sprintRepository;

    public SprintService(ISprintRepository sprintRepository)
    {
        _sprintRepository = sprintRepository;
    }

    public async Task<SprintResponseDto> CreateAsync(SprintRequestDto dto)
    {
        var sprint = new Sprint(dto.ProjectId, dto.Name.Trim(), dto.StartDate, dto.EndDate, dto.Goal.Trim());
        await _sprintRepository.AddAsync(sprint);
        return Map(sprint);
    }

    public async Task<SprintResponseDto> UpdateAsync(Guid sprintId, SprintRequestDto dto)
    {
        var sprint = await _sprintRepository.GetByIdAsync(sprintId);
        if (sprint == null)
            throw new Exception("Sprint not found");
        
        sprint.UpdateDetails(dto.Name, dto.StartDate, dto.EndDate, dto.Goal);

        await _sprintRepository.UpdateAsync(sprint);

        return Map(sprint);
    }

    public async Task DeleteAsync(Guid sprintId)
    {
        var sprint = await _sprintRepository.GetByIdAsync(sprintId);
        if (sprint == null)
            throw new Exception("Sprint not found");

        await _sprintRepository.DeleteAsync(sprint);
    }

    public async Task<SprintResponseDto?> GetByIdAsync(Guid sprintId)
    {
        var sprint = await _sprintRepository.GetByIdAsync(sprintId);
        return sprint == null ? null : Map(sprint);
    }

    public async Task<IEnumerable<SprintResponseDto>> GetByProjectIdAsync(Guid projectId)
    {
        var sprints = await _sprintRepository.GetByProjectIdAsync(projectId);
        return sprints.Select(Map);
    }

    private static SprintResponseDto Map(Sprint sprint)
    {
        return new SprintResponseDto
        {
            Id = sprint.Id,
            ProjectId = sprint.ProjectId,
            Name = sprint.Name,
            StartDate = sprint.StartDate,
            EndDate = sprint.EndDate,
            Goal = sprint.Goal
        };
    }
}

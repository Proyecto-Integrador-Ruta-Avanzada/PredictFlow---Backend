using PredictFlow.Application.DTOs.Boards;
using PredictFlow.Application.DTOs.BoardColumns;
using PredictFlow.Application.DTOs.Projects;
using PredictFlow.Application.DTOs.Tasks;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectResponseDto> CreateAsync(CreateProjectRequestDto dto)
    {
        var project = new Project(dto.TeamId, dto.Name, dto.Description);
        await _projectRepository.AddAsync(project);

        return Map(project);
    }

    public async Task<ProjectResponseDto?> GetByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return project is null ? null : Map(project);
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetByTeamAsync(Guid teamId)
    {
        var projects = await _projectRepository.GetByTeamIdAsync(teamId);
        return projects.Select(Map);
    }

    public async Task<ProjectResponseDto> UpdateAsync(Guid id, UpdateProjectRequestDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project is null) throw new InvalidOperationException("Project not found.");

        project.UpdateDetails(dto.Name, dto.Description);
        await _projectRepository.UpdateAsync(project);

        return Map(project);
    }

    public async Task DeleteAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project is null) return; // idempotente

        await _projectRepository.DeleteAsync(project);
    }

    private static ProjectResponseDto Map(Project project)
    {
        return new ProjectResponseDto
        {
            Id = project.Id,
            TeamId = project.TeamId,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            Boards = project.Boards
                .OrderBy(b => b.Position)
                .Select(b => new BoardResponseDto
                {
                    Id = b.Id,
                    ProjectId = b.ProjectId,
                    Name = b.Name,
                    Position = b.Position,
                    Columns = b.Columns
                        .OrderBy(c => c.Position)
                        .Select(c => new BoardColumnResponseDto
                        {
                            Id = c.Id,
                            BoardId = c.BoardId,
                            Name = c.Name,
                            Position = c.Position
                        })
                        .ToList()
                })
                .ToList()
        };
    }
    
    
    public async Task<ProjectFullResponseDto?> GetFullAsync(Guid projectId)
    {
        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId is required.", nameof(projectId));

        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project is null) return null;

        return new ProjectFullResponseDto
        {
            Id = project.Id,
            TeamId = project.TeamId,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            Boards = project.Boards
                .OrderBy(b => b.Position)
                .Select(b => new BoardFullResponseDto
                {
                    Id = b.Id,
                    ProjectId = b.ProjectId,
                    Name = b.Name,
                    Position = b.Position,
                    Columns = b.Columns
                        .OrderBy(c => c.Position)
                        .Select(c => new BoardColumnFullResponseDto
                        {
                            Id = c.Id,
                            BoardId = c.BoardId,
                            Name = c.Name,
                            Position = c.Position,
                            Tasks = c.Tasks
                                .OrderBy(t => t.CreatedAt)
                                .Select(MapTask)
                                .ToList()
                        })
                        .ToList()
                })
                .ToList()
        };
    }
    
    
    private static TaskResponseDto MapTask(TaskEntity task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            BoardColumnId = task.BoardColumnId,
            Title = task.Title,
            Description = task.Description,
            CreatedBy = task.CreatedBy,
            AssignedTo = task.AssignedTo,
            Priority = (int)task.Priority,
            StoryPoints = task.StoryPoints.Value,
            State = task.State.ToString(),
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }


}

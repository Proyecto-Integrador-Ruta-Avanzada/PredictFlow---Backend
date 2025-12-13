using PredictFlow.Application.DTOs.Boards;
using PredictFlow.Application.DTOs.BoardColumns;
using PredictFlow.Application.DTOs.Tasks;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Application.Services;

public class BoardService : IBoardService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IBoardRepository _boardRepository;

    public BoardService(IProjectRepository projectRepository, IBoardRepository boardRepository)
    {
        _projectRepository = projectRepository;
        _boardRepository = boardRepository;
    }

    public async Task<BoardResponseDto> CreateAsync(CreateBoardRequestDto dto)
    {
        // validar que el proyecto exista 
        var project = await _projectRepository.GetByIdAsync(dto.ProjectId);
        if (project is null) throw new InvalidOperationException("Project not found.");

        var board = new Board(dto.ProjectId, dto.Name, dto.Position);
        await _boardRepository.AddAsync(board);

        return Map(board);
    }

    public async Task<BoardResponseDto?> GetByIdAsync(Guid id)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        return board is null ? null : Map(board);
    }

    public async Task<IEnumerable<BoardResponseDto>> GetByProjectAsync(Guid projectId)
    {
        var boards = await _boardRepository.GetByProjectIdAsync(projectId);
        return boards.OrderBy(b => b.Position).Select(Map);
    }

    public async Task<BoardResponseDto> UpdateAsync(Guid id, UpdateBoardRequestDto dto)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board is null) throw new InvalidOperationException("Board not found.");

        board.Rename(dto.Name);
        board.Reposition(dto.Position);

        await _boardRepository.UpdateAsync(board);

        return Map(board);
    }

    public async Task DeleteAsync(Guid id)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board is null) return;

        await _boardRepository.DeleteAsync(board);
    }

    private static BoardResponseDto Map(Board board)
    {
        return new BoardResponseDto
        {
            Id = board.Id,
            ProjectId = board.ProjectId,
            Name = board.Name,
            Position = board.Position,
            Columns = board.Columns
                .OrderBy(c => c.Position)
                .Select(c => new BoardColumnResponseDto
                {
                    Id = c.Id,
                    BoardId = c.BoardId,
                    Name = c.Name,
                    Position = c.Position
                })
                .ToList()
        };
    }
    
    public async Task<BoardFullResponseDto?> GetFullAsync(Guid boardId)
    {
        if (boardId == Guid.Empty) throw new ArgumentException("BoardId is required.", nameof(boardId));

        var board = await _boardRepository.GetByIdAsync(boardId);
        if (board is null) return null;

        return new BoardFullResponseDto
        {
            Id = board.Id,
            ProjectId = board.ProjectId,
            Name = board.Name,
            Position = board.Position,
            Columns = board.Columns
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

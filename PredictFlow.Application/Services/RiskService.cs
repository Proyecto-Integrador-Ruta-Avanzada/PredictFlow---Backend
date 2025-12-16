using PredictFlow.Application.DTOs.Risk;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Application.Services;

public class RiskService : IRiskService
{
    private readonly ITaskRepository _taskRepository;

    // Regla simple configurable
    private const int HighRiskEstimatedHoursThreshold = 16;

    public RiskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskRiskResultDto> EvaluateTaskRiskAsync(Guid taskId)
    {
        if (taskId == Guid.Empty) throw new ArgumentException("TaskId is required.", nameof(taskId));

        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task is null) throw new InvalidOperationException("Task not found.");

        var reasons = new List<string>();
        var risk = RiskLevel.Low;

        // 1) Exceso de horas estimadas => High
        if (task.EstimatedHours > HighRiskEstimatedHoursThreshold)
        {
            risk = RiskLevel.High;
            reasons.Add($"EstimatedHours is greater than {HighRiskEstimatedHoursThreshold}h.");
        }

        // 2) Dependencias presentes (lista actual) => Medium (si no es High)
        if (task.Dependencies.Count > 0 && risk != RiskLevel.High)
        {
            risk = RiskLevel.Medium;
            reasons.Add("Task has dependencies.");
        }

        // 3) Prioridad alta => Medium (si no es High)
        if (task.Priority == Priority.High && risk == RiskLevel.Low)
        {
            risk = RiskLevel.Medium;
            reasons.Add("Priority is High.");
        }

        return new TaskRiskResultDto
        {
            TaskId = task.Id,
            RiskLevel = risk,
            Reasons = reasons
        };
    }

    public async Task<MemberWorkloadResultDto> EvaluateMemberWorkloadAsync(
        Guid userId,
        int additionalHours = 0,
        int thresholdHours = 40)
    {
        if (userId == Guid.Empty) throw new ArgumentException("UserId is required.", nameof(userId));
        if (additionalHours < 0) throw new ArgumentOutOfRangeException(nameof(additionalHours));
        if (thresholdHours <= 0) throw new ArgumentOutOfRangeException(nameof(thresholdHours));

        // tareas del miembro (recomendación: solo contar no-terminadas)
        var tasks = await _taskRepository.GetByAssigneeAsync(userId);
        var current = tasks
            .Where(t => t.State != TaskState.Done)
            .Sum(t => t.EstimatedHours);

        var totalIfAssigned = current + additionalHours;

        return new MemberWorkloadResultDto
        {
            UserId = userId,
            ThresholdHours = thresholdHours,
            CurrentEstimatedHours = current,
            AdditionalHours = additionalHours,
            TotalIfAssigned = totalIfAssigned,
            IsOverloaded = totalIfAssigned > thresholdHours
        };
    }

    public async Task EnsureMemberCanTakeMoreWorkAsync(Guid userId, int additionalHours, int thresholdHours = 40)
    {
        var result = await EvaluateMemberWorkloadAsync(userId, additionalHours, thresholdHours);

        if (result.IsOverloaded)
            throw new InvalidOperationException(
                $"User workload exceeded: {result.TotalIfAssigned}h > {result.ThresholdHours}h.");
    }
}

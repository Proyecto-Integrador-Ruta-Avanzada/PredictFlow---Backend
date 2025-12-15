using PredictFlow.Application.DTOs.Risk;

namespace PredictFlow.Application.Interfaces;

public interface IRiskService
{
    Task<TaskRiskResultDto> EvaluateTaskRiskAsync(Guid taskId);

    Task<MemberWorkloadResultDto> EvaluateMemberWorkloadAsync(
        Guid userId,
        int additionalHours = 0,
        int thresholdHours = 40);

    Task EnsureMemberCanTakeMoreWorkAsync(
        Guid userId,
        int additionalHours,
        int thresholdHours = 40);
}
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Application.DTOs.Risk;

public class TaskRiskResultDto
{
    public Guid TaskId { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public List<string> Reasons { get; set; } = new();
}
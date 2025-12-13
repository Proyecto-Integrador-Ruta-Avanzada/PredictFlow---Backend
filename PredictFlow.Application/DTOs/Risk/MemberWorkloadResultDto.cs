namespace PredictFlow.Application.DTOs.Risk;

public class MemberWorkloadResultDto
{
    public Guid UserId { get; set; }
    public int ThresholdHours { get; set; }
    public int CurrentEstimatedHours { get; set; }
    public int AdditionalHours { get; set; }
    public int TotalIfAssigned { get; set; }
    public bool IsOverloaded { get; set; }
}
using PredictFlow.Application.DTOs.Profile;

namespace PredictFlow.Application.Interfaces;

public interface IProfileService
{
    Task<ProfileResponseDto> GetMyProfileAsync(Guid userId);
}
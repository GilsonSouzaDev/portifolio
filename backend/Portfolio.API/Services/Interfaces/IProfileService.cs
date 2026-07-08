using Portfolio.API.DTOs;

namespace Portfolio.API.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileDto?> GetProfileAsync();
    Task<ProfileDto?> UpdateProfileAsync(UpdateProfileRequest request);
}

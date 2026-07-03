using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOs;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var profile = await _profileService.GetProfileAsync();

        if (profile == null)
            return NotFound(new { message = "Perfil não encontrado." });

        return Ok(profile);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequest updated)
    {
        var profile = await _profileService.UpdateProfileAsync(updated);

        if (profile == null)
            return NotFound(new { message = "Perfil não encontrado." });

        return Ok(profile);
    }
}
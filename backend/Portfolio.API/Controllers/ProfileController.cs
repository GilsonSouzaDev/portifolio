using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync();

        if (profile == null)
            return NotFound(new { message = "Perfil não encontrado." });

        return Ok(profile);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Profile updated)
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync();

        if (profile == null)
            return NotFound(new { message = "Perfil não encontrado." });

        profile.Name = updated.Name;
        profile.Title = updated.Title;
        profile.Bio = updated.Bio;
        profile.AvatarUrl = updated.AvatarUrl;
        profile.ResumeUrl = updated.ResumeUrl;
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(profile);
    }
}
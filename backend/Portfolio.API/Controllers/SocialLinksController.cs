using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/social-links")]
public class SocialLinksController : ControllerBase
{
    private readonly AppDbContext _context;

    public SocialLinksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var links = await _context.SocialLinks
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();
        return Ok(links);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] SocialLink updated)
    {
        var link = await _context.SocialLinks.FindAsync(id);
        if (link == null)
            return NotFound(new { message = "Link social não encontrado." });

        link.Platform = updated.Platform;
        link.Url = updated.Url;
        link.IconUrl = updated.IconUrl;
        link.DisplayOrder = updated.DisplayOrder;
        link.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(link);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SocialLink newLink)
    {
        newLink.CreatedAt = DateTime.UtcNow;
        newLink.UpdatedAt = DateTime.UtcNow;
        _context.SocialLinks.Add(newLink);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = newLink.Id }, newLink);
    }
}
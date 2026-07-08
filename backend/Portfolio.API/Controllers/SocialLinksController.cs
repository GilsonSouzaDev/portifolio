using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOs;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/social-links")]
public class SocialLinksController : ControllerBase
{
    private readonly ISocialLinksService _socialLinksService;

    public SocialLinksController(ISocialLinksService socialLinksService)
    {
        _socialLinksService = socialLinksService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var socialLinks = await _socialLinksService.GetAllSocialLinksAsync();
        return Ok(socialLinks);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateSocialLinkRequest socialLinkRequest)
    {
        var socialLink = await _socialLinksService.CreateSocialLinkAsync(socialLinkRequest);
        return CreatedAtAction(nameof(GetAll), new { id = socialLink.Id }, socialLink);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateOrUpdateSocialLinkRequest socialLinkRequest)
    {
        var socialLink = await _socialLinksService.UpdateSocialLinkAsync(id, socialLinkRequest);

        if (socialLink == null)
            return NotFound(new { message = "Link social não encontrado." });

        return Ok(socialLink);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _socialLinksService.DeleteSocialLinkAsync(id);

        if (!result)
            return NotFound(new { message = "Link social não encontrado." });

        return NoContent();
    }
}
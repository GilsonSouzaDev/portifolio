namespace Portfolio.API.Models;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? ProjectUrl { get; set; }
    public string? RepositoryUrl { get; set; }
    public string Technologies { get; set; } = string.Empty;
    public bool Featured { get; set; } = false;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
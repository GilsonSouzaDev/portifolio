namespace Portfolio.API.DTOs;

public class ProjectDto
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
}

public class CreateOrUpdateProjectRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? ProjectUrl { get; set; }
    public string? RepositoryUrl { get; set; }
    public string Technologies { get; set; } = string.Empty;
    public bool Featured { get; set; } = false;
    public int DisplayOrder { get; set; }
}

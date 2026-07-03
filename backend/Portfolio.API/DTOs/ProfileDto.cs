namespace Portfolio.API.DTOs;

public class ProfileDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? ResumeUrl { get; set; }
}

public class UpdateProfileRequest
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? ResumeUrl { get; set; }
}

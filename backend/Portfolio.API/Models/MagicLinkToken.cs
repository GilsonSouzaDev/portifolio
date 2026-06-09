namespace Portfolio.API.Models;

public class MagicLinkToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public bool IsUsed { get; set; } = false;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
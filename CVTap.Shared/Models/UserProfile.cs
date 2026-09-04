using SQLite;

namespace CVTap.Shared.Models;

public class UserProfile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string LinkedIn { get; set; } = string.Empty;

    public string GitHub { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;

    public Guid? DefaultCvId { get; set; }

    public Guid? DefaultTemplateId { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

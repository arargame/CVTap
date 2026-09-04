using SQLite;

namespace CVTap.Shared.Models;

public class EmailTemplate
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string SubjectTemplate { get; set; } = string.Empty;

    public string BodyTemplate { get; set; } = string.Empty;

    public string Language { get; set; } = "EN"; // "EN", "TR", etc.

    public bool IsDefault { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

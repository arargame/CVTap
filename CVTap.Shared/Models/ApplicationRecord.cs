using SQLite;

namespace CVTap.Shared.Models;

public enum ApplicationStatus
{
    Draft,
    ComposerOpened,
    Applied
}

public class ApplicationRecord
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CvProfileId { get; set; }

    public string CvTitle { get; set; } = string.Empty;

    public string RecipientEmail { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;

    public DateTime ActionTimestamp { get; set; } = DateTime.UtcNow;

    public string? Notes { get; set; }
}

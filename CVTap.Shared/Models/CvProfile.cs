using SQLite;

namespace CVTap.Shared.Models;

public class CvProfile
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string LocalPath { get; set; } = string.Empty;

    public string? Language { get; set; } = "EN"; // "EN", "TR", etc.

    public long FileSizeBytes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDefault { get; set; }

    [Ignore]
    public string FormattedSize
    {
        get
        {
            if (FileSizeBytes < 1024) return $"{FileSizeBytes} B";
            if (FileSizeBytes < 1024 * 1024) return $"{FileSizeBytes / 1024.0:F1} KB";
            return $"{FileSizeBytes / (1024.0 * 1024.0):F2} MB";
        }
    }
}

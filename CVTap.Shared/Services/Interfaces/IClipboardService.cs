namespace CVTap.Shared.Services.Interfaces;

/// <summary>
/// Service abstraction for interacting with system clipboard across platforms.
/// </summary>
public interface IClipboardService
{
    /// <summary>
    /// Copies the specified text to the system clipboard.
    /// </summary>
    /// <param name="text">The text to copy.</param>
    /// <returns>True if copy was successful; otherwise false.</returns>
    Task<bool> SetTextAsync(string? text);
}

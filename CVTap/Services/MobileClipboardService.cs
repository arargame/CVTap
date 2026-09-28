using CVTap.Shared.Services.Interfaces;

namespace CVTap.Services;

/// <summary>
/// MAUI native clipboard implementation using Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.
/// </summary>
public class MobileClipboardService : IClipboardService
{
    public async Task<bool> SetTextAsync(string? text)
    {
        try
        {
            await Clipboard.Default.SetTextAsync(text ?? string.Empty);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[MobileClipboardService] Failed to copy text to clipboard: {ex.Message}");
            return false;
        }
    }
}

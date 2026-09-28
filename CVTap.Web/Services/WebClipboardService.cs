using CVTap.Shared.Services.Interfaces;
using Microsoft.JSInterop;

namespace CVTap.Web.Services;

/// <summary>
/// Web platform clipboard service utilizing browser navigator.clipboard API.
/// </summary>
public class WebClipboardService : IClipboardService
{
    private readonly IJSRuntime _js;

    public WebClipboardService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<bool> SetTextAsync(string? text)
    {
        try
        {
            await _js.InvokeVoidAsync("navigator.clipboard.writeText", text ?? string.Empty);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WebClipboardService] Failed to copy text to clipboard: {ex.Message}");
            return false;
        }
    }
}

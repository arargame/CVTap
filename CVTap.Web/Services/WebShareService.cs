using CVTap.Shared.Services.Interfaces;
using Microsoft.JSInterop;

namespace CVTap.Web.Services;

public class WebShareService : IShareService
{
    private readonly IJSRuntime _js;

    public WebShareService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<bool> ShareFileAsync(string title, string physicalFilePath, string? textContent = null)
    {
        try
        {
            // Web share invocation or fallback alert/clipboard
            var supported = await _js.InvokeAsync<bool>("eval", "typeof navigator.share !== 'undefined'");
            if (supported)
            {
                await _js.InvokeVoidAsync("navigator.share", new
                {
                    title,
                    text = textContent ?? title
                });
                return true;
            }

            // Fallback: copy text to clipboard if navigator.clipboard is available
            if (!string.IsNullOrEmpty(textContent))
            {
                await _js.InvokeVoidAsync("navigator.clipboard.writeText", textContent);
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WebShareService] Share failed: {ex.Message}");
            return false;
        }
    }
}

using CVTap.Shared.Services.Interfaces;
using Microsoft.JSInterop;

namespace CVTap.Web.Services;

public class WebEmailComposerService : IEmailComposerService
{
    private readonly IJSRuntime _js;

    public WebEmailComposerService(IJSRuntime js)
    {
        _js = js;
    }

    public Task<bool> CanComposeEmailAsync() => Task.FromResult(true);

    public async Task<bool> ComposeEmailAsync(string recipient, string subject, string body, string? attachmentPhysicalPath = null)
    {
        try
        {
            var mailtoUri = $"mailto:{Uri.EscapeDataString(recipient ?? "")}?subject={Uri.EscapeDataString(subject ?? "")}&body={Uri.EscapeDataString(body ?? "")}";
            await _js.InvokeVoidAsync("open", mailtoUri, "_blank");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WebEmailComposerService] Failed to open mailto: {ex.Message}");
            return false;
        }
    }
}
